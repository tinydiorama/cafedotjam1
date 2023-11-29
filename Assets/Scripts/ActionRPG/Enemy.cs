using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Vector2 defaultCoords;

    public int Health
    {
        set
        {
            health = value;
            StartCoroutine(flashRed());
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

    [SerializeField] private int health = 10;
    public int damage = 5;

    private void Start()
    {
        //defaultCoords = transform.localPosition;
    }

    public void resetPosition()
    {
        transform.localPosition = defaultCoords;
    }

    public void Defeated()
    {
        Destroy(gameObject);
    }

    public IEnumerator flashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();

            if (player != null)
            {
                Debug.Log("player hit for " + damage + " plus " + AudioManager.GetInstance().getCurrentPlayerDefenseChange() + " modifier = " + (damage + AudioManager.GetInstance().getCurrentPlayerDefenseChange()));
                player.Health -= (damage + AudioManager.GetInstance().getCurrentPlayerDefenseChange());
            }
        }
    }
}
