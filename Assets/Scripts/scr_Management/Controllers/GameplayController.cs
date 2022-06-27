using scr_Interfaces;
using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Management.Controllers
{
    public static class GameplayController
    {
        public static void ActivateController()
        {
            MovementMethods();
            CombatMethods();
            Interact();
            Menu();
        }

        private static void MovementMethods()
        {
            Move();
            Jump();
            Crouch();
            Roll();
        }

        private static void CombatMethods()
        {
            Shoot();
            Melee();
        }

        private static void Move()
        {
            Actions.OnMoveInput(Input.GetAxisRaw("Horizontal"));
        }

        private static void Jump()
        {
            Actions.OnJumpPressed(Input.GetKeyDown(KeyCode.Space));
        }

        private static void Crouch()
        {
            Actions.OnCrouchPressed(Input.GetKey(KeyCode.S));
        }

        private static void Roll()
        {
            Actions.OnRollPressed(Input.GetKeyDown(KeyCode.K));
        }

        private static void Shoot()
        {
            Actions.OnShootPressed(Input.GetMouseButtonUp(0));
        }

        private static void Melee()
        {
            Actions.OnMeleePressed(Input.GetKeyDown(KeyCode.L));
        }

        private static void Interact()
        {
            Actions.OnInteractPressed(Input.GetKeyDown(KeyCode.E));
        }

        private static void Menu()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Actions.OnMenuOpen();
            }
        }
    }
}