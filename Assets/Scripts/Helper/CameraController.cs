using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        vcam.LookAt = playerObj.transform;
        vcam.Follow = playerObj.transform;
    }
}
