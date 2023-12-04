using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TextAsset beginningDialogue;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager gm = GameManager.GetInstance();
            if ( ! gm.isGameStarted )
            {
                DialogueManager.GetInstance().EnterDialogueMode(beginningDialogue, null);
                gm.startGame();
            }
        }
    }
}
