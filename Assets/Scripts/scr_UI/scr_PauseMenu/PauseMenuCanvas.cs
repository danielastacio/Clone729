using System;
using scr_UI.scr_Utilities;
using UnityEngine;

namespace scr_UI.scr_PauseMenu
{
    public class PauseMenuCanvas : MonoBehaviour
    {
        public static PauseMenuCanvas Instance { get; private set; }
        
        [SerializeField] private Canvas pauseMenu;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;
        }

        public void PauseGame()
        {
            CanvasController.ShowCanvas(pauseMenu);
            Time.timeScale = 0;
        }
    }
}
