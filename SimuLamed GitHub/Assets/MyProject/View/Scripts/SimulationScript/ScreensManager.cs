using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public static bool isQuitMenu = false;

    public GameObject pauseMenuUI;
    public GameObject quitMenuUI;
    public GameObject failMenuUI;
    public GameObject successMenuUI;
    public GameObject questionsMenuUI;
    

    // private SceneLoader sceneLoader;

    private SimulationVM simulationVM;

    void Start()
    {
        //sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        SetViewModel();
    }
    private void SetViewModel()
    {
        simulationVM = GameObject.Find("View").GetComponent<SimulationVM>();
        simulationVM.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Lives"))
            {
                if (simulationVM.Lives <= 0)
                {
                    FailMenu();
                }
            }
            else if (eventArgs.PropertyName.Equals("LevelFinished"))
            {
                SuccessMenu();
            }

        };

    }

    // Check if space or esc is clicked
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space is pressed");
            
            if (!isQuitMenu)
            {

                if (gameIsPaused)
                {

                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            QuitMenu();
        }
    }

    // pause the game
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    
    public void OnClickFinishAns()
    {
        questionsMenuUI.SetActive(false);
        Time.timeScale = 1;
    }
    // continue the game
    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    // present the quit menu
    private void QuitMenu()
    {
        if (gameIsPaused)
        {
            pauseMenuUI.SetActive(false);
        }
        quitMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        isQuitMenu = true;
    }

    // stay in game - dont quit the game
    public void OnClickNoButton()
    {
        quitMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        isQuitMenu = false;
    }

    //// move to finish scene - quit the game  
    //public void OnClickYesButton()
    //{

    //}

    // present the fail menu
    public void FailMenu()
    {
        questionsMenuUI.SetActive(false);
        failMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    //// move to learning from questions scene
    //public void OnClickLearningFromQ()
    //{
    //    sceneLoader.LoadNextScene(Utils.LEARNING_FROM_Q_SCENE);

    //}

    //// reapet scene
    //public void OnClickTryAgain()
    //{

    //    sceneLoader.LoadNextScene(SceneManager.GetActiveScene().name);

    //}

    //// move to lock scene
    //public void OnClickBack()
    //{

    //    sceneLoader.LoadNextScene(Utils.LEVELS_SCENE);

    //}

    // present the success menu
    public void SuccessMenu()
    {
        successMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    //public void OnClickNextLevel()
    //{
    //    sceneLoader.LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}


    public void DisplayQuestion(int questionNumber)
    {
        questionsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        simulationVM.DisplayQuestion(questionNumber);
    }

}
