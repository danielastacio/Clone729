using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scr_Interfaces;

public class MiniBomb : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float direction;
    [HideInInspector] public float radius;
    [HideInInspector] public float detonationTime;
    [HideInInspector] public float spread;
    [HideInInspector] public float spreadHeight;
    [HideInInspector] public LayerMask whatisObstacle;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
   
        rb.AddForce(new Vector2(direction * spread, spreadHeight),ForceMode2D.Impulse);
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

        CheckCollidingObjects();
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Ground"))
            GetComponent<CircleCollider2D>().isTrigger = false;
    }
    private void CheckCollidingObjects()
    {

        Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D hit in hitList)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {

            if(!CheckForObstacles(hit))
                hit.GetComponent<IDamageable>().TakeDamage(damage);
            }
        };
    }
    private bool CheckForObstacles(Collider2D hit)
    {
        if (Physics2D.Raycast(transform.position, hit.transform.position, radius,whatisObstacle))
            return true;
        else
            return false;
    }
}
