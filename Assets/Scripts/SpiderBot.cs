using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO: Set up ability for spider-bot to walk on walls
// TODO: Set up die state

public class SpiderBot : MonoBehaviour, IDamageable<float>
{
    private enum EnemyState
    {
        Idle,
        Die
    };
    
    private EnemyState _currentState = EnemyState.Idle;

    [Header("Stats")] 
    public float maxHp;
    private float _currentHp;
    
    [Header("Movement")]
    public float speed;
    public float gcRaycastDist;
    public bool facingRight = true;
    public bool walkOnCeiling;
    
    // Controls for stopping and restarting movement
    [Header("Stop Movement")]
    public float minTimeBeforeNextStop;
    public float maxTimeBeforeNextStop;
    public float minStopTime;
    public float maxStopTime;
    private float _timeSinceLastStop;
    private float _timeBeforeNextStop;
    
    // Cached references
    private Rigidbody2D _rb2d;
    private Transform _groundCheck;
    private IEnumerator _walkCycle;
    
    private void Start()
    {
        _currentHp = maxHp;
        _rb2d = GetComponent<Rigidbody2D>();
        _groundCheck = transform.GetChild(1).transform;

        if (!facingRight)
        {
            Flip(-180);
            speed *= -1;
        }
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

            _rb2d.velocity = new Vector2(speed, 0);

            yield return null;
        }
    }

    private void CheckForGround()
    {
        // If spider-bot is on ceiling, this sets raycast to shoot up instead of down.
        var groundCheckDirection = walkOnCeiling ? Vector2.up : Vector2.down;
        var groundCheck = 
            Physics2D.Raycast(_groundCheck.position, groundCheckDirection, gcRaycastDist);

        if (!groundCheck && facingRight)
        {
            Flip(-180);
        }
        else if (!groundCheck && !facingRight)
        {
            Flip(0);
        }
    }

    private void CheckForWall()
    {
        // Depending on facing direction, raycast will point forward
        var wallCheckDirection = facingRight ? Vector2.right : Vector2.left;
        var wallCheck =
            Physics2D.Raycast(_groundCheck.position, wallCheckDirection, gcRaycastDist);
        if (wallCheck && facingRight)
        {
            Flip(-180);
        }
        else if (wallCheck && !facingRight)
        {
            Flip(0);
        }
    }
    
    private void Flip(int flipAmount)
    {
        transform.eulerAngles = new Vector3(0, -flipAmount, 0);
        facingRight = !facingRight;
        speed *= -1;
    }

    private IEnumerator Stop()
    {
        var stopTime = Random.Range(minStopTime, maxStopTime);
        var currentSpeed = speed;
        speed = 0;
        _rb2d.Sleep();
       
        _timeSinceLastStop = 0f;
        _timeBeforeNextStop = Random.Range(minTimeBeforeNextStop, maxTimeBeforeNextStop);

        yield return new WaitForSeconds(stopTime);
        
        speed = currentSpeed;
    }


    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            _currentState = EnemyState.Die;
        }
    }
}
