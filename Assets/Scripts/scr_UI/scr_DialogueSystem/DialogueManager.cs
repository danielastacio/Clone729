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
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField] private TextMeshProUGUI declineText;
        [SerializeField] private List<Button> buttons = new();

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
            // TODO: CLEAN THIS UP PLS FUTURE TYLER
            var currentLine = 0;
            _dialogueEnabled = true;

            var currentItem = currentList.dialogueStrings[currentLine];
            char[] currentSentence = currentItem.dialogueText.ToCharArray();

            IEnumerator showButtons;

            CanvasController.ShowCanvas(dialogueCanvas);
            if (_activePrinter == null)
            {
                _activePrinter = PrintDialogue(currentSentence);
                StartCoroutine(_activePrinter);
            }
            
            while (_dialogueEnabled)
            {
                Debug.Log(currentLine);
                if (currentItem.interactable)
                {
                    showButtons = ShowButtons(currentItem);
                    yield return StartCoroutine(showButtons);
                    currentLine++;
                    _activePrinter = null;
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    StopCoroutine(_activePrinter);
                    currentLine++;
                    _activePrinter = null;
                }

                if (currentLine == currentList.dialogueStrings.Count)
                {
                    HideDialog();
                    yield break;
                }
                
                if (_activePrinter == null)
                {
                    currentItem = currentList.dialogueStrings[currentLine];
                    currentSentence = currentItem.dialogueText.ToCharArray();
                    _activePrinter = PrintDialogue(currentSentence);
                    StartCoroutine(_activePrinter);
                }       
                yield return null;
            }
        }

        private IEnumerator CreateNewBubbleDialogue(Dialogue currentList)
        {
            
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
            _activePrinter = null;
            CanvasController.HideCanvas(dialogueCanvas);
        }

        private IEnumerator ShowButtons(Dialogue.DialogueString currentItem)
        {
            Button selectedButton = null;
            buttons[0].gameObject.SetActive(true); 
            buttons[1].gameObject.SetActive(true);
            confirmText.text = currentItem.confirmText;
            declineText.text = currentItem.declineText;

            while (selectedButton == null)
            {
                foreach (var button in buttons)   
                {
                    button.onClick.AddListener(() =>
                    {
                        selectedButton = button;
                        if (button == buttons[0])
                        {
                            currentItem.OnConfirmInteraction();
                        }
                        else if (button == buttons[1])
                        {
                            currentItem.OnDeclineInteraction();
                        }
                    });
                }

                yield return null;
            }
            HideButtons();
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
