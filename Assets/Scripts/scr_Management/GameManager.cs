using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Framerate")]
    [SerializeField] private TMP_Text txt_fpsValue;

    //private variables
    private float deltaTime;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = Mathf.FloorToInt(deltaTime * 1000.0f);
        float fps = Mathf.FloorToInt(1.0f / deltaTime);

        txt_fpsValue.text = fps + " (" + msec + ")";
    }
}