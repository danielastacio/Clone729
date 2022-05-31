using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scr_Interfaces;

public class CannonBomb : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    [HideInInspector] public float radius;
    [HideInInspector] public float damageReduction;
    [HideInInspector] public float spread;
    [HideInInspector] public float spreadHeight;
    [HideInInspector] public float detonationTime;
    [SerializeField] private GameObject miniBomb;
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] LayerMask whatIsObstacle;
    private Rigidbody2D  rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(Vector2.right * speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }

    private void SplitIntoMiniBombs()
    {
        miniBomb.GetComponent<MiniBomb>().direction = 1;
        Instantiate(miniBomb, transform.position, Quaternion.identity);
        miniBomb.GetComponent<MiniBomb>().direction = 0;
        Instantiate(miniBomb, transform.position, Quaternion.identity);
        miniBomb.GetComponent<MiniBomb>().direction = -1;
        Instantiate(miniBomb, transform.position, Quaternion.identity);
    }

    private void CheckCollision(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            CheckCollidingObjects();
            SetStats();
            SplitIntoMiniBombs();
            Destroy(gameObject);

        }
    }

    private void CheckCollidingObjects()
    {

        Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hitList)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {

                if (!CheckForObstacles(hit))
                {
              hit.GetComponent<IDamageable>().TakeDamage(damage);
                }
            }
        };
    }
    private bool CheckForObstacles(Collider2D hit)
    {
        if (Physics2D.Raycast(transform.position, hit.transform.position, radius, whatIsObstacle))
            return true;
        else
            return false;
    }
    private void SetStats()
    {
        miniBomb.GetComponent<MiniBomb>().damage = damage / damageReduction;
        miniBomb.GetComponent<MiniBomb>().radius = radius;
        miniBomb.GetComponent<MiniBomb>().spread = spread;
        miniBomb.GetComponent<MiniBomb>().spreadHeight = spreadHeight;
        miniBomb.GetComponent<MiniBomb>().detonationTime = detonationTime;
        miniBomb.GetComponent<MiniBomb>().whatisObstacle = whatIsObstacle;


    }

}
