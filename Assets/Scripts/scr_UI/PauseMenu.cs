using System.Collections;
using System.Collections.Generic;
using scr_Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace scr_UI
{
    public class PauseMenu : MonoBehaviour
    {
        public List<Button> buttons = new List<Button>();
        public List<Canvas> canvases = new List<Canvas>();
        private readonly List<Vector2> _buttonStartingPositions = new List<Vector2>();
        private Button _selectedButton;

        private readonly Vector2 _openMenuPos = new Vector2(394, 350);
        private Vector2 _hiddenPos;

        [SerializeField] private Canvas statsCanvas;

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
                            CloseMenu();
                        }
                        else if (_selectedButton != null)
                        {
                            StopAllCoroutines();
                            ShowHideCanvas(statsCanvas, false);
                            ActivateSelectedMenu(_selectedButton);
                            HideButtons();
                        }
                    }
                );
            }
            
            if (Input.GetKeyDown(KeyCode.Escape) && _selectedButton != null)
            {
                ResetButtonPositions();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && _selectedButton == null)
            {
                CloseMenu();
            }
        }

        private void CloseMenu()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        private void ActivateSelectedMenu(Button btn)
        {
            btn.image.color = Colors.HighlightedMenuButtonColor;
            _selectedButton.GetComponent<MenuButtonHover>().enabled = false;
            ShowHideCanvas(canvases[buttons.IndexOf(btn)], true);
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

        private void ResetButtonPositions()
        {
            StopAllCoroutines();
            ShowHideCanvas(statsCanvas, true);
            ShowHideCanvas(canvases[buttons.IndexOf(_selectedButton)], false);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].image.color = Colors.DefaultMenuButtonColor;
                StartCoroutine(MoveButtons(buttons[i], _buttonStartingPositions[i]));
            }
            _selectedButton.GetComponent<MenuButtonHover>().enabled = true;
            _selectedButton = null;
        }

        private void ShowHideCanvas(Canvas canvas, bool canvasActive)
        {
            canvas.gameObject.SetActive(canvasActive);
        }

        private IEnumerator MoveButtons(Button btn, Vector2 endPos)
        {
            var currentPos = btn.GetComponent<RectTransform>().anchoredPosition;
            var startPos = currentPos;
            var transitionTime = 0f;

            while (Vector2.Distance(startPos, endPos) > 0.1f)
            {
                currentPos = btn.GetComponent<RectTransform>().anchoredPosition;

                currentPos = Vector2.Lerp(startPos, endPos, transitionTime);

                transitionTime += 0.1f;

                btn.GetComponent<RectTransform>().anchoredPosition = currentPos;

                yield return null;
            }
        }
    }
}