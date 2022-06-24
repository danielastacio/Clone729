using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitBoard : MonoBehaviour
{
    public void PlayAnimation()
    {
        GetComponent<Animator>().enabled = true;
    }
}
