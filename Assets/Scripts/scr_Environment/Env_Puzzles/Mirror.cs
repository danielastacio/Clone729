using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private bool reflectRight = true;
    [HideInInspector] public LayerMask whatisMirror;
    [SerializeField] private LineRenderer beam;
    public void Reflect(Vector3 position)
    {
        Debug.Log("Reflect Activated!");
        Vector2 direction = (position - transform.position).normalized;
        if (Mathf.Round(direction.x) ==  -1)
        { 
            Debug.Log("Left");
            if(reflectRight)
            ShootBeam(Vector2.down);
            else
                ShootBeam(Vector2.up);

        }
        else if (Mathf.Round(direction.x) == 1)
        {
            Debug.Log("Right");
            if (reflectRight)
                ShootBeam(Vector2.up);
            else
                ShootBeam(Vector2.down);
        }
        else if (Mathf.Round(direction.y) == -1)
        {
            Debug.Log("Down");
            if (reflectRight)
                ShootBeam(Vector2.right);
            else
                ShootBeam(Vector2.left);
        }
        else if (Mathf.Round(direction.y) == 1)
        {
            Debug.Log("Up");
            if (reflectRight)
                ShootBeam(Vector2.left);
            else
                ShootBeam(Vector2.right);
        }
        
    }

    private void DrawBeam(Vector2 startPos, Vector2 endPos)
    {
        beam.SetPosition(0, startPos);
        beam.SetPosition(1, endPos);
    }

    private void DrawBeam(bool isDrawing)
    {
        if (isDrawing)
            beam.enabled = true;
        else
            beam.enabled = false;
    }
    private void ShootBeam(Vector2 position)
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, position, 100,whatisMirror);
        if (hit)
        {
            DrawBeam(true);
            DrawBeam(transform.position, hit.point);

            Debug.Log("Mirror Name:" + hit.collider.name + hit.collider.transform.position);
         Debug.DrawRay(transform.position, hit.transform.position - transform.position);
            if (hit.collider.CompareTag("Mirror"))
            {
                hit.collider.GetComponent<Mirror>().whatisMirror = whatisMirror;
                hit.collider.GetComponent<Mirror>().Reflect(transform.position);
            }
            else if (hit.collider.CompareTag("Trigger"))
            {
                hit.collider.GetComponent<Trigger>().UnlockDoor();
            }

            else
            {
                DrawBeam(false);
            }
        }


    }

    public void Toggle()
    {
        if (reflectRight)
            reflectRight = false;
        else
            reflectRight = true;
    }

}
