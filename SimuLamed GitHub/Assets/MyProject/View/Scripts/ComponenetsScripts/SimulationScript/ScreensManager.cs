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
    }

    // Pause the game.
    private void Pause()
    {
        ActivateMenu(() => { soundManager.PauseSimulation(); }, pauseMenuUI, true);
        
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
    }

    // Continue the game after answering the question.
    public void OnClickFinishAns()
    {
        ActivateMenu(() => { soundManager.OnClickButton(); }, questionsMenuUI, false);

        
    }
    // Continue the game.
    private void Resume()
    {
        ActivateMenu(() => { soundManager.PauseSimulation(); }, pauseMenuUI, false);
        
        gameIsPaused = false;
    }

    // Dispaly the quit menu UI.
    private void QuitMenu()
    {
        ActivateMenu(()=> { soundManager.QuitSimulation(); }, quitMenuUI, true);
        
        if (gameIsPaused)
        {
            pauseMenuUI.SetActive(false);
        }
        
        isQuitMenu = true;
    }

    // Stay in game - dont quit the game.
    public void OnClickNoButton()
    {
        ActivateMenu(() => { soundManager.OnClickButton(); }, quitMenuUI, false);
       
        isQuitMenu = false;
    }

    

    // Display the fail menu UI.
    public void FailMenu()
    {
        ActivateMenu(() => { soundManager.FailLevel(); }, failMenuUI, true);
        
        questionsMenuUI.SetActive(false);
       
    }


    // Display the success menu UI.
    public void SuccessMenu()
    {
        ActivateMenu(() => { soundManager.PassLevel(); }, successMenuUI, true);
        questionsMenuUI.SetActive(false);
        
    }

    // Display the last success menu UI.
    public void LastSuccessMenu()
    {
        ActivateMenu(() => { soundManager.PassLevel(); }, lastSuccessMenuUI, true);
        
        questionsMenuUI.SetActive(false);
        
    }

    

    // Display the questions menu UI with the given question.
    public void DisplayQuestion(int questionNumber)
    {
        ActivateMenu(() => { soundManager.DisplayQuestion(); }, questionsMenuUI, true);
        
        simulationVM.DisplayQuestion(questionNumber);
    }

}
