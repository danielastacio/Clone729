using System;
using System.Collections;
using Cinemachine;
using scr_Management;
using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Camera
{
    public class SwapCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private string cameraId;
        private WaitForSeconds _showTime = new(1.5f);

        private void OnEnable()
        {
            Actions.OnDoorTriggered += ActivateCam;
        }

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void ActivateCam(string id)
        {
            if (cameraId.Equals(id))
            {
                StartCoroutine(DisplayCamera());
            }
        }

        private IEnumerator DisplayCamera()
        {
            Actions.OnControllerChanged(ControllerType.Cutscene);
            _virtualCamera.Priority = 999;
            yield return _showTime;
            _virtualCamera.Priority = 0;
            Actions.OnControllerChanged(ControllerType.Gameplay);
        }
    }
}