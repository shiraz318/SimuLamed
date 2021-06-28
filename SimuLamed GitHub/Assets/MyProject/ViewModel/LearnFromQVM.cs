using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;


[Binding]
public class LearnFromQVM : SaveViewModel
{
    private string selectedSubject;

    // Properties.
    [Binding]
    public string SelectedSubject
    {
        get { return selectedSubject; }
        set 
        { 
            selectedSubject = value;
            model.SelectedSubject = Question.FromCategoryToTypeEnglish(value);
            NotifyPropertyChanged(); 
            //NotifyPropertyChanged("SelectedSubject"); 
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


    // Override methods.
    protected override ErrorTypes[] GetErrorTypes() { return new ErrorTypes[] { ErrorTypes.SaveScore }; }

}
