using System.Collections;
using System.Collections.Generic;
using scr_Management.Management_Events;
using scr_NPCs.scr_NPCDialogue;
using scr_UI.scr_Utilities;
using ScriptObjs;
using TMPro;
using UnityEngine;

namespace scr_UI.scr_DialogueSystem
{
    public class DialogueManager : MonoBehaviour
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
            var currentDialogue = inputDialogue.dialogueStrings[inputDialogue.timesInteracted];
            inputDialogue.IncreaseInteractCount();
            if (currentDialogue.dialogueType == DialogueType.TextBox)
            {
                DisplayDialogueBox(currentDialogue);
            }
        }

        private void DisplayDialogueBox(Dialogue inputDialogue)
        {
            _dialogueEnabled = true;
            _activeDialogue = UpdateDialogText(inputDialogue.dialogueStrings);
            if (_activeDialogue != null)
            {
                StartCoroutine(_activeDialogue);
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
                        yield break;
                    }
                    dialogueText.text = dialogue[currentString];
                }
                yield return null;
            }
        }
    }
}
