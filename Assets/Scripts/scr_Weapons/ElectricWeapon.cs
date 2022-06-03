using System;
using System.Collections;
using System.Linq;
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
    private int[] ID;
    private int bounceNum;
    

    
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
        ResetVariables();
        
        //cast "lightning"
        RaycastHit2D hit = Physics2D.Raycast(transform.position, difference, distance,whatisConductor);
        if (hit)
        {
            if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                //reduce number of bounces remaining
                _bounce--;
                //enemy takes damage
                hit.collider.GetComponent<IDamageable>().TakeDamage(damage);
                //add enemy ID to array to prevent same enemy from taking hits
                AddToHitArray(hit);   

                //set position for circle to enemy position so it can "bounce" from there
                bouncePosition = hit.collider.transform.position;
                CheckCircle();
               
            }
        }
    }

    private void CheckCircle()
    {
        //check if there are remaining bounces
        
        if(_bounce > 0)
        {
            //overlapcirlce for bounceable objects that are in bounce radius
            Collider2D[] circle = Physics2D.OverlapCircleAll(bouncePosition, bounceRadius, whatisConductor);
            //check whether its an enemy or a conductor
        foreach (Collider2D collided in circle)
        {
            //if it's an enemy that wasn't bounced to, bounce to them
            if (collided.gameObject.CompareTag("Enemy") &&  ID.All(id => id != collided.gameObject.GetInstanceID()))
            {

                collided.gameObject.GetComponent<IDamageable>().TakeDamage(damage);

                _bounce--;

                ID[bounceNum] = collided.gameObject.GetInstanceID();
                Debug.Log(ID[bounceNum]);
                    bounceNum++;
                bouncePosition = collided.transform.position;
                break;
                
            }
            //if it's a conductor that wasn't bounced to, bounce to it
            //NOTE: the gun will favor enemies over conductors for overlapcircle
            else if(collided.gameObject.CompareTag("Conductor") && ID.All(id => id != collided.gameObject.GetInstanceID()))
            {
                _bounce--;

                ID[bounceNum] = collided.gameObject.GetInstanceID();
                Debug.Log(ID[bounceNum]);
                bounceNum++;
                   

                bouncePosition = collided.transform.position;

                break;
            }
        }


            //just in case _bounce doesn't reduce itself and starts an endless loop because it's the only object in the circle
            if (circle.Length == 1)
                _bounce = 0;
            CheckCircle();
        }
        
      
    }
    private void ResetVariables()
    {
        ID = new int[bounce];
        bounceNum = 0;
        _bounce = bounce;

    }
    private void AddToHitArray(RaycastHit2D hit
        )
    {
        ID[bounceNum] = hit.collider.gameObject.GetInstanceID();
        Debug.Log(ID[bounceNum]);
        bounceNum++;
    }

}
