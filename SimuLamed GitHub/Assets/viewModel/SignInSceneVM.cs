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
    public SceneLoader sceneLoader;
    public Image loadingCircle;
    //public Canvas canvas;

    public void Start()
    {
         
        //DontDestroyOnLoad(loadingCircle);
        databaseHandler = FirebaseManager.Instance;
        databaseHandler.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Error"))
            {
                if (databaseHandler.Error.ErrorType == ErrorTypes.SignIn)
                {
                    errorText.text = databaseHandler.Error.Message;
                    
                    if (!databaseHandler.Error.ErrorType.Equals(""))
                    {
                        SetLoadingCircle(false);
                    }
                }
            }
        };
        SetLoadingCircle(false);
       

    }



    // On click new user button.
    public void OnClickNewUser()
    {
        sceneLoader.LoadNextScene("SignUpScene");
    }

    // On click sign in button.
    public void OnClickSignIn()
    {
        errorText.text = "";

        Debug.Log("IN SIGN IN SCENE VM");
        string email = inputEmail.text.ToString();
        string password = inputPassword.text.ToString();
        
        if (email.Equals("") || password.Equals(""))
        {
            errorText.text = "PLEASE FILL ALL FIELDS";
            return;
        }

        SetLoadingCircle(true);
        databaseHandler.SignInUser(password, email, OnSuccess);
    }

    private void SetAlpha(float alpha, Image image)
    {
        var tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }

    private void SetLoadingCircle(bool toShow)
    {
        float alpha = 0f;
        if (toShow)
        {
            alpha = 1f;
        }

        Image[] children = loadingCircle.GetComponentsInChildren<Image>();
        foreach (Image child in children)
        {
            SetAlpha(alpha, child);
        }
        //SetAlpha(alpha, progress);
        SetAlpha(alpha, loadingCircle);



    }

    public void OnSuccess()
    {
        sceneLoader.LoadNextScene("MenuScene");

    }

    public void OnClickForgotPassword()
    {
        sceneLoader.LoadNextScene("ForgotPasswordScene");

    }

    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
