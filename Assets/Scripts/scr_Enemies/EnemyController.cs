using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;
namespace scr_Enemies
{
    public class EnemyController : MonoBehaviour
    {
        protected enum EnemyState
        {
            Idle,
            Attack,
            Die
        };
    
        private EnemyState _currentState = EnemyState.Idle;

        [Header("Stats")] 
        public float maxHp;
        private float _currentHp;
        public float attackDamage;

        [Header("Movement")] 
        public float speed;
        private float _horizSpeed;
        [SerializeField] protected float gcRaycastDist;
        [SerializeField] protected bool facingRight;
        protected Vector2 GroundCheckDirection;
        protected Vector2 WallCheckDirection;

        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField] protected LayerMask whatIsWall;
        
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
    
        protected virtual void SetRotationAndSpeed()
        {
            if (facingRight)
            {
                _horizSpeed = speed;
                SetHorizontalRotation();
                SetWallCheck(Vector2.right);
            }
            else if (!facingRight)
            {
                _horizSpeed = -speed;
                SetHorizontalRotation();
                SetWallCheck(Vector2.left);
            }
            GroundCheckDirection = Vector2.down;
        }
    
        private void SetHorizontalRotation()
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
    
        private void SetWallCheck(Vector2 direction)
        {
            WallCheckDirection = direction;
        }

        private void CheckForGround()
        {
            var groundCheck = 
                Physics2D.Raycast(_groundCheck.position, GroundCheckDirection, gcRaycastDist, whatIsGround);

            if (!groundCheck)
            {
                Flip();
            }

        }

        private void CheckForWall()
        {
            var wallCheck =
                Physics2D.Raycast(_groundCheck.position, WallCheckDirection, gcRaycastDist, whatIsWall);
        
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

                _rb.velocity = new Vector2(_horizSpeed, 0);

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
}
