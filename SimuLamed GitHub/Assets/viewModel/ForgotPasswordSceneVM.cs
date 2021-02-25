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

    public void Start()
    {
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
        SceneManager.LoadScene("SignInScene");
    }
    public void OnClickBack()
    {
        SceneManager.LoadScene("SignInScene");
    }


}
