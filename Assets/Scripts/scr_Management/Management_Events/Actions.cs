using System;
using scr_NPCs.scr_NPCDialogue;
using UnityEngine;

namespace scr_Management.Management_Events
{
    public static class Actions
    {
        public static Action<NPCDialogue> OnDialogueTriggered;
        public static Action<float> OnTextSpeedChanged;
    }
}
