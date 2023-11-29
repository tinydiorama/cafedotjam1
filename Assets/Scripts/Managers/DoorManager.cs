using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] public GameObject visualCue;
    [SerializeField] private Vector2 newCoordinates;
    [SerializeField] private RoomManager oldRoom;
    [SerializeField] private RoomManager newRoom;
    [SerializeField] RectTransform fader;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private bool isTrigger;

    private bool playerInRange;
    private GameObject player;

    private void Start()
    {
        playerInRange = false;
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
        player = PlayerManager.GetInstance().gameObject;
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }

    private void Update()
    {
        if (visualCue != null)
        {
            if (playerInRange)
            {
                visualCue.SetActive(true);
            }
            else
            {
                visualCue.SetActive(false);
            }
        }
    }

    public void goToCoordinates()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            Vector2 posDelta = newCoordinates - (Vector2)player.transform.position;
            oldRoom.gameObject.SetActive(false);
            player.transform.position = newCoordinates;
            newRoom.gameObject.SetActive(true);
            newRoom.resetEnemies();
            PlayerManager.GetInstance().setActiveRoom(newRoom);
            vcam.OnTargetObjectWarped(player.transform, posDelta);
            LeanTween.alpha(fader, 0, 1f).setOnComplete(() =>
            {
                fader.gameObject.SetActive(false);
            });
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            if ( isTrigger )
            {
                goToCoordinates();
            }
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
