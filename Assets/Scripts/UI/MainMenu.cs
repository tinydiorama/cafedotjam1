using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private AudioClip goSound;

    public LevelLoader loader;

    private void Awake()
    {
        loader = LevelLoader.instance;
    }

    public void startGame()
    {
        loader.LoadLevel((int)SceneIndexes.MAIN_WORLD, new Vector2(0f, 0f));
        AudioManager.GetInstance().playSFX(goSound);
    }
    public void showCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void hideCredits()
    {
        creditsPanel.SetActive(false);
    }
}
