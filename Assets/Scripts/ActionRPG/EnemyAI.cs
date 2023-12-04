using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] private float speed = 200;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float distanceForWander = 4f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Vector2 targetDirection;
    public bool enemyPaused;
    public bool enemyKnockback;

    // wander randomly
    private float wanderTimer;
    private float wanderCooldown = 1f;

    private GameObject player;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.3f);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void UpdatePath()
    {
        if ( seeker.IsDone() )
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if ( ! p.error )
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if ( path == null )
        {
            return;
        }

        if ( ! GameManager.GetInstance().isPaused && ! enemyPaused && ! enemyKnockback )
        {

            float playerDistance = Vector2.Distance(transform.position, player.transform.position);

            if (playerDistance < distanceForWander)
            {
                if (currentWaypoint >= path.vectorPath.Count)
                {
                    reachedEndOfPath = true;
                    return;
                }
                else
                {
                    reachedEndOfPath = false;
                }

                targetDirection = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 force = targetDirection * speed * Time.deltaTime;

                //rb.AddForce(force);
                rb.velocity = targetDirection * speed;

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }

                if (force.x >= 0.01f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else if (force.x <= -0.01f)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
            else
            {
                //player not in range, don't pursue
                updateTargetDirection();
                rb.velocity = targetDirection * speed;
            }
        }


    }

    private void updateTargetDirection()
    {
        wanderTimer -= Time.deltaTime;

        if ( wanderTimer <= 0 )
        {
            float angle = Random.Range(-90, 90);
            Quaternion rotation = Quaternion.AngleAxis(angle, transform.forward);
            targetDirection = rotation * targetDirection;

            wanderTimer = Random.Range(1f, wanderCooldown);
        }
    }
}


