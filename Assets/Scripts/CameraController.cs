using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private enum CameraStates
    {
        Disabled = 0,
        Active = 1
    };
    
    public CinemachineVirtualCamera cam2D;
    public CinemachineVirtualCamera cam3D;

    private void Awake()
    {
        CheckForCameraController();
    }

    private void CheckForCameraController()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetStartingCamera(bool isIn3DSpace)
    {
        if (isIn3DSpace)
        {
            Set3DCam();
        }
        else
        {
            Set2DCam();
        }
    }

    public void Set2DCam()
    {
        cam2D.Priority = (int) CameraStates.Active;
        cam3D.Priority = (int) CameraStates.Disabled;
    }

    public void Set3DCam()
    {
        cam2D.Priority = (int) CameraStates.Disabled;
        cam3D.Priority = (int) CameraStates.Active;
    }
}
