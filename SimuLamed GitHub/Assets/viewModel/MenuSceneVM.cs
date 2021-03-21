using Assets.model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneVM : MonoBehaviour
{

    private IModel model;
    //public IDatabaseHandler databaseHandler;
    public Text usernameText;
    public SceneLoader sceneLoader;

    public void Start()
    {
        model = Model.Instance;
        usernameText.text = model.GetCurrentUsername();
        //databaseHandler = FirebaseManager.Instance;
        //usernameText.text = databaseHandler.GetUsername();
    }


    public void OnClickStartSimulation()
    {
        Debug.Log("Start simulation");
    }

    public void OnClickLearnFromQuestions()
    {
        sceneLoader.LoadNextScene("LearningFromQuestionsScene");
    }

    public void OnClickStatisticsAndPropgress()
    {
        Debug.Log("Statistics and progress");
    }
    public void OnClickOptions()
    {
        Debug.Log("Options");
    }

    public void OnClickLogOut()
    {
        model.ResetCurrentUser();
        //databaseHandler.ResetCurrentUser();
        sceneLoader.LoadNextScene("SignInScene");
        //SceneManager.LoadScene("SignInScene");
    }
}
