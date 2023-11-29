using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;

    public void resetEnemies()
    {
        foreach( Enemy enemy in enemies )
        {
            if (enemy != null)
            {
                enemy.resetPosition();
            }
        }
    }
}
