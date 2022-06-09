using System;
using scr_Player;
using UnityEngine;

namespace scr_UI
{
    public class Menu : MonoBehaviour
    {
        public static bool Paused = false;
        
        [SerializeField] private Canvas pauseMenu;

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
