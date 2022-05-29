using System;
using System.Collections;
using scr_Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace scr_NPCs.scr_Enemies
{
    // TODO: Fix CheckFacingPlayer(). Enemy either faces the wrong way, or flips forever.
    // TODO: Figure out how to keep enemy from running off platforms when retreating
    // TODO: Make sure enemy is facing the player before trying to retreat, otherwise it retreats into the player...
    public class Enemy : NPCController, IDamageable
    {
        [Header("Stats")] 
        public float maxHp;
        private float _currentHp;
        public float touchDamage; // Damage when player bumps the enemy
        public float attackDamage;
        
        [Header("Combat")]
        [SerializeField] private float sightRange;
        [SerializeField] private float attackRange;
        [SerializeField] private float retreatRange;
        
        [Header("Enemy Layer Masks")]
        [SerializeField] protected LayerMask whatIsPlayer;
        
        // Player Checks
        private Collider2D _playerInSightRange;
        private Collider2D _playerInAttackRange;
        private Collider2D _playerInRetreatRange;
        private bool _playerSpotted;
        private RaycastHit2D _playerSpottingRay;

        protected Vector3 PlayerPos;
        
        protected virtual void Start()
        {
            _currentHp = maxHp;
        }

        private void Update()
        {
            CheckForPlayer();
        }
        
        protected void CheckForPlayer()
        {
            _playerSpottingRay =
                Physics2D.Raycast(transform.position, WallCheckDirection, sightRange, whatIsPlayer);
            _playerInSightRange = 
                Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer);
            _playerInAttackRange =
                Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
            _playerInRetreatRange =
                Physics2D.OverlapCircle(transform.position, retreatRange, whatIsPlayer);

            if (_playerSpottingRay)
            {
                _playerSpotted = true;
            }
            
            if (_playerInSightRange && _playerSpotted)
            {
                Debug.Log("Player spotted!");
                PlayerPos = _playerInSightRange.transform.position;
            
                if (_playerInRetreatRange)
                {
                    CurrentState = State.Retreat;
                    Debug.Log("Run Away!");
                }
                else if (_playerInAttackRange)
                {
                    CurrentState = State.Attack;
                    Debug.Log("Attacking!");
                }
            }
            else
            {
                _playerSpotted = false;
                CurrentState = State.Patrol;
                Debug.Log("Where'd he go?");
            }
        }

        public void TakeDamage(float damage)
        {
            _currentHp -= damage;
            if (_currentHp <= 0)
            {
                CurrentState = State.Die;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
                damageable.TakeDamage(touchDamage);
                Flip();
            }
        }

        protected void CheckFacingPlayer()
        {
            if (PlayerPos.x > transform.position.x && !facingRight)
            {
                Flip();
            }
            else if (PlayerPos.x < transform.position.x && facingRight)
            {
                Flip();
            }
        }

        protected override IEnumerator Attack()
        {
            while (CurrentState == State.Attack)
            {
                CheckFacingPlayer();
                
                // Set up attack logic.

                yield return null;
            }
        }

        protected override IEnumerator Retreat()
        {
            Flip();
            while (CurrentState == State.Retreat && _playerInRetreatRange)
            {
                CheckForGround();
                CheckForWall();
                Rb.velocity = new Vector2(HorizSpeed, 0);
                yield return null;
            }
            CheckFacingPlayer();
        }
    }
}
