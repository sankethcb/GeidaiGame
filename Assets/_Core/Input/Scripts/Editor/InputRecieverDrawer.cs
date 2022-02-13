using UnityEditor;
using UnityEngine;

namespace Core.Input
{

  // [CustomPropertyDrawer(typeof(InputReciever), true)]
    public class InputRecieverDrawer : PropertyDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Input Action"), EditorStyles.whiteLabel);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var amountRect = new Rect(position.x, position.y, position.width, 20);
            var unitRect = new Rect(position.x, position.y + 25, position.width, 20);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels

            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("inputActionData"), GUIContent.none);

            position.x = 50;
            position.y += 25;

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("On Input Started"), EditorStyles.miniBoldLabel);
            EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("onStarted"), GUIContent.none);
            unitRect.y += 25;
            position.y += 25;

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("On Input Performed"), EditorStyles.miniBoldLabel);
            EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("onPerformed"), GUIContent.none);
            unitRect.y += 25;
            position.y += 25;

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("On Input Cancelled"), EditorStyles.miniBoldLabel);
            EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("onCancelled"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 100;
        }
        
    }

}