using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.SceneManagement;
using Utilities;

public class SceneLoadInBackground : MonoBehaviour
{
    [Header("Settings")]
    public SCENES CurrentScene;
    public SCENES SceneToLoad;
    public ScreenTransition ScreenTransition;
    public bool BeginOnStart = true;
    public bool readyToTransition = false;
    public bool ReadyToTransition { get => readyToTransition; set => readyToTransition = value; }


    [Header("Variables")]
    [SerializeField] [ReadOnly] float progress;
    [SerializeField] [ReadOnly] bool completed;

    public float Progress => progress;
    public bool Completed => completed;

    AsyncOperation m_sceneLoad = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (BeginOnStart) StartSceneLoad();
    }

    public void StartSceneLoad()
    {
        if (m_sceneLoad != null) return;

        completed = false;
        progress = 0;

        m_sceneLoad = SceneToLoad.LoadAsync(UnityEngine.SceneManagement.LoadSceneMode.Additive);
        m_sceneLoad.allowSceneActivation = false;

        LoadSceneInBackGround().Start();
    }

    public IEnumerator LoadSceneInBackGround()
    {
        while (!m_sceneLoad.isDone)
        {
            progress = m_sceneLoad.progress;

            if (progress >= 0.9f)
            {
                completed = true;
                if (ReadyToTransition)
                {
                    if (ScreenTransition)
                        yield return ScreenTransition.BeginTransitionHalf();
                    m_sceneLoad.allowSceneActivation = true;

                }
            }
            yield return null;
        }

        Destroy(Camera.main.GetComponent<AudioListener>());

        m_sceneLoad = CurrentScene.Unload();
        while (!m_sceneLoad.isDone) yield return null;
        m_sceneLoad = null;

        if (ScreenTransition)
            yield return ScreenTransition.BeginTransitionHalf();

        Destroy(gameObject);
    }
}
