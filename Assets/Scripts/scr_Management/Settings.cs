using System;
using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Management
{
    public class Settings : MonoBehaviour
    {
        public static float TextSpeed = 0.05f;

        private void OnEnable()
        {
            Actions.OnTextSpeedChanged += SetTextSpeed;
        }

        private void Start()
        {
            Actions.OnTextSpeedChanged(TextSpeed);
        }

        private void SetTextSpeed(float t)
        {
            TextSpeed = t;
        }
    }
}
