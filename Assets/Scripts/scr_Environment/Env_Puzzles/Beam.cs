using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] private int distance;
    [SerializeField] private LayerMask whatisMirror;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootBeam();
    }
    private void ShootBeam()
    {
     
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distance,whatisMirror);
        if (hit)
        {

        Debug.DrawRay(transform.position,hit.transform.position - transform.position);
        Debug.Log("Mirror Name:" + hit.collider.name);
        if(hit.collider.CompareTag("Mirror"))
        {
            hit.collider.GetComponent<Mirror>().whatisMirror = whatisMirror;
            hit.collider.GetComponent<Mirror>().Reflect(transform.position);
        }
        else if (hit.collider.CompareTag("Trigger"))
            {
             //   hit.collider.GetComponent<Trigger>().SwitchOn(hit);
            }
        }

    }
}
