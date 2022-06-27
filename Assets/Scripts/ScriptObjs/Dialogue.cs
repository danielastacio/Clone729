using System.Collections.Generic;
using scr_Management;
using scr_Management.Management_Events;
using UnityEngine;

namespace ScriptObjs
{
    [CreateAssetMenu(menuName = "New Dialogue", fileName = "New Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [System.Serializable]
        public class DialogueString
        {
            public string dialogueText;
            public bool interactable;
            
            [Header("Confirm Button")]
            public string confirmText;
            public TriggerType confirmInteraction;
            [SerializeField] private string confirmInteractableId;
            
            [Header("Decline Button")]
            public string declineText;
            public TriggerType declineInteraction;
            [SerializeField] private string declineInteractableId;

            public void OnConfirmInteraction()
            {
                if (confirmInteraction == TriggerType.Door)
                {
                    Actions.OnDoorTriggered(confirmInteractableId);
                }
                else if (confirmInteraction == TriggerType.None)
                {
                    Debug.Log("Confirm pressed!");
                }
            }

            public void OnDeclineInteraction()
            {
                // Set up declined interactions
            }
        }

        public enum DialogueType
        {
            Bubble,
            TextBox
        }
        
        public DialogueType dialogueType;
        public List<DialogueString> dialogueStrings = new();
    }
}