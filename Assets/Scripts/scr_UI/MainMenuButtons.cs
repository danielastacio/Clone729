using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace scr_UI
{
    public class MainMenuButtons : MonoBehaviour
    {
        public Canvas selectedMenuCanvas;

        public List<Button> buttons = new List<Button>();
        private List<Vector2> _buttonStartingPositions = new List<Vector2>();
        private Button _selectedButton;

        private readonly Vector2 _openMenuPos = new Vector2(394, 350);
        private Vector2 _hiddenPos;

        public void Start()
        {
            foreach (var button in buttons)
            {
                _buttonStartingPositions.Add(button.GetComponent<RectTransform>().anchoredPosition);
                selectedMenuCanvas.gameObject.SetActive(false);
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
                            Application.Quit();
                        }
                        else if (_selectedButton != null)
                        {
                            StopAllCoroutines();
                            MoveSelectedButton(_selectedButton);
                            selectedMenuCanvas.gameObject.SetActive(true);
                            HideButtons();
                        }
                    }
                );
            }

            if (Input.GetKeyDown(KeyCode.Escape) && _selectedButton != null)
            {
                ResetButtonPositions();
                _selectedButton = null;
            }
        }

        private void MoveSelectedButton(Button btn)
        {
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
            selectedMenuCanvas.gameObject.SetActive(false);
            for (int i = 0; i < buttons.Count; i++)
            {
                StartCoroutine(MoveButtons(buttons[i], _buttonStartingPositions[i]));
            }
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