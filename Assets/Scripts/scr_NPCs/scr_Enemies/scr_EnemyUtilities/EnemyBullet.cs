using scr_Interfaces;
using UnityEngine;

namespace scr_NPCs.scr_Enemies.scr_EnemyUtilities
{
    public class EnemyBullet: MonoBehaviour
    {
        private string _target;
        private Vector3 _targetPos;
        [SerializeField] private float speed;
        private float _damage;
    
        private void Update()
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, step);
            if (Vector2.Distance(transform.position, _targetPos) <= 0.01)
            {
                Destroy(gameObject);
            }
        }

        public void CreateBullet(string targetTag, Vector3 targetPos, float damage)
        {
            _target = targetTag;
            _targetPos = targetPos;
            _damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(_target))
            {
                other.gameObject.GetComponent<IDamageable>().TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }
}
