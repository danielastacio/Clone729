using System;
using scr_NPCs.scr_NPCDialogue;

namespace scr_Management.Management_Events
{
    public enum TriggerType
    {
        None,
        Mirror,
        Door
    }
    
    public static class Actions
    {
        // Setting Actions
        public static Action<float> OnTextSpeedChanged;
        
        // Trigger Actions
        public static Action<CharacterDialogue> OnDialogueTriggered;
        public static Action<string> OnConfirmTriggered;
        public static Action<string> OnDoorTriggered;
        
        // Controller Actions
        public static Action<ControllerType> OnControllerChanged;
        public static Action<float> OnMoveInput;
        public static Action<bool> OnJumpPressed;
        public static Action<bool> OnCrouchPressed;
        public static Action<bool> OnRollPressed;
        public static Action<bool> OnShootPressed;
        public static Action<bool> OnMeleePressed;
        public static Action<bool> OnInteractPressed;
        public static Action OnMenuOpen;
        
        // Menu Actions
        public static Action OnMenuClose;
        public static Action OnSubmenuOpen;
        public static Action OnSubmenuClose;
        
    }
}
