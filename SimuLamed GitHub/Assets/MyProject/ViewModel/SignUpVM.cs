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


    protected override ErrorTypes GetErrorType()
    {
        return ErrorTypes.SignUp;
    }
    protected override string[] GetFields()
    {
        return new string[] { Email, Password, Username};
    }
    protected override void RegisterAction(Utils.OnSuccessFunc onSuccess)
    {
        model.SignUp(Username, Password, Email, onSuccess);
    }

}
