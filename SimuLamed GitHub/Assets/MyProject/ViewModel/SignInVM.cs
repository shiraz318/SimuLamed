using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class SignInVM : RegisterViewModel
{
   
    private string password = "";
    [Binding]
    public string Password
    {
        get { return password; }
        set { password = value; }
    }
    
    protected override ErrorTypes GetErrorType()
    {
        return ErrorTypes.SignIn;
    }

    protected override string[] GetFields()
    {
        return new string[] { Email, Password};
    }

    protected override void RegisterAction(Utils.OnSuccessFunc onSuccess)
    {
        onSuccess += delegate () { PlayerPrefs.SetString(Utils.SHOW_QUESTIONS, Utils.DEFAULT_TO_SHOW_QUESTIONS == true ? "show" : string.Empty); };
        model.SignIn(Password, Email, onSuccess);
    }
}
