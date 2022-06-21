using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Mirror mirror;
    [SerializeField] private LayerMask whatIsPlayer;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {

            if (mirror.currentDirection != Mirror.BeamDirections.right)
            {
                mirror.currentDirection = Mirror.BeamDirections.right;
            }

            else 
            {
                mirror.currentDirection = Mirror.BeamDirections.left;
            }
        }
    }
}
