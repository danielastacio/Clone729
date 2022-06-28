using scr_Management.Controllers;
using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Management
{
    public enum ControllerType
    {
        Menu,
        Gameplay,
        Dialogue,
        Cutscene
    }
    
    public class InputManager : MonoBehaviour
    {
        private ControllerType _activeController;

        private void OnEnable()
        {
            Actions.OnControllerChanged += SetActiveController;
        }

        private void OnDisable()
        {
            Actions.OnControllerChanged -= SetActiveController;
        }

        private void SetActiveController(ControllerType c)
        {
            _activeController = c;
            if (_activeController != ControllerType.Gameplay)
                GameplayController.DeactivateController();
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
                case ControllerType.Cutscene:
                    break;
            }
            Debug.Log(_activeController);
        }
    }
}