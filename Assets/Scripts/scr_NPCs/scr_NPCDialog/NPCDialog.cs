using System;
using System.Collections.Generic;
using DialogSOs;
using scr_Interfaces;
using scr_Management.Management_Events;
using UnityEngine;

namespace scr_NPCs.scr_NPCDialog
{
    public class NPCDialog : MonoBehaviour, IInteractable
    {
        public List<Dialog> dialogStrings = new();
        public int timesInteracted = 0;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnInteract();
        }

        public void OnInteract()
        {
            Actions.OnDialogTriggered(this);
        }

        public void IncreaseInteractCount()
        {
            if (timesInteracted < dialogStrings.Count - 1)
            {
                timesInteracted++;
            }
        }
    }
}
