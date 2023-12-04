using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private EnemyShooting enemyShooting;
    [SerializeField] private EnemyAttacking enemyAttacking;
    [SerializeField] private EnemySpawning enemySpawning;
    [SerializeField] private PlayerMovement player;

    private EnemyAI enemyAI;

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void enableBoss()
    {
        enemyAI.enemyPaused = false;
        enemyShooting.enabled = true;
        enemyAttacking.enabled = true;
        enemySpawning.enabled = true;
        AudioManager.GetInstance().playBossMusic();
    }

    public override void Defeated()
    {
        GameManager.GetInstance().isPaused = true;
        GameManager.GetInstance().warpToEnd();
        player.setAlbumFaceUp();
        Destroy(gameObject);
    }
}
