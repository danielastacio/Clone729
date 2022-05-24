using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [HideInInspector] public float bulletSpeed;
    [HideInInspector] public  Vector3 direction;
    [HideInInspector] public float bulletDamage;
    private Rigidbody2D rb;
    private float timebeforedeletion = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

            Destroy(this);
        rb.AddForce(direction * bulletSpeed);
    
       

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Player")
            Debug.Log("Something Hit!");
           Destroy(collision.gameObject);
            Destroy(gameObject);
      
    }
}
