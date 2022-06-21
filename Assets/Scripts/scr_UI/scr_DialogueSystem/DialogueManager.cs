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
        private IEnumerator _activePrinter = null;
        private WaitForSeconds _textSpeed = new(0.05f);

        private void OnEnable()
        {
            Actions.OnDialogueTriggered += DisplayDialogue;
            Actions.OnTextSpeedChanged += SetTextSpeed;
        }

        private void OnDisable()
        {
            Actions.OnDialogueTriggered -= DisplayDialogue;
            Actions.OnTextSpeedChanged -= SetTextSpeed;
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
            char[] currentSentence = dialogue[currentString].ToCharArray();

            CanvasController.ShowCanvas(dialogueCanvas);
            if (_activePrinter == null)
            {
                _activePrinter = PrintDialogue(currentSentence);
                StartCoroutine(_activePrinter);
            }
            
            while (_dialogueEnabled)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StopCoroutine(_activePrinter);
                    currentString++;
                    if (currentString == dialogue.Count)
                    {
                        HideDialog();
                        _activePrinter = null;
                        yield break;
                    }
                    currentSentence = dialogue[currentString].ToCharArray();
                    _activePrinter = PrintDialogue(currentSentence);
                    yield return StartCoroutine(_activePrinter);
                }
                yield return null;
            }
        }

        private IEnumerator PrintDialogue(char[] currentSentence)
        {
            dialogueText.text = "";
            foreach (var chara in currentSentence)
            {
                dialogueText.text += chara;
                yield return _textSpeed;
            }
        }

        private void SetTextSpeed(float speed)
        {
            _textSpeed = new WaitForSeconds(speed);
        }
    }
}
