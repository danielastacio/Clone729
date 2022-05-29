using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour


{
    //Inspector Dudes/Variables
    [Header("Debug")]
    [SerializeField][Tooltip("if gun isn't looking at cursor keep increasing offset")] int offset;
    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;

    //Private Dudes/Variables
    private Vector3 difference;
    private float angle;
    private bool canShoot = true;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
         LookAtCursor();
         RotateAroundMech();
        if(Input.GetMouseButtonUp(0) && canShoot)
        {
            Shoot();
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
    public void RotateAroundMech()
    {
        //set Position around the mech by taking which direction it's pointing at
        transform.position = transform.parent.position + difference;
    }

    public virtual void  InstantiateBullet()
    {
        bullet.GetComponent<BulletScript>().bulletDamage = bulletDamage;
        bullet.GetComponent<BulletScript>().direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized ;
        bullet.GetComponent<BulletScript>().bulletSpeed = bulletSpeed;
        Instantiate(bullet, transform.position, Quaternion.Euler(0f, 0f, angle + offset));
    }
    IEnumerator  Shoot()
    {
        canShoot = false;
        InstantiateBullet();
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
