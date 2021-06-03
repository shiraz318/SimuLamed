using Assets;
using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public abstract class RegisterViewModel : BaseViewModel {

    // Private fields.
    private string email = "";
    private string errorMessage;
    private bool isLoadingCircleOn = false;

    // Properties.
    [Binding]
    public string Email { get { return email; } set { email = value; } }
    [Binding]
    public override string ErrorMessage
    {
        get { return errorMessage; }
        set
        {
            errorMessage = value;
            if (value != "") { IsLoadingCircleOn = false; }
            NotifyPropertyChanged("ErrorMessage");
        }
    }
    [Binding]
    public bool IsLoadingCircleOn { get { return isLoadingCircleOn; }
        set { isLoadingCircleOn = value; NotifyPropertyChanged("IsLoadingCircleOn"); }
    }

    // Called when a registretion action accures.
    public void OnRegisterAction()
    {
        IsLoadingCircleOn = true;
        ErrorMessage = "";
        // If the fields are valid - do the registretion action.
        if (CheckFieldsValidity()) { RegisterAction(); }
    }

    // Check if the fields are valid.
    private bool CheckFieldsValidity()
    {
        string[] fields = GetFields();
        foreach (string field in fields)
        {
            if (field.Equals(""))
            {
                ErrorMessage = ErrorObject.EMPTY_FIELD_MESSAGE;
                return false;
            }
        }
        return true;
    }


    // Override methods.
    protected override void OnStart()
    {
        IsLoadingCircleOn = false;
    }
    protected override void AdditionalModelSettings(PropertyChangedEventArgs eventArgs)
    {
        string propertyName = GetOnFinishActionPropertyName();
        if (eventArgs.PropertyName.Equals(propertyName))
        {
            NotifyPropertyChanged(propertyName);
        }

    }


    // Abstract methods.
    protected abstract string[] GetFields();
    protected abstract void RegisterAction();
    public abstract string GetOnFinishActionPropertyName();
    



}
