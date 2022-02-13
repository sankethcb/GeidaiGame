using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public static class CoroutineUtility
    {
        static CoroutineHolder m_CoroutineHolder = null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initalize()
        {
            if (m_CoroutineHolder == null)
            {
                m_CoroutineHolder = new GameObject("CoroutineHolder").AddComponent<CoroutineHolder>();
                m_CoroutineHolder.gameObject.hideFlags = HideFlags.HideInHierarchy;

                Application.quitting += Release;

#if UNITY_EDITOR || DEV_BUILD
                Debug.Log("CoroutineHolder Set");
#endif
            }
        }

        static void Release()
        {
            if (m_CoroutineHolder != null)
            {
                StopAll();
                UnityEngine.MonoBehaviour.Destroy(m_CoroutineHolder.gameObject);
            }
            
            m_CoroutineHolder = null;
            Application.quitting -= Release;

#if UNITY_EDITOR || DEV_BUILD
            Debug.Log("CoroutineHolder Released");
#endif
        }

        public static Coroutine Start(this IEnumerator coroutine, MonoBehaviour tracker = null)
        {
            if (tracker)
                return tracker.StartCoroutine(coroutine);

            return m_CoroutineHolder.StartCoroutine(coroutine);
        }

        public static void Stop(this IEnumerator coroutine, MonoBehaviour tracker = null)
        {
            if (tracker)
                tracker.StopCoroutine(coroutine);

            m_CoroutineHolder.StopCoroutine(coroutine);
        }

        public static void StopAll()
        {
            m_CoroutineHolder.StopAllCoroutines();
        }
    }

    public class CoroutineHolder : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit()
        {
            StopAllCoroutines();
        }

        public void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
