using UnityEngine;
using UnityEditor;


namespace Utilities
{
    [CustomPropertyDrawer(typeof(CollectionLabelAttribute))]
    public class CollectionLabelDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CollectionLabelAttribute propertyName = attribute as CollectionLabelAttribute;

            int pathIndex = property.propertyPath.IndexOf(fieldInfo.Name);

            string path = property.propertyPath.Substring(pathIndex, property.propertyPath.Length - pathIndex);

            int index = System.Convert.ToInt32(path.Substring(path.IndexOf("[")).Replace("[", "").Replace("]", ""));

            path = property.propertyPath;
            path = path.Substring(0, path.LastIndexOf('.'));

            SerializedProperty array = property.serializedObject.FindProperty(path);

            SerializedProperty instance = array.GetArrayElementAtIndex(index).FindPropertyRelative(propertyName.elementName);
            label.text = instance.objectReferenceValue == null ? "None" : instance.objectReferenceValue.name;

            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}