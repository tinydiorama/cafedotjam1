using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingGameDialogue : MonoBehaviour
{
    [SerializeField] public TextAsset finalDialogue;

    private LevelLoader loader;

    // Start is called before the first frame update
    void Start()
    {
        loader = LevelLoader.instance;
        DialogueManager.GetInstance().EnterDialogueMode(finalDialogue, transitionToEnding);
    }

    private void transitionToEnding()
    {
        loader.LoadLevel((int)SceneIndexes.END_SCREEN);
    }
}
