using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ForgotPasswordVM : RegisterViewModel { 
    
    // Override methods.
    protected override ErrorTypes[] GetErrorTypes()
    {
        return new ErrorTypes[] { ErrorTypes.ResetPassword };
    }
    protected override string[] GetFields()
    {
        return new string[] { Email };
    }
    protected override void RegisterAction()
    {
        model.ResetPassword(Email);
    }
    public override string GetOnFinishActionPropertyName()
    {
        return nameof(model.IsResetPassword);
    }
}
