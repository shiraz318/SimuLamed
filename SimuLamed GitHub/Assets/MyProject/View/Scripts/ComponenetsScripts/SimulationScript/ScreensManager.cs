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
    public static bool isQuestionMenu = false;
    public static bool isEndMenu = false;


    public static SoundManager soundManager;

    private const int PAUSE_MENU_UI_IDX = 0;
    private const int QUIT_MENU_UI_IDX = 1;
    private const int FAIL_MENU_UI_IDX = 2;
    private const int SUCCESS_MENU_UI_IDX = 3;
    private const int QUESTIONS_MENU_UI_IDX = 4;
    private const int LAST_SUCCESS_MENU_UI_IDX = 5;
    private const int ERROR_SUCCESS_MENU_UI_IDX = 6;

    //public GameObject pauseMenuUI;
    //public GameObject quitMenuUI;
    //public GameObject failMenuUI;
    //public GameObject successMenuUI;
    //public GameObject questionsMenuUI;
    //public GameObject lastSuccessMenuUI;
    //public GameObject ErrorMenuUI;

    public GameObject[] menues;


    private SimulationVM simulationVM;

    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
       
        SetViewModel();
        isQuitMenu = false;
    }
    private void SetViewModel()
    {
        simulationVM = GameObject.Find("View").GetComponent<SimulationVM>();
        simulationVM.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }
            if (eventArgs.PropertyName.Equals("Lives"))
            {
                if (simulationVM.Lives < 0)
                {
                    FailMenu();
                }
            } 
            else if (eventArgs.PropertyName.Equals("IsSaveingFailed") && simulationVM.IsSaveingFailed)
            {
                DiactivateAllMenues();
                ActivateMenu(()=>{ }, menues[ERROR_SUCCESS_MENU_UI_IDX], true);
                
            }

        };

    }

    public void Abort()
    {
        simulationVM.IsSaveingFailed = false;
        ActivateMenu(()=> { }, menues[ERROR_SUCCESS_MENU_UI_IDX], false);
    }

    private void DiactivateAllMenues()
    {
        foreach (GameObject menu in menues)
        {
            ActivateMenu(()=> { }, menu, false);
        }
    }

    // Check if space or esc is clicked.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isQuitMenu && !isQuestionMenu && !isEndMenu)
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
            if (!gameIsPaused)
            {
                QuitMenu();
            }
            
        }
    }

    // Reset the screens to false - no screen is displaying right now.
    public static void ResetScreens()
    {
            gameIsPaused = false;
            isQuitMenu = false;
            isQuestionMenu = false;
            isEndMenu = false;
    }

    // Activate or disactivate the given menu UI and make the given sound action.
    private void ActivateMenu(Action soundAction, GameObject menuUI, bool toActivate)
    {
        soundAction();
        menuUI.SetActive(toActivate);
        Time.timeScale = toActivate? 0f: 1f;
        gameIsPaused = toActivate? true: false;
        isQuitMenu = false;
        if (gameIsPaused)
        {
            SoundManager.muteCar = true;
        }
        else
        {
            SoundManager.muteCar = soundManager.IsMute;
        }
        isQuestionMenu = false;
        isEndMenu = false;
    }

    // Pause the game.
    private void Pause()
    {
        ActivateMenu(() => { soundManager.PauseSimulation(); }, menues[PAUSE_MENU_UI_IDX], true);
    }

    // Display the right finish level menu UI.
    public void FinishLevel(bool isLastLevel)
    {
        if (isLastLevel)
        {
            LastSuccessMenu();
        }
        else
        {
            SuccessMenu();
        }
        isEndMenu = true;
        
    }

    // Continue the game after answering the question.
    public void OnClickFinishAns()
    {
        ActivateMenu(() => { soundManager.OnClickButton(); }, menues[QUESTIONS_MENU_UI_IDX], false);

        
    }
    // Continue the game.
    private void Resume()
    {
        ActivateMenu(() => { soundManager.PauseSimulation(); }, menues[PAUSE_MENU_UI_IDX], false);
        
        gameIsPaused = false;
    }

    // Dispaly the quit menu UI.
    private void QuitMenu()
    {
        ActivateMenu(()=> { soundManager.QuitSimulation(); }, menues[QUIT_MENU_UI_IDX], true);
        
        //if (gameIsPaused)
        //{
        //    pauseMenuUI.SetActive(false);
        //}
        
        isQuitMenu = true;
    }

    // Stay in game - dont quit the game.
    public void OnClickNoButton()
    {
        ActivateMenu(() => { soundManager.OnClickButton(); }, menues[QUIT_MENU_UI_IDX], false);
       
        isQuitMenu = false;
    }

    

    // Display the fail menu UI.
    public void FailMenu()
    {
        ActivateMenu(() => { soundManager.FailLevel(); }, menues[FAIL_MENU_UI_IDX], true);
        isEndMenu = true;
        menues[QUESTIONS_MENU_UI_IDX].SetActive(false);
       
    }


    // Display the success menu UI.
    public void SuccessMenu()
    {
        ActivateMenu(() => { soundManager.PassLevel(); }, menues[SUCCESS_MENU_UI_IDX], true);
        menues[QUESTIONS_MENU_UI_IDX].SetActive(false);
        
    }

    // Display the last success menu UI.
    public void LastSuccessMenu()
    {
        ActivateMenu(() => { soundManager.PassLevel(); }, menues[LAST_SUCCESS_MENU_UI_IDX], true);
        
        menues[QUESTIONS_MENU_UI_IDX].SetActive(false);
        
    }

    

    // Display the questions menu UI with the given question.
    public void DisplayQuestion(int questionNumber)
    {
        ActivateMenu(() => { soundManager.DisplayQuestion(); }, menues[QUESTIONS_MENU_UI_IDX], true);
        isQuestionMenu = true;
        simulationVM.DisplayQuestion(questionNumber);
    }

}
