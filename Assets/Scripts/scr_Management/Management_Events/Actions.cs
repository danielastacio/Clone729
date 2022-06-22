using System;
using scr_NPCs.scr_NPCDialogue;
using UnityEngine;

namespace scr_Management.Management_Events
{
    public static class Actions
    {
        public static Action<CharacterDialogue> OnDialogueTriggered;
        public static Action<float> OnTextSpeedChanged;
        public static Action<string> OnDoorTriggered;
    }
}
