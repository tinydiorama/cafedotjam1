using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [SerializeField] private GameObject enemySpawn1;
    [SerializeField] private GameObject enemySpawn2;
    [SerializeField] private GameObject enemySpawn3;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform spawnParent;

    private GameObject player;
    private float timer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10f)
        {
            timer = 0;
            spawn();
        }
    }
    private void spawn()
    {
        int randomSpawnNumber = Random.Range(1, 10);
        if ( randomSpawnNumber < 5 )
        {
            GameObject spawn = Instantiate(enemySpawn1, spawnParent.transform);
            spawn.transform.localPosition = spawnPosition.transform.localPosition;
            spawn.GetComponent<EnemyAI>().target = player.transform;
        } else if ( randomSpawnNumber < 8 )
        {
            GameObject spawn = Instantiate(enemySpawn2, spawnParent.transform);
            spawn.transform.localPosition = spawnPosition.transform.localPosition;
            spawn.GetComponent<EnemyAI>().target = player.transform;
        } else
        {
            GameObject spawn = Instantiate(enemySpawn3, spawnParent.transform);
            spawn.transform.localPosition = spawnPosition.transform.localPosition;
            spawn.GetComponent<EnemyAI>().target = player.transform;
        }
    }
}
