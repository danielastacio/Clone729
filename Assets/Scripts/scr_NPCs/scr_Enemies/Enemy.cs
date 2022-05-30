using System;
using System.Collections;
using scr_Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace scr_NPCs.scr_Enemies
{
    // TODO: Figure out how to keep enemy from running off platforms when retreating
    // TODO: Make sure enemy is facing the player before trying to retreat, otherwise it retreats into the player...
    public class Enemy : NPCController, IDamageable
    {
        [Header("Stats")] 
        [SerializeField] protected float maxHp;
        private float _currentHp;
        [SerializeField] protected float touchDamage; // Damage when player bumps the enemy
        [SerializeField] protected float attackDamage;
        [SerializeField] protected float timeBetweenAttacks;

        [Header("Combat")] 
        [SerializeField] private int itemDropChance;
        [SerializeField] private float sightRange;
        [SerializeField] private float attackRange;
        [SerializeField] private float retreatRange;
        private float _currentRetreatRange;
        
        [Header("Enemy Layer Masks")]
        [SerializeField] protected LayerMask whatIsPlayer;
        
        // Player Checks
        private Collider2D _playerInSightRange;
        private Collider2D _playerInAttackRange;
        private Collider2D _playerInRetreatRange;
        private bool _playerSpotted;

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
            _playerInSightRange = 
                Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer);
            _playerInAttackRange =
                Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
            _playerInRetreatRange =
                Physics2D.OverlapCircle(transform.position, _currentRetreatRange, whatIsPlayer);

            PlayerSpottingRaycast();

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
                _currentRetreatRange = retreatRange;
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
                other.gameObject.GetComponent<IDamageable>().TakeDamage(touchDamage);
            }
        }

        protected void CheckFacingPlayer()
        {
            if ((PlayerPos.x > transform.position.x && !facingRight)
                || (PlayerPos.x < transform.position.x && facingRight))
            {
                Flip();
            }
        }

        private void PlayerSpottingRaycast()
        {
            var playerSpottingRay =
                Physics2D.Raycast(transform.position, WallCheckDirection, sightRange, whatIsPlayer);
            
            if (playerSpottingRay)
            {
                _playerSpotted = true;
            }

            if (playerSpottingRay && CurrentState == State.Retreat)
            {
                Flip();
            }
        }

        protected override IEnumerator Attack()
        {
            while (CurrentState == State.Attack)
            {
                // Set up attack logic.

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        protected override IEnumerator Retreat()
        {
            while (CurrentState == State.Retreat && _playerInRetreatRange)
            {
                PlayerSpottingRaycast();
                if (!GroundCheck || WallCheck)
                {
                    _currentRetreatRange = 0;
                }
                Rb.velocity = new Vector2(HorizSpeed, 0);
                yield return null;
            }
            CheckFacingPlayer();
        }

        protected override void Die()
        {
            GetComponent<EnemyDropTable>().CalculateDropChance(itemDropChance);
            Destroy(gameObject);
        }
    }
}
