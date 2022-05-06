using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvaniajam.Audio;

public class Steam : MonoBehaviour
{
    private AudioManager audioManager;
    private AK.Wwise.Event steamAudio;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.Instance;
        steamAudio = audioManager.wwiseEvents.steamAudioEvent;

        audioManager.Play(steamAudio);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            audioManager.Pause(steamAudio);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            audioManager.Resume(steamAudio);
        }
    }
}
