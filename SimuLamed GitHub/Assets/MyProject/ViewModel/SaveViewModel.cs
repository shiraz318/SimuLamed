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

    [Binding]
    public bool IsSaveingFailed { get { return isSaveingFailed; } set { isSaveingFailed = value; NotifyPropertyChanged("IsSaveingFailed"); } }

    
    protected override void AdditionalModelSettings(PropertyChangedEventArgs eventArgs)
    {

        if (eventArgs.PropertyName.Equals("IsUserSaved"))
        {
            NotifyPropertyChanged("IsUserSaved");
        }
        else if (eventArgs.PropertyName.Equals("IsSaveingFailed"))
        {
            IsSaveingFailed = true;
        }
    }


    //[Binding]
    //public bool IsError { get { return isError; } set { isError = value; NotifyPropertyChanged("IsError"); }}



}
