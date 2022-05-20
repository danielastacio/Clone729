using scr_Interfaces;
using UnityEngine;

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
        
        // Raycasts
        private RaycastHit2D _playerInSightRange;
        private RaycastHit2D _playerInAttackRange;
        private RaycastHit2D _playerInRetreatRange;
        
        private void Start()
        {
            _currentHp = maxHp;
        }

        private void Update()
        {
            CheckForPlayer();
            Debug.DrawRay(transform.position, WallCheckDirection * sightRange, Color.blue);
            Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), WallCheckDirection * attackRange, Color.red);
            Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), WallCheckDirection * retreatRange, Color.green);
        }
        
        protected void CheckForPlayer()
        {
            _playerInSightRange = 
                Physics2D.Raycast(transform.position, WallCheckDirection, sightRange, whatIsPlayer);
            _playerInAttackRange =
                Physics2D.Raycast(transform.position, WallCheckDirection, attackRange, whatIsPlayer);
            _playerInRetreatRange =
                Physics2D.Raycast(transform.position, WallCheckDirection, retreatRange, whatIsPlayer);

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
            else if (_playerInSightRange)
            {
                Debug.Log("Player spotted!");
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
    }
}
