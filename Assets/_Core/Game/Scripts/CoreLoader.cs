using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-9999)]
    public class CoreLoader : MonoBehaviour
    {
        [SerializeField] GameSystems gameSystems;

        string m_clone = "(Clone)";
        void Awake()
        {
            LoadSystems();

            DontDestroyOnLoad(gameObject);

        }

        public void LoadSystems()
        {
            GameObject system;
            for (int i = 0; i < gameSystems.GetSystems.Length; i++)
            {
                system = Instantiate(gameSystems.GetSystems[i]);
                system.name = system.name.Replace(m_clone, string.Empty);
            }

            Debug.Log("Game Systems Loaded");
            Destroy(gameObject);
        }
    }
}
