using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderBot : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Die
    };
    
    public float speed;
    public bool facingRight = true;
    
    // Controls for stopping and restarting movement
    private float _timeSinceLastStop;
    private float _timeBeforeNextStop;
    public float minTimeBeforeNextStop;
    public float maxTimeBeforeNextStop;
    public float minStopTime;
    public float maxStopTime;

    // Cached references
    private Rigidbody2D _rb2d;
    private Transform _groundCheck;
    private IEnumerator _walkCycle;
    public float gcRaycastDist;
    public LayerMask whatIsGround;

    private EnemyState _currentState = EnemyState.Idle;
    
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _groundCheck = transform.GetChild(1).transform;
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
                var currentSpeed = speed;
                yield return StartCoroutine(Stop());
                _timeSinceLastStop = 0f;
                _timeBeforeNextStop = Random.Range(minTimeBeforeNextStop, maxTimeBeforeNextStop);
                speed = currentSpeed;
            }
            else
            {
                _timeSinceLastStop += Time.deltaTime;
            }
            
            CheckForGround();

            _rb2d.velocity = new Vector2(speed, 0);

            yield return null;
        }
    }

    private void CheckForGround()
    {
        var groundCheck = 
            Physics2D.Raycast(_groundCheck.position, Vector2.down, gcRaycastDist, whatIsGround);

        if (!groundCheck && facingRight)
        {
            Flip(-180);
        }
        else if (!groundCheck && !facingRight)
        {
            Flip(0);
        }
    }
    
    public void Flip(int flipAmount)
    {
        transform.eulerAngles = new Vector3(0, -flipAmount, 0);
        facingRight = !facingRight;
        speed *= -1;
    }

    private IEnumerator Stop()
    {
        var stopTime = Random.Range(minStopTime, maxStopTime);
        speed = 0;
        yield return new WaitForSeconds(stopTime);
    }
}
