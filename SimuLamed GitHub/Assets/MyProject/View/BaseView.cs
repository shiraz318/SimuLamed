using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    public static SceneLoader sceneLoader;
    public static SoundManager soundManager;
    public void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    //protected virtual Action<string> onPropertyChanged { get; set; }
    ////protected BaseViewModel viewModel;

    //public virtual void SetOnPropertyChanged() { onPropertyChanged = null; }
    //public virtual BaseViewModel GetViewModel() { return null; }
    //private void Start()
    //{
    //    SetOnPropertyChanged();
    //    SetViewModelPropertyChanged(onPropertyChanged);
    //}
    //protected virtual void SetViewModelPropertyChanged(Action<string> onPropertyChanged)
    //{
    //    if (onPropertyChanged != null && GetViewModel() != null)
    //    {
    //        GetViewModel().PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
    //        {
    //            onPropertyChanged(eventArgs.PropertyName);
    //        };
    //    }
    //}

    public void GoToOtherScene(string sceneName)
    {

        OnClickButton();
        sceneLoader.LoadNextScene(sceneName);
    }
    public void GoToOtherSceneNoSound(string sceneName)
    {
        sceneLoader.LoadNextScene(sceneName);
    }
    public void OnClickButton()
    {
        soundManager.OnClickButton();
    }
}
