
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Utilities.Editor
{
    public class EditorIOUtility
    {
        public static string LoadFileText(MonoBehaviour file)
        {
            MonoScript fileScript = MonoScript.FromMonoBehaviour(file);
            return fileScript.text;
        }

        public static string LoadFileText(Object file)
        {
            string filePath = AssetDatabase.GetAssetPath(file);
            return File.ReadAllText(filePath);
        }

        public static void SaveToFile(string fileText, Object file)
        {
            string filePath = AssetDatabase.GetAssetPath(file);
            File.WriteAllText(filePath, fileText);
        }
    }


    public class EditorWindowUtility : EditorWindow
    {
        public enum DragDropType
        {
            SCENE,
            GAMEOBJECT,
            AUDIO,
            TEXTURE,
            SCRIPT
        }

        public static void HandleDragDrop(params DragDropType[] dragDropTypes)
        {
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();

            }
            else if (Event.current.type == EventType.DragPerform)
            {
                // To consume drag data.
                DragAndDrop.AcceptDrag();

                // GameObjects from hierarchy.
                if (DragAndDrop.paths.Length == 0 && DragAndDrop.objectReferences.Length > 0)
                {
                    Debug.Log("GameObject");
                    foreach (Object obj in DragAndDrop.objectReferences)
                    {
                        Debug.Log("- " + obj);
                    }
                }
                // Object outside project. It mays from File Explorer (Finder in OSX).
                else if (DragAndDrop.paths.Length > 0 && DragAndDrop.objectReferences.Length == 0)
                {
                    Debug.Log("File");
                    foreach (string path in DragAndDrop.paths)
                    {
                        Debug.Log("- " + path);
                    }
                }
                // Unity Assets including folder.
                else if (DragAndDrop.paths.Length == DragAndDrop.objectReferences.Length)
                {
                    Debug.Log("UnityAsset");
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        Object obj = DragAndDrop.objectReferences[i];
                        string path = DragAndDrop.paths[i];
                        Debug.Log(obj.GetType().Name);

                        // Folder.
                        if (obj is DefaultAsset)
                        {
                            Debug.Log(path);
                        }
                        // C# or JavaScript.
                        else if (obj is MonoScript)
                        {
                            Debug.Log(path + "\n" + obj);
                        }
                        else if (obj is Texture2D)
                        {

                        }

                    }
                }
                // Log to make sure we cover all cases.
                else
                {
                    Debug.Log("Out of reach");
                    Debug.Log("Paths:");
                    foreach (string path in DragAndDrop.paths)
                    {
                        Debug.Log("- " + path);
                    }

                    Debug.Log("ObjectReferences:");
                    foreach (Object obj in DragAndDrop.objectReferences)
                    {
                        Debug.Log("- " + obj);
                    }
                }
            }
        }
    }
}
