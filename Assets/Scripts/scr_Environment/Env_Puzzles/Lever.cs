using scr_Interfaces;
using scr_Management;
using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Environment.Env_Puzzles
{
    public class Lever : MonoBehaviour, IInteractable
    {
        public TriggerType interaction;
        [SerializeField] private string interactableId;
    
        [SerializeField] private Mirror mirror;
        [SerializeField] private LayerMask whatIsPlayer;

        private void LeverInteract()
        {
            if (interaction == TriggerType.Mirror)
            {
                FlipMirror();
            }
            else if (interaction == TriggerType.Door)
            {
                ChangeDoorState();
            }
        }
    
        private void FlipMirror()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (mirror.currentDirection != Mirror.BeamDirections.right)
                {
                    mirror.currentDirection = Mirror.BeamDirections.right;
                }

                else
                {
                    mirror.currentDirection = Mirror.BeamDirections.left;
                }
            }
        }

        private void ChangeDoorState()
        {
            Actions.OnDoorTriggered(interactableId);
            GetComponent<SpriteRenderer>().color = Color.green;
        }

        public void OnInteract()
        {
            LeverInteract();
        }
    }
}
