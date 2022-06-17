using System.Collections.Generic;
using UnityEngine;

namespace ScriptObjs
{
    public enum DialogueType
    {
        Bubble,
        TextBox,
        InteractableTextBox
    }
    
    [CreateAssetMenu (menuName = "New Dialogue", fileName = "New Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public List<string> dialogueStrings = new();
        public DialogueType dialogueType;
    }
}
