using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

namespace DialogSOs
{
    [CreateAssetMenu (menuName = "New Dialog", fileName = "New Dialog")]
    public class Dialog : ScriptableObject
    {
        public List<string> dialogStrings = new();
    }
}
