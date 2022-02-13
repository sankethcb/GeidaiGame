
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utilities;
using Utilities.Editor;

namespace Core.SceneManagement
{
    public class BuildSceneWindow : EditorWindow
    {
        List<SceneAsset> m_SceneAssets = new List<SceneAsset>();
        int m_currentControlID = 0;
        bool m_firstOpened = true;


        [MenuItem("Tools/Custom/Build Scenes")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            //EditorWindow.GetWindow(typeof(BuildSceneWindow));

            GetWindow<BuildSceneWindow>(false, "Build Scenes", true);
        }

        void OnGUI()
        {
            if (m_firstOpened)
            {
                m_firstOpened = false;
                InitalizeWindow();
            }

            GUILayout.Label("Build Scenes:", EditorStyles.boldLabel);
            for (int i = 0; i < m_SceneAssets.Count; i++)
            {
                GUI.SetNextControlName((i).ToString());
                m_SceneAssets[i] = (SceneAsset)EditorGUILayout.ObjectField(m_SceneAssets[i], typeof(SceneAsset), false);
            }

            if (GUILayout.Button("Add"))
            {
                m_SceneAssets.Add(null);
            }

            if (GUILayout.Button("Remove"))
            {
                if (m_currentControlID != 0)
                {
                    m_SceneAssets.RemoveAt(int.Parse(GUI.GetNameOfFocusedControl()));
                }
            }

            GUILayout.Space(8);

            if (GUILayout.Button("Save Build Scenes"))
            {
                SetEditorBuildSettingsScenes();
            }

            if (GUIUtility.keyboardControl != 0)
            {
                m_currentControlID = GUIUtility.keyboardControl;
            }

            EditorWindowUtility.HandleDragDrop();
        }

        void InitalizeWindow()
        {
            int index = 0;
            foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
            {
                if (m_SceneAssets.Count <= index)
                    m_SceneAssets.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path));
                else
                    m_SceneAssets[index] = AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path);

                index++;
            }
        }

        void SetEditorBuildSettingsScenes()
        {
            m_SceneAssets.RemoveAll(item => item == null);

            // Find valid Scene paths and make a list of EditorBuildSettingsScene
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            foreach (var sceneAsset in m_SceneAssets)
            {
                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }

            // Set the Build Settings window Scene list
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            SaveToEnum();

        }

        void SaveToEnum()
        {
            string filePath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)).Replace("Editor/BuildSceneWindow.cs", "SceneUtility.cs");
            string fileText = System.IO.File.ReadAllText(filePath);
            fileText = RegexUtility.ReplaceEnums(GetEnumText(m_SceneAssets), fileText);
            System.IO.File.WriteAllText(filePath, fileText);
        }


        string GetEnumText(List<SceneAsset> enumData)
        {
            System.Text.StringBuilder enumStr = new System.Text.StringBuilder();
            enumStr.Append(System.Environment.NewLine);
            foreach (SceneAsset enumVal in enumData)
            {
                enumStr.Append(RegexUtility.CleanText(enumVal.name));
                enumStr.Append(",");
                enumStr.Append(System.Environment.NewLine);
            }
            enumStr.Append(System.Environment.NewLine);
            return enumStr.ToString();
        }

    }
}