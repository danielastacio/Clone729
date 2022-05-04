using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public CinemachineVirtualCamera cam2D;
    public CinemachineVirtualCamera cam3D;

    private void Awake()
    {
        instance = this;
        cam2D.Priority = 1;
        cam3D.Priority = 0;
    }

    public void Set2DCam()
    {
        cam2D.Priority = 1;
        cam3D.Priority = 0;
    }

    public void Set3DCam()
    {
        cam2D.Priority = 0;
        cam3D.Priority = 1;
    }
}
