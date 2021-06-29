using Assets;
using Assets.MyProject.View.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenToScene : MonoBehaviour
{
    private static SceneLoader sceneLoader;
    private static Action onSuccessFunc;
    private SimulationVM viewModel;


    private void Start()
    {
        sceneLoader = GameObject.Find(GameObjectNames.SCENE_LOADER).GetComponent<SceneLoader>();
        SetViewModel();
    }
    
    // Set the view model.
    private void SetViewModel()
    {
        viewModel = GameObject.Find(GameObjectNames.VIEW).GetComponent<SimulationVM>();
        viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }            
            
            // User saved successfully.
            if (eventArgs.PropertyName.Equals(viewModel.GetOnFinishActionPropertyName()))
            {
                if (onSuccessFunc != null) { onSuccessFunc(); }
            }

        };

    }

    // Check time and then activate the given action.
    private void OnClickCheckTime(Action action)
    {
        CheckTime();
        action();
    }

    // Go back without saving.
    public void OnClickNoSave()
    {
        OnClickCheckTime(() => { ScreensManager.ResetScreens(); sceneLoader.LoadNextScene(ScenesNames.LEVELS_SCENE); });
    }

    // Check time and exit the simulation.
    private void OnClickCheckTimeAndExit(Action onSuccess)
    {
        OnClickCheckTime(()=> { SetOnSuccessFunc(onSuccess); });
        ScreensManager.ResetScreens();
        viewModel.OnExitSimulation();
    }
    

    // Setter for onSuccessFunc.
    private void SetOnSuccessFunc(Action onSuccess)
    {
        onSuccessFunc = onSuccess;
    }


    // Reapet scene.
    public void OnClickTryAgain()
    {
        OnClickCheckTimeAndExit(delegate () { sceneLoader.LoadLevel(SceneManager.GetActiveScene().name); });

    }
    // Exit the simulation and go to the given scene.
    public void ExitToOtherScene(string sceneName)
    {
        OnClickCheckTimeAndExit(delegate () { sceneLoader.LoadNextScene(sceneName); });
    }


    // On click go to the next level event handler.
    public void OnClickNextLevel()
    {
        LevelsVM.chosenLevelIdx++;
        OnClickCheckTimeAndExit(delegate () 
        {
            sceneLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        });       
    }

    // If the game is frozen - undo it.
    public void CheckTime()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

}
