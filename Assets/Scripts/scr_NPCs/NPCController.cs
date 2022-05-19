using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    // NPC Classes can inherit directly from this script
    
    protected enum State
    {
        Idle,
        Patrol,
        Attack,
        Die
    };
    
    protected State CurrentState = State.Idle;
    
    [Header("Movement")] 
    public float speed;
    protected float HorizSpeed;
    [SerializeField] protected float gcRaycastDist;
    [SerializeField] protected bool facingRight;
    protected Vector2 GroundCheckDirection;
    private Vector2 _wallCheckDirection;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
        
    // Controls for stopping and restarting movement
    [Header("Idle delay")]
    public float minTimeBeforeNextIdle;
    public float maxTimeBeforeNextIdle;
    public float minIdleTime;
    public float maxIdleTime;
    protected float TimeSinceLastIdle;
    protected float TimeBeforeNextIdle;
        
    // Cached references
    protected Rigidbody2D Rb;
    protected Transform GroundCheck;
    protected IEnumerator _idle;
    protected IEnumerator _patrol;
    protected IEnumerator _attack;
    protected IEnumerator _die;
    
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        GroundCheck = transform.GetChild(1).transform;
            
        SetRotationAndSpeed();
    }
        
    protected virtual void FixedUpdate()
    {
        CheckActiveState();
    }
    
    protected void CheckActiveState()
    {
        // TODO: Add the rest of the states
        if (CurrentState == State.Idle && _idle == null)
        {
            StopAllCoroutines();
            _patrol = null;
            _idle = Idle();
            StartCoroutine(_idle);
        }
        else if (CurrentState == State.Patrol && _patrol == null)
        {
            StopAllCoroutines();
            _idle = null;
            _patrol = WalkCycle();
            StartCoroutine(_patrol);
        }
        else if (CurrentState == State.Die)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
    
    protected virtual void SetRotationAndSpeed()
    {
        if (facingRight)
        {
            HorizSpeed = speed;
            SetHorizontalRotation();
            SetWallCheck(Vector2.right);
        }
        else if (!facingRight)
        {
            HorizSpeed = -speed;
            SetHorizontalRotation();
            SetWallCheck(Vector2.left);
        }
        GroundCheckDirection = Vector2.down;
    }

    
    protected virtual void SetHorizontalRotation()
    {
        if (facingRight)
        {
            transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
        }
        else if (!facingRight)
        {
            transform.eulerAngles = new Vector3(transform.rotation.x, 180, transform.rotation.z);
        }
    }
    
    protected void SetWallCheck(Vector2 direction)
    {
        _wallCheckDirection = direction;
    }

    protected void CheckForGround()
    {
        var groundCheck = 
            Physics2D.Raycast(GroundCheck.position, GroundCheckDirection, gcRaycastDist, whatIsGround);

        if (!groundCheck)
        {
            Flip();
        }

    }

    protected void CheckForWall()
    {
        var wallCheck =
            Physics2D.Raycast(GroundCheck.position, _wallCheckDirection, gcRaycastDist, whatIsWall);
        
        if (wallCheck)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
        facingRight = !facingRight;
        SetRotationAndSpeed();
    }

    
    protected virtual IEnumerator WalkCycle()
    {
        while (CurrentState == State.Patrol)
        {
            var stopChance = Random.Range(0f, 100f);
            
            if (stopChance < 10 && TimeSinceLastIdle > TimeBeforeNextIdle)
            {
                CurrentState = State.Idle;
            }
            else
            {
                TimeSinceLastIdle += Time.deltaTime;
            }
            
            CheckForGround();
            CheckForWall();

            Rb.velocity = new Vector2(HorizSpeed, 0);

            yield return null;
        }
    }
    
    protected IEnumerator Idle()
    {
        while (CurrentState == State.Idle)
        {
            var stopTime = Random.Range(minIdleTime, maxIdleTime);
            var currentSpeed = Rb.velocity;
            Rb.velocity = Vector2.zero;
            Rb.Sleep();

            TimeSinceLastIdle = 0f;
            TimeBeforeNextIdle = Random.Range(minTimeBeforeNextIdle, maxTimeBeforeNextIdle);

            yield return new WaitForSeconds(stopTime);

            Rb.velocity = currentSpeed;
            CurrentState = State.Patrol;
        }
    }
}
