using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
namespace scr_Enemies
{
    public class Enemy : NPCController, IDamageable
    {
        [Header("Stats")] 
        public float maxHp;
        private float _currentHp;
        public float attackDamage;



         void Start()
        {
            _currentHp = maxHp;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("current health is"+ _currentHp);
            _currentHp = _currentHp - damage;
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
                damageable.TakeDamage(attackDamage);
            }
        }
        public void SetHealth()
        {
            _currentHp = maxHp;
        }
    }
}
