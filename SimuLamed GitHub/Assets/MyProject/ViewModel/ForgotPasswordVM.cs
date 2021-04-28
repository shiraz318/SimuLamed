using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ForgotPasswordVM : MonoBehaviour, INotifyPropertyChanged
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
                if (model.Error.ErrorType == ErrorTypes.ResetPassword)
                {
                    ErrorMessage = model.Error.Message;
                }
            }

        };
    }

    // Reset Password.
    public void ResetPassword(Utils.OnSuccessFunc onSuccess)
    {
        IsLoadingCircleOn = true;
        ErrorMessage = "";
        
        // Email is empty.
        if (Email.Equals(""))
        {
            ErrorMessage = Utils.EMPTY_EMAIL_MESSAGE;
        }
        else
        {
            model.ResetPassword(Email, onSuccess);
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
