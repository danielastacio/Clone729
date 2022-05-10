using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public GameEvent 
        PlayerAttacked,
        PlayerDefended;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            PlayerAttacked?.Invoke();
        }
    }
}
