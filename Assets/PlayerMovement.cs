using System;
using System.Collections;
using System.Collections.Generic;
using scr_Interfaces;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField]
    private float maxHp;

    [SerializeField]
    private float currentHp;

    [SerializeField]
    private float speed = 0;

    [SerializeField]
    private float defaultSpeed = 10;

    [SerializeField]
    private float crouchSpeed = 5;

    [SerializeField]
    private float jumpForce = 25;

    [SerializeField]
    private float rollForce = 1000;

    [SerializeField]
    private bool _isGrounded;

    [SerializeField]
    private bool isCrouching, isRolling;

    [SerializeField]
    private float playerHeight;

    [SerializeField]
    private float crouchHeight;
    private float horizontal;

    [SerializeField]
    private bool canMove;
    [SerializeField]
    private bool isMovingRight;
    [SerializeField]
    private bool isMovingLeft;

    public bool isfacingRight;
    public bool isfacingLeft;

    [SerializeField]
    private float rollTime;
    [SerializeField]
    private float defaultRollTime = 0.5f;

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

        speed = defaultSpeed;
        canMove = true;
        playerHeight = transform.localScale.y;
        crouchHeight = playerHeight / 2;
        rollTime = defaultRollTime;

        isfacingRight = true;
        isfacingLeft = false;
        
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.S) && _isGrounded && !isRolling)
        {
            isCrouching = true;
        }

        else
        {
            isCrouching = false;
        }

        if(Input.GetKey(KeyCode.A) && !isRolling)
        {
            isMovingLeft = true;            
            isfacingLeft = true;

            isfacingRight = false;
        }

        else
        {
           isMovingLeft = false;
        }

        if (Input.GetKey(KeyCode.D) && !isRolling)
        {
            isMovingRight = true;
            isfacingRight = true;

            isfacingLeft = false;            
        }

        else
        {
            isMovingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.K) && _isGrounded && !isRolling && !isCrouching)
        {
            isRolling = true;
        }

        if (isRolling)
        {            
            rollTime -= Time.deltaTime;
            if (rollTime <= 0 )
            {
                rollTime = 0;
                isRolling = false;
            }
        }

        else
        {
            rollTime = defaultRollTime;
        }
    }

    void FixedUpdate()
    {
        Move();
        CheckIfGrounded();
        Jump();
        Crouch();
        Roll();
    }
    void Move()
    {
        if (canMove)
        {
            if (isMovingLeft)
            {
                _rb.velocity = new Vector2(-speed, _rb.velocity.y);
            }

            else if(isMovingRight)
            {
                _rb.velocity = new Vector2(speed, _rb.velocity.y);
            }

            else
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }

        }
    }

    void Crouch()
    {
        if (isCrouching)
        {            
            if (transform.localScale.y != crouchHeight)
            {
                transform.localScale = new Vector2(transform.localScale.x, crouchHeight);
            }
            speed = crouchSpeed;
        }

        else
        {

            transform.localScale = new Vector2(transform.localScale.x, playerHeight);
            speed = defaultSpeed;
        }
    }

    void Roll()
    {
        Vector2 rollDirection = isfacingLeft ? Vector2.left : Vector2.right;

        if(isRolling)
        {
            _rb.AddForce(rollDirection * rollForce, ForceMode2D.Impulse);
            transform.localScale = new Vector2(transform.localScale.x, crouchHeight);
        }

        else if(!isCrouching)
        {
            transform.localScale = new Vector2(transform.localScale.x, playerHeight);
        }
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

        if (jumpInput && _isGrounded && !isRolling)
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
