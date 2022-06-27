using System;
using UnityEngine;

namespace scr_Management.Controllers
{
    public static class DialogueController
    {
        public static Action<bool> OnInteractPressed;

        public static void ActivateController()
        {
            Interact();
        }
        
        private static void Interact()
        {
            OnInteractPressed(Input.GetKeyDown(KeyCode.E));
        }
    }
}