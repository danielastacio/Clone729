using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger: MonoBehaviour 
{
    public static event System.Action TriggeredDoor;
    public static bool isDoorTriggered;
    public Material material;
    [SerializeField] private string interactableId;

    public void OnTriggeredDoor()
    {
        TriggeredDoor?.Invoke();
        ChangeMaterial();
    }

    public void ChangeMaterial()
    {
        GetComponent<SpriteRenderer>().material = material;
    }
}
