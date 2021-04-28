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
public class QuestionsVM : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private int lastHintsNumber;
    [Binding]
    public int HintsNumber { get { lastHintsNumber = AppModel.Instance.HintsNumber; return lastHintsNumber; } }

    private bool isNewHint;
    [Binding]
    public bool IsNewHint { get { return isNewHint; } set { isNewHint = value; NotifyPropertyChanged("IsNewHint"); } }

    [Binding]
    public string SelectedSubject
    {
        get { return Question.FromTypeToCategory(AppModel.Instance.SelectedSubject); }
    }

    private bool isHintButtonOn;
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
    private string questionNumText = "0 / 0";
    [Binding]
    public string QuestionNumText { get { return questionNumText; } set { questionNumText = value + " / " + numberOfQuestions.ToString(); NotifyPropertyChanged("QuestionNumText"); } }
   // public string QuestionNumText { get { if (!IsQuestionSet) { return "0 / 0"; } return (questionNum + 1).ToString() + " / " + numberOfQuestions.ToString(); } }


    private IAppModel model;


    private static int questionNum;
    private bool isQuestionSet;
    public bool IsQuestionSet { get { return isQuestionSet; } set { isQuestionSet = value; NotifyPropertyChanged("QuestionNumText"); } }

    private int numberOfQuestions;
    public QuestionsManager questionsManager;

    void Start()
    {
        IsQuestionSet = false;

        SetModel();

        lastHintsNumber = model.HintsNumber;

        // Get the questions of the selected subject category.
        model.SetQuestionsByCategory(SelectedSubject, toRnd:true);

        IsHintButtonOn = true;
        NotifyPropertyChanged("HintsNumber");

        SetQuestionsManager();
    }

    private void SetQuestionsManager()
    {
        
        questionsManager.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("LastAnswerResults"))
            {
                // questionNumber, isCorrect.
                Tuple<int, bool> result = questionsManager.LastAnswerResults;
                model.SetUserScore(result.Item1, result.Item2);
            }

        };

            
    }

    // Set the model.
    private void SetModel()
    {
        model = AppModel.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            // The model got the questions from the database.
            if (eventArgs.PropertyName.Equals("Questions") && !IsQuestionSet)
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
        };

    }

    // Present a question.
    private void DisplayQuestion()
    {

        questionsManager.DisplayQuestion(questionNum);

        // New question - update the question number.
        QuestionNumText = (questionNum + 1).ToString();
        //NotifyPropertyChanged("QuestionNumText");

        IsHintButtonOn = true;
    }


    // NO ONE IS CALLING THIS!!!!!
    // On click event handler for clicking an answer.
    public void OnClickAnswer(bool isAnsCorrect)
    {

        // Set the answer buttons to be not clickable.
        // IsAnsInteractable = false;

        IsHintButtonOn = false;
        //questionsManager.OnClickAnswer(isAnsCorrect);
        //if (!isAnsCorrect) { NotifyPropertyChanged("WrongAnswer"); }
        // model.SetUserScore(questions[questionNum].questionNumber, isAnsCorrect);
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
       // return "";
        //return questions[questionNum].correctAns;
    }

    // On click event handler for clicking hint button.
    public void OnClickHint()
    {
        model.DecreaseHint();
        // Disable the hint button - can't have more then one hint per question.
        IsHintButtonOn = false;
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

