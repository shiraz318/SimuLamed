using Assets.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInSceneVM : MonoBehaviour
{
    public IDatabaseHandler databaseHandler;

    public InputField inputPassword;
    public InputField inputEmail;
    public Text errorText;

    public void Start()
    {
        databaseHandler = FirebaseManager.Instance;
        databaseHandler.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Error"))
            {
                if (databaseHandler.Error.ErrorType == ErrorTypes.SignIn)
                {
                    errorText.text = databaseHandler.Error.Message;
                }
            }
        };
    }

    // On click new user button.
    public void OnClickNewUser()
    {
        SceneManager.LoadScene("SignUpScene");
    }

    // On click sign in button.
    public void OnClickSignIn()
    {
        string email = inputEmail.text.ToString();
        string password = inputPassword.text.ToString();
        
        databaseHandler.SignInUser(password, email, OnSuccess);
        //SceneManager.LoadScene("MenuScene");
    }

    

    public void OnSuccess()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OnClickForgotPassword()
    {
        SceneManager.LoadScene("ForgotPasswordScene");
    }

    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
