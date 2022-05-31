using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scr_Interfaces;

public class MiniBomb : MonoBehaviour
{
    [HideInInspector] public float damage = 1;
    [HideInInspector] public float direction;
    [HideInInspector] public float radius = 1;
    [HideInInspector] public float detonationTime = 3;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
   
        rb.AddForce(new Vector2(direction, 1),ForceMode2D.Impulse);
        StartCoroutine(DetonationTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }
     
    IEnumerator DetonationTimer()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<CircleCollider2D>().isTrigger = false;
        yield return new WaitForSeconds(detonationTime);
      
            Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (var hit in hitList)
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.red, 10);
                if (hit.gameObject.CompareTag("Enemy"))
                {

                    hit.GetComponent<IDamageable>().TakeDamage(damage);
                }
            };
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
            GetComponent<CircleCollider2D>().isTrigger = false;
    }
}
