using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyArea : MonoBehaviour
{
    private bool canVolley;

    private void Update()
    {
        if ( canVolley && Input.GetKeyDown("space") )
        {
            BattleManager.GetInstance().volleyBack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Volley")
        {
            canVolley = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ( collision.tag == "Volley" )
        {
            canVolley = false;
        }
    }
}
