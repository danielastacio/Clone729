using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using scr_Management;
using scr_Management.Controllers;
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
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField] private TextMeshProUGUI declineText;
        [SerializeField] private List<Button> buttons = new();

        private WaitForSeconds _textSpeed = new(0.05f);
        private bool _dialogueEnabled;
        private IEnumerator _activePrinter;
        private bool _selecting;

        private int _currentLine = 0;
        private char[] _currentSentence;
        private Dialogue _currentList;
        private Dialogue.DialogueString _currentItem;
        private Canvas _activeCanvas;

        private void OnEnable()
        {
            Actions.OnDialogueTriggered += ActivateDialogueSystem;
            DialogueController.OnInteractPressed += CallNextLine;
            Actions.OnTextSpeedChanged += SetTextSpeed;
        }

        private void OnDisable()
        {
            Actions.OnDialogueTriggered -= ActivateDialogueSystem;
            DialogueController.OnInteractPressed -= CallNextLine;
            Actions.OnTextSpeedChanged -= SetTextSpeed;
        }

        private void Update()
        {
            if (_activePrinter == null && _dialogueEnabled)
            {
                _activePrinter = PrintDialogue(_currentSentence);
                StartCoroutine(_activePrinter);
                
                if (_currentItem.interactable)
                {
                    ShowButtons(_currentItem);
                }
            }
        }

        private void ActivateDialogueSystem(CharacterDialogue dialogue)
        {
            _currentList = dialogue.characterDialogueStrings[dialogue.timesInteracted];
            
            if (_currentList.dialogueType == Dialogue.DialogueType.TextBox)
            {
                _activeCanvas = dialogueCanvas;
                StartDialogue();
            }
            else if (_currentList.dialogueType == Dialogue.DialogueType.TextBox)
            {
                // Set up bubble dialogue
                Debug.Log("No bubble dialogue!");
                HideDialog();
            }
        }

        private void StartDialogue()
        {
            dialogueText.text = "";
            Actions.OnControllerChanged(ControllerType.Dialogue);
            SetNextSentence();
            _dialogueEnabled = true;
            CanvasController.ShowCanvas(_activeCanvas);
        }

        private void HideDialog()
        {
            _currentLine = 0;
            _activePrinter = null;
            _dialogueEnabled = false;
            CanvasController.HideCanvas(_activeCanvas);
            Actions.OnControllerChanged(ControllerType.Gameplay);
        }

        private void SetNextSentence()
        {
            if (_currentLine == _currentList.dialogueStrings.Count)
            {
                HideDialog();
                return;
            }
            _currentItem = _currentList.dialogueStrings[_currentLine];
            _currentSentence = _currentItem.dialogueText.ToCharArray();
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

        private void CallNextLine(bool interactInput)
        {
            // This method is for calling the next line with E
            if (interactInput && !_currentItem.interactable)
            {
                CallNextLine();
            }
        }

        private void CallNextLine()
        {
            // This method is to be called directly from Confirm/Decline button presses
            if (_activePrinter != null)
            {
                StopCoroutine(_activePrinter);
            }
            _currentLine++;
            SetNextSentence();
            _activePrinter = null;
        }

        private void SetTextSpeed(float speed)
        {
            _textSpeed = new WaitForSeconds(speed);
        }

        private void ShowButtons(Dialogue.DialogueString currentItem)
        {
            buttons[0].gameObject.SetActive(true);
            buttons[1].gameObject.SetActive(true);
            confirmText.text = currentItem.confirmText;
            declineText.text = currentItem.declineText;

            foreach (var button in buttons)
            {
                button.onClick.AddListener(() =>
                {
                    if (button == buttons[0])
                    {
                        currentItem.OnConfirmInteraction();
                        CallNextLine();
                        HideButtons();

                    }
                    else if (button == buttons[1])
                    {
                        currentItem.OnDeclineInteraction();
                        CallNextLine();
                        HideButtons();
                    }
                });
            }
        }

        private void HideButtons()
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
