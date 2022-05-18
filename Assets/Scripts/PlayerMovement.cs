using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpingPower;
    public float groundCheckRadius;

    private bool _isFacingRight;
    public float horizontal { get; private set; }

    private Rigidbody2D _rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    //roll
    Vector2 rollDir;
    bool isRolling;
    public float rollVelocity = 14f;
    public float rollTime = 0.5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _isFacingRight = true;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        //on hold jump little higher
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpingPower);
        }
        if(Input.GetButtonUp("Jump") && _rb.velocity.y > 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }

        //ground roll animation yet to include
        //if (Input.GetButtonDown("Fire3") && IsGrounded())
        if (Input.GetKeyDown(KeyCode.DownArrow) && IsGrounded())
        {
            isRolling = true;
            rollDir = new Vector2(horizontal, 0f);

            //
            StartCoroutine(nameof(StopRolling));
        }
        if (isRolling)
        {
            _rb.velocity = rollDir.normalized * rollVelocity;
            return;
        }


        Flip();
    }

    private void FixedUpdate()
    {
        if (!isRolling)
        {
            _rb.velocity = new Vector2(horizontal * moveSpeed, _rb.velocity.y);

        }
    }

    public IEnumerator StopRolling()
    {
        yield return new WaitForSeconds(rollTime);
        isRolling = false;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    //flip player horizontal x-axis
    private void Flip()
    {
        if(_isFacingRight && horizontal < 0f || !_isFacingRight && horizontal > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }
}
