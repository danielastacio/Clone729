using scr_Consumables;
using scr_Interfaces;
using scr_UI;
using UnityEngine;

namespace scr_Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [Header("Stats")] public float maxHp = 100;
        public float currentHp;

        [Header("Speed and Force")] [SerializeField]
        private float speed = 0;

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
            isRolling;

        private bool
            isInputJump,
            isInputCrouch,
            isInputRoll,
            isInputMoveLeft,
            isInputMoveRight;

        private bool isFacingLeft;

        private bool
            isInsideMech,
            isReadyForMech,
            isCollidingWithMech,
            isPlayerLaunched;

        protected internal Rigidbody2D rb;
        private MechController mechController;
        
        #region MonoBehavior Cycles

        private void Awake()
        {
            SetRigidbodySettings();
            SetPlayerSettings();
            currentHp = maxHp;
        }

        private void Update()
        {
            CheckRollInput();
            CheckCrouchInput();
            CheckJumpInput();
            CheckMoveInput();
            CheckMechInput();
            CheckPauseInput();
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

        public void Heard()
        {
            print("switched");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_groundCheckPos, groundCheckRadius);
        }

        #endregion


        protected virtual void SetRigidbodySettings()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 10;
            rb.freezeRotation = true;
        }

        private void SetPlayerSettings()
        {
            _playerHeight = transform.localScale.y;
            _crouchHeight = _playerHeight / 2;
            rollTime = defaultRollTime;

            speed = defaultSpeed;

            isFacingLeft = false;
        }

        private void CheckIfGrounded()
        {
            _groundCheckPos = new Vector2(transform.position.x, transform.position.y + offsetRadius);
            var groundCheck =
                Physics2D.OverlapCircle(_groundCheckPos, groundCheckRadius, whatIsGround);

            if (groundCheck)
            {
                isGrounded = true;
            }
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
                isCollidingWithMech = true;
                mechController = col.gameObject.GetComponent<MechController>();
            }

            if (!isInsideMech && isCollidingWithMech)
            {
                isReadyForMech = true;
            }
        }

        protected virtual void OnCollisionExit2D(Collision2D col)
        {
            if (!isInsideMech && isCollidingWithMech)
            {
                isReadyForMech = false;
                isCollidingWithMech = false;
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
                if (isReadyForMech)
                {
                    ActivateMech();
                }

                else if (isInsideMech)
                {
                    DeactivateMech();
                    isPlayerLaunched = true;
                }
            }
        }

        protected virtual void CheckCrouchInput()
        {
            isInputCrouch = Input.GetKey(KeyCode.S);
        }

        protected virtual void CheckRollInput()
        {
            if (!isCrouching)
            {
                if (Input.GetKeyDown(KeyCode.K))
                {
                    isInputRoll = true;
                }
            }

            if (isRolling)
            {
                rollTime -= Time.deltaTime;
                if (rollTime <= 0)
                {
                    rollTime = 0;
                    isRolling = false;
                    isInputRoll = false;
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
                isInputMoveLeft = Input.GetKey(KeyCode.A);
                isInputMoveRight = Input.GetKey(KeyCode.D);
            }
        }

        private void CheckJumpInput()
        {
            isInputJump = Input.GetKey(KeyCode.Space);
        }

        private void CheckPauseInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !Menu.Paused)
            {
                GameObject.FindGameObjectWithTag("Menu").GetComponent<Menu>().PauseGame();
            }
        }

        #endregion

        #region Rigidbody

        protected void FreezeRigidBody()
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }


        protected void UnFreezeRigidBody()
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.freezeRotation = true;
        }

        #endregion

        #region Movement Methods

        protected virtual void Move()
        {
            if (isInputMoveLeft)
            {
                isFacingLeft = true;

                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }

            else if (isInputMoveRight)
            {
                isFacingLeft = false;

                rb.velocity = new Vector2(speed, rb.velocity.y);
            }

            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        private void Jump()
        {
            if (isInputJump && isGrounded && !isRolling)
            {
                isGrounded = false;
                rb.velocity = Vector2.up * jumpForce;
            }
            else if (rb.velocity.y < 0 && !isInputJump)
            {
                rb.velocity += (fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            }
        }

        private void Crouch()
        {
            bool canCrouch = isInputCrouch && isGrounded && !isRolling;

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
            Vector2 rollDirection = isFacingLeft ? Vector2.left : Vector2.right;
            bool canRoll = isInputRoll && isGrounded && !isCrouching;

            if (canRoll)
            {
                isRolling = true;

                if (isRolling)
                {
                    rb.AddForce(rollDirection * rollForce, ForceMode2D.Impulse);
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
            if (isPlayerLaunched)
            {
                rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);

                isPlayerLaunched = false;
            }
        }

        #endregion

        #region Mech Methods

        private void ActivateMech()
        {
            isReadyForMech = false;
            isInsideMech = true;

            rb.GetComponent<CapsuleCollider2D>().isTrigger = true;

            mechController.enabled = true;
            mechController.rb.constraints = RigidbodyConstraints2D.None;
            mechController.rb.freezeRotation = true;
        }

        private void DeactivateMech()
        {
            mechController.rb.constraints = RigidbodyConstraints2D.None;
            mechController.rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            mechController.rb.freezeRotation = true;

            rb.GetComponent<CapsuleCollider2D>().isTrigger = false;
            mechController.enabled = false;
            isReadyForMech = false;
            isInsideMech = false;
        }

        private void UpdatePlayerPosition()
        {
            if (isInsideMech)
            {
                transform.position = mechController.transform.position;
                rb.Sleep();
            }
        }

        #endregion
    }
}