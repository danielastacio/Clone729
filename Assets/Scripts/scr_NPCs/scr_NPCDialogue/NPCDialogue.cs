using System.Collections.Generic;
using scr_Interfaces;
using scr_Management.Management_Events;
using ScriptObjs;
using UnityEngine;

namespace scr_NPCs.scr_NPCDialogue
{
    public class NPCDialogue : MonoBehaviour, IInteractable
    {
        public List<Dialogue> dialogueStrings = new();
        public int timesInteracted = 0;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnInteract();
        }

        public void OnInteract()
        {
            Actions.OnDialogueTriggered(this);
        }

        public void IncreaseInteractCount()
        {
            if (timesInteracted < dialogueStrings.Count - 1)
            {
                timesInteracted++;
            }
        }
    }
}
