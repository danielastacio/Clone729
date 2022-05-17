using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

// TODO: Link SFX and Animations
// TODO: Set up a max jump height, variable created
// TODO: Set up a way for player to heal
public class Player : MonoBehaviour, IDamageable
{
    [Header("Stats")] 
    public float maxHp;
    public float currentHp;
    public float speed;
    public float jumpForce;
    public float maxJumpHeight;
    private bool _isGrounded;
    
    // Cached References
    private Rigidbody2D _rb;

    [Header("Fall Variables")] 
    public float fallMultiplier;

    [Header("Ground Check")]
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private Vector3 _groundCheckPos;

    void Awake()
    {
        currentHp = maxHp;
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        Move();
        CheckIfGrounded();
        Jump();
    }

    void Move()
    {
        var horizontal = Input.GetAxisRaw("Horizontal") * speed;

        _rb.velocity = new Vector2(horizontal, _rb.velocity.y);
    }

    void CheckIfGrounded()
    {
        _groundCheckPos = transform.GetChild(1).transform.position;
        var groundCheck = 
            Physics2D.OverlapCircle(_groundCheckPos, groundCheckRadius, whatIsGround);

        if (groundCheck)
        {
            _isGrounded = true;
        }
    }
    
    void Jump()
    {
        var jumpInput = Input.GetKey(KeyCode.Space);

        if (jumpInput && _isGrounded)
        {
            _isGrounded = false;
            _rb.velocity = Vector2.up * jumpForce;
        }
        else if (_rb.velocity.y < 0 && !jumpInput)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            // Play death animation
            // Reset level on death
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_groundCheckPos, groundCheckRadius);
    }
}
