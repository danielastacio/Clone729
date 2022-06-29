using scr_Management.Management_Events;
using UnityEngine;

namespace scr_Management.Controllers
{
    public static class GameplayController
    {
        private static float _horizontal;
        private static bool _jump;
        private static bool _crouch;
        private static bool _roll;
        private static bool _shoot;
        private static bool _melee;
        private static bool _interact;
        private static bool _menu;
        
        public static void ActivateController()
        {
            Inputs();
            MovementMethods();
            CombatMethods();
            Interact();
            Menu();
        }

        public static void DeactivateController()
        {
            _horizontal = 0f;
            _jump = false;
            _crouch = false;
            _roll = false;
            _shoot = false;
            _melee = false;
            _interact = false;
            _menu = false;
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

        private static void Inputs()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _jump = Input.GetKeyDown(KeyCode.Space);
            _crouch = Input.GetKeyDown(KeyCode.S);
            _roll = Input.GetKeyDown(KeyCode.K);
            _shoot = Input.GetMouseButtonUp(0);
            _melee = Input.GetMouseButtonUp(1);
            _interact = Input.GetKeyDown(KeyCode.E);
            _menu = Input.GetKeyDown(KeyCode.Escape);
        }

        private static void Move()
        {
            Actions.OnMoveInput(_horizontal);
        }

        private static void Jump()
        {
            Actions.OnJumpPressed(_jump);
        }

        private static void Crouch()
        {
            Actions.OnCrouchPressed(_crouch);
        }

        private static void Roll()
        {
            Actions.OnRollPressed(_roll);
        }

        private static void Shoot()
        {
            Actions.OnShootPressed(_shoot);
        }

        private static void Melee()
        {
            Actions.OnMeleePressed(_melee);
        }

        private static void Interact()
        {
            Actions.OnInteractPressed(_interact);
        }

        private static void Menu()
        {
            if (_menu)
            {
                Actions.OnMenuOpen();
            }
        }
    }
}