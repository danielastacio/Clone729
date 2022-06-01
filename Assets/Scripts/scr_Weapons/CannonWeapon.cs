using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonWeapon : Weapon
{
    [Header("Cannon")]
    [SerializeField] private float radius = 1;
    [SerializeField] private float detonationTime = 3;
    [SerializeField] private float damageReduction = 1;
    [SerializeField] private float spread = 2;
    [SerializeField] private float spreadHeight = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   protected override void Update()
    {

        base.Update();

       
    }

    protected override void SetStats()
    {
        bullet.GetComponent<CannonBomb>().damage = damage;
        bullet.GetComponent<CannonBomb>().speed = speed;
        bullet.GetComponent<CannonBomb>().radius = radius;
        bullet.GetComponent<CannonBomb>().detonationTime = detonationTime;
        bullet.GetComponent<CannonBomb>().damageReduction = damageReduction;
        bullet.GetComponent<CannonBomb>().spread = spread;
        bullet.GetComponent<CannonBomb>().spreadHeight = spreadHeight;
    }
}
