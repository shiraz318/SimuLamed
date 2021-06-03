using Assets;
using Assets.model;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityWeld.Binding;

[Binding]
public class QuestionsVM : BaseViewModel
{
    // Private fields.
    private int lastHintsNumber;
    private bool isNewHint;
    private bool isHintButtonOn;
    private string questionNumText = "0 / 0";
    private static int questionNum;
    private bool isQuestionSet;
    private int numberOfQuestions;

    // Properties.
    [Binding]
    public int HintsNumber { get { lastHintsNumber = AppModel.Instance.HintsNumber; return lastHintsNumber; } }

    [Binding]
    public bool IsNewHint 
    { 
        get { return isNewHint; } 
        set { isNewHint = value; NotifyPropertyChanged("IsNewHint"); } 
    }

    [Binding]
    public string SelectedSubject { get { return Question.FromTypeToCategory(AppModel.Instance.SelectedSubject); }}

    [Binding]
    public bool IsHintButtonOn
    {
        get { return isHintButtonOn; }
        set
        {
            if (HintsNumber == 0)
            { isHintButtonOn = false; }
            else
            { isHintButtonOn = value; }
            NotifyPropertyChanged("IsHintButtonOn");
        }
    }
    [Binding]
    public string QuestionNumText 
    {
        get { return questionNumText; } 
        set 
        {
            questionNumText = value + " / " + numberOfQuestions.ToString();
            NotifyPropertyChanged("QuestionNumText"); 
        } 
    }
    public bool IsQuestionSet 
    {
        get { return isQuestionSet; } 
        set { isQuestionSet = value; NotifyPropertyChanged("QuestionNumText"); } 
    }

    public QuestionsManager questionsManager;


    // Set the propertychanged property of the question manager.
    private void SetQuestionsManager()
    {
        questionsManager.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }

            if (eventArgs.PropertyName.Equals("LastAnswerResults"))
            {
                Tuple<int, bool> result = questionsManager.LastAnswerResults;  // questionNumber, isCorrect.
                model.SetUserScore(result.Item1, result.Item2);
                IsHintButtonOn = false;
            }
        };
    }

    // Present a question.
    private void DisplayQuestion()
    {
        questionsManager.DisplayQuestion(questionNum);

        // New question - update the question number.
        QuestionNumText = (questionNum + 1).ToString();
        IsHintButtonOn = true;
    }

    // On click event handler for clicking next question button.
    public void OnClickNextQuestion()
    {
        questionNum = (questionNum + 1) % numberOfQuestions;
        DisplayQuestion();
    }

    // On click event handler for clicking last question button.
    public void OnClickLastQuestion()
    {
        if (questionNum != 0)
        {
            questionNum = (questionNum - 1) % numberOfQuestions;
            DisplayQuestion();
        }
    }

    // Get the correct answer of the current question.
    public string GetCorrectAns()
    {
        return questionsManager.CurrentQuestion.correctAns;
    }

    // On click event handler for clicking hint button.
    public void OnClickHint()
    {
        model.DecreaseHint();
        // Disable the hint button - can't have more then one hint per question.
        IsHintButtonOn = false;
    }



    // Override methods.
    protected override ErrorTypes[] GetErrorTypes()
    {
        return new ErrorTypes[] { ErrorTypes.LoadQuestions };
    }
    protected override void OnStart()
    {
        IsQuestionSet = false;
    }
    protected override void SetModel()
    {
        base.SetModel();
        lastHintsNumber = model.HintsNumber;

        // Get the questions of the selected subject category.
        model.SetQuestionsByCategory(SelectedSubject, true);

        IsHintButtonOn = true;
        NotifyPropertyChanged("HintsNumber");

        SetQuestionsManager();

    }
    protected override void AdditionalModelSettings(PropertyChangedEventArgs eventArgs)
    {
        // The model got the questions from the database.
        if (eventArgs.PropertyName.Equals("Questions") && !IsQuestionSet && model.Questions.Length > 0)
        {
            questionNum = 0;
            numberOfQuestions = model.Questions.Length;
            questionsManager.SetQuestions(model.Questions.ToArray());
            IsQuestionSet = true;
            model.InitUserLastAns();
            DisplayQuestion();
        }
        else if (eventArgs.PropertyName.Equals("HintsNumber"))
        {
            IsNewHint = (lastHintsNumber < HintsNumber);
            NotifyPropertyChanged("HintsNumber");
        }

    }

}

