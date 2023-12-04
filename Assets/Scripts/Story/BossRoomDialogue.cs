using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDialogue : MonoBehaviour
{
    [SerializeField] private TextAsset bossRoomDialogue;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager gm = GameManager.GetInstance();
            if (!gm.isBossRoomDialogue)
            {
                gm.bossRoom(bossRoomDialogue);
            }
        }
    }
}
