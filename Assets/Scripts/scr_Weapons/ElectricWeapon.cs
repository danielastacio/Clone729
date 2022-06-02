using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scr_Interfaces;

public class ElectricWeapon : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField][Tooltip("if gun isn't looking at cursor keep increasing offset")] protected int offset;
    [Header("Stats")]
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float EnergyCost = 5;
    [SerializeField] protected float fireRate = 0.5f;
    [SerializeField] private float distance = 1;
    [SerializeField] private int bounce;
                     private int _bounce = 3;
                     private Vector2 bouncePosition;
    [SerializeField] private float bounceRadius;
    [SerializeField] private LayerMask whatisConductor;

    //Private Dudes/Variables
    protected Vector3 difference;
    protected float angle;
    protected bool canShoot = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtCursor();
        RotateAroundMech();
        if (Input.GetMouseButtonUp(0) && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }
    public void LookAtCursor()
    {
        // Get Direction and Normalize It
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        //Get Rotation number
        angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //Rotate by said amount
        transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);

    }

    protected void RotateAroundMech()
    {
        //set Position around the mech by taking which direction it's pointing at
        transform.position = transform.parent.position + difference;
    }

    protected IEnumerator Shoot()
    {
        canShoot = false;
        CastRay();
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
    public void CastRay()
    {
        _bounce = bounce;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, difference, distance,whatisConductor);
        if (hit)
        {
            if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                _bounce--;
                hit.collider.GetComponent<IDamageable>().TakeDamage(damage);
                bouncePosition = hit.collider.transform.position;
                CheckCircle();
               
            }
        }
    }

    private void CheckCircle()
    {
        if(_bounce > 0)
        {
           
        Collider2D[] circle = Physics2D.OverlapCircleAll(bouncePosition, bounceRadius);
        foreach (Collider2D collided in circle)
        {
            if (collided.gameObject.CompareTag("Enemy"))
            {
                collided.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
                _bounce--;
                bouncePosition = collided.transform.position;
                break;
                
            }
            else if(collided.gameObject.CompareTag("Conductor"))
            {
                _bounce--;
                bouncePosition = collided.transform.position;
                break;
            }
        }
                CheckCircle();
        }
    }
}
