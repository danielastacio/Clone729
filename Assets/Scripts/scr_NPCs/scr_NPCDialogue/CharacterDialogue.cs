using System;
using System.Collections.Generic;
using System.Linq;
using scr_Interfaces;
using scr_Management;
using scr_Management.Management_Events;
using scr_Player;
using ScriptObjs;
using UnityEngine;

namespace scr_NPCs.scr_NPCDialogue
{
    public class CharacterDialogue : MonoBehaviour, IInteractable
    {
        public List<Dialogue> characterDialogueStrings = new();
        public int timesInteracted = 0;
        public List<string> triggerIds = new();
        public GameObject speechBubble;

        private void OnEnable()
        {
            Actions.OnConfirmTriggered += IncreaseInteractCount;
        }

        private void Update()
        {
            PlayerCheck();
        }

        private void PlayerCheck()
        {
            var playerCheck =
                Physics2D.OverlapCircleAll(transform.position, 5f);
            if (playerCheck.Contains(PlayerController.Instance.GetComponent<CapsuleCollider2D>()))
            {
                speechBubble.SetActive(true);
            }
            else
            {
                speechBubble.SetActive(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, 5f);
        }

        public void OnInteract()
        {
            Actions.OnDialogueTriggered(this);
            Actions.OnControllerChanged(ControllerType.Dialogue);
        }

        private void IncreaseInteractCount(string s)
        {
            if (triggerIds.Contains(s) && timesInteracted < characterDialogueStrings.Count - 1)
            {
                timesInteracted++;
            }
        }
    }
}
