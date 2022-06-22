using System.Collections;
using System.Collections.Generic;
using scr_Management.Management_Events;
using scr_NPCs.scr_NPCDialogue;
using scr_UI.scr_Utilities;
using ScriptObjs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace scr_UI.scr_DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private Canvas dialogueCanvas;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField] private Button declineButton;
        [SerializeField] private TextMeshProUGUI declineText;

        private int currentLine = 0;
        private WaitForSeconds _textSpeed = new(0.05f);
        private bool _dialogueEnabled;
        private IEnumerator _activeDialogue;
        private IEnumerator _activePrinter;
        private bool _selecting;

        private void OnEnable()
        {
            Actions.OnDialogueTriggered += ActivateDialogueSystem;
            Actions.OnTextSpeedChanged += SetTextSpeed;
        }

        private void OnDisable()
        {
            Actions.OnDialogueTriggered -= ActivateDialogueSystem;
            Actions.OnTextSpeedChanged -= SetTextSpeed;
        }

        private void ActivateDialogueSystem(CharacterDialogue dialogue)
        {
            var currentList = dialogue.characterDialogueStrings[dialogue.timesInteracted];
            _activeDialogue = CreateNewDialogue(currentList);
            StartCoroutine(_activeDialogue);
        }
        
        private void SetTextSpeed(float speed)
        {
            _textSpeed = new WaitForSeconds(speed);
        }

        private IEnumerator CreateNewDialogue(Dialogue currentList)
        {
            currentLine = 0;
            _dialogueEnabled = true;

            var currentItem = currentList.dialogueStrings[currentLine];
            char[] currentSentence = currentItem.dialogueText.ToCharArray();

            CanvasController.ShowCanvas(dialogueCanvas);
            if (_activePrinter == null)
            {
                _activePrinter = PrintDialogue(currentSentence);
                StartCoroutine(_activePrinter);
            }
            
            while (_dialogueEnabled)
            {
                if (currentItem.interactable)
                {
                    currentItem = currentList.dialogueStrings[currentLine];
                    currentSentence = currentItem.dialogueText.ToCharArray();
                    _activePrinter = PrintDialogue(currentSentence);
                    yield return StartCoroutine(_activePrinter);
                    yield return StartCoroutine(ShowButtons(currentItem));
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    StopCoroutine(_activePrinter);
                    currentLine++;
                    
                    currentItem = currentList.dialogueStrings[currentLine];
                    currentSentence = currentItem.dialogueText.ToCharArray();
                    _activePrinter = PrintDialogue(currentSentence);
                    yield return StartCoroutine(_activePrinter);
                }
                else if (currentLine == currentList.dialogueStrings.Count)
                {
                    HideDialog();
                    _activePrinter = null;
                    yield break;
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
        
        private void HideDialog()
        {
            _dialogueEnabled = false;
            _activeDialogue = null;
            CanvasController.HideCanvas(dialogueCanvas);
        }

        private IEnumerator ShowButtons(Dialogue.DialogueString currentItem)
        {
            _selecting = true;
            confirmButton.gameObject.SetActive(true); 
            declineButton.gameObject.SetActive(true);
            confirmText.text = currentItem.confirmText;
            declineText.text = currentItem.declineText;

            while (_selecting)
            {
                confirmButton.onClick.AddListener(() =>
                {
                    Debug.Log("Confirm Pressed!");
                    currentLine++;
                    _selecting = false;
                });
                declineButton.onClick.AddListener(() =>
                {
                    Debug.Log("Decline Pressed");
                    currentLine++;
                    _selecting = false;
                });
                yield return null;
            }
        }

        private void HideButtons()
        {
            confirmButton.gameObject.SetActive(false);
            declineButton.gameObject.SetActive(false);
        }

        
        /*
        [SerializeField] private Canvas interactableDialogueCanvas;
        

        private bool _dialogueEnabled = false;
        private IEnumerator _activePrinter = null;

        

        private void DisplayDialogue(NPCDialogue dialogueObj)
        {
            var currentDialogue = dialogueObj.dialogueStrings[dialogueObj.timesInteracted];
            dialogueObj.IncreaseInteractCount();
            if (currentDialogue.dialogueType == DialogueType.TextBox)
            {
                DisplayDialogueBox(currentDialogue);
            }
            else if (currentDialogue.dialogueType == DialogueType.InteractableTextBox)
            {
                DisplayInteractableDialogueBox(dialogueObj, currentDialogue);
            }
        }

        private void DisplayDialogueBox(Dialogue inputDialogue)
        {
            _dialogueEnabled = true;
            _activeDialogue = UpdateDialogText(inputDialogue.dialogueStrings, dialogueCanvas);
            if (_activeDialogue != null)
            {
                StartCoroutine(_activeDialogue);
            }
        }

        private void DisplayInteractableDialogueBox(NPCDialogue dialogueObj, Dialogue inputDialogue)
        {
            _dialogueEnabled = true;
            _activeDialogue = UpdateDialogText(inputDialogue, inputDialogue.dialogueStrings, interactableDialogueCanvas);
            if (_activeDialogue != null)
            {
                StartCoroutine(_activeDialogue);
            }
        }



        private IEnumerator UpdateDialogText(List<string> dialogue, Canvas canvas)
        {
            var currentString = 0;
            char[] currentSentence = dialogue[currentString].ToCharArray();

            
            
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
        
        private IEnumerator UpdateDialogText(Dialogue dObj, List<string> dialogue, Canvas canvas)
        {
             var currentString = 0;
            char[] currentSentence = dialogue[currentString].ToCharArray();
            confirmText.text = dObj.confirm;
            declineText.text = dObj.decline;

            CanvasController.ShowCanvas(canvas);
            if (_activePrinter == null)
            {
                _activePrinter = PrintDialogue(currentSentence);
                StartCoroutine(_activePrinter);
            }
            
            while (_dialogueEnabled)
            {
                confirmButton.onClick.AddListener(() => Debug.Log("Confirm Pressed!"));
                declineButton.onClick.AddListener(() => Debug.Log("Decline Pressed"));
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
*/
    }
}
