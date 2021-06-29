using Assets;
using Assets.MyProject.View.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour
{
    private static bool gameIsPaused = false;
    private static bool isQuitMenu = false;
    private static bool isQuestionMenu = false;
    private static bool isEndMenu = false;
    private bool isLastQuestion;
    private bool isLastLevel;


    private static SoundManager soundManager;

    private const int PAUSE_MENU_UI_IDX = 0;
    private const int QUIT_MENU_UI_IDX = 1;
    private const int FAIL_MENU_UI_IDX = 2;
    private const int SUCCESS_MENU_UI_IDX = 3;
    private const int QUESTIONS_MENU_UI_IDX = 4;
    private const int LAST_SUCCESS_MENU_UI_IDX = 5;
    private const int ERROR_MENU_UI_IDX = 6;

    [SerializeField]
    private GameObject[] menues;


    private SimulationVM simulationVM;

    void Start()
    {
        soundManager = GameObject.Find(GameObjectNames.SOUND_MANAGER).GetComponent<SoundManager>();
        SetViewModel();
        isQuitMenu = false;
    }

    // Set the view model.
    private void SetViewModel()
    {
        simulationVM = GameObject.Find(GameObjectNames.VIEW).GetComponent<SimulationVM>();
        simulationVM.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }
            
            // Number of lives has changed. 
            if (eventArgs.PropertyName.Equals(nameof(simulationVM.Lives)))
            {
                if (simulationVM.Lives < 0)
                {
                    FailMenu();
                }
            }
            
            // The user opened a new level.
            else if (eventArgs.PropertyName.Equals(SimulationVM.OPENED_LEVEL))
            {
                isLastQuestion = true;
                isLastLevel = false;
            }
            // The user finished the last level.
            else if (eventArgs.PropertyName.Equals(SimulationVM.FINISHED_LEVEL))
            {
                isLastQuestion = true;
                isLastLevel = true;
            }
            // Saving the user state has been failed.
            else if (eventArgs.PropertyName.Equals(nameof(simulationVM.IsSaveingFailed)) && simulationVM.IsSaveingFailed)
            {
                DiactivateAllMenues();
                ActivateMenu(()=>{ }, menues[ERROR_MENU_UI_IDX], true);
                
            }

        };

    }

    public void SetIsQuitMenu(bool isQuit)
    {
        isQuitMenu = isQuit;
    }


    // The user does not want to go back with no saving - hide the error screen and continue the game.
    public void Abort()
    {
        simulationVM.IsSaveingFailed = false;
        ActivateMenu(()=> { }, menues[ERROR_MENU_UI_IDX], false);
    }

    // Diactivate all menues.
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
        if (!OtherMenuActive())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Other menues are not active.
                if (!isQuitMenu && !isQuestionMenu && !isEndMenu)
                {
                    if (gameIsPaused) { Resume(); }
                    else { Pause(); }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gameIsPaused) { QuitMenu(); }

            }
        }
    }

    // Checks if other menu then pause and quit is active.
    private bool OtherMenuActive()
    {
        return menues[ERROR_MENU_UI_IDX].activeSelf || menues[SUCCESS_MENU_UI_IDX].activeSelf ||
            menues[LAST_SUCCESS_MENU_UI_IDX].activeSelf || menues[FAIL_MENU_UI_IDX].activeSelf;
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
        if (isQuitMenu && toActivate) { return; }
        
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
    public void FinishLevel()
    {
        if (isLastLevel) { LastSuccessMenu(); }
        else { SuccessMenu(); }
        isEndMenu = true; 
    }

    // Continue the game after answering the question.
    public void OnFinishAns()
    {
        ActivateMenu(() => { soundManager.OnClickButton(); }, menues[QUESTIONS_MENU_UI_IDX], false);
        if (isLastQuestion) { FinishLevel(); }

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
        if (!isQuitMenu)
        {
            isQuestionMenu = true;
            simulationVM.DisplayQuestion(questionNumber);
        }
    }

}
