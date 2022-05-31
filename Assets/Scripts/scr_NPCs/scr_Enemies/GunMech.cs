using System;
using System.Collections;
using scr_NPCs.scr_Enemies.scr_EnemyUtilities;
using UnityEngine;

namespace scr_NPCs.scr_Enemies
{
    public class GunMech : Enemy
    {
        private Transform _gun;
        [SerializeField] private GameObject bullet;

        protected override void Start()
        {
            base.Start();
            _gun = transform.GetChild(2).transform;
        }

        protected override IEnumerator Attack()
        {
            while (CurrentState == State.Attack)
            {
                CheckFacingPlayer();
                Rb.Sleep();

                yield return new WaitForSeconds(timeBetweenAttacks);
                
                FireBullet();
            }
        }

        private void FireBullet()
        {
            var newBullet = Instantiate(bullet, _gun);
            var bulletScript = newBullet.GetComponent<EnemyBullet>();
            bulletScript.CreateBullet("Player", PlayerPos, attackDamage);
        }
    }
}
