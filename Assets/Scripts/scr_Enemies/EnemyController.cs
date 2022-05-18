using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;
namespace scr_Enemies
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        protected enum EnemyState
        {
            Idle,
            Attack,
            Die
        };
    
        protected EnemyState CurrentState = EnemyState.Idle;

        [Header("Stats")] 
        public float maxHp;
        private float _currentHp;
        public float attackDamage;

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
        [Header("Stop Movement")]
        public float minTimeBeforeNextStop;
        public float maxTimeBeforeNextStop;
        public float minStopTime;
        public float maxStopTime;
        protected float TimeSinceLastStop;
        protected float TimeBeforeNextStop;
        
        // Cached references
        protected Rigidbody2D Rb;
        private Transform _groundCheck;
        private IEnumerator _walkCycle;
        
        private void Awake()
        {
            _currentHp = maxHp;
            Rb = GetComponent<Rigidbody2D>();
            _groundCheck = transform.GetChild(1).transform;
            
            SetRotationAndSpeed();
        }
        
        protected virtual void FixedUpdate()
        {
            if (CurrentState == EnemyState.Idle && _walkCycle == null)
            {
                _walkCycle = WalkCycle();
                StartCoroutine(WalkCycle());
            }
            else if (CurrentState == EnemyState.Die)
            {
                StopCoroutine(WalkCycle());
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
                Physics2D.Raycast(_groundCheck.position, GroundCheckDirection, gcRaycastDist, whatIsGround);

            if (!groundCheck)
            {
                Flip();
            }

        }

        protected void CheckForWall()
        {
            var wallCheck =
                Physics2D.Raycast(_groundCheck.position, _wallCheckDirection, gcRaycastDist, whatIsWall);
        
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
            while (CurrentState == EnemyState.Idle)
            {
                var stopChance = Random.Range(0f, 100f);
            
                if (stopChance < 10 && TimeSinceLastStop > TimeBeforeNextStop)
                {
                    yield return StartCoroutine(Stop());
                }
                else
                {
                    TimeSinceLastStop += Time.deltaTime;
                }
            
                CheckForGround();
                CheckForWall();

                Rb.velocity = new Vector2(HorizSpeed, 0);

                yield return null;
            }
        }
    
        protected IEnumerator Stop()
        {
            var stopTime = Random.Range(minStopTime, maxStopTime);
            var currentSpeed = Rb.velocity;
            Rb.velocity = Vector2.zero;
            Rb.Sleep();
       
            TimeSinceLastStop = 0f;
            TimeBeforeNextStop = Random.Range(minTimeBeforeNextStop, maxTimeBeforeNextStop);

            yield return new WaitForSeconds(stopTime);
        
            Rb.velocity = currentSpeed;
        }

        public void TakeDamage(float damage)
        {
            _currentHp -= damage;
            if (_currentHp <= 0)
            {
                CurrentState = EnemyState.Die;
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
    }
}
