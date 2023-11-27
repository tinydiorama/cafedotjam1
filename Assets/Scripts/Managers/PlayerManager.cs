using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    [SerializeField] public int health = 50;
    [SerializeField] public int maxHealth = 50;
    public int playerDamage = 10;

    private static PlayerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than one player manager");
        }
        instance = this;
    }

    public static PlayerManager GetInstance()
    {
        return instance;
    }

    private void Defeated()
    {
        Destroy(gameObject);
    }
}
