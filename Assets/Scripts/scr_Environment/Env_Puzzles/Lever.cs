using System.Collections;
using System.Collections.Generic;
using scr_Interfaces;
using scr_Management.Management_Events;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public enum LeverType
    {
        Mirror,
        Door
    }

    public LeverType interaction;
    [SerializeField] private string interactableId;
    
    [SerializeField] private Mirror mirror;
    [SerializeField] private LayerMask whatIsPlayer;

    private void LeverInteract()
    {
        if (interaction == LeverType.Mirror)
        {
            FlipMirror();
        }
        else if (interaction == LeverType.Door)
        {
            ChangeDoorState();
        }
    }
    
    private void FlipMirror()
    {
        if (Input.GetKeyDown(KeyCode.E))
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

    private void ChangeDoorState()
    {
        Actions.OnDoorTriggered(interactableId);
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void OnInteract()
    {
        LeverInteract();
    }
}
