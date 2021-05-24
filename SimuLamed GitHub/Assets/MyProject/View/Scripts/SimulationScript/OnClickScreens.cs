using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickScreens : MonoBehaviour
{
    //private delegate void onSuccessUserSaveFunc();

    private static ScreensManager screenManger;
    private static SceneLoader sceneLoader;

    private SimulationVM viewModel;
    private bool isLastQuestion;
    private bool isLastLevel;
    private static Action onSuccessFunc;
    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        screenManger = GameObject.Find("Screens").GetComponent<ScreensManager>();
        viewModel = GameObject.Find("View").GetComponent<SimulationVM>();
        viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }
            if (eventArgs.PropertyName.Equals("OpenedLevel"))
            {
                isLastQuestion = true;
                isLastLevel = false;
            }
            else if (eventArgs.PropertyName.Equals("FinishLastLevel"))
            {
                isLastQuestion = true;
                isLastLevel = true;
            }
            // User saved successfully.
            else if (eventArgs.PropertyName.Equals(viewModel.GetPropertyName()))                
            {
                if (onSuccessFunc == null)
                {
                    Debug.Log("NULLL");
                }
                onSuccessFunc();
            }

        };
    }

    private void OnClickCheckTime(Action action)
    {
        CheckTime();
        action();
    }
    private void OnClickCheckTimeAndExist(Action action)
    {
        OnClickCheckTime(action);
        viewModel.OnExitSimulation();
    }
    // stay in game - dont quit the game
    public void OnClickNoButton()
    {
        OnClickCheckTime(() => { screenManger.OnClickNoButton(); });
    }

    // move to finish scene - quit the game  
    public void OnClickYesButton()
    {
        OnClickCheckTimeAndExist(() =>
        {
            onSuccessFunc = delegate() { sceneLoader.LoadNextScene(Utils.LEVELS_SCENE); };
        });
    }

    // move to learning from questions scene
    public void OnClickLearningFromQ()
    {
        OnClickCheckTimeAndExist(() =>
        {
            onSuccessFunc = delegate(){ sceneLoader.LoadNextScene(Utils.LEARNING_FROM_Q_SCENE); };
        });

        
    }

    public void OnClickStatistics()
    {
        OnClickCheckTimeAndExist(() => 
        {
            onSuccessFunc = delegate(){ sceneLoader.LoadNextScene(Utils.STATISTICS_SCENE); };
        });

        
    }

    // reapet scene
    public void OnClickTryAgain()
    {
        OnClickCheckTime(()=> { sceneLoader.LoadNextScene(SceneManager.GetActiveScene().name); });
    }

    // move to lock scene
    public void OnClickBack()
    {
        OnClickCheckTimeAndExist(() => 
        {
            onSuccessFunc = delegate() { sceneLoader.LoadNextScene(Utils.LEVELS_SCENE); };
        });

        
    }

    public void OnClickFinishAnswer()
    {
        screenManger.OnClickFinishAns();

        if (isLastQuestion)
        {
            screenManger.OnClickLevel(isLastLevel);

        }

    }

    public void OnClickNextLevel()
    {
        OnClickCheckTimeAndExist(()=> 
        {
            //SimulationVM.currentLevelName = NextLevelName(SimulationVM.currentLevelName);
            LevelsVM.chosenLevelIdx++;
            //SimulationVM.currentLevel++;
            onSuccessFunc = delegate() { sceneLoader.LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1); };
        });

        
    }

    private string NextLevelName(string levelName)
    {
        if (levelName.Equals("Level1"))
        {
            return "Level2";
        }
        else
        {
            return "Level3";
        }
            
    }
    private void CheckTime()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

}
