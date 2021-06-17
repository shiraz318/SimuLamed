﻿using Assets;
using UnityEngine;
using UnityEngine.Rendering;

public class SignInView : RegisterView
{

    public void Awake()
    {
        //Application.targetFrameRate = -1;
        //OnDemandRendering.renderFrameInterval = 1;
        // Reset player prefab.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString(Utils.SHOW_QUESTIONS, Utils.DEFAULT_TO_SHOW_QUESTIONS == true ? "show" : string.Empty);
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
    }
}
