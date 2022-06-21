using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitBoard : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        DoorTrigger.TriggeredDoor += PlayAnimation;
    }

    private void OnDisable()
    {
        DoorTrigger.TriggeredDoor -= PlayAnimation;
    }

    public void PlayAnimation()
    {
        GetComponent<Animator>().enabled = true;
    }
}
