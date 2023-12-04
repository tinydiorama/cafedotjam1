using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField] public GameObject visualCue;
    [SerializeField] public TextAsset cantEnterDoor;
    [SerializeField] public TextAsset canEnterDoor;
    [SerializeField] public Sprite doorOpenSprite;

    [SerializeField] private Vector2 newCoordinates;
    [SerializeField] private RoomManager oldRoom;
    [SerializeField] private RoomManager newRoom;
    [SerializeField] RectTransform fader;
    [SerializeField] private CinemachineVirtualCamera vcam;

    private bool isOpen;
    private bool playerInRange;
    private PlayerManager pm;
    private GameObject player;
    private float shakeDuration = 2f;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        pm = PlayerManager.GetInstance();
        player = pm.gameObject;
    }

    private void Update()
    {

        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown("space"))
            {
                if (!DialogueManager.GetInstance().dialogueIsPlaying)
                {
                    if (isOpen) // go through door
                    {
                        if ( ! GameManager.GetInstance().isConfrontBoss )
                        {
                            GameManager.GetInstance().isPaused = true;
                            GameManager.GetInstance().isConfrontBoss = true;
                            AudioManager.GetInstance().FadeOutMusic();
                        }
                        goToCoordinates();
                    }
                    else if (pm.numNotes == 5) // can go through door
                    {
                        StartCoroutine(shaking());
                    }
                    else // can't go through door
                    {
                        StartCoroutine(showCantEnterDialogue());
                    }
                }
            }
        }
    }

    public IEnumerator showCantEnterDialogue()
    {
        yield return new WaitForSeconds(0.2f);
        DialogueManager.GetInstance().EnterDialogueMode(cantEnterDoor, null);
    }
    public IEnumerator shaking()
    {
        float elapsedTime = 0f;
        CinemachineBasicMultiChannelPerlin cinemachinePerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        while ( elapsedTime < shakeDuration )
        {
            elapsedTime += Time.deltaTime;
            cinemachinePerlin.m_AmplitudeGain = 5f;
            yield return null;
        }
        GetComponent<SpriteRenderer>().sprite = doorOpenSprite;
        isOpen = true;
        DialogueManager.GetInstance().EnterDialogueMode(canEnterDoor, null);
        cinemachinePerlin.m_AmplitudeGain = 0f;
    }

    public void goToCoordinates()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1, 2f).setOnComplete(() =>
        {
            Vector2 posDelta = newCoordinates - (Vector2)player.transform.position;
            oldRoom.gameObject.SetActive(false);
            player.transform.position = newCoordinates;
            newRoom.gameObject.SetActive(true);
            newRoom.resetEnemies();
            PlayerManager.GetInstance().setActiveRoom(newRoom);
            vcam.OnTargetObjectWarped(player.transform, posDelta);
            LeanTween.alpha(fader, 0, 2f).setOnComplete(() =>
            {
                fader.gameObject.SetActive(false);
                GameManager.GetInstance().isPaused = false;
            });
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
