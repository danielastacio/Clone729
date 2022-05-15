using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWallCheck : MonoBehaviour
{
    private Cube parent;

    private void Awake()
    {
        parent = transform.parent.GetComponent<Cube>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") && parent.facingRight)
        {
            parent.Flip(-180);
        }
        else if (other.gameObject.CompareTag("Wall") && !parent.facingRight)
        {
            parent.Flip(0);
        }
    }
}
