using System.Collections;
using System.Collections.Generic;
using DialogSOs;
using scr_Management.Management_Events;
using scr_NPCs.scr_NPCDialog;
using scr_UI.scr_Utilities;
using TMPro;
using UnityEngine;

namespace scr_UI.scr_DialogSystem
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private Canvas dialogCanvas;
        [SerializeField] private TextMeshProUGUI dialogText;
        private bool _dialogEnabled = false;
        private IEnumerator _activeDialog = null;

        private void OnEnable()
        {
            Actions.OnDialogTriggered += DisplayDialog;
        }

        private void OnDisable()
        {
            Actions.OnDialogTriggered -= DisplayDialog;
        }

        private void DisplayDialog(NPCDialog inputDialog)
        {
            _dialogEnabled = true;
            _activeDialog = UpdateDialogText(inputDialog.dialogStrings[inputDialog.timesInteracted].dialogStrings);
            if (_activeDialog != null)
            {
                StartCoroutine(_activeDialog);
                inputDialog.IncreaseInteractCount();
            }
        }

        private void HideDialog()
        {
            _dialogEnabled = false;
            _activeDialog = null;
            CanvasController.HideCanvas(dialogCanvas);
        }

        private IEnumerator UpdateDialogText(List<string> dialog)
        {
            var currentString = 0;
            CanvasController.ShowCanvas(dialogCanvas);
            
            dialogText.text = dialog[currentString];
            
            while (_dialogEnabled)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentString++;
                    if (currentString == dialog.Count)
                    {
                        HideDialog();
                    }
                    dialogText.text = dialog[currentString];
                }
                yield return null;
            }
        }
    }
}
