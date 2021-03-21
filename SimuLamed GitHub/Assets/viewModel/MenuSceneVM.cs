using Assets.model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneVM : MonoBehaviour
{

    private IModel model;
    public Text usernameText;
    public SceneLoader sceneLoader;

    public void Start()
    {
        model = Model.Instance;
        usernameText.text = model.GetCurrentUsername();
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
        sceneLoader.LoadNextScene("StatisticsScene");
    }
    public void OnClickOptions()
    {
        Debug.Log("Options");
    }

    public void OnClickLogOut()
    {
        model.ResetCurrentUser();
        sceneLoader.LoadNextScene("SignInScene");
    }
     
}
