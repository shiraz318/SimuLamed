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
    public SceneLoader sceneLoader;

    //public Canvas canvas;


    public void Start()
    {
        //DontDestroyOnLoad(canvas);
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
        errorText.text = "";

        string username = inputUsername.text.ToString();
        string password = inputPassword.text.ToString();
        string email = inputEmail.text.ToString();
        if (username.Equals("") || password.Equals("") || email.Equals(""))
        {
            errorText.text = "PLEASE FILL ALL FIELDS";
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
        sceneLoader.LoadNextScene("SignInScene");

    }

    public void OnClickBack()
    {
        sceneLoader.LoadNextScene("SignInScene");

    }
}
