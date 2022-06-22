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
        public static Action<float> OnTextSpeedChanged;
        
        // ActionTriggers
        public static Action<CharacterDialogue> OnDialogueTriggered;
        public static Action<string> OnDoorTriggered;
    }
}
