using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scr_Environment;

public class Beam : MonoBehaviour
{
    [SerializeField] private int distance;
    [SerializeField] private LayerMask whatIsMirror;
    [SerializeField] private LineRenderer beam;
    // Update is called once per frame
    void Update()
    {
        ShootBeam();
    }

    private void ShootBeam()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distance,whatIsMirror);
        if (hit)
        {
            DrawBeam(true);
            DrawBeam(transform.position, hit.point);
            Debug.DrawRay(transform.position,hit.transform.position - transform.position);

            if (hit.collider.CompareTag("Mirror"))
            {
                hit.collider.GetComponent<Mirror>().whatIsMirror = whatIsMirror;
                hit.collider.GetComponent<Mirror>().isReflecting = true;
            }

            else if (hit.collider.CompareTag("DoorTrigger"))
            {
                if (!DoorTrigger.isDoorTriggered)
                {
                    hit.collider.GetComponent<DoorTrigger>().OnTriggeredDoor();

                    DoorTrigger.isDoorTriggered = true;
                }
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
