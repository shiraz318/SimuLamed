using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class SignUpVM : RegisterViewModel
{
    private string password = "";
    [Binding]
    public string Password
    {
        get { return password; }
        set { password = value; }
    }

    private string username = "";
    [Binding]
    public string Username
    {
        get { return username; }
        set { username = value; }
    }


    protected override ErrorTypes[] GetErrorTypes()
    {
        return new ErrorTypes[] { ErrorTypes.SignUp };
    }
    protected override string[] GetFields()
    {
        return new string[] { Email, Password, Username};
    }
    public override string GetOnFinishActionPropertyName()
    {
        return "IsSignedUp";
    }
    protected override void RegisterAction()
    {
        model.SignUp(Username, Password, Email);
    }

}
