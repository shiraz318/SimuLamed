using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class StatisticsVM : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private IAppModel model;
    private int totalCorrectAns = 0;
    private int totalAns = 0;
    private string errorMessage;


    [Binding]
    public string ErrorMessage
    {
        get { return errorMessage; }
        set { errorMessage = value; NotifyPropertyChanged("ErrorMessage"); }
    }

    [Binding]
    public float SafetyValue
    {
        //get { return 1f; }
        get { return GetValue(Utils.SAFETY_HEBREW); }
    }

    [Binding]
    public float TransactionRulesValue
    {
        //get { return 0.6f; }
        get { return GetValue(Utils.TRANSACTION_RULES_HEBREW); }
    }

    [Binding]
    public float SignsValue
    {
        //get { return 0.4f; }
        get { return GetValue(Utils.SIGNS_HEBREW); }
    }
    
    [Binding]
    public float UnderstandingVehicleValue
    {
        //get { return 0.2f; }
        get { return GetValue(Utils.UNDERSTANDING_VEHICLE_HEBREW); }
    }

    [Binding]
    public float MixedValue
    {
        //get { return 1f; }
        get { if (model == null) { return 0; } return (float)totalCorrectAns / (float)totalAns; }
    }


    private void Start()
    {

        model = AppModel.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Error"))
            {
                if (model.Error.ErrorType == ErrorTypes.Statistics)
                {
                    ErrorMessage = model.Error.Message;
                }
            }
        };
        // Notify that the values changed - model is set now.
        NotifyPropertyChanged("UnderstandingVehicleValue");
        NotifyPropertyChanged("SignsValue");
        NotifyPropertyChanged("MixedValue");
        NotifyPropertyChanged("TransactionRulesValue");
        NotifyPropertyChanged("SafetyValue");

    }

    // Get the value of the score of the given category
    private float GetValue(string category)
    {
        // Model not set yet.
        if (model == null) { return 0; }

        int numOfQuestions = model.GetNumOfQuestionsByCategory(category);
        int correctAnswers = model.GetNumOfCorrectAnswersByCategory(category);

        totalAns += numOfQuestions;
        totalCorrectAns += correctAnswers;

        return (float)correctAnswers / (float)numOfQuestions;
    }


    // On propery changed.
    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
