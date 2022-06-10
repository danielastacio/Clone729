using UnityEngine;

namespace scr_UI.scr_PauseMenu
{
    public class PauseMenuCanvas : MonoBehaviour
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
