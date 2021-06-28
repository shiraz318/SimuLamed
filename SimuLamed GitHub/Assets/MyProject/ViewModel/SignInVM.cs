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
    
    // Override methods.
    protected override ErrorTypes[] GetErrorTypes()
    {
        return new ErrorTypes[] { ErrorTypes.SignIn };
    }
    protected override string[] GetFields()
    {
        return new string[] { Email, Password};
    }
    public override string GetOnFinishActionPropertyName()
    {
        return nameof(model.IsSignedIn);
    }
    protected override void RegisterAction()
    {
        model.SignIn(Password, Email);
    }
}
