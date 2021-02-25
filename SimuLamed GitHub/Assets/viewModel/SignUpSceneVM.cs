using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpSceneVM : MonoBehaviour
{
    private IDatabaseHandler databaseHandler;

    public InputField inputUsername;
    public InputField inputPassword;
    public InputField inputEmail;
    public Text errorText;


    public void Start()
    {
        databaseHandler = FirebaseManager.Instance;
        databaseHandler.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (databaseHandler.Error.ErrorType == ErrorTypes.SignUp)
            {
                errorText.text = databaseHandler.Error.Message;
            }
        };
    }

    // On click sign up button.
    public void OnClickSignUp()
    {
        string username = inputUsername.text.ToString();
        string password = inputPassword.text.ToString();
        string email = inputEmail.text.ToString();
        if (username.Equals(""))
        {
            errorText.text = "ENTER A USERNAME";
            return;
        }
        databaseHandler.SignUpUser(username, password, email, OnSuccess);

        //firebaseManager.SignUpUser(new User(username, password, email));
        //string username  = Inpu
        //FirebaseManager.SignUpUser();
        //SceneManager.LoadScene("MenuScene");
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
