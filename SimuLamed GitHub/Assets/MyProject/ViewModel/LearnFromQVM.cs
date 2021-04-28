using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;


[Binding]
public class LearnFromQVM : MonoBehaviour, INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private IAppModel model;
    private string errorMessage;

    [Binding]
    public string ErrorMessage
    {
        get { return errorMessage; }
        set { errorMessage = value; NotifyPropertyChanged("ErrorMessage"); }
    }

    private string selectedSubject;
    public string SelectedSubject
    {
        get { return selectedSubject; }
        set { selectedSubject = value; model.SelectedSubject = Question.FromCategoryToTypeEnglish(value); }
    }


    
    private void Start()
    {
        model = AppModel.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Error"))
            {
                if (model.Error.ErrorType == ErrorTypes.SaveScore)
                {
                    ErrorMessage = model.Error.Message;
                }
            }
        };
    }

    // Save current user.
    public void Back(Utils.OnSuccessFunc onSuccess)
    {
        model.SaveUser(onSuccess);
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
