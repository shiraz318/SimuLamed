using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public SoundManager soundManager;

    //public BaseViewModel viewModel;
    public void GoToOtherScene(string sceneName)
    {
        OnClickButton();
        sceneLoader.LoadNextScene(sceneName);
    }
    public void GoToOtherSceneNoSound(string sceneName)
    {
        sceneLoader.LoadNextScene(sceneName);
    }
    protected void OnClickButton()
    {
        soundManager.OnClickButton();
    }
}
