using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] private int distance;
    [SerializeField] private LayerMask whatisMirror;
    [SerializeField] private LineRenderer beam;
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
            DrawBeam(true);
            DrawBeam(transform.position, hit.point);
            Debug.DrawRay(transform.position,hit.transform.position - transform.position);
            Debug.Log("Mirror Name:" + hit.collider.name);

            if (hit.collider.CompareTag("Mirror"))
            {
                hit.collider.GetComponent<Mirror>().whatisMirror = whatisMirror;
                hit.collider.GetComponent<Mirror>().Reflect(transform.position);
            }            
        }
        else
        {

            DrawBeam(false);
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

}
