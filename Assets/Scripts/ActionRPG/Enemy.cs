using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health
    {
        set
        {
            health = value;
            if ( health <= 0 )
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    [SerializeField] private int health = 1;
    [SerializeField] private int damage = 5;

    public void Defeated()
    {
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();

            if (player != null)
            {
                player.Health -= damage;
            }
        }
    }
}
