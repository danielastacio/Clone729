using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private float offsetRadius;
    // Cached References
    private Rigidbody2D _rb;

    [Header("Fall Variables")] 
    public float fallMultiplier;

    [Header("Ground Check")]
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private Vector2 _groundCheckPos;

    [Header("Scripts")]
    [SerializeField] private GameObject par_Managers;
    private Manager_UIReuse UIReuseScript;

    void Awake()
    {
        currentHp = maxHp;
        _rb = GetComponent<Rigidbody2D>();

        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();
        UIReuseScript.UpdatePlayerHealthUI(currentHp, maxHp);
    }

    private void Update()
    {
        /*
        --------------------------------------------
        DEBUGGING FEATURE - REMOVE BEFORE RELEASE!!!

        reduces players health by 10 while health is over 0
        --------------------------------------------
        */
        if (!par_Managers.GetComponent<UI_PauseMenu>().isGamePaused
            && Input.GetKeyDown(KeyCode.V)
            && currentHp -10 >= 0)
        {
            TakeDamage(10);
        }
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
            
            foreach(Transform child in UIReuseScript.PlayerHealthBar.transform)
            {
                if (child.name == "bar")
                {
                    child.gameObject.SetActive(false);
                    break;
                }
            }
        }

        Debug.Log("Player took " + damage + " damage!");
        UIReuseScript.UpdatePlayerHealthUI(currentHp, maxHp);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_groundCheckPos, groundCheckRadius);
    }
}
