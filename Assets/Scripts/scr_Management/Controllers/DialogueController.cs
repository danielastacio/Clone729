using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Management.Controllers
{
    public static class DialogueController
    {
        public static void ActivateController()
        {
            Interact();
        }
        
        private static void Interact()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Actions.OnInteractPressed(true);
            }
        }
    }
}