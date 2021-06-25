using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;


[Binding]
public class LearnFromQVM : SaveViewModel
{
    private string selectedSubject;
//    private bool isSaveingFailed;

    //[Binding]
    //public bool IsSaveingFailed { get { return isSaveingFailed; } set { isSaveingFailed = value; NotifyPropertyChanged("IsSaveingFailed"); } }

    // Properties.
    [Binding]
    public string SelectedSubject
    {
        get { return selectedSubject; }
        set 
        { 
            selectedSubject = value;
            model.SelectedSubject = Question.FromCategoryToTypeEnglish(value);
            NotifyPropertyChanged("SelectedSubject"); 
        }
    }

    private new void Start()
    {
        IsSaveingFailed = false;
        base.Start();
    }

    // Save current user.
    public void SaveUser() 
    {
        model.SaveUser(); 
    }
   // public string GetOnFinishActionPropertyName() { return "IsUserSaved"; }


    // Override methods.
    protected override ErrorTypes[] GetErrorTypes() { return new ErrorTypes[] { ErrorTypes.SaveScore }; }
    //protected override void AdditionalModelSettings(PropertyChangedEventArgs eventArgs)
    //{
    //    string propertyName = GetOnFinishActionPropertyName();
    //    if (eventArgs.PropertyName.Equals(propertyName))
    //    {
    //        NotifyPropertyChanged(propertyName);
    //    } else if (eventArgs.PropertyName.Equals("IsSaveingFailed"))
    //    {
    //        IsSaveingFailed = true;
    //    }
    //}
}
