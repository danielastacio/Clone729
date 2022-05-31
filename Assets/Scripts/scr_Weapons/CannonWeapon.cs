using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonWeapon : Weapon
{
    [SerializeField] private float cannonDamage = 1;
    [SerializeField] private int shootingForce = 100;
    [SerializeField] private float miniBombDamage;
    [SerializeField] private float radius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        base.Update();

       
    }
     protected override void InstantiateBullet()
    {
        bullet.GetComponent<CannonBomb>().cannonDamage = cannonDamage;
        bullet.GetComponent<CannonBomb>().cannonForce = shootingForce;
        bullet.GetComponent<CannonBomb>().scatterBombDamage = miniBombDamage;
        bullet.GetComponent<CannonBomb>().radius = radius;

        Instantiate(bullet, transform.position, Quaternion.Euler(0f, 0f, angle + offset));
        
    }
}
