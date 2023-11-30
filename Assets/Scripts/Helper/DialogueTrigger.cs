using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] public GameObject visualCue;
    [SerializeField] public TextAsset dialogue;
    [SerializeField] private bool journalEntryTrigger;
    [SerializeField] private int journalIndex;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        journalIndex = -1;
    }

    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown("space"))
            {
                if ( ! DialogueManager.GetInstance().dialogueIsPlaying )
                {
                    if (journalEntryTrigger && journalIndex == -1)
                    {
                        Debug.Log("playing next journal entry");
                        journalIndex = JournalManager.GetInstance().playNextJournalEntry();
                    }
                    else if (journalEntryTrigger && journalIndex != -1)
                    {
                        JournalManager.GetInstance().playJournalEntry(journalIndex);
                    }
                    else
                    {
                        DialogueManager.GetInstance().EnterDialogueMode(dialogue);
                    }
                }
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            playerInRange = true;
            PlayerMovement pm = collision.GetComponent<PlayerMovement>();
            pm.dialogueInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            PlayerMovement pm = collision.GetComponent<PlayerMovement>();
            pm.dialogueInRange = false;
        }
    }
}
