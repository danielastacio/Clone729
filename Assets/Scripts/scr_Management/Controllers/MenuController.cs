using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Management.Controllers
{
    public static class MenuController
    {
        private static bool _inSubmenu;

        public static void ActivateController()
        {
            ListenForEvent();
            CloseMenu();
        }
        
        private static void ListenForEvent()
        {
            Actions.OnSubmenuOpen += ToggleSubmenuControl;
        }
        
        private static void CloseMenu()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_inSubmenu)
                {
                    Actions.OnMenuClose();
                }
                else if (_inSubmenu)
                {
                    _inSubmenu = false;
                    Actions.OnSubmenuClose();
                }
            }
        }

        private static void ToggleSubmenuControl()
        {
            _inSubmenu = true;
        }
    }
}