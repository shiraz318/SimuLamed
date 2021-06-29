using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class StatisticsVM : BaseViewModel
{
    // Private fields.
    private int totalCorrectAns = 0;
    private int totalAns = 0;
    private bool isLoadingCircleOn;
    private string errorMessage;


    // Properties.
    [Binding]
    public float SafetyValue
    {
        get { return GetValue(Question.SAFETY_HEBREW); }
    }

    [Binding]
    public float TransactionRulesValue
    {
        get { return GetValue(Question.TRANSACTION_RULES_HEBREW); }
    }

    [Binding]
    public float SignsValue
    {
        get { return GetValue(Question.SIGNS_HEBREW); }
    }
    
    [Binding]
    public float UnderstandingVehicleValue
    {
        get { return GetValue(Question.UNDERSTANDING_VEHICLE_HEBREW); }
    }

    [Binding]
    public float MixedValue
    {
        get { if(model == null) { return 0; } return (float)totalCorrectAns / (float)totalAns; }
    }

    [Binding]
    public override string ErrorMessage
    {
        get { return errorMessage; }
        set
        {
            errorMessage = value;
            if (value != "") { IsLoadingCircleOn = false; }
            NotifyPropertyChanged();
        }
    }
    [Binding]
    public bool IsLoadingCircleOn
    {
        get { return isLoadingCircleOn; }set { isLoadingCircleOn = value; NotifyPropertyChanged(); }
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

        return numOfQuestions != 0 ? (float)correctAnswers / (float)numOfQuestions : 0f;
    }


    // Override methods.
    protected override void OnStart()
    {
        base.OnStart();
        IsLoadingCircleOn = true;
    }
    protected override void SetModel()
    {
        base.SetModel();
        model.SetFromNumToType(ErrorTypes.Statistics);
    }
    protected override void AdditionalModelSettings(PropertyChangedEventArgs eventArgs)
    {
        if (eventArgs.PropertyName.Equals(nameof(model.FromQuestionNumToType)))
        {
            NotifyPropertyChanged(nameof(UnderstandingVehicleValue));
            NotifyPropertyChanged(nameof(SignsValue));
            NotifyPropertyChanged(nameof(MixedValue));
            NotifyPropertyChanged(nameof(TransactionRulesValue));
            NotifyPropertyChanged(nameof(SafetyValue));
            IsLoadingCircleOn = false;
            
            
        }
    }
    protected override ErrorTypes[] GetErrorTypes()
    {
        return new ErrorTypes[] { ErrorTypes.Statistics };
    }


}
