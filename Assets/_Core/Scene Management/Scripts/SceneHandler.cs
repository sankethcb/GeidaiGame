using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Core.SceneManagement
{
    public class SceneHandler
    {
        #region Global Events
        public static event System.Action<SCENES> OnSceneLoadStart;
        public static event System.Action<SCENES> OnSceneLoaded;
        public static event System.Action<SCENES> OnSceneUnloadStart;
        public static event System.Action<SCENES> OnSceneUnloaded;
        #endregion

        #region Private Members
        private static AsyncOperation m_asyncOperation = null;
        #endregion

        //private static readonly int SCENECOUNT = System.Enum.GetNames(typeof(SCENE)).Length;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initalize()
        {
            Application.quitting += Release;

            Debug.Log("SceneHandler Set");
        }

        static void Release()
        {
            m_asyncOperation = null;

            OnSceneLoaded = null;
            OnSceneLoadStart = null;
            OnSceneUnloaded = null;
            OnSceneUnloadStart = null;

            Application.quitting -= Release;

            Debug.Log("SceneHandler Released");
        }

        public static void LoadScene(SCENES scene, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(scene + " Load start");
#endif

            OnSceneLoadStart?.Invoke(scene);
            SceneManager.LoadScene((int)scene, sceneMode);
            OnSceneLoaded?.Invoke(scene);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(scene + " Load Complete");
#endif
        }

        public static AsyncOperation LoadSceneAsync(SCENES scene, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(scene + " Load Async start");
#endif
            OnSceneLoadStart?.Invoke(scene);
            m_asyncOperation = SceneManager.LoadSceneAsync((int)scene, sceneMode);
            WaitForAsyncOperation(m_asyncOperation, scene, OnSceneLoaded).Start();
            return m_asyncOperation;
        }

        public static AsyncOperation UnloadSceneAsync(SCENES scene)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(scene + " Unload Async start");
#endif

            OnSceneUnloadStart?.Invoke(scene);
            m_asyncOperation = SceneManager.UnloadSceneAsync((int)scene);
            WaitForAsyncOperation(m_asyncOperation, scene, OnSceneUnloaded).Start();
            return m_asyncOperation;
        }

        static IEnumerator WaitForAsyncOperation(AsyncOperation asyncOperation, SCENES scene, System.Action<SCENES> callback)
        {
            yield return asyncOperation;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(scene + " Async Operation Complete");
#endif

            callback?.Invoke(scene);
        }
    }

}
