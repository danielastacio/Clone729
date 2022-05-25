using System;
using System.Collections;
using UnityEngine;

namespace scr_NPCs.scr_Enemies
{
    // TODO: Override attack function for ranged attacks.
    // TODO: Set up prefab for bullet
    // TODO: Set up delay between shots
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

                yield return new WaitForSeconds(timeBetweenAttacks);
                
                FireBullet();
            }
        }

        private void FireBullet()
        {
            var newBullet = Instantiate(bullet, _gun);
            var bulletScript = newBullet.GetComponent<Bullet>();
            bulletScript.CreateBullet("Player", PlayerPos, attackDamage);
        }
    }
}
