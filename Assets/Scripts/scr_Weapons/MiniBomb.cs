using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scr_Interfaces;

public class MiniBomb : MonoBehaviour
{
    [HideInInspector] public float damage = 1;
    [HideInInspector] public float direction;
    [HideInInspector] public float radius = 1;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(direction, 1),ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hitList)
        {
               
            if (hit.gameObject.CompareTag("Enemy"))
                {

                 hit.GetComponent<IDamageable>().TakeDamage(damage);
                }
        };
        Destroy(gameObject);

    }
}
