using Assets;
using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpSceneVM : MonoBehaviour
{
    //private IDatabaseHandler databaseHandler;
    private IModel model;

    public InputField inputUsername;
    public InputField inputPassword;
    public InputField inputEmail;
    public static Text errorText;
    public SceneLoader sceneLoader;



    public void Start()
    {
        errorText = GameObject.FindWithTag("ErrorMessage").GetComponent<Text>() as Text;
        model = Model.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (model.Error.ErrorType == ErrorTypes.SignUp)
            {
                errorText.text = model.Error.Message;
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
            errorText.text = Utils.EMPTY_FIELD_MESSAGE;
            return;
        }
        model.SignUp(username, password, email, () => sceneLoader.LoadNextScene("SignInScene"));
    }

    public void OnClickBack()
    {
        sceneLoader.LoadNextScene("SignInScene");

    }
}
