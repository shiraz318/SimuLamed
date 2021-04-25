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
        screenManger.OnClickNoButton();
    }

    // move to finish scene - quit the game  
    public void OnClickYesButton()
    {

    }

    // move to learning from questions scene
    public void OnClickLearningFromQ()
    {
        sceneLoader.LoadNextScene(Utils.LEARNING_FROM_Q_SCENE);

    }

    // reapet scene
    public void OnClickTryAgain()
    {

        sceneLoader.LoadNextScene(SceneManager.GetActiveScene().name);

    }

    // move to lock scene
    public void OnClickBack()
    {

        sceneLoader.LoadNextScene(Utils.LEVELS_SCENE);

    }


    public void OnClickNextLevel()
    {
        sceneLoader.LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
