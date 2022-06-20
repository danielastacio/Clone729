using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    //[SerializeField] private GameObject door;
    public bool doorIsUnlocked;
    public void UnlockDoor()
    {
        doorIsUnlocked = true;
       // door.getComponent<>().doorLocked = false;
    }
}
