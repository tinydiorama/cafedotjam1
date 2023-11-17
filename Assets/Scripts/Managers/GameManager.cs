using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused;

    private static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than one dialogue manager");
        }
        instance = this;
    }
    public static GameManager GetInstance()
    {
        return instance;
    }
}
