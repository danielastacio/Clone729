using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private bool push;

    void Update()
    {
        PushPull();

    }
    private void PushPull()
    {
     RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.right, 5);
        Debug.DrawRay(transform.position,transform.right * 5,Color.red, 10);
        foreach (RaycastHit2D moveable in hit)
        {
            if (moveable.collider.CompareTag("Moveable"))
            {
                if(push)
                {
                moveable.transform.Translate(transform.right);
                break;
                }
                else
                { 
                 moveable.transform.Translate(transform.right * -1);
                 break;
                }
               
            }
        }

    }
    
    

   
}
