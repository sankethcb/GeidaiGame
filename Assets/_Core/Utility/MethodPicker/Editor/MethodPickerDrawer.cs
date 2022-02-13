using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Utilities
{
    [CustomPropertyDrawer(typeof(MethodPicker), true)]
    public class MethodPickerDrawer : PropertyDrawer
    {
        public static readonly BindingFlags FLAGS = BindingFlags.Public | BindingFlags.Instance;

        Dictionary<string, Tuple<UnityEngine.Object, MethodInfo>> _lookUp = new Dictionary<string, Tuple<UnityEngine.Object, MethodInfo>>();

        string _selectedRaw = "";
        string _selected = "";
        string _default = "No Method";

        Rect _objRect;
        Rect _dropDownRect;

        SerializedProperty _property;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if ((!fieldInfo.FieldType.IsGenericType || fieldInfo.FieldType.GetGenericTypeDefinition() != typeof(List<>)) && !fieldInfo.FieldType.IsArray)
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            _property = property;

            SerializedProperty target = property.FindPropertyRelative("target");
            SerializedProperty methodName = property.FindPropertyRelative("key");
            SerializedProperty script = property.FindPropertyRelative("behaviour");

            _objRect = new Rect(position.x, position.y, position.width * 0.4f, position.height);
            _dropDownRect = new Rect(position.x + position.width * 0.4f + 5, position.y, position.width * 0.6f, position.height);

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(_objRect, target, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
                ResetValues();

            DrawDropDown(_dropDownRect, target, script, methodName);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
            _selected = _selectedRaw = "";
        }

        void DrawDropDown(Rect dropDownRect, SerializedProperty target, SerializedProperty script, SerializedProperty methodName)
        {
            _lookUp.Clear();

            if (target.objectReferenceValue != null)
            {
                _selected = methodName.stringValue;

                if (_selected == "" || _selected == _default || _selected == null)
                    _selected = _selectedRaw = _default;
                else
                    _selectedRaw = script.objectReferenceValue.GetType().Name + "." + _selected;

                if (EditorGUI.DropdownButton(dropDownRect, new GUIContent(_selectedRaw), FocusType.Passive))
                {
                    GenericMenu dropDown = new GenericMenu();

                    AddItem(dropDown, _default, _default);
                    dropDown.AddSeparator("");

                    BuildDropDown(target.objectReferenceValue, dropDown);

                    dropDown.DropDown(dropDownRect);
                }
            }
            else
            {
                _selected = _default;
            }
        }

        void OnSelect(object value)
        {
            _selectedRaw = value.ToString();
            _selected = _selectedRaw.Replace("/", ".");

            if (_selectedRaw != _default || _selectedRaw != string.Empty)
            {
                _property.FindPropertyRelative("key").stringValue = _lookUp[_selectedRaw].Item2.Name;
                _property.FindPropertyRelative("behaviour").objectReferenceValue = _lookUp[_selectedRaw].Item1;

                _property.FindPropertyRelative("key").serializedObject.ApplyModifiedProperties();
                _property.FindPropertyRelative("behaviour").serializedObject.ApplyModifiedProperties();
            }
            else
                ResetValues();
        }

        void ResetValues()
        {
            _lookUp.Clear();
            _selected = "";
            _selectedRaw = _default;
            _property.FindPropertyRelative("key").stringValue = "";
            _property.FindPropertyRelative("behaviour").objectReferenceValue = null;

            _property.FindPropertyRelative("key").serializedObject.ApplyModifiedProperties();
            _property.FindPropertyRelative("behaviour").serializedObject.ApplyModifiedProperties();
        }

        void AddItem(GenericMenu dropDown, string path, string value)
        {
            dropDown.AddItem(new GUIContent(path), path == _selectedRaw, OnSelect, value);
        }

        void BuildDropDown(UnityEngine.Object target, GenericMenu dropDown)
        {
            UnityEngine.Object[] behaviours;

            if (target as ScriptableObject != null)
            {
                Debug.Log((target as ScriptableObject).GetType());

                behaviours = new UnityEngine.Object[1];
                behaviours[0] = target as UnityEngine.Object;
            }
            else if (target as MonoBehaviour != null || target as GameObject != null)
            {
                if (target as MonoBehaviour != null)
                {
                    target = (target as MonoBehaviour).gameObject;
                }

                behaviours = Array.ConvertAll((target as GameObject).GetComponents<MonoBehaviour>(), item => item as UnityEngine.Object);
            }
            else
                return;


            string path;
            System.Type type;

            foreach (UnityEngine.Object behaviour in behaviours)
            {
                type = behaviour.GetType();
                path = type.Name + "/";
                string identifier;
                foreach (var method in type.GetMethods(FLAGS).Where(m => !m.IsSpecialName))
                {
                    identifier = path + method.Name;

                    if (_lookUp.ContainsKey(identifier))
                        continue;

                    if (FilterTypes(method))
                        AddItem(dropDown, identifier, identifier);

                    _lookUp.Add(identifier, new Tuple<UnityEngine.Object, MethodInfo>(behaviour, method));
                }
            }
        }

        bool FilterTypes(MethodInfo method)
        {
            Type parameterType;
            if (fieldInfo.FieldType.IsArray)
                parameterType = fieldInfo.FieldType.GetElementType();
            else if ((!fieldInfo.FieldType.IsGenericType || fieldInfo.FieldType.GetGenericTypeDefinition() != typeof(List<>)))
                parameterType = fieldInfo.FieldType;
            else
                parameterType = fieldInfo.FieldType.GetGenericArguments().Single();

            if (parameterType.GenericTypeArguments.Length == 0 && method.GetParameters().Length == 0)
                return true;

            if (parameterType.GetGenericArguments().Length != method.GetParameters().Length)
                return false;

            for (int i = 0; i < method.GetParameters().Length; i++)
            {
                if (method.GetParameters()[i].ParameterType != parameterType.GetGenericArguments()[i])
                    return false;
            }

            return true;
        }
    }
}





