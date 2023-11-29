using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float force;
    public int damage;

    private GameObject player;
    private Rigidbody2D rb;
    private float timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if ( timer > 5 )
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();

            if (player != null)
            {
                Debug.Log("player hit for " + damage + " plus " + AudioManager.GetInstance().getCurrentPlayerDefenseChange() + " modifier = " + (damage + AudioManager.GetInstance().getCurrentPlayerDefenseChange()));
                player.Health -= (damage + AudioManager.GetInstance().getCurrentPlayerDefenseChange());
                Destroy(gameObject);
            }
        }
    }
}
