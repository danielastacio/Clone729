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
        public virtual void WwiseAudio(AK.Wwise.Event wwiseEvent)
        {
            wwiseEvent.Post(this.gameObject);
        }

        public virtual void WwiseAudio(AK.Wwise.State wwiseState)
        {
            wwiseState.SetValue();
        }

        public virtual void WwiseAudio(AK.Wwise.Switch wwiseSwitch, GameObject gameObject = null)
        {
            wwiseSwitch.SetValue(gameObject);
        }

        public virtual void WwiseAudio(AK.Wwise.RTPC wwiseRTPC, float value, GameObject gameObject = null)
        {
            wwiseRTPC.SetValue(gameObject, value);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
