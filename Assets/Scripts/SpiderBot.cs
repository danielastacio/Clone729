using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

// TODO: Set up die state

public class SpiderBot : MonoBehaviour, IDamageable
{
    private enum EnemyState
    {
        Idle,
        Die
    };

    private enum Positions
    {
        Ground,
        Ceiling,
        LeftWall,
        RightWall
    };
    
    private readonly Dictionary<Positions, Vector3> _startingRotations = new Dictionary<Positions, Vector3>()
    {
        {Positions.Ground, new Vector3(0, 0, 0)},
        {Positions.Ceiling, new Vector3(0, 0, 180)},
        {Positions.LeftWall, new Vector3(0, 0, 270)},
        {Positions.RightWall, new Vector3(0, 0, 90)}
    };

    private EnemyState _currentState = EnemyState.Idle;

    [Header("Stats")] 
    public float maxHp;
    private float _currentHp;
    public float attackDamage;

    [Header("Movement")] 
    public float speed;
    private float _horizSpeed;
    private float _vertSpeed;
    private float _gcRaycastDist;
    [SerializeField] private bool facingRight;
    [SerializeField] private Positions startingPosition;
    private Vector3 _startingRotation;
    private Vector2 _groundCheckDirection;
    private Vector2 _wallCheckDirection;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    // Controls for stopping and restarting movement
    [Header("Stop Movement")]
    public float minTimeBeforeNextStop;
    public float maxTimeBeforeNextStop;
    public float minStopTime;
    public float maxStopTime;
    private float _timeSinceLastStop;
    private float _timeBeforeNextStop;
    
    // Cached references
    private Rigidbody2D _rb;
    private Transform _groundCheck;
    private IEnumerator _walkCycle;
    
    private void Start()
    {
        _currentHp = maxHp;
        _rb = GetComponent<Rigidbody2D>();
        _groundCheck = transform.GetChild(1).transform;

        SetStartingRotation();
        SetRotationAndSpeed();
    }

    private void FixedUpdate()
    {
        if (_currentState == EnemyState.Idle && _walkCycle == null)
        {
            _walkCycle = WalkCycle();
            StartCoroutine(WalkCycle());
        }
        else if (_currentState == EnemyState.Die)
        {
            StopCoroutine(WalkCycle());
            Destroy(gameObject);
        }
    }

    private void SetStartingRotation()
    {
        switch (startingPosition)
        {
            case Positions.Ground:
                _startingRotation = _startingRotations[Positions.Ground];
                _groundCheckDirection = Vector2.down;
                break;
            case Positions.Ceiling:
                _startingRotation = _startingRotations[Positions.Ceiling];
                _groundCheckDirection = Vector2.up;
                break;
            case Positions.LeftWall:
                _startingRotation = _startingRotations[Positions.LeftWall];
                _groundCheckDirection = Vector2.left;
                break;
            case Positions.RightWall:
                _startingRotation = _startingRotations[Positions.RightWall];
                _groundCheckDirection = Vector2.right;
                break;
        }
        transform.eulerAngles = _startingRotation;
    }

    private void SetRotationAndSpeed()
    {
        if ((facingRight && startingPosition == Positions.Ground) || 
            (!facingRight && startingPosition == Positions.Ceiling))
        {
            _horizSpeed = speed;
            SetHorizontalRotation();
            SetWallCheck(Vector2.right);
        }
        else if ((!facingRight && startingPosition == Positions.Ground) ||
                 facingRight && startingPosition == Positions.Ceiling)
        {
            _horizSpeed = -speed;
            SetHorizontalRotation();
            SetWallCheck(Vector2.left);
        }
        else if ((facingRight && startingPosition == Positions.RightWall) || 
                 !facingRight && startingPosition == Positions.LeftWall)
        {
            _vertSpeed = speed;
            SetVerticalRotation();
            SetWallCheck(Vector2.up);
        }
        else if ((!facingRight && startingPosition == Positions.RightWall) || 
                 facingRight && startingPosition == Positions.LeftWall)
        {
            _vertSpeed = -speed;
            SetVerticalRotation();
            SetWallCheck(Vector2.down);
        }
    }
    
    private void SetHorizontalRotation()
    {
        if (facingRight)
        {
            transform.eulerAngles = new Vector3(_startingRotation.x, 0, _startingRotation.z);
        }
        else if (!facingRight)
        {
            transform.eulerAngles = new Vector3(_startingRotation.x, 180, _startingRotation.z);
        }
    }

    private void SetVerticalRotation()
    {
        if (facingRight)
        {
            transform.eulerAngles = new Vector3(0, _startingRotation.y, _startingRotation.z);
        }
        else if (!facingRight)
        {
            transform.eulerAngles = new Vector3(180, _startingRotation.y, _startingRotation.z);
        }
    }
    
    private void SetWallCheck(Vector2 direction)
    {
        _wallCheckDirection = direction;
    }

    private void CheckForGround()
    {
        var groundCheck = 
            Physics2D.Raycast(_groundCheck.position, _groundCheckDirection, _gcRaycastDist, whatIsGround);

        if (!groundCheck)
        {
            Flip();
        }

    }

    private void CheckForWall()
    {
        var wallCheck =
            Physics2D.Raycast(_groundCheck.position, _wallCheckDirection, _gcRaycastDist, whatIsWall);
        
        if (wallCheck && facingRight)
        {
            Flip();
        }
        else if (wallCheck && !facingRight)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
        facingRight = !facingRight;
        SetRotationAndSpeed();
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            _currentState = EnemyState.Die;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable.TakeDamage(attackDamage);
        }
    }
    
    private IEnumerator WalkCycle()
    {
        while (_currentState == EnemyState.Idle)
        {
            var stopChance = Random.Range(0f, 100f);
            
            if (stopChance < 10 && _timeSinceLastStop > _timeBeforeNextStop)
            {
                yield return StartCoroutine(Stop());
            }
            else
            {
                _timeSinceLastStop += Time.deltaTime;
            }
            
            CheckForGround();
            CheckForWall();

            _rb.velocity = new Vector2(_horizSpeed, _vertSpeed);

            yield return null;
        }
    }
    
    private IEnumerator Stop()
    {
        var stopTime = Random.Range(minStopTime, maxStopTime);
        var currentSpeed = _rb.velocity;
        _rb.velocity = Vector2.zero;
        _rb.Sleep();
       
        _timeSinceLastStop = 0f;
        _timeBeforeNextStop = Random.Range(minTimeBeforeNextStop, maxTimeBeforeNextStop);

        yield return new WaitForSeconds(stopTime);
        
        _rb.velocity = currentSpeed;
    }
}
