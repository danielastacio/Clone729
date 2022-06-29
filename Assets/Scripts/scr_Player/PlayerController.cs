using scr_Consumables;
using scr_Interfaces;
using System.Collections;
using scr_Management;
using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Player
{
    // TODO: No deadline- Tidy this script up a little bit. Make it a bit easier to navigate through. 
    public class PlayerController : MonoBehaviour, IDamageable, IDataPersistence
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
        
        // TODO: Set up ability to jump through certain platforms
        // Search for JumpThrough()
        [Header("Ceiling Check")] [SerializeField]
        protected internal float ceilingCheckRadius = 0.1f;
        [SerializeField] protected internal float ceilingOffsetRadius = 1f;
        private Vector2 _ceilingCheckPos;
        
        [SerializeField] protected float interactRange = 5f;

        private bool _canRoll;

        
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

        private void OnEnable()
        {
            Actions.OnMoveInput += Move;
            Actions.OnJumpPressed += Jump;
            Actions.OnCrouchPressed += Crouch;
            Actions.OnRollPressed += StartRoll;
            Actions.OnMeleePressed += StartMeleeAttack;
            Actions.OnInteractPressed += Interact;
        }

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

        private void Start()
        {
            // LEAVE THIS FOR DEBUG/BUILDING PURPOSES.
            // When starting game from Main Menu, controller type will be set to Gameplay
            Actions.OnControllerChanged(ControllerType.Gameplay);
        }

        private void Update()
        {
            CheckMechInput();
            CheckAnimationState();
        }

        private void FixedUpdate()
        {
            CheckIfGrounded();
            Roll();
            Fall();
            // Instead of UpdatePlayerPosition, set player to child of mech
            UpdatePlayerPosition();
            LaunchPlayer();
        }

        #endregion

        public void LoadData(GameData data)
        {
            currentHp = data.playerCurrentHp;
            transform.position = data.playerSpawnPoint;
        }

        public void SaveData(GameData data)
        {
            data.playerCurrentHp = currentHp;
            data.playerSpawnPoint = transform.position;
        }

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
            var groundCheckPos = new Vector2(transform.position.x, transform.position.y + offsetRadius);
            isGrounded =
                Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, whatIsGround);

            isJumping = !isGrounded;
        }

        private void CheckAnimationState()
        {
            _animator.SetBool("isRunning", isRunning && !isCrouching);
            _animator.SetBool("isJumping", isJumping && !isGrounded);
            _animator.SetBool("isCrouching", isCrouching);
            _animator.SetBool("isRolling", isRolling);

        }

        private void StartMeleeAttack(bool meleePressed)
        {
            if (meleePressed && !isMeleeing)
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
            // TODO: Give mech IInteractable interface, entering mech will happen on "E", exit on "LEFT SHIFT"
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

        private void Interact(bool interactInput)
        {
            // Change interact with NPC's to use the NPC circle
            if (interactInput)
            {
                var direction = _isFacingLeft ? Vector2.left : Vector2.right;
                var interactRay =
                    Physics2D.Raycast(transform.position, direction, interactRange);

                if (interactRay)
                {
                    interactRay.transform.gameObject.GetComponent<IInteractable>().OnInteract();
                }
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

        protected virtual void Move(float moveInput)
        {
            if (moveInput > 0 && !isRolling)
            {
                _sprite.flipX = false;
                _isFacingLeft = false;
                isRunning = true;

                Rb.velocity = new Vector2(speed * moveInput, Rb.velocity.y);
            }
            else if (moveInput < 0 && !isRolling)
            {
                _sprite.flipX = true;
                _isFacingLeft = true;
                isRunning = true;

                Rb.velocity = new Vector2(speed * moveInput, Rb.velocity.y);
            }
            else
            {
                isRunning = false;
                Rb.velocity = new Vector2(0, Rb.velocity.y);
            }
        }

        private void Jump(bool jumpInput)
        {
            // Figure out why we can double jump
            if (jumpInput && isGrounded && !isRolling)
            {
                isJumping = true;
                Rb.velocity = Vector2.up * jumpForce;
            }
        }

        private void Fall()
        {
            if (Rb.velocity.y < 0 && !isGrounded)
            {
                Rb.velocity += (fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            }
        }

        private void JumpThrough()
        {
            // Set up parameters for a ceiling check circle. Similar checks to the ground check circle
            // When jumping through, disable player collider until the ground check circle passes through the platform,
            // then turn collider back on
            // Build a drop through method after this.
        }

        private void Crouch(bool crouchInput)
        {
            bool canCrouch = crouchInput && isGrounded && !isRolling;

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

        private void StartRoll(bool rollInput)
        {
            if (rollInput)
            {
                _canRoll = isGrounded && !isCrouching;
                isRolling = true;
            }
            
            else if (!isCrouching)
            {
                transform.localScale = new Vector2(transform.localScale.x, _playerHeight);
            }
        }

        private void Roll()
        {
            if (_canRoll)
            {
                Vector2 rollDirection = _isFacingLeft ? Vector2.left : Vector2.right;
                if (isRolling)
                {
                    rollTime -= Time.deltaTime;
                    if (rollTime <= 0)
                    {
                        rollTime = 0;
                        isRolling = false;
                    }

                    Rb.AddForce(rollDirection * rollForce, ForceMode2D.Impulse);
                    transform.localScale = new Vector2(transform.localScale.x, _crouchHeight);
                }
                else
                {
                    rollTime = defaultRollTime;
                }
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