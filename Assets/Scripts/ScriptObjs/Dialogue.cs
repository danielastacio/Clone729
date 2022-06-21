using System;
using System.Collections.Generic;
using UnityEditor;
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
        [CustomEditor(typeof(Dialogue)), CanEditMultipleObjects]
        public class DialogueEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                var dialogue = target as Dialogue;

                using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(dialogue._isInteractable)))
                {
                    if (dialogue._isInteractable)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PrefixLabel("Confirm");
                        dialogue.confirm = EditorGUILayout.TextField(dialogue.confirm);
                        EditorGUILayout.PrefixLabel("Decline");
                        dialogue.decline = EditorGUILayout.TextField(dialogue.decline);
                    }
                }
            }
        }
        
        public List<string> dialogueStrings = new();
        public DialogueType dialogueType;
        
        private bool _isInteractable;
        [HideInInspector] public string confirm;
        [HideInInspector] public string decline;

        private void OnValidate()
        {
            _isInteractable = dialogueType == DialogueType.InteractableTextBox;
        }
    }
}