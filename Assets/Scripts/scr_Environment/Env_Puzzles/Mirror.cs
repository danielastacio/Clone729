using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scr_Environment;
public class Mirror : MonoBehaviour
{
    [SerializeField] private LineRenderer beam;
    [HideInInspector] public LayerMask whatIsMirror;
    [HideInInspector] public Vector2 beamDirection;
    public bool isReflecting;
    private Mirror mirror;
    public enum BeamDirections { left, right, up, down };
    public BeamDirections currentDirection;
    public void SetBeamDirection()
    {
        switch (currentDirection)
        {
            case BeamDirections.left:
                beamDirection = Vector2.left;
                break;
            case BeamDirections.right:
                beamDirection = Vector2.right;
                break;
            case BeamDirections.up:
                beamDirection = Vector2.up;
                break;
            case BeamDirections.down:
                beamDirection = Vector2.down;
                break;
        }
    }

    private void FixedUpdate()
    {
        SetBeamDirection();
        DisableOtherMirror();
    }


    public void DisableOtherMirror()
    {
        if (mirror != null)
        {
            mirror.DrawBeam(false);
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
    public void ShootBeam(Vector2 position)
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, position, 100, whatIsMirror);
        if (hit)
        {

            DrawBeam(true);
            DrawBeam(transform.position, hit.point);

            Debug.Log("Mirror Name:" + hit.collider.name + hit.collider.transform.position);
            Debug.DrawRay(transform.position, hit.transform.position - transform.position);

            if (hit.collider.CompareTag("Mirror"))
            {
                mirror = hit.collider.GetComponent<Mirror>();

                mirror.whatIsMirror = whatIsMirror;
                mirror.isReflecting = true;
                mirror.ShootBeam(mirror.beamDirection);

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

}