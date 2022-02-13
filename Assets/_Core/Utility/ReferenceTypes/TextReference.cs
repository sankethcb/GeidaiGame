using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Utilities
{
    [CreateAssetMenu(fileName = "TextReference", menuName = "Utilities/Reference Data/Text", order = 1)]
    public class TextReference : ScriptableObject
    {
        [SerializeField, TextArea(3, 20)]
        public string value = "";

        public void SetValue(string val) => value = val;
    }
}