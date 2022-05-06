using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK;

namespace Metroidvaniajam.Audio
{

    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        public static AudioManager Instance { get { return instance; } }

        public WwiseEvents wwiseEvents;
        public WwiseStates wwiseStates;
        public WwiseSwitches wwiseSwitches;

        [System.Serializable]
        public struct WwiseEvents
        {
            
            [Header("SFX")]
            [Space]
            public AK.Wwise.Event steamAudioEvent;

            [Space]
            [Header("Music")]
            public AK.Wwise.Event levelOneMusicEvent;
        }

        [System.Serializable]
        public struct WwiseStates
        {
            public AK.Wwise.State setFireAudioState;
        }

        [System.Serializable]
        public struct WwiseSwitches
        {
            public AK.Wwise.Switch setFireAudioSwitch;
        }


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public void Play(AK.Wwise.Event wwiseEvent)
        {
            wwiseEvent.Post(this.gameObject);
        }
        public void Stop(AK.Wwise.Event wwiseEvent)
        {
            wwiseEvent.Stop(this.gameObject);
        }
        public void Pause(AK.Wwise.Event wwiseEvent, int transitionDuration = 600)
        {
            wwiseEvent.ExecuteAction(this.gameObject,
                AkActionOnEventType.AkActionOnEventType_Pause,
                transitionDuration,
                AkCurveInterpolation.AkCurveInterpolation_Linear);
        }

        public void Resume(AK.Wwise.Event wwiseEvent, int transitionDuration = 200)
        {
            wwiseEvent.ExecuteAction(this.gameObject,
                AkActionOnEventType.AkActionOnEventType_Resume,
                transitionDuration,
                AkCurveInterpolation.AkCurveInterpolation_Linear);
        }

        public void SetState(AK.Wwise.State wwiseState)
        {
            wwiseState.SetValue();
        }

        public void SetSwitch(AK.Wwise.Switch wwiseSwitch, GameObject gameObject)
        {
            wwiseSwitch.SetValue(gameObject);
        }

        public void SetRTPC(AK.Wwise.RTPC wwiseRTPC, float value, GameObject gameObject)
        {
            wwiseRTPC.SetValue(gameObject, value);
        }
    }
}
