using Assets.model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneVM : MonoBehaviour
{

    public IDatabaseHandler databaseHandler;
    public Text usernameText;

    public void Start()
    {
        databaseHandler = FirebaseManager.Instance;
        usernameText.text = databaseHandler.GetUsername();
    }

    public void OnClickStartSimulation()
    {
        Debug.Log("Start simulation");
    }

    public void OnClickLearnFromQuestions()
    {
        Debug.Log("Learn from questions");
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
        databaseHandler.ResetCurrentUser();
        SceneManager.LoadScene("SignInScene");
    }
}
