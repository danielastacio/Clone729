using System;
using System.Collections;
using System.Collections.Generic;
using scr_UI.scr_Utilities;
using UnityEngine;

public class YouWin : MonoBehaviour
{
    public Canvas YOUWIN;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CanvasController.ShowCanvas(YOUWIN);
        }
    }
}
