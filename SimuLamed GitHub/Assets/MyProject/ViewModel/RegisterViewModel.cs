using Assets;
using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public abstract class RegisterViewModel : BaseViewModel {


    private string email = "";
    [Binding]
    public string Email
    {
        get { return email; }
        set { email = value; }
    }


    private string errorMessage;
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

    private bool isLoadingCircleOn = false;
    [Binding]
    public bool IsLoadingCircleOn
    {
        get { return isLoadingCircleOn; }
        set { isLoadingCircleOn = value; NotifyPropertyChanged("IsLoadingCircleOn"); }
    }


    public void OnAction(Utils.OnSuccessFunc onSuccess)
    {
        IsLoadingCircleOn = true;
        ErrorMessage = "";

        if (CheckFieldsValidity())
        {
            RegisterAction(onSuccess);
        }

    }

    private bool CheckFieldsValidity()
    {
        string[] fields = GetFields();
        foreach (string field in fields)
        {
            if (field.Equals(""))
            {
                ErrorMessage = Utils.EMPTY_FIELD_MESSAGE_H;
                return false;
            }
        }
        return true;
    }
    protected abstract string[] GetFields();
    protected abstract void RegisterAction(Utils.OnSuccessFunc onSuccess);
    protected override void OnStart()
    {
        IsLoadingCircleOn = false;
    }




}
