using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ForgotPasswordSceneVM : MonoBehaviour
{
    public IDatabaseHandler databaseHandler;
    public InputField inputEmail;
    public Text errorText;
    public SceneLoader sceneLoader;

    //public Canvas canvas;


    public void Start()
    {
        //DontDestroyOnLoad(canvas);

        databaseHandler = FirebaseManager.Instance;
        databaseHandler.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (databaseHandler.Error.ErrorType == ErrorTypes.ResetPassword)
            {
                errorText.text = databaseHandler.Error.Message;
            }
        };
    }


    public void OnClickResetPassword()
    {
        errorText.text = "";

        string email = inputEmail.text.ToString();
        if (email.Equals(""))
        {
            errorText.text = "PLEASE ENTER YOUR EMAIL";
        }
        else
        {
            databaseHandler.ResetPassword(email, OnSuccess);
        }
    }
    public void OnSuccess()
    {
        sceneLoader.LoadNextScene("SignInScene");
    }
    public void OnClickBack()
    {
        sceneLoader.LoadNextScene("SignInScene");
    }


}
