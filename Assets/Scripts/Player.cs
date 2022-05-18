using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    #region Player Events
    public static event Action PlayerHealed;

    #endregion
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PlayerHealed?.Invoke();        
    }
}


