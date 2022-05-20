using System;
using System.Collections;
using scr_Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace scr_NPCs.scr_Enemies
{
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

        private Vector3 _playerPos;
        
        private void Start()
        {
            _currentHp = maxHp;
        }

        private void Update()
        {
            Debug.DrawRay(transform.position, WallCheckDirection * sightRange, Color.blue);
            CheckForPlayer();
        }
        
        protected void CheckForPlayer()
        {
            _playerInSightRange = 
                Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer);
            _playerInAttackRange =
                Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
            _playerInRetreatRange =
                Physics2D.OverlapCircle(transform.position, retreatRange, whatIsPlayer);

            
            if (_playerInSightRange)
            {
                Debug.Log("Player spotted!");
                _playerPos = _playerInSightRange.transform.position;
            
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

        protected override IEnumerator Attack()
        {
            while (CurrentState == State.Attack)
            {
                var facingPlayer =
                    Physics2D.Raycast(transform.position, WallCheckDirection, sightRange, whatIsPlayer);

                if (!facingPlayer)
                {
                    Flip();
                }
                else
                {
                    // Set up attack logic.
                }

                yield return null;
            }
        }

        protected override IEnumerator Retreat()
        {
            Flip();
            while (CurrentState == State.Retreat)
            {
                while (_playerInRetreatRange)
                {
                    Rb.velocity = new Vector2(HorizSpeed, 0);
                    yield return null;
                }
            }
        }
    }
}
