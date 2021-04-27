using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickScreens : MonoBehaviour
{
    private static ScreensManager screenManger;
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        screenManger = GameObject.Find("Screens").GetComponent<ScreensManager>();
    }
    // stay in game - dont quit the game
    public void OnClickNoButton()
    {
        CheckTime();
        screenManger.OnClickNoButton();
    }

    // move to finish scene - quit the game  
    public void OnClickYesButton()
    {
        CheckTime();
        // or menu
        sceneLoader.LoadNextScene(Utils.LEVELS_SCENE);

    }

    // move to learning from questions scene
    public void OnClickLearningFromQ()
    {
        CheckTime();
        sceneLoader.LoadNextScene(Utils.LEARNING_FROM_Q_SCENE);
    }

    // reapet scene
    public void OnClickTryAgain()
    {

        CheckTime();
        sceneLoader.LoadNextScene(SceneManager.GetActiveScene().name);
    }

    // move to lock scene
    public void OnClickBack()
    {
        CheckTime();
        sceneLoader.LoadNextScene(Utils.LEVELS_SCENE);

    }

    public void OnClickFinishAnswer()
    {
        screenManger.OnClickFinishAns();
    }

    public void OnClickNextLevel()
    {
        CheckTime();
        sceneLoader.LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void CheckTime()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

}
