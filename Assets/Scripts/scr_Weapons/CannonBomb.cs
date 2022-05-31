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
    [HideInInspector] public float radius = 1;
    [SerializeField] LayerMask whatisEnemy;

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
            
            Debug.Log(hitList.Length);
            foreach (Collider2D hit in hitList)
            {
                Debug.Log(hit.gameObject.name);
                Debug.DrawRay(transform.position,hit.transform.position,Color.red,30);
                if (hit.gameObject.CompareTag("Enemy"))
                {

                    hit.GetComponent<IDamageable>().TakeDamage(cannonDamage);
                }
            };
            miniBomb.GetComponent<MiniBomb>().direction = 1;
            Instantiate(miniBomb,new Vector2(transform.position.x + 0.1f,transform.position.y + 0.1f),Quaternion.identity);
            miniBomb.GetComponent<MiniBomb>().direction = 0;
            Instantiate(miniBomb, new Vector2(transform.position.x + 0.1f, transform.position.y + 0.1f), Quaternion.identity);
            miniBomb.GetComponent<MiniBomb>().direction = -1;
            Instantiate(miniBomb,new Vector2(transform.position.x + 0.1f,transform.position.y + 0.1f),Quaternion.identity);
            Destroy(gameObject);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
