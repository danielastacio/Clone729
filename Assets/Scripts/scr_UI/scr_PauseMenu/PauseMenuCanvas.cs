using System;
using scr_Management;
using scr_Management.Management_Events;
using scr_UI.scr_Utilities;
using UnityEngine;

namespace scr_UI.scr_PauseMenu
{
    public class PauseMenuCanvas : MonoBehaviour
    {
        private static PauseMenuCanvas Instance;
        
        [SerializeField] private Canvas pauseMenu;

        private void OnEnable()
        {
            Actions.OnMenuOpen += PauseGame;
            Actions.OnMenuClose += UnpauseGame;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;
        }

        private void PauseGame()
        {
            CanvasController.ShowCanvas(pauseMenu);
            Actions.OnControllerChanged(ControllerType.Menu);
            Time.timeScale = 0;
        }

        private void UnpauseGame()
        {
            CanvasController.HideCanvas(pauseMenu);
            Actions.OnControllerChanged(ControllerType.Gameplay);
            Time.timeScale = 1;
        }
    }
}
