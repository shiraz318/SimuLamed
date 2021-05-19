using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public abstract class BaseViewModel : MonoBehaviour, INotifyPropertyChanged
{

    // Fields.
    private string errorMessage;

    protected IAppModel model;

    public event PropertyChangedEventHandler PropertyChanged;
    [Binding]
    public virtual string ErrorMessage
    {
        get { return errorMessage; }
        set
        {
            errorMessage = value;
            NotifyPropertyChanged("ErrorMessage");
        }
    }


    // Methods.
    protected virtual void SetModel() { 
        
        model = AppModel.Instance;
        ErrorTypes errorType = GetErrorType();

        if (!errorType.Equals(ErrorTypes.None))
        {
            model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
            {
                if (eventArgs.PropertyName.Equals("Error"))
                {
                    if (model.Error.ErrorType == errorType)
                    {
                        ErrorMessage = model.Error.Message;
                    }
                }

            };
        }
        


    }
    protected virtual void OnStart() { }
    protected virtual ErrorTypes GetErrorType() { return ErrorTypes.None; }

    public void Start()
    {
        OnStart();  
        SetModel();
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
