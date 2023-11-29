using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterScreen : MonoBehaviour
{
    [SerializeField] private AudioClip goSound;
    [SerializeField] private CanvasGroup[] groupsToFade;

    public LevelLoader loader;

    private void Awake()
    {
        loader = LevelLoader.instance;
    }

    private void Start()
    {
        float delay = 0;
        foreach( CanvasGroup group in groupsToFade )
        {

            group.alpha = 0f;
            float duration = 1f;
            LeanTween.alphaCanvas(group, 1.0f, duration).setDelay(delay);
            delay = delay + 2f;
        }
    }

    public void startGame()
    {
        loader.LoadLevel((int)SceneIndexes.MAIN_WORLD, new Vector2(0f, 0f));
        AudioManager.GetInstance().playSFX(goSound);
    }
}
