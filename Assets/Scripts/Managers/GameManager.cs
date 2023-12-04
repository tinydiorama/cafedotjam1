using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    [SerializeField] private BossEnemy finalBoss;
    public bool isPaused;
    public bool isGameStarted;
    public bool isBeginningDialogue;
    public bool isBossRoomDialogue;
    public bool isConfrontBoss;

    // ending warp
    [SerializeField] private Vector2 newCoordinates;
    [SerializeField] private RoomManager oldRoom;
    [SerializeField] private RoomManager newRoom;
    [SerializeField] RectTransform fader;
    [SerializeField] private CinemachineVirtualCamera vcam;
    private PlayerManager pm;

    private static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than one dialogue manager");
        }
        instance = this;
        if ( isGameStarted )
        {
            startGame();
        }
    }
    private void Start()
    {
        pm = PlayerManager.GetInstance();
    }
    public static GameManager GetInstance()
    {
        return instance;
    }

    public void startGame()
    {
        isGameStarted = true;
        hud.SetActive(true);
        AudioManager.GetInstance().StartPlaylist();
    }

    public void bossRoom(TextAsset bossRoomDialogue)
    {
        DialogueManager.GetInstance().EnterDialogueMode(bossRoomDialogue, startBossBattle);
    }

    public void startBossBattle()
    {
        isBossRoomDialogue = true;
        finalBoss.enableBoss();
        HUD.GetInstance().updateCurrentStats();
    }

    public void warpToEnd()
    {
        AudioManager.GetInstance().FadeOutMusic();
        isGameStarted = false;
        hud.SetActive(false);
        goToCoordinates();
    }

    public void goToCoordinates()
    {
        isPaused = true;
        fader.gameObject.SetActive(true);
        GameObject player = pm.gameObject;
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
            });
        });
    }
}
