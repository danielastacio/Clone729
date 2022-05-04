using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public CinemachineVirtualCamera cam2D;
    public CinemachineVirtualCamera cam3D;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
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
