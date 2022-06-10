using scr_Consumables;
using scr_Interfaces;
using scr_UI.scr_PauseMenu;
using System.Collections;
using UnityEngine;

namespace scr_Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        public static PlayerController Instance { get; private set; }

        [Header("Stats")] public float maxHp = 100;
        public float currentHp;

        [Header("Speed and Force")] [SerializeField]
        private float speed;

        [SerializeField] protected internal float defaultSpeed = 10;
        [SerializeField] private float crouchSpeed = 5;
        [SerializeField] protected internal float jumpForce = 25;
        [SerializeField] protected internal float launchForce = 45;
        [SerializeField] protected internal float fallMultiplier = 7;
        [SerializeField] private float rollForce = 20;
        [SerializeField] private float rollTime;
        [SerializeField] private float defaultRollTime = 0.5f;

        [Header("Ground Check")] [SerializeField]
        protected internal float groundCheckRadius = 0.1f;

        [SerializeField] protected internal float offsetRadius = -1f;
        [SerializeField] private LayerMask whatIsGround;
        private Vector2 _groundCheckPos;

        private float
            _playerHeight,
            _crouchHeight;

        protected internal bool
            isGrounded,
            isCrouching,
            isRolling,
            isRunning,
            isJumping,
            isMeleeing;

        private bool
            _isInputJump,
            _isInputCrouch,
            _isInputRoll,
            _isInputMoveLeft,
            _isInputMoveRight;

        private bool _isFacingLeft;

        private bool
            _isInsideMech,
            _isReadyForMech,
            _isCollidingWithMech,
            _isPlayerLaunched;

        protected Rigidbody2D Rb;
        private MechController _mechController;
        private SpriteRenderer _sprite;
        private Animator _animator;

        [Header("Animation Timeouts")]
        public float meleeDuration;
        private WaitForSeconds _meleeTimeout;

        #region MonoBehavior Cycles

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;
            SetRigidbodySettings();
            SetPlayerSettings();            
        }

        private void Update()
        {
            CheckRollInput();
            CheckCrouchInput();
            CheckJumpInput();
            CheckMoveInput();
            CheckMechInput();
            CheckPauseInput();
            CheckAnimationState();
        }

        private void FixedUpdate()
        {
            CheckIfGrounded();
            Move();
            Jump();
            Crouch();
            Roll();
            UpdatePlayerPosition();
            LaunchPlayer();
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_groundCheckPos, groundCheckRadius);
        }

        #endregion


        protected virtual void SetRigidbodySettings()
        {
            Rb = GetComponent<Rigidbody2D>();
            Rb.gravityScale = 10;
            Rb.freezeRotation = true;
        }

        private void SetPlayerSettings()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            _playerHeight = transform.localScale.y;
            _crouchHeight = _playerHeight / 2;
            _isFacingLeft = false;
            _meleeTimeout = new WaitForSeconds(meleeDuration);

            meleeDuration = 0.3f;
            rollTime = defaultRollTime;
            speed = defaultSpeed;
            currentHp = maxHp;
        }

        private void CheckIfGrounded()
        {
            _groundCheckPos = new Vector2(transform.position.x, transform.position.y + offsetRadius);
            var groundCheck =
                Physics2D.OverlapCircle(_groundCheckPos, groundCheckRadius, whatIsGround);

            if (groundCheck)
            {
                isGrounded = true;
                isJumping = false;
            }
        }

        private void CheckAnimationState()
        {
            _animator.SetBool("isRunning", isRunning && !isCrouching);
            _animator.SetBool("isJumping", isJumping && !isGrounded);
            _animator.SetBool("isCrouching", isCrouching);
            _animator.SetBool("isRolling", isRolling);

            if (Input.GetKeyDown(KeyCode.L) && !isMeleeing)
            {
                StartCoroutine(MeleeAttack());
            }
            
        } 
        protected IEnumerator MeleeAttack()
        {
            isMeleeing = true;
            _animator.SetTrigger("Melee");
            yield return _meleeTimeout;
            isMeleeing = false;
        }
        
        public void TakeDamage(float damage)
        {
            currentHp -= damage;

            if (currentHp <= 0)
            {
                Destroy(gameObject);
                // Play death animation

                // Reset level on death
                //SceneManager.LoadScene(1);

                currentHp = 0;
            }
        }


        #region Triggers

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Consumable"))
            {
                CheckConsumablePickup(col);
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<MechController>())
            {
                _isCollidingWithMech = true;
                _mechController = col.gameObject.GetComponent<MechController>();
            }

            if (!_isInsideMech && _isCollidingWithMech)
            {
                _isReadyForMech = true;
            }
        }

        protected virtual void OnCollisionExit2D(Collision2D col)
        {
            if (!_isInsideMech && _isCollidingWithMech)
            {
                _isReadyForMech = false;
                _isCollidingWithMech = false;
            }
        }

        #endregion

        #region Trigger Checks

        private void CheckConsumablePickup(Collider2D consumable)
        {
            if (consumable.GetComponent<HealthConsumable>() && currentHp <= maxHp)
            {
                RestoreHP(consumable.GetComponent<HealthConsumable>().ConsumeItem());
            }
        }

        #endregion

        #region Restoration Methods

        private void RestoreHP(float restoreAmount)
        {
            currentHp += restoreAmount;
            if (currentHp >= maxHp)
            {
                currentHp = maxHp;
            }
        }

        #endregion

        #region Inputs

        protected virtual void CheckMechInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_isReadyForMech)
                {
                    ActivateMech();
                }

                else if (_isInsideMech)
                {
                    DeactivateMech();
                    _isPlayerLaunched = true;
                }
            }
        }

        protected virtual void CheckCrouchInput()
        {
            _isInputCrouch = Input.GetKey(KeyCode.S);
        }

        protected virtual void CheckRollInput()
        {
            if (!isCrouching)
            {
                if (Input.GetKeyDown(KeyCode.K))
                {
                    _isInputRoll = true;
                }
            }

            if (isRolling)
            {
                rollTime -= Time.deltaTime;
                if (rollTime <= 0)
                {
                    rollTime = 0;
                    isRolling = false;
                    _isInputRoll = false;
                }
            }

            else
            {
                rollTime = defaultRollTime;
            }
        }

        private void CheckMoveInput()
        {
            if (!isRolling)
            {
                _isInputMoveLeft = Input.GetKey(KeyCode.A);
                _isInputMoveRight = Input.GetKey(KeyCode.D);
            }
        }

        private void CheckJumpInput()
        {
            _isInputJump = Input.GetKey(KeyCode.Space);
        }

        private void CheckPauseInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !PauseMenuCanvas.Paused)
            {
                GameObject.FindGameObjectWithTag("Menu").GetComponent<PauseMenuCanvas>().PauseGame();
            }
        }

        #endregion

        #region Rigidbody

        protected void FreezeRigidBody()
        {
            Rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }


        protected void UnFreezeRigidBody()
        {
            Rb.constraints = RigidbodyConstraints2D.None;
            Rb.freezeRotation = true;
        }

        #endregion

        #region Movement Methods

        protected virtual void Move()
        {
            if (_isInputMoveLeft)
            {
                _sprite.flipX = true;
                _isFacingLeft = true;
                isRunning = true;

                Rb.velocity = new Vector2(-speed, Rb.velocity.y);
            }

            else if (_isInputMoveRight)
            {
                _sprite.flipX = false;
                _isFacingLeft = false;
                isRunning = true;

                Rb.velocity = new Vector2(speed, Rb.velocity.y);
            }

            else
            {
                isRunning = false;
                Rb.velocity = new Vector2(0, Rb.velocity.y);
            }
        }

        private void Jump()
        {
            if (_isInputJump && isGrounded && !isRolling)
            {
                isGrounded = false;
                isJumping = true;
                Rb.velocity = Vector2.up * jumpForce;
            }
            else if (Rb.velocity.y < 0 && !_isInputJump)
            {
                Rb.velocity += (fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            }
        }

        private void Crouch()
        {
            bool canCrouch = _isInputCrouch && isGrounded && !isRolling;

            if (canCrouch)
            {
                isCrouching = true;

                if (transform.localScale.y != _crouchHeight)
                {
                    transform.localScale = new Vector2(transform.localScale.x, _crouchHeight);
                }

                speed = crouchSpeed;
            }
            else
            {
                transform.localScale = new Vector2(transform.localScale.x, _playerHeight);
                speed = defaultSpeed;

                isCrouching = false;
            }
        }

        private void Roll()
        {
            Vector2 rollDirection = _isFacingLeft ? Vector2.left : Vector2.right;
            bool canRoll = _isInputRoll && isGrounded && !isCrouching;

            if (canRoll)
            {
                isRolling = true;

                if (isRolling)
                {
                    Rb.AddForce(rollDirection * rollForce, ForceMode2D.Impulse);
                    transform.localScale = new Vector2(transform.localScale.x, _crouchHeight);
                }
            }

            else if (!isCrouching)
            {
                transform.localScale = new Vector2(transform.localScale.x, _playerHeight);
            }
        }

        public void LaunchPlayer()
        {
            if (_isPlayerLaunched)
            {
                Rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);

                _isPlayerLaunched = false;
            }
        }

        #endregion

        #region Mech Methods

        private void ActivateMech()
        {
            _isReadyForMech = false;
            _isInsideMech = true;

            Rb.GetComponent<CapsuleCollider2D>().isTrigger = true;

            _mechController.enabled = true;
            _mechController.Rb.constraints = RigidbodyConstraints2D.None;
            _mechController.Rb.freezeRotation = true;
        }

        private void DeactivateMech()
        {
            _mechController.Rb.constraints = RigidbodyConstraints2D.None;
            _mechController.Rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            _mechController.Rb.freezeRotation = true;

            Rb.GetComponent<CapsuleCollider2D>().isTrigger = false;
            _mechController.enabled = false;
            _isReadyForMech = false;
            _isInsideMech = false;
        }

        private void UpdatePlayerPosition()
        {
            if (_isInsideMech)
            {
                transform.position = _mechController.transform.position;
                Rb.Sleep();
            }
        }

        #endregion
    }
}