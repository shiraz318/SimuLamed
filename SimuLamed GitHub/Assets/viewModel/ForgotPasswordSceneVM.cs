using Assets;
using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ForgotPasswordSceneVM : MonoBehaviour
{
    private IModel model;
    public InputField inputEmail;
    public static Text errorText;
    public SceneLoader sceneLoader;



    public void Start()
    {
        errorText = GameObject.FindWithTag("ErrorMessage").GetComponent<Text>() as Text;

        model = Model.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (model.Error.ErrorType == ErrorTypes.ResetPassword)
            {
                errorText.text = model.Error.Message;
            }
        };
    }


    public void OnClickResetPassword()
    {
        errorText.text = "";

        string email = inputEmail.text.ToString();
        if (email.Equals(""))
        {
            errorText.text = Utils.EMPTY_EMAIL_MESSAGE;
        }
        else
        {
            model.ResetPassword(email, () => sceneLoader.LoadNextScene("SignInScene"));
        }
    }
    public void OnClickBack()
    {
        sceneLoader.LoadNextScene("SignInScene");
    }


}
