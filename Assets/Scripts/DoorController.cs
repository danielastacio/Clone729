using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private PlayerController _playerController;
    private CameraController _cameraController;
    private Vector3 _closedPos;
    private Vector3 _openPos;
    private float _transitionTime = 5f;

    private void Start()
    {
        _playerController = PlayerController.Instance;
        _cameraController = CameraController.Instance;
        
        _closedPos = transform.position;
        _openPos = new Vector3(
            transform.position.x,
            -2f,
            transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(OpenDoor())
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Set up door to transition from open to closed instead of jumping
            StartCoroutine(CloseDoor())
            
            // TODO: If player enters from a 2D space, and goes back, camera still transitions to 3D
            // TODO: Need to check where the player is coming from and if that change needs to happen.
            if (_playerController.isIn3DSpace)
            {
                transform.position = 
                    new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
                _cameraController.Set2DCam();
            }
            else
            {
                _cameraController.Set3DCam();
            }

            _playerController.isIn3DSpace = !_playerController.isIn3DSpace;
        }
    }

    IEnumerator OpenDoor()
    {
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(_closedPos, _openPos, _transitionTime),
            transform.position.z);
        yield return null;
    }

    IEnumerator CloseDoor()
    {
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(_openPos, _closedPos, _transitionTime,
            transform.position.z);
        yield return null;
    }
}
