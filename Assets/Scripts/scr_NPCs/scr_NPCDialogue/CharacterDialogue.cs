using System.Collections.Generic;
using scr_Interfaces;
using scr_Management;
using scr_Management.Management_Events;
using ScriptObjs;
using UnityEngine;

namespace scr_NPCs.scr_NPCDialogue
{
    public class CharacterDialogue : MonoBehaviour, IInteractable
    {
        public List<Dialogue> characterDialogueStrings = new();
        public int timesInteracted = 0;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnInteract();
        }

        public void OnInteract()
        {
            Actions.OnDialogueTriggered(this);
            Actions.OnControllerChanged(ControllerType.Dialogue);
        }

        public void IncreaseInteractCount()
        {
            if (timesInteracted < characterDialogueStrings.Count - 1)
            {
                timesInteracted++;
            }
        }
    }
}
