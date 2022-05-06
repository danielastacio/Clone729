using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private CameraController _cameraController;

    public float speed = 10f;
    public float turnSpeed = 20f;
    private float _horizontal;
    private float _vertical;
    
    public float jumpForce = 20f;
    private Vector3 _maxJumpHeight;

    private Vector3 _groundCheckPos;
    public float groundCheckRadius;
    private bool _isGrounded;
    public LayerMask[] groundLayers;
    private bool _isIn3DSpace;

    private Rigidbody _rb;

    void Awake()
    {
        _isIn3DSpace = false;
        CheckForPlayerController();
        SetUpCamera();
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Check2DOr3D();
        CheckIfGrounded();
        Move();
        Jump();
    }

    private void CheckForPlayerController()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void SetUpCamera()
    {
        _cameraController = CameraController.Instance;
        _cameraController.SetStartingCamera(_isIn3DSpace);
    }

    void Move()
    {
        _horizontal = Input.GetAxisRaw("Horizontal") * speed;
        _vertical = Input.GetAxisRaw("Vertical") * speed;

        if (!_isIn3DSpace)
        {
            Move2D();
        }
        else
        {
            Move3D();
        }
    }

    void Jump()
    {
        // This works, but doesn't feel good. 
        _maxJumpHeight = new Vector3(0, 3, 0);
        if (Input.GetKey(KeyCode.Space) && _isGrounded && transform.position.y < _maxJumpHeight.y)
        {
            _rb.AddForce(0, jumpForce, 0);
        }
    }

    private void Move2D()
    {
        var movement = new Vector3(_horizontal, 0, 0);
        _rb.velocity = movement;
        RotatePlayer(movement);
    }

    private void Move3D()
    {
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

    private void CheckIfGrounded()
    {
        _groundCheckPos = gameObject.transform.GetChild(1).transform.position;
        if (Physics.CheckSphere(_groundCheckPos, groundCheckRadius, groundLayers[0]) ||
            Physics.CheckSphere(_groundCheckPos, groundCheckRadius, groundLayers[1]))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

}

    private void Check2DOr3D()
    {
        _groundCheckPos = gameObject.transform.GetChild(1).transform.position;
        if (Physics.CheckSphere(_groundCheckPos, groundCheckRadius, groundLayers[0]))
        {
            _isIn3DSpace = false;
            _cameraController.Set2DCam();
        }
        else if (Physics.CheckSphere(_groundCheckPos, groundCheckRadius, groundLayers[1]))
        {
            _isIn3DSpace = true;
            _cameraController.Set3DCam();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_groundCheckPos, groundCheckRadius);
    }
}
