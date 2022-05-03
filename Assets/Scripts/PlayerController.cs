using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float speed = 300f;
    public float turnSpeed = 15f;
    
    private float _horizontal;
    private float _vertical;
    
    private Rigidbody _rb;

    void Start()
    {
        // Create reference to rigidbody
        _rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        Move();
    }

    void Move()
    {
        // Set each input, with calculations for consistent movement
        _horizontal = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        _vertical = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

        Vector3 movement = new Vector3(_horizontal, 0, _vertical);
        _rb.velocity = movement;
        
        RotatePlayer(movement);
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
}
