using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 20f;

    private float _horizontal;
    private float _vertical;

    private bool _isIn3DSpace = false;
    
    private Rigidbody _rb;

    void Start()
    {
        // Create reference to rigidbody
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Set each input, with calculations for consistent movement
        _horizontal = Input.GetAxisRaw("Horizontal") * speed;
        _vertical = Input.GetAxisRaw("Vertical") * speed;

        if (!_isIn3DSpace)
        {
            Vector3 movement = new Vector3(_horizontal, 0, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            _rb.velocity = movement;
            RotatePlayer(movement);
            GameManager.Instance.Set2DCam();
        }
        else
        {
            Vector3 movement = new Vector3(_horizontal, 0, _vertical);
            _rb.velocity = movement;
            RotatePlayer(movement);
            GameManager.Instance.Set3DCam();
        }
    }

    private void RotatePlayer(Vector3 movement)
    {
        // If this isn't wrapped in an if statement, it dumps to the console
        // about LookRotation having a "zero vector".
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(movement),
                turnSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WorldTransition"))
        {
           _isIn3DSpace = !_isIn3DSpace;
        }
    }
}
