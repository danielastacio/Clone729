using System.Collections;
using UnityEngine;

namespace scr_NPCs
{
    public class NPCController : MonoBehaviour
    {
        // NPC Classes can inherit directly from this script
    
        protected enum State
        {
            Idle,
            Patrol,
            Attack,
            Retreat,
            Die
        };
    
        protected State CurrentState = State.Idle;
    
        [Header("Movement")] 
        public float speed;
        protected float HorizSpeed;
        [SerializeField] protected float groundCheckRaycastDist;
        [SerializeField] protected bool facingRight;
        protected Vector2 GroundCheckDirection;
        protected Vector2 WallCheckDirection;

        [Header("Ground/Wall Checks")] 
        protected RaycastHit2D GroundCheck;
        protected RaycastHit2D WallCheck;
        
        [Header("Layer Masks")]
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsWall;

        // Controls for stopping and restarting movement
        [Header("Idle")]
        [Tooltip("Set chance to stop to 100 if NPC is supposed to remain in idle state")]
        public float chanceToStop;
        public float minTimeBeforeNextIdle;
        public float maxTimeBeforeNextIdle;
        public float minIdleTime;
        public float maxIdleTime;
        protected float TimeSinceLastIdle;
        protected float TimeBeforeNextIdle;
        
        // Cached references
        protected Rigidbody2D Rb;
        private Transform _groundCheck;
        private IEnumerator _idle;
        private IEnumerator _patrol;
        private IEnumerator _attack;
        private IEnumerator _retreat;
        private IEnumerator _die;

        private void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            _groundCheck = transform.GetChild(1).transform;
            
            SetRotationAndSpeed();
        }
        
        protected virtual void FixedUpdate()
        {
            CheckForGround();
            CheckForWall();
            CheckActiveState();
        }
    
        private void CheckActiveState()
        {
            if (CurrentState == State.Idle && _idle == null)
            {
                StopAllCoroutines();
                _idle = Idle();
                _patrol = null;
                _attack = null;
                _retreat = null;
                StartCoroutine(_idle);
            }
            else if (CurrentState == State.Patrol && _patrol == null)
            {
                StopAllCoroutines();
                _idle = null;
                _patrol = Patrol();
                _attack = null;
                _retreat = null;
                StartCoroutine(_patrol);
            }
            else if (CurrentState == State.Attack && _attack == null)
            {
                StopAllCoroutines();
                _idle = null;
                _patrol = null;
                _attack = Attack();
                _retreat = null;
                StartCoroutine(_attack);
            }
            else if (CurrentState == State.Retreat && _retreat == null)
            {
                StopAllCoroutines();
                _idle = null;
                _patrol = null;
                _attack = null;
                _retreat = Retreat();
                StartCoroutine(_retreat);
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
            WallCheckDirection = direction;
        }

        protected void CheckForGround()
        {
            GroundCheck = 
                Physics2D.Raycast(_groundCheck.position, GroundCheckDirection, groundCheckRaycastDist, whatIsGround);

            if (!GroundCheck)
            {
                Flip();
            }
        }

        protected void CheckForWall()
        {
            WallCheck =
                Physics2D.Raycast(_groundCheck.position, WallCheckDirection, groundCheckRaycastDist, whatIsWall);
        
            if (WallCheck)
            {
                Flip();
            }
        }

        protected void Flip()
        {
            facingRight = !facingRight;
            SetRotationAndSpeed();
        }

        // Override this to change Idle state
        protected virtual IEnumerator Idle()
        {
            while (CurrentState == State.Idle)
            {
                if (chanceToStop >= 100)
                {
                    yield return null;
                }
                else
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
    
        // Override this to change patrol state
        protected virtual IEnumerator Patrol()
        {
            while (CurrentState == State.Patrol)
            {
                var stopChance = Random.Range(0f, 100f);
            
                if (stopChance < chanceToStop && TimeSinceLastIdle > TimeBeforeNextIdle)
                {
                    CurrentState = State.Idle;
                }
                else
                {
                    TimeSinceLastIdle += Time.deltaTime;
                }

                Rb.velocity = new Vector2(HorizSpeed, 0);

                yield return null;
            }
        }

        protected virtual IEnumerator Attack()
        {
            // Set up attack logic
            yield return null;
        }

        protected virtual IEnumerator Retreat()
        {
            // Set up retreat logic
            yield return null;
        }
    }
}
