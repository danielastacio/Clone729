using System;
using System.Collections;
using scr_Interfaces;
using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Environment
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private string doorID;
        [SerializeField] private bool doorLocked;
        private Animator _anim;
        private BoxCollider2D _bc2d;

        private void OnEnable()
        {
            Actions.OnDoorTriggered += CheckDoorState;
        }

        private void OnDisable()
        {
            Actions.OnDoorTriggered -= CheckDoorState;

        }
        private void Awake()
        {
            _anim = gameObject.GetComponent<Animator>();
            _bc2d = gameObject.GetComponent<BoxCollider2D>();
            StartingDoorState();
        }

        private void StartingDoorState()
        {
            _anim.SetBool("IsClosed", doorLocked);
            _bc2d.enabled = doorLocked;
        }
        
        private void CheckDoorState(string id)
        {
            if (doorID.Equals(id))
            {
                ChangeDoorState();
            }
            
        }

        private void ChangeDoorState()
        {
            doorLocked = !doorLocked;
            _anim.SetBool("IsClosed", doorLocked);
            _bc2d.enabled = doorLocked;
        }
    }
}
