using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class BaseViewModel : MonoBehaviour, INotifyPropertyChanged
{

    private string errorMessage = "";
    protected IAppModel model;
    public event PropertyChangedEventHandler PropertyChanged;
    
    // Properties.
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



    // Virtual methods.
    protected virtual void SetModel() { 
        
        model = AppModel.Instance;
        ErrorTypes[] errorTypes = GetErrorTypes();

        if (!errorTypes[0].Equals(ErrorTypes.None))
        {
            model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
            {
                if (this == null) { return; }
                if (eventArgs.PropertyName.Equals("Error"))
                {
                    foreach (ErrorTypes errorType in errorTypes)
                    {
                        if (model.Error.ErrorType == errorType)
                        {
                            ErrorMessage = model.Error.Message;
                        }
                    }
                }
                else { AdditionalModelSettings(eventArgs); }

            };
        }
    }
    protected virtual void AdditionalModelSettings(PropertyChangedEventArgs eventArgs) { }
    protected virtual void OnStart() { }
    protected virtual ErrorTypes[] GetErrorTypes() { return new ErrorTypes[] { ErrorTypes.None }; }



}
