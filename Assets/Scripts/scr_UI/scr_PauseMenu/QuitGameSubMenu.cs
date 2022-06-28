using scr_Management.Management_Events;
using UnityEngine;
using UnityEngine.UI;

namespace scr_UI.scr_PauseMenu
{
    public class QuitGameSubMenu : MonoBehaviour
    {
        public Button yes;
        public Button no;
        private bool _clicked = false;

        private void Update()
        {
            yes.onClick.AddListener(QuitGame);
            no.onClick.AddListener(CloseSubMenu);
        }

        private void QuitGame()
        {
            Application.Quit();
            Debug.Log("QUIT");
        }

        private void CloseSubMenu()
        {
            Actions.OnSubmenuClose();
        }
    }
}