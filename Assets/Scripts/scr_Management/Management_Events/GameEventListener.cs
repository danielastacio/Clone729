using System;
using UnityEngine;
using UnityEngine.Events;

namespace scr_Management.Management_Events
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        public UnityEvent unityEvent;

        private void OnEnable() => gameEvent.Register(this);

        private void OnDisable() => gameEvent.DeRegister(this);

        public void RaiseEvent() => unityEvent.Invoke();
    }
}
