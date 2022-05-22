using System;
using System.Collections;
using System.Collections.Generic;
using scr_Interfaces;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField]
    public float maxHp;

    [SerializeField]
    public float currentHp;

    [SerializeField]
    public float speed = 10;

    [SerializeField]
    private float jumpForce = 25 ;
    private bool _isGrounded;

    [SerializeField]
    private float offsetRadius = -1.7f;
    // Cached References
    private Rigidbody2D _rb;

    [Header("Fall Variables")]
    [SerializeField]
    private float fallMultiplier = 7;

    [Header("Ground Check")]
    [SerializeField]
    private float groundCheckRadius = 0.5f;
    public LayerMask whatIsGround;
    private Vector2 _groundCheckPos;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 10;
        _rb.freezeRotation = true;
    }

    private void Update()
    {

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
        _groundCheckPos = new Vector2(transform.position.x, transform.position.y + offsetRadius);
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
            _rb.velocity += (fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            // Play death animation

            // Reset level on death
            //SceneManager.LoadScene(1);

            currentHp = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_groundCheckPos, groundCheckRadius);
    }
}
