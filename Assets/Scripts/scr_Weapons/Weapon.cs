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

    //Private Dudes/Variables
    private Vector3 difference;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         LookAtCursor();
         RotateAroundMech();
        if(Input.GetMouseButtonUp(0))
        {
            Shoot();
        }
    }

    void LookAtCursor()
    {

        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);

    }
    void RotateAroundMech()
    {
        transform.position = transform.parent.position + difference;
    }

    void Shoot()
    {
        bullet.GetComponent<BulletScript>().bulletDamage = bulletDamage;
        bullet.GetComponent<BulletScript>().direction = Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized;
        bullet.GetComponent<BulletScript>().bulletSpeed = bulletSpeed;
        Instantiate(bullet, transform.position, Quaternion.Euler(0f, 0f, angle + offset));
    }
}
