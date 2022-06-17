using System;
using UnityEngine;

namespace scr_UI
{
    public class Menu : MonoBehaviour
    {
        public static bool Paused = false;
        
        [SerializeField] private Canvas pauseMenu;
        
        protected readonly Color32 DefaultColor = new Color32(159, 159, 159, 255);
        protected readonly Color32 HighlightedColor = new Color32(255, 255, 255, 255);
        
        public void PauseGame()
        {
            if (!pauseMenu.gameObject.activeSelf)
            {
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
