using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : Player
{
    #region Player Audio

    [Header("Player Audio")]

    public AK.Wwise.Event healing;
    public void PlayHealing(GameObject gameObject) => healing.Post(gameObject);

    void OnEnable()
    {
        PlayerHealed += PlayHealing;
    }
    void OnDisable()
    {
        PlayerHealed += PlayHealing;
    }
    #endregion
}
