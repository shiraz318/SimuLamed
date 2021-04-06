﻿using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class SignUpVM : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private IAppModel model;
    private string errorMessage;

    [Binding]
    public string ErrorMessage
    {
        get { return errorMessage; }
        set
        {
            errorMessage = value;
            if (value != "") { IsLoadingCircleOn = false; }
            NotifyPropertyChanged("ErrorMessage");
        }
    }

    private string email = "";
    [Binding]
    public string Email
    {
        get { return email; }
        set { email = value; }
    }

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

    private bool isLoadingCircleOn = false;
    [Binding]
    public bool IsLoadingCircleOn
    {
        get { return isLoadingCircleOn; }
        set { isLoadingCircleOn = value; NotifyPropertyChanged("IsLoadingCircleOn"); }
    }


    private void Start()
    {
        IsLoadingCircleOn = false;
        model = AppModel.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Error"))
            {
                if (model.Error.ErrorType == ErrorTypes.SignUp)
                {
                    ErrorMessage = model.Error.Message;
                }
            }

        };
    }

    // Sign up.
    public void SignUp(Utils.OnSuccessFunc onSuccess)
    {
        IsLoadingCircleOn = true;
        ErrorMessage = "";

        // Empty fields.
        if (Username.Equals("") || Password.Equals("") || Email.Equals(""))
        {
            ErrorMessage = Utils.EMPTY_FIELD_MESSAGE;
        }
        else
        {
            model.SignUp(Username, Password, Email, onSuccess);
        }
    }

    // On property changed.
    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
