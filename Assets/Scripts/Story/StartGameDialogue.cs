using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameDialogue : MonoBehaviour
{
    [SerializeField] private TextAsset beginningDialogue;

    void Start()
    {
        if ( ! GameManager.GetInstance().isBeginningDialogue )
        {
            DialogueManager.GetInstance().EnterDialogueMode(beginningDialogue, null);
            GameManager.GetInstance().isBeginningDialogue = true;
        }
    }
}
