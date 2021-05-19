using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ForgotPasswordVM : RegisterViewModel { 
    
    protected override ErrorTypes GetErrorType()
    {
        return ErrorTypes.ResetPassword;
    }
    protected override string[] GetFields()
    {
        return new string[] { Email };
    }
    protected override void RegisterAction(Utils.OnSuccessFunc onSuccess)
    {
        model.ResetPassword(Email, onSuccess);
    }

}
