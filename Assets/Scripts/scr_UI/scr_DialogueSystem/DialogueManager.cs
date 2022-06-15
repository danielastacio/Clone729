using System.Collections;
using System.Collections.Generic;
using scr_Management.Management_Events;
using scr_NPCs.scr_NPCDialogue;
using scr_UI.scr_Utilities;
using TMPro;
using UnityEngine;

namespace scr_UI.scr_DialogueSystem
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private Canvas dialogueCanvas;
        [SerializeField] private TextMeshProUGUI dialogueText;
        
        private bool _dialogueEnabled = false;
        private IEnumerator _activeDialogue = null;

        private void OnEnable()
        {
            Actions.OnDialogueTriggered += DisplayDialogue;
        }

        private void OnDisable()
        {
            Actions.OnDialogueTriggered -= DisplayDialogue;
        }

        private void DisplayDialogue(NPCDialogue inputDialogue)
        {
            _dialogueEnabled = true;
            _activeDialogue = UpdateDialogText(inputDialogue.dialogueStrings[inputDialogue.timesInteracted].dialogueStrings);
            if (_activeDialogue != null)
            {
                StartCoroutine(_activeDialogue);
                inputDialogue.IncreaseInteractCount();
            }
        }

        private void HideDialog()
        {
            _dialogueEnabled = false;
            _activeDialogue = null;
            CanvasController.HideCanvas(dialogueCanvas);
        }

        private IEnumerator UpdateDialogText(List<string> dialogue)
        {
            var currentString = 0;
            CanvasController.ShowCanvas(dialogueCanvas);
            
            dialogueText.text = dialogue[currentString];
            
            while (_dialogueEnabled)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentString++;
                    if (currentString == dialogue.Count)
                    {
                        HideDialog();
                    }
                    dialogueText.text = dialogue[currentString];
                }
                yield return null;
            }
        }
    }
}
