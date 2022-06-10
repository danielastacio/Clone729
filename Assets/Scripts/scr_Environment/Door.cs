using System;
using System.Collections;
using scr_Interfaces;
using UnityEngine;

namespace scr_Environment
{
    public class Door : MonoBehaviour, IUnlockable
    {
        private Animator _anim;
        private BoxCollider2D _bc2d;

        private void Awake()
        {
            _anim = gameObject.GetComponent<Animator>();
            _bc2d = gameObject.GetComponent<BoxCollider2D>();
        }

        public void UnlockDoor()
        { 
            _anim.SetBool("IsOpen", true);
            _bc2d.enabled = false;
        }
    }
}
