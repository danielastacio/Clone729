using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scr_Interfaces;

public class CannonBomb : MonoBehaviour
{
    [HideInInspector]public float cannonDamage;
    [HideInInspector]public float cannonForce;
    [HideInInspector] public float scatterBombDamage;
    [SerializeField] private GameObject miniBomb;
    [HideInInspector] public float radius;

    // Start is called before the first frame update
    Rigidbody2D rb;
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(Vector2.right * cannonForce,ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (var hit in hitList)
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                if (hit.gameObject.CompareTag("Enemy"))
                {

                    hit.GetComponent<IDamageable>().TakeDamage(cannonDamage);
                }
            };
            miniBomb.GetComponent<MiniBomb>().direction = 1;
            Instantiate(miniBomb,transform.position,Quaternion.identity);
            miniBomb.GetComponent<MiniBomb>().direction = 0;
            Instantiate(miniBomb,transform.position,Quaternion.identity);
            miniBomb.GetComponent<MiniBomb>().direction = -1;
            Instantiate(miniBomb,transform.position,Quaternion.identity);
            Destroy(gameObject);

        }
    }
}
