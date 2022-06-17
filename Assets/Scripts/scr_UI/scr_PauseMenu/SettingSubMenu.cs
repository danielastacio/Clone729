using System.Collections.Generic;
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
        
        private void Update()
        {
            foreach (var button in buttons)
            {
                button.onClick.AddListener
                (() =>
                    {
                        _selectedButton = button;
                        ChangeTextSpeed();
                    }
                );
            }
        }

        private void ChangeTextSpeed()
        {
            if (_selectedButton == buttons[0])
            {
                Actions.OnTextSpeedChanged(0.2f);
            }
            else if (_selectedButton == buttons[1])
            {
                Actions.OnTextSpeedChanged(0.1f);
            }
            else if (_selectedButton == buttons[2])
            {
                Actions.OnTextSpeedChanged(0.05f);
            }
        }
    }
}
