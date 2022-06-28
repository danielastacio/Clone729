using System;
using System.Collections;
using System.Collections.Generic;
using scr_Management.Management_Events;
using scr_UI.scr_Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace scr_UI.scr_PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        public List<Button> buttons = new();
        public List<Canvas> canvases = new();
        private readonly List<Vector2> _buttonStartingPositions = new List<Vector2>();
        private Button _selectedButton;

        private readonly Vector2 _openMenuPos = new Vector2(394, 350);
        private Vector2 _hiddenPos;
        
        [SerializeField] private Canvas statsCanvas;
        [SerializeField] private float buttonMoveTime;

        private void OnEnable()
        {
            Actions.OnSubmenuClose += CloseSubMenu;
        }

        public void Start()
        {
            foreach (var button in buttons)
            {
                _buttonStartingPositions.Add(button.GetComponent<RectTransform>().anchoredPosition);
                button.image.color = Colors.DefaultMenuButtonColor;
            }
        }

        public void Update()
        {
            foreach (var button in buttons)
            {
                button.onClick.AddListener
                (() =>
                    {
                        _selectedButton = button;
                        if (_selectedButton.name.Equals("ExitButton"))
                        {
                            Actions.OnMenuClose();
                        }
                        else if (_selectedButton != null)
                        {
                            StopAllCoroutines();
                            CanvasController.HideCanvas(statsCanvas);
                            ActivateSelectedMenu(_selectedButton);
                            HideButtons();
                        }
                    }
                );
            }
        }

        private void ActivateSelectedMenu(Button btn)
        {
            Actions.OnSubmenuOpen();
            btn.image.color = Colors.HighlightedMenuButtonColor;
            _selectedButton.GetComponent<MenuButtonHover>().enabled = false;
            CanvasController.ShowCanvas(canvases[buttons.IndexOf(btn)]);
            StartCoroutine(MoveButtons(btn, _openMenuPos));
        }

        private void HideButtons()
        {
            foreach (var button in buttons)
            {
                if (button != _selectedButton)
                {
                    _hiddenPos = new Vector2(-248, button.GetComponent<RectTransform>().anchoredPosition.y);
                    StartCoroutine(MoveButtons(button, _hiddenPos));
                }
            }
        }

        private void CloseSubMenu()
        {
            StopAllCoroutines();
            CanvasController.ShowCanvas(statsCanvas);
            CanvasController.HideCanvas(canvases[buttons.IndexOf(_selectedButton)]);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].image.color = Colors.DefaultMenuButtonColor;
                StartCoroutine(MoveButtons(buttons[i], _buttonStartingPositions[i]));
            }
            _selectedButton.GetComponent<MenuButtonHover>().enabled = true;
            _selectedButton = null;
        }

        private IEnumerator MoveButtons(Button btn, Vector2 endPos)
        {
            var currentPos = btn.GetComponent<RectTransform>().anchoredPosition;
            var startPos = currentPos;
            var transitionTime = 0f;

            while (Vector2.Distance(startPos, endPos) > 0.1f)
            {
                currentPos = Vector2.Lerp(startPos, endPos, transitionTime);

                transitionTime += buttonMoveTime;

                btn.GetComponent<RectTransform>().anchoredPosition = currentPos;

                yield return null;
            }
        }
    }
}
