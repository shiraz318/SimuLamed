using Assets;
using UnityEngine;

public class SignInView : RegisterView
{

    public void Awake()
    {
        // Reset player prefab.
        PlayerPrefs.DeleteAll();
    }
    //public void OnClickNewUser()
    //{
    //    GoToOtherScene(Utils.SIGN_UP_SCENE);
    //}
    //public void OnClickForgotPassword()
    //{
    //    GoToOtherScene(Utils.FORGOT_PASSWORD_SCENE);
    //}
    //public void OnClickSignIn()
    //{
    //    OnRegisterAciton(Utils.MENU_SCENE);
    //}

    // On click quit button.
    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
