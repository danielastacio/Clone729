using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public List<AK.Wwise.Event> wwiseEvents = new();
    public List<AK.Wwise.State> wwiseStates = new();
    public void PlayAudio()
    {
        foreach(AK.Wwise.Event wwiseEvent in wwiseEvents)
        {
            wwiseEvent.Post(gameObject);
        }
    }

    public void SetAudioState()
    {
        foreach (AK.Wwise.State wwiseState in wwiseStates)
        {
            wwiseState.SetValue();
        }
    }
}
