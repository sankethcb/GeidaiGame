using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-9999)]
    class GameInitialization
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitalizeGameLoading()
        {

#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
            Debug.unityLogger.logEnabled = false;
#endif
            Debug.Log("Initalizing Game");

            GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(GameConstants.CORE_RESOURCES_PATH));

            Debug.Log("Game Systems Loaded");
        }
    }
}
