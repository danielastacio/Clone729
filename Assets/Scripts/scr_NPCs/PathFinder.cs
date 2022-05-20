using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public float smoothTime;
    public float maxDistance;
    public float rotateTime;

    public Transform target;
    public Transform Cylinder;
    private Vector3 velocity = Vector3.zero;


    public Renderer ground;
    private Vector3 groundBounds;
    private bool canMove;

    public enum Directions
    {
        bottomLeft,
        bottomRight,
        topLeft,
        topRight,
        centerUp,
        centerDown,
        centerLeft,
        centerRight,
        center
    };

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

        groundBounds = ground.bounds.extents;

        if (Input.GetKey(KeyCode.W))
        {
            MoveCube(Directions.topRight);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveCube(Directions.bottomLeft);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveCube(Directions.topLeft);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveCube(Directions.bottomRight);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveCube(Directions.centerUp);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveCube(Directions.centerLeft);

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveCube(Directions.centerRight);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveCube(Directions.centerDown);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveCube(Directions.topRight);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveCube(Directions.bottomLeft);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveCube(Directions.topLeft);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveCube(Directions.bottomRight);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCube(Directions.centerUp);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCube(Directions.centerLeft);

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCube(Directions.centerRight);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCube(Directions.centerDown);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            MoveCube(Directions.center);
        }
    }
    private void FixedUpdate()
    {
        MoveCylinder();
    }

    public void MoveCube(Directions directions)
    {
        Vector3 heading;
        heading = groundBounds - (transform.localScale / 2);

        switch (directions)
        {
            case Directions.topRight:
                transform.position = Vector3.SmoothDamp(transform.position, -heading, ref velocity, smoothTime);
                break;
            case Directions.bottomLeft:
                transform.position = Vector3.SmoothDamp(transform.position, heading, ref velocity, smoothTime);
                break;

            case Directions.topLeft:
                heading = new(heading.x, -heading.y, -heading.z);
                transform.position = Vector3.SmoothDamp(transform.position, heading, ref velocity, smoothTime);
                break;
            case Directions.bottomRight:
                heading = new(-heading.x, -heading.y, heading.z);
                transform.position = Vector3.SmoothDamp(transform.position, heading, ref velocity, smoothTime);
                break;

            case Directions.centerUp:
                heading.x = 0;
                transform.position = Vector3.SmoothDamp(transform.position, -heading, ref velocity, smoothTime);
                break;
            case Directions.centerLeft:
                heading.z = 0;
                transform.position = Vector3.SmoothDamp(transform.position, heading, ref velocity, smoothTime);
                break;

            case Directions.centerDown:
                heading.x = 0;
                transform.position = Vector3.SmoothDamp(transform.position, heading, ref velocity, smoothTime);
                break;
            case Directions.centerRight:
                heading.z = 0;
                transform.position = Vector3.SmoothDamp(transform.position, -heading, ref velocity, smoothTime);
                break;

            default:
                heading = Vector3.zero;
                transform.position = Vector3.SmoothDamp(transform.position, heading, ref velocity, smoothTime);
                break;
        }

    }

    public void MoveCylinder()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - Cylinder.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotateTime * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(Cylinder.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(Cylinder.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        Cylinder.rotation = Quaternion.LookRotation(newDirection);
    }
}
