using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerHP = 100;
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
}
