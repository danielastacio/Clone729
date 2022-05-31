using System;
using System.Collections;
using System.Collections.Generic;
using scr_Consumables;
using scr_Interfaces;
using UnityEngine;

namespace MetroidvaniaJam.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        public float maxHp = 100;
        public float currentHp;

        [Header("Speed and Force")]

        [SerializeField] private float speed = 0;
        [SerializeField] protected internal float defaultSpeed = 10;
        [SerializeField] private float crouchSpeed = 5;
        [SerializeField] protected internal float jumpForce = 25;
        [SerializeField] protected internal float fallMultiplier = 7;
        [SerializeField] private float rollForce = 20;
        [SerializeField] private float rollTime;
        [SerializeField] private float defaultRollTime = 0.5f;

        [Header("Ground Check")]

        [SerializeField] protected internal float groundCheckRadius = 0.1f;
        [SerializeField] protected internal float offsetRadius = -1f;
        [SerializeField] private LayerMask whatIsGround;
        private Vector2 groundCheckPos;

        private float
            playerHeight,
            crouchHeight;

        protected internal bool
            isGrounded,
            isCrouching,
            isRolling;

        private bool
            isInputJump,
            isInputCrouch,
            isInputRoll,
            isInputMoveLeft,
            isInputMoveRight,
            isInputSwitchPlayer;
        

        private bool isFacingLeft;

        private bool
            isInsideMech,
            isReadyForMech,
            isCollidingWithMech;

        protected internal Rigidbody2D rb;

        private MechController mechController;
        protected virtual void SetRigidbodySettings()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 10;
            rb.freezeRotation = true;            
        }

        private void SetPlayerSettings()
        {
            playerHeight = transform.localScale.y;
            crouchHeight = playerHeight / 2;
            rollTime = defaultRollTime;

            speed = defaultSpeed;

            isFacingLeft = false;
        }

        private void CheckIfGrounded()
        {
            groundCheckPos = new Vector2(transform.position.x, transform.position.y + offsetRadius);
            var groundCheck =
                Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, whatIsGround);

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

        }

        private void FixedUpdate()
        {
            CheckIfGrounded();
            Move();
            Jump();
            Crouch();
            Roll();
            SwitchToMechAndBack();

        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundCheckPos, groundCheckRadius);
        }
        #endregion
        #region Triggers
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Consumable"))
            {
                CheckConsumablePickup(collider);
            }
        }
        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<MechController>())
            {
                isCollidingWithMech = true;
                mechController = collision.gameObject.GetComponent<MechController>();
            }
            if (!isInsideMech && isCollidingWithMech)
            {
                isReadyForMech = true;
            }

        }
        protected virtual void OnCollisionExit2D(Collision2D collision)
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
            isInputSwitchPlayer = Input.GetKeyDown(KeyCode.LeftShift);
            if (isInputSwitchPlayer)
            {
                if (isReadyForMech)
                {
                    ActivateMech();
                }

                else if (isInsideMech)
                {
                    DeactivateMech();
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

                if (transform.localScale.y != crouchHeight)
                {
                    transform.localScale = new Vector2(transform.localScale.x, crouchHeight);
                }
                speed = crouchSpeed;
            }
            else
            {
                transform.localScale = new Vector2(transform.localScale.x, playerHeight);
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
                    transform.localScale = new Vector2(transform.localScale.x, crouchHeight);
                }
            }

            else if (!isCrouching)
            {
                transform.localScale = new Vector2(transform.localScale.x, playerHeight);
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

        private void SwitchToMechAndBack()
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