using System;
using System.Collections;
using scr_Interfaces;
using UnityEngine;

namespace scr_Environment
{
    public class Door : MonoBehaviour, IUnlockable
    {
        private Vector2 _openPos;
        private Vector2 _closedPos;
        private float _doorSize;

        private void Awake()
        {
            _doorSize = transform.localScale.y;
            _closedPos = transform.position;
            _openPos = new Vector2(transform.position.x, transform.position.y + _doorSize);
        }

        public void UnlockDoor()
        { 
            StartCoroutine(OpenDoor());
        }

        private IEnumerator OpenDoor()
        {
            var transitionTime = 0f;

            while (Vector2.Distance(transform.position, _openPos) >= 0.1)
            {
                Vector2 position = Vector2.Lerp(_closedPos, _openPos, transitionTime);
                transform.position = position;
                transitionTime += 0.1f;
                yield return null;
            }
        }
    }
}
