using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickScreens : MonoBehaviour
{
    // Private fields.
    private static ScreensManager screenManger;
    private static SceneLoader sceneLoader;
    private static Action onSuccessFunc;
    private SimulationVM viewModel;
    private bool isLastQuestion;
    private bool isLastLevel;


    private void Start()
    {
        sceneLoader = GameObject.Find(Utils.SCENE_LOADER).GetComponent<SceneLoader>();
        screenManger = GameObject.Find(Utils.SCREENS).GetComponent<ScreensManager>();

        SetViewModel();
    }

    private void SetViewModel()
    {
        viewModel = GameObject.Find(Utils.VIEW).GetComponent<SimulationVM>();
        viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }
            
            if (eventArgs.PropertyName.Equals(SimulationVM.OPENED_LEVEL))
            {
                isLastQuestion = true;
                isLastLevel = false;
            }
            else if (eventArgs.PropertyName.Equals(SimulationVM.FINISHED_LEVEL))
            {
                isLastQuestion = true;
                isLastLevel = true;
            }
            // User saved successfully.
            else if (eventArgs.PropertyName.Equals(viewModel.GetOnFinishActionPropertyName()))
            {
                if (onSuccessFunc != null) { onSuccessFunc(); }
            }

        };

    }

    private void OnClickCheckTime(Action action)
    {
        CheckTime();
        action();
    }

    public void OnClickNoSave()
    {
        OnClickCheckTime(() => { ScreensManager.ResetScreens(); sceneLoader.LoadNextScene(Utils.LEVELS_SCENE); });
    }

    private void OnClickCheckTimeAndExit(Action onSuccess)
    {
        OnClickCheckTime(()=> { SetOnSuccessFunc(onSuccess); });
        ScreensManager.ResetScreens();
        viewModel.OnExitSimulation();
    }
    
    // Stay in game - dont quit the game.
    public void OnClickNoButton()
    {
        OnClickCheckTime(() => { screenManger.OnClickNoButton(); });
    }

    // Setter for onSuccessFunc.
    private void SetOnSuccessFunc(Action onSuccess)
    {
        onSuccessFunc = onSuccess;
    }

    // Move to finish scene - quit the game. 
    public void OnClickYesButton()
    {
        OnClickCheckTimeAndExit(delegate () { sceneLoader.LoadNextScene(Utils.LEVELS_SCENE); });
    }

    // Move to learning from questions scene.
    public void OnClickLearningFromQ()
    {
        OnClickCheckTimeAndExit(delegate () { sceneLoader.LoadNextScene(Utils.LEARNING_FROM_Q_SCENE); });
    }

    // On click go to statistics event handler.
    public void OnClickStatistics()
    {
        OnClickCheckTimeAndExit(delegate () { sceneLoader.LoadNextScene(Utils.STATISTICS_SCENE); });
    }

    // Reapet scene.
    public void OnClickTryAgain()
    {
        OnClickCheckTimeAndExit(delegate () { sceneLoader.LoadNextScene(SceneManager.GetActiveScene().name); });

    }

    // move to lock scene.
    public void OnClickBack()
    {
        OnClickCheckTimeAndExit(delegate () { sceneLoader.LoadNextScene(Utils.LEVELS_SCENE); });

    }

    // On click finish answer event handler.
    public void OnClickFinishAnswer()
    {
        screenManger.OnClickFinishAns();

        if (isLastQuestion)
        {
            screenManger.FinishLevel(isLastLevel);
        }

    }

    // On click go to the next level event handler.
    public void OnClickNextLevel()
    {
        LevelsVM.chosenLevelIdx++;
        OnClickCheckTimeAndExit(delegate () 
        {
            sceneLoader.LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1);
        });       
    }

    // If the game is frozen - undo it.
    private void CheckTime()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

}
