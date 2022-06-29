using System.Collections.Generic;
using scr_Management;
using scr_Management.Management_Events;
using scr_UI.scr_Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace scr_UI.scr_PauseMenu
{
    public class SettingSubMenu : MonoBehaviour
    {
        public List<Button> buttons = new();
        private Button _selectedButton;
        private Button _selectedTextSpeed;

        [SerializeField] private float _slow = 0.1f;
        [SerializeField] private float _medium = 0.05f; 
        [SerializeField] private float _fast = 0.02f;

        private readonly Dictionary<Button, float> _textSpeeds = new();

        private float _currentTextSpeed;

        private void OnEnable()
        {
            Actions.OnTextSpeedChanged += SetCurrentTextSpeed;
        }

        private void Start()
        {
            _textSpeeds.Add(buttons[0], _slow);
            _textSpeeds.Add(buttons[1], _medium);
            _textSpeeds.Add(buttons[2], _fast);
            SetCurrentTextSpeed(Settings.TextSpeed);
        }

        private void Update()
        {
            foreach (var button in buttons)
            {
                button.onClick.AddListener(() => Actions.OnTextSpeedChanged(_textSpeeds[button]));
            }
        }
        
        private void SetCurrentTextSpeed(float s)
        {
            _currentTextSpeed = s;
            HighlightSelectedButton(_currentTextSpeed);
        }

        private void HighlightSelectedButton(float s)
        {
            foreach (var button in _textSpeeds)
            {
                if (button.Value.Equals(s))
                {
                    button.Key.GetComponent<MenuButtonHover>().SetHighlighted();
                    button.Key.GetComponent<MenuButtonHover>().enabled = false;
                }
                else
                {
                    button.Key.GetComponent<MenuButtonHover>().enabled = true;
                    button.Key.GetComponent<MenuButtonHover>().SetDefault();
                }
            }
        }
    }
}
