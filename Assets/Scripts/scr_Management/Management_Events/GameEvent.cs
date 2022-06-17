using System.Collections.Generic;
using UnityEngine;

namespace scr_Management.Management_Events
{
    [CreateAssetMenu(menuName = "Game Event", fileName = "New Game Event")]
    public class GameEvent : ScriptableObject
    {
        List<GameEventListener> _listeners = new();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].RaiseEvent();
            }
        }

        public void Register(GameEventListener gameEventListener) => _listeners.Add(gameEventListener);
        public void DeRegister(GameEventListener gameEventListener) => _listeners.Remove(gameEventListener);
    }
}

