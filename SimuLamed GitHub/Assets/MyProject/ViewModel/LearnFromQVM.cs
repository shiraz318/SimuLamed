using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;


[Binding]
public class LearnFromQVM : BaseViewModel
{
    // Private fields.
    private string selectedSubject;

    // Properties.
    [Binding]
    public string SelectedSubject
    {
        get { return selectedSubject; }
        set { selectedSubject = value; model.SelectedSubject = Question.FromCategoryToTypeEnglish(value); NotifyPropertyChanged("SelectedSubject"); }
    }

    private new void Start()
    {
        base.Start();

    }
    // Methods.

    // Save current user.
    public void SaveUser() 
    {
        
        model.SaveUser(); 
    }

    protected override ErrorTypes[] GetErrorTypes() { return new ErrorTypes[] { ErrorTypes.SaveScore }; }

    public string GetPropertyName() { return "IsUserSaved"; }

    protected override void SetModel()
    {
        base.SetModel();
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }

            Debug.Log(eventArgs.PropertyName);
            string propertyName = GetPropertyName();
            if (eventArgs.PropertyName.Equals(propertyName))
            {
                Debug.Log("NOTIFY");
                NotifyPropertyChanged(propertyName);
            }

        };
    }
}
