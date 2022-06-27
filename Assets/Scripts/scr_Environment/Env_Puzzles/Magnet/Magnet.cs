using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private bool push = true;
    [SerializeField] private float force = 10 ;

    void Update()
    {
        PushPull();

    }
    private void PushPull()
    {
     RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right,5);
        Debug.DrawRay(transform.position, transform.right * 5, Color.red, 10);
        
        if(hit)
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.CompareTag("Moveable"))
            {

                if(push)
                {
                    hit.collider.GetComponent<Rigidbody2D>().AddForce(transform.right * force);
               
                }
                else
                {
                    hit.collider.GetComponent<Rigidbody2D>().AddForce(transform.right* -1 * force);

                }
               
            }
        }
        

    }


    private void OnMouseUp()
    {
        if (push)
            push = false;
        else
            push = true;
    }

}
