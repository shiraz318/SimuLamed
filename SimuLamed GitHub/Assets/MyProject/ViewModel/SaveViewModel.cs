using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class SaveViewModel : BaseViewModel
{
    private bool isSaveingFailed;
    private bool isUserSaved;

    [Binding]
    public bool IsSaveingFailed { get { return isSaveingFailed; } set { isSaveingFailed = value; 
            NotifyPropertyChanged(); } }
            //NotifyPropertyChanged("IsSaveingFailed"); } }
    public bool IsUserSaved { get { return isUserSaved; } set { isUserSaved = value; 
            NotifyPropertyChanged(); } }
            //NotifyPropertyChanged("IsUserSaved"); } }

    
    protected override void AdditionalModelSettings(PropertyChangedEventArgs eventArgs)
    {

        if (eventArgs.PropertyName.Equals(nameof(IsUserSaved)))
        {
            IsUserSaved = true;
        }
        else if (eventArgs.PropertyName.Equals(nameof(IsSaveingFailed)))
        {
            IsSaveingFailed = true;
        }
    }


}
