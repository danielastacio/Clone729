using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace scr_UI
{
    public class MenuButtons : MonoBehaviour
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
            }
        }

        public void Update()
        {
            foreach (var button in buttons)
            {
                button.onClick.AddListener(() =>
                    {
                        _selectedButton = button;
                        if (_selectedButton.name.Equals("ExitButton"))
                        {
                            Application.Quit();
                        }
                        else
                        {
                            StartCoroutine(MoveButton(_selectedButton));
                            selectedMenuCanvas.GetComponent<Canvas>().enabled = true;
                            StartCoroutine(HideButtons());
                        }
                    }
                );
            }
        }

        private IEnumerator MoveButton(Button btn)
        {
            var currentPos = btn.GetComponent<RectTransform>().anchoredPosition;
            var startPos = currentPos;
            var transitionTime = 0f;

            
            while (currentPos.y < _openMenuPos.y)
            {
                currentPos = btn.GetComponent<RectTransform>().anchoredPosition;
                
                currentPos = new Vector2(currentPos.x,
                    Mathf.Lerp(startPos.y, _openMenuPos.y, transitionTime));
                transitionTime += 0.1f;
                
                btn.GetComponent<RectTransform>().anchoredPosition = currentPos;

                yield return null;
            }
        }

        private IEnumerator HideButtons()
        {
            foreach (var button in buttons)
            {
                if (button != _selectedButton)
                {
                    var currentPos = button.GetComponent<RectTransform>().anchoredPosition;
                    var startPos = currentPos;
                    var transitionTime = 0f;
                    
                    _hiddenPos = new Vector2(-248, currentPos.y);

                    while (currentPos.x > _hiddenPos.x)
                    {
                        currentPos = button.GetComponent<RectTransform>().anchoredPosition;
                        
                        currentPos = new Vector2(Mathf.Lerp(startPos.x, _hiddenPos.x, transitionTime), 
                            currentPos.y);
                        transitionTime += 0.1f;
                        
                        button.GetComponent<RectTransform>().anchoredPosition = currentPos;

                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}
