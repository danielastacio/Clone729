using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject door;

    public void Unlock()
    {
       // door.getComponent<>().doorLocked = false;
    }
}
