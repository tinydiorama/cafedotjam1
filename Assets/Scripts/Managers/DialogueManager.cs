using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Dialogue Options")]

    [SerializeField] private float typingSpeed = 0.04f;

    private GameObject currentPanel;
    private TextMeshProUGUI currentText;
    private GameObject currentContinueIcon;

    private Story currentStory;
    private Coroutine displayLineCoroutine;
    private Coroutine makingChoice;
    private Coroutine exitCoroutine;

    private bool canContinueToNextLine = false;
    private bool isDialogue = false;
    private bool canChoose = false;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;

        // get all of the choices text 
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        if (canContinueToNextLine && currentStory.currentChoices.Count == 0 && Input.GetKeyDown("space"))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);

        currentPanel = dialoguePanel;
        currentText = dialogueText;
        currentContinueIcon = continueIcon;
        isDialogue = true;
        // reset portrait, layout, and speaker
        displayNameText.text = "???";
        portraitAnimator.Play("default");

        dialogueIsPlaying = true;
        currentPanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        currentPanel.SetActive(false);
        currentText.text = "";

        GameManager.GetInstance().isPaused = false;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            if (isDialogue)
            {
                // handle tags
                HandleTags(currentStory.currentTags);
            }
        }
        else
        {
            if (exitCoroutine != null)
            {
                StopCoroutine(exitCoroutine);
            }
            exitCoroutine = StartCoroutine(ExitDialogueMode());
        }
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > 0)
        {
            canChoose = true;
        }

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }
        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine && canChoose)
        {
            canChoose = false;
            if (makingChoice != null)
            {
                StopCoroutine(makingChoice);
            }
            makingChoice = StartCoroutine(ChooseOption(choiceIndex));
        }
    }

    private IEnumerator ChooseOption(int choiceIndex)
    {
        yield return new WaitForSeconds(0.2f);
        currentStory.ChooseChoiceIndex(choiceIndex);
        Input.GetKeyDown("space");
        ContinueStory();
    }

    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        canContinueToNextLine = false;
        currentText.text = line;
        currentText.maxVisibleCharacters = 0;
        currentContinueIcon.SetActive(false);
        if (isDialogue)
        {
            HideChoices();
        }

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray())
        {
            // if the submit button is pressed, finish up displaying the line right away
            if (Input.GetKeyDown("space"))
            {
                currentText.maxVisibleCharacters = line.Length;
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                currentText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        if (isDialogue)
        {
            DisplayChoices();
        }
        canContinueToNextLine = true;
        currentContinueIcon.SetActive(true);
    }

    // get variables like this:
    // string variableName = ((Ink.Runtime.StringValue) DialogueManager.GetInstance().GetVariableState("variablename")).value;
    // whew!
    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        if (variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }
}
