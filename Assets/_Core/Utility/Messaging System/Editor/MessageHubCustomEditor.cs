using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MessageHub))]
public class MessageHubCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        /*
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EventList"), true);
		serializedObject.ApplyModifiedProperties();
        */
        MessageHub logButton = (MessageHub)target;

        if(GUILayout.Button("Log Current Messages"))
        {
            logButton.LogMessages();
        }
        
        
    }
}
