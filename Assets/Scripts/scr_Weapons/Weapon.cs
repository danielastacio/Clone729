using System;
using System.Collections;
using scr_Management.Management_Events;
using Unity.VisualScripting;
using UnityEngine;

namespace scr_Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject barrel;

        //Inspector Dudes/Variables
        [Header("Debug")] [SerializeField] [Tooltip("if gun isn't looking at cursor keep increasing offset")]
        protected int offset;

        [Header("Stats")] [SerializeField] protected float damage = 1;
        [SerializeField] protected float speed = 5;
        [SerializeField] protected float fireRate = 0.5f;
        private float _currentTime = 0f;
        [SerializeField] protected GameObject bullet;

        //Private Dudes/Variables
        protected Vector3 difference;
        protected float angle;
        protected bool canShoot = true;

        private void OnEnable()
        {
            Actions.OnShootPressed += Shoot;
        }

        private void Awake()
        {
            _currentTime = fireRate;
        }

        protected virtual void Update()
        {
            LookAtCursor();
            RotateAroundMech();
            ShotTimer();
        }

        public void LookAtCursor()
        {
            // Get Direction and Normalize It
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();
            //Get Rotation number
            angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            //Rotate by said amount
            transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
        }

        protected void RotateAroundMech()
        {
            //set Position around the mech by taking which direction it's pointing at
            transform.position = transform.parent.position + difference;
        }

        private void Shoot(bool shootInput)
        {
            if (shootInput && canShoot)
            {
                canShoot = false;
                _currentTime = 0f;
                InstantiateBullet();
            }
        }

        private void ShotTimer()
        {
            if (_currentTime <= fireRate && !canShoot)
            {
                _currentTime += Time.deltaTime;
            }
            else
            {
                canShoot = true;
            }
        }
        
        protected virtual void InstantiateBullet()
        {
            var newBullet = Instantiate(bullet, barrel.transform.position, Quaternion.Euler(0f, 0f, angle + offset));
            newBullet.GetComponent<BulletScript>().CreateBullet("Enemy", damage, speed);
        }
    }
}
