using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.SceneManagement
{
    public enum SCENES
    {
        //###START###
MainMenu,
Game,
Ending,
Game_Systems,

//&&&END&&&
	}


    public static class SceneExtensions
    {
        public static void Load(this SCENES scene, LoadSceneMode sceneMode = LoadSceneMode.Single)
        => SceneHandler.LoadScene(scene, sceneMode);

        public static AsyncOperation LoadAsync(this SCENES scene, LoadSceneMode sceneMode = LoadSceneMode.Single)
        => SceneHandler.LoadSceneAsync(scene, sceneMode);

        public static AsyncOperation Unload(this SCENES scene)
        => SceneHandler.UnloadSceneAsync(scene);

    }
}