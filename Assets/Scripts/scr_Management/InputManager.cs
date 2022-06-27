using System;
using scr_Management.Controllers;
using scr_Management.Management_Events;
using UnityEditor;
using UnityEngine;

namespace scr_Management
{
    public enum ControllerType
    {
        Menu,
        Gameplay,
        Dialogue
    }
    
    public class InputManager : MonoBehaviour
    {
        private ControllerType _activeController;
        private ControllerType _lastController;

        private void OnEnable()
        {
            Actions.OnControllerChanged += SetActiveController;
        }

        private void OnDisable()
        {
            Actions.OnControllerChanged -= SetActiveController;
        }

        private void SetActiveController(ControllerType t)
        {
            _lastController = _activeController;
            _activeController = t;
        }

        private void Update()
        {
            switch (_activeController)
            {
                case ControllerType.Dialogue:
                    DialogueController.ActivateController();
                    break;
                case ControllerType.Gameplay:
                    GameplayController.ActivateController();
                    break;
                case ControllerType.Menu:
                    MenuController.ActivateController();
                    break;
            }
        }
    }
}