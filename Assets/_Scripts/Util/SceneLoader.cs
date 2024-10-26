using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{
    public enum Scene
    {
        MainMenu,
        GameScene,
        LoadingScene
    }

    private static Scene targetScene;

    public static void LoadScene(Scene targetScene)
    {
        // record the targetSceneEnum, for later to be loaded on Callback.
        SceneLoader.targetScene = targetScene;

        // load the loading scene first
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        
    }


    /// <summary>
    /// The callback function that's supposed to be called by LoaderCallback.cs.
    /// Purpose is to notify 
    /// </summary>
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
