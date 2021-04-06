using Assets;
using UnityEngine;

public class OnClickMenuScript : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    // On click event handler for clicking the start simulation button.
    public void OnClickStartSimulation()
    {
        Debug.Log("Start simulation");
    }

    // On click event handler for clicking the learn from questions button.
    public void OnClickLearnFromQuestions()
    {
        sceneLoader.LoadNextScene(Utils.LEARNING_FROM_Q_SCENE);
    }

    // On click event handler for clicking the statistics button.
    public void OnClickStatistics()
    {
        sceneLoader.LoadNextScene(Utils.STATISTICS_SCENE);
    }
    
    // On click event handler for clicking the options button.
    public void OnClickOptions()
    {
        Debug.Log("Options");
    }

    // On click event handler for clicking the log out button.
    public void OnClickLogOut()
    {
        MenuVM viewModel = GameObject.Find("View").GetComponent<MenuVM>();
        viewModel.LogOut();
        sceneLoader.LoadNextScene(Utils.SIGN_IN_SCENE);
    }
}
