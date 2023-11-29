using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPos;

    private GameObject player;
    private float timer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if ( distance < 4 )
        {
            timer += Time.deltaTime;
            if (timer > 2f)
            {
                timer = 0;
                shoot();
            }
        }

    }

    private void shoot()
    {
        GameObject proj = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        proj.GetComponent<Projectile>().damage = GetComponent<Enemy>().damage;
    }
}
