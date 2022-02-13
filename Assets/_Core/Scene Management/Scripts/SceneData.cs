using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Core.SaveSystem;

namespace Core.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "Core/Scene Handling/Scene Data", order = 0)]
    public class SceneData : ScriptableObject
    {
        static List<SCENES> loadedScenes;
        public List<SCENES> LoadedScenes => loadedScenes;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initalize()
        {
            loadedScenes = new List<SCENES>();

            SceneHandler.OnSceneLoaded += SceneLoaded;
            SceneHandler.OnSceneUnloaded += SceneUnloaded;

            Application.quitting += Release;
        }

        static void Release()
        {
            loadedScenes.Clear();
            loadedScenes = null;

            SceneHandler.OnSceneLoaded -= SceneLoaded;
            SceneHandler.OnSceneUnloaded -= SceneUnloaded;

            Application.quitting -= Release;
        }

        static void SceneLoaded(SCENES scene)
        {
            if (!loadedScenes.Contains(scene))
                loadedScenes.Add(scene);
        }

        static void SceneUnloaded(SCENES scene)
        {
            if (loadedScenes.Contains(scene))
                loadedScenes.Remove(scene);
        }

        public object CaptureData()
        {
            return new List<SCENES>(loadedScenes);
        }

        public void RestoreData(object data)
        {
            loadedScenes = data as List<SCENES>;
        }
    }
}

