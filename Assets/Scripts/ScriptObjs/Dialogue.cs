using System.Collections.Generic;
using UnityEngine;

namespace ScriptObjs
{
    [CreateAssetMenu(menuName = "New Dialogue", fileName = "New Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [System.Serializable]
        public class DialogueString
        {
            public string dialogueText;
            public bool interactable;
            public string confirmText;
            public string declineText;
        }

        public List<DialogueString> dialogueStrings = new();
    }
}