using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class BulletScript : MonoBehaviour
{
    [HideInInspector] public float bulletSpeed;
    [HideInInspector] public  Vector3 direction;
    [HideInInspector] public float bulletDamage;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

            
       rb.AddRelativeForce(Vector3.right * bulletSpeed);
    
       

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       
         if (other.gameObject.CompareTag("Enemy"))
         {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable.TakeDamage(bulletDamage);
            Destroy(gameObject);
         }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
        
     
    }
}

