using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    public bool isPaused;
    public bool isGameStarted;
    public bool isBeginningDialogue;

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
}
