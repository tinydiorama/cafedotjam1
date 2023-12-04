using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    [SerializeField] private float force;
    private GameObject player;
    private float timer;
    private float nextTimer;
    private Rigidbody2D rb;
    private int damage;
    private bool isAttacking;
    private EnemyAI enemyAI;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        damage = GetComponent<Enemy>().damage;
        enemyAI = GetComponent<EnemyAI>();
        nextTimer = Random.Range(1f, 3f);
    }

    private void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (!GameManager.GetInstance().isPaused)
        {
            if (distance < 4)
            {
                timer += Time.deltaTime;
                if (timer > nextTimer && ! isAttacking)
                {
                    StartCoroutine(attackWindup());
                }
            }
        }
        if ( enemyAI.enemyKnockback )
        {
            isAttacking = false;
            timer = 0;
            nextTimer = Random.Range(1f, 3f);
            StopCoroutine(attackWindup());
            StopCoroutine(attack());
        }

    }

    private IEnumerator attackWindup()
    {
        isAttacking = true;
        enemyAI.enemyPaused = true;
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1f);
        StartCoroutine(attack());
    }

    private IEnumerator attack()
    {
        isAttacking = true;
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector2(0, 0);
        isAttacking = false;
        enemyAI.enemyPaused = false;
        timer = 0;
        nextTimer = Random.Range(1f, 3f);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && isAttacking)
        {

            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();

            if (player != null)
            {
                //Debug.Log("player hit for " + damage + " plus " + AudioManager.GetInstance().getCurrentPlayerDefenseChange() + " modifier = " + (damage + AudioManager.GetInstance().getCurrentPlayerDefenseChange()));
                player.Health -= (damage + AudioManager.GetInstance().getCurrentPlayerDefenseChange());
            }
        }
    }
}
