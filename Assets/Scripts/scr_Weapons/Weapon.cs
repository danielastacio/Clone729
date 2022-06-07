using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scr_Weapons;

public class Weapon : MonoBehaviour


{
    //Inspector Dudes/Variables
    [Header("Debug")]
    [SerializeField][Tooltip("if gun isn't looking at cursor keep increasing offset")] protected int offset;
    [Header("Stats")]
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float speed = 5;
    [SerializeField] protected float fireRate = 0.5f;
    [SerializeField] protected GameObject bullet;

    //Private Dudes/Variables
    protected Vector3 difference;
    protected float angle;
    protected bool canShoot = true;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    protected virtual void Update()
    {
         LookAtCursor();
         RotateAroundMech();
        if(Input.GetMouseButtonUp(0) && canShoot)
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

    protected virtual void  InstantiateBullet()
    {
        Instantiate(bullet, transform.position, Quaternion.Euler(0f, 0f, angle + offset));
    }
    protected IEnumerator  Shoot()
    {
        canShoot = false;
        SetStats();
        InstantiateBullet();
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
    protected virtual void SetStats()
    {

        bullet.GetComponent<BulletScript>().bulletDamage = damage;
        bullet.GetComponent<BulletScript>().bulletSpeed = speed;
    }
         
}
