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

    public static SoundManager soundManager;

    public GameObject pauseMenuUI;
    public GameObject quitMenuUI;
    public GameObject failMenuUI;
    public GameObject successMenuUI;
    public GameObject questionsMenuUI;
    public GameObject lastSuccessMenuUI;
    

    private SimulationVM simulationVM;

    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        SetViewModel();
    }
    private void SetViewModel()
    {
        simulationVM = GameObject.Find("View").GetComponent<SimulationVM>();
        simulationVM.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }
            if (eventArgs.PropertyName.Equals("Lives"))
            {
                if (simulationVM.Lives <= 0)
                {
                    FailMenu();
                }
            }

        };

    }

    // Check if space or esc is clicked.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
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


    private void ActivateMenu(Action soundAction, GameObject menuUI, bool toActivate)
    {
        soundAction();
        menuUI.SetActive(toActivate);
        Time.timeScale = toActivate? 0f: 1f;
        gameIsPaused = toActivate? true: false;
    }

    // Pause the game.
    private void Pause()
    {
        ActivateMenu(() => { soundManager.PauseSimulation(); }, pauseMenuUI, true);
        
        //pauseMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        //gameIsPaused = true;
    }

    public void OnClickLevel(bool isLastLevel)
    {
        if (isLastLevel)
        {
            LastSuccessMenu();
        }
        else
        {
            SuccessMenu();
        }
    }

    public void OnClickFinishAns()
    {
        ActivateMenu(() => { soundManager.OnClickButton(); }, questionsMenuUI, false);
        
        //questionsMenuUI.SetActive(false);
        //Time.timeScale = 1;
    }
    // continue the game
    private void Resume()
    {
        ActivateMenu(() => { soundManager.PauseSimulation(); }, pauseMenuUI, false);
        //pauseMenuUI.SetActive(false);
        //Time.timeScale = 1;
        gameIsPaused = false;
    }

    // present the quit menu
    private void QuitMenu()
    {
        ActivateMenu(()=> { soundManager.QuitSimulation(); }, quitMenuUI, true);
        
        if (gameIsPaused)
        {
            pauseMenuUI.SetActive(false);
        }
        //quitMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        //gameIsPaused = true;
        isQuitMenu = true;
    }

    // stay in game - dont quit the game
    public void OnClickNoButton()
    {
        ActivateMenu(() => { soundManager.OnClickButton(); }, quitMenuUI, false);
       
        //quitMenuUI.SetActive(false);
        //Time.timeScale = 1;
        //gameIsPaused = false;
        isQuitMenu = false;
    }

    //// move to finish scene - quit the game  
    //public void OnClickYesButton()
    //{

    //}

    // present the fail menu
    public void FailMenu()
    {
        ActivateMenu(() => { soundManager.FailLevel(); }, failMenuUI, true);
        
        questionsMenuUI.SetActive(false);
        //failMenuUI.SetActive(true);
        //Time.timeScale = 0f;
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
        ActivateMenu(() => { soundManager.PassLevel(); }, successMenuUI, true);
        questionsMenuUI.SetActive(false);
        //soundManager.PassLevel();
        //successMenuUI.SetActive(true);
        //Time.timeScale = 0f;
    }
    public void LastSuccessMenu()
    {

        ActivateMenu(() => { soundManager.PassLevel(); }, lastSuccessMenuUI, true);
        
        questionsMenuUI.SetActive(false);
        
        //lastSuccessMenuUI.SetActive(true);
        //Time.timeScale = 0f;

    }

    //public void OnClickNextLevel()
    //{
    //    sceneLoader.LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}


    public void DisplayQuestion(int questionNumber)
    {
        ActivateMenu(() => { soundManager.DisplayQuestion(); }, questionsMenuUI, true);
        
        //questionsMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        simulationVM.DisplayQuestion(questionNumber);
    }

}
