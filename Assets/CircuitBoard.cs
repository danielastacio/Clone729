using System.Collections;
using System.Collections.Generic;
using scr_Management.Management_Events;
using UnityEngine;

public class CircuitBoard : MonoBehaviour
{
    [SerializeField] private string interactableId;
    
    private void OnEnable()
    {
        DoorTrigger.TriggeredDoor += PlayAnimation;
        Actions.OnDoorTriggered += PlayAnimation;
    }

    private void OnDisable()
    {
        DoorTrigger.TriggeredDoor -= PlayAnimation;
        Actions.OnDoorTriggered -= PlayAnimation;
    }

    public void PlayAnimation()
    {
        GetComponent<Animator>().enabled = true;
    }
    
    public void PlayAnimation(string id)
    {
        if (interactableId.Equals(id))
        {
            GetComponent<Animator>().enabled = true;

        }
    }
}
