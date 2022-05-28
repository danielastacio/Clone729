using System;
using System.Collections;
using System.Collections.Generic;
using scr_Interfaces;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public Vector3 targetPos;

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, step);
        if (Vector2.Distance(transform.position, targetPos) <= 0.01)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
