using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTarget;
    public float follwSpeed = 4f;
    public float OffsetY;
    public Vector3 aheadAmount;


    private void Update()
    {
        Vector3 newPosition = new Vector3(playerTarget.position.x, playerTarget.position.y + OffsetY, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosition + (aheadAmount * Input.GetAxis("Horizontal")), follwSpeed * Time.deltaTime);
    }

}
