using Assets;
using Assets.MyProject.View.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    private static SceneLoader sceneLoader;
    protected static SoundManager soundManager;
    public void Start()
    {
        sceneLoader = GameObject.Find(GameObjectNames.SCENE_LOADER).GetComponent<SceneLoader>();
        soundManager = GameObject.Find(GameObjectNames.SOUND_MANAGER).GetComponent<SoundManager>();
    }

    // Go to the given scene name scnene while making a sound of a button being clicked.
    public void GoToOtherScene(string sceneName)
    {
        OnClickButton();
        sceneLoader.LoadNextScene(sceneName);
    }

    // Go to the given scene name scnene with no sound.
    public void GoToOtherSceneNoSound(string sceneName)
    {
        sceneLoader.LoadNextScene(sceneName);
    }
    
    // On click button event handler.
    public void OnClickButton()
    {
        soundManager.OnClickButton();
    }
}
