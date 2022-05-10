using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [Header("Player Events")]
    [Space]
    public GameEvent PlayerAttacked;
    public GameEvent PlayerDefended;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            PlayerAttacked?.Invoke();
        }
    }
}
