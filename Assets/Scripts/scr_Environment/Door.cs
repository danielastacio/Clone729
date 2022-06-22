using System;
using System.Collections;
using scr_Interfaces;
using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Environment
{
    public class Door : MonoBehaviour, IUnlockable
    {
        [SerializeField] private string doorID;
        private bool _doorLocked = true;
        private Animator _anim;
        private BoxCollider2D _bc2d;

        private void OnEnable()
        {
            DoorTrigger.TriggeredDoor += UnlockDoor;
            Actions.OnDoorTriggered += CheckDoorState;
        }

        private void OnDisable()
        {
            DoorTrigger.TriggeredDoor -= UnlockDoor;
            Actions.OnDoorTriggered -= CheckDoorState;

        }
        private void Awake()
        {
            _anim = gameObject.GetComponent<Animator>();
            _bc2d = gameObject.GetComponent<BoxCollider2D>();
        }

        /*public void UnlockDoor()
        { 
            _anim.SetBool("IsOpen", true);
            _bc2d.enabled = false;
        }*/

        private void CheckDoorState(string id)
        {
            if (doorID.Equals(id) && !_doorLocked)
            {
                LockDoor();
            }
            else if (doorID.Equals(id) && _doorLocked)
            {
                UnlockDoor();
            }
        }

        private void LockDoor()
        {
            _anim.SetBool("IsOpen", false);
            _bc2d.enabled = true;
            _doorLocked = true;
        }

        public void UnlockDoor()
        {
            _anim.SetBool("IsOpen", true);
            _bc2d.enabled = false;
            _doorLocked = false;
        }
    }
}
