using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Vector3 _closedPos;
    private Vector3 _openPos;
    private float _transitionTime;
    private GameObject _doorParent;

    private void Start()
    {
        _doorParent = transform.parent.gameObject;
        _closedPos = _doorParent.transform.position;
        _openPos = new Vector3(
            _closedPos.x,
            -2f,
            _closedPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(OpenDoor());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CloseDoor());
        }
    }

    /*private IEnumerator CenterPlayer2D(Collider other)
    {
        var playerPos = other.transform.position;

        playerPos = other.transform.position;
        playerPos =
            new Vector3(playerPos.x, playerPos.y, 
                Mathf.Lerp(playerPos.z, transform.position.z, _transitionTime));
        other.transform.position = playerPos;
        
        yield return null;
    }*/

    IEnumerator OpenDoor()
    {
        _transitionTime = 0;
        
        while (_doorParent.transform.position.y > _openPos.y)
        {
            var position = _doorParent.transform.position;
            
            position = new Vector3(
                position.x,
                Mathf.Lerp(_closedPos.y, _openPos.y, _transitionTime),
                position.z);
            
            _doorParent.transform.position = position;
            _transitionTime += 0.1f;
            
            yield return null;
        }


    }

    IEnumerator CloseDoor()
    {
        _transitionTime = 0;
        
        while (_doorParent.transform.position.y < _closedPos.y)
        {
            var position = _doorParent.transform.position;
            
            position = new Vector3(
                position.x,
                Mathf.Lerp(_openPos.y, _closedPos.y, _transitionTime),
                position.z);
            
            _doorParent.transform.position = position;
            _transitionTime += 0.1f;
            
            yield return null;
        }
    }
}
