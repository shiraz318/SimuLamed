using Assets;
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
    private IModel model;

    public InputField inputPassword;
    public InputField inputEmail;
    public static Text errorText;
    public SceneLoader sceneLoader;
    public static Image loadingCircle;

    public void Start()
    {
        errorText = GameObject.FindWithTag("ErrorMessage").GetComponent<Text>() as Text;
        loadingCircle = GameObject.FindWithTag("LoadingCircle").GetComponent<Image>() as Image;

        model = Model.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Error"))
            {
                if (model.Error.ErrorType == ErrorTypes.SignIn)
                {
                    errorText.text = model.Error.Message;

                    if (!model.Error.ErrorType.Equals(""))
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
            errorText.text = Utils.EMPTY_FIELD_MESSAGE;
            return;
        }

        SetLoadingCircle(true);
        model.SignIn(password, email, () => sceneLoader.LoadNextScene("MenuScene"));
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
        SetAlpha(alpha, loadingCircle);

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
