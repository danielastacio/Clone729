using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    //Public Inspector variables here

    [SerializeField]
    float speed;

    //Private Variables
    RaycastHit2D hit;
    //Component variables
    Rigidbody2D rb;
    
    void Start()
    {
       

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position,new Vector2(1,-1),3);
       if (hit)
        {
            Debug.Log("Object Hit?: " + hit+", What was hit?" + hit.collider.name);
            Debug.DrawRay(transform.position, new Vector2(1, -1) * 2, Color.green, 60);
            transform.Translate(Vector2.right * speed/10);


        }
       
    }
    
}   
