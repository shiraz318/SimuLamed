using Assets;
using Assets.model;
using Assets.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityWeld.Binding;

[Binding]
public class SimulationVM : BaseViewModel
{
    // Private fields.
    private const int MAX_NUMBER_OF_ERRORS = 1;
    private int displayedQuestionsCounter;
    private int lives;
    private string questionNumberText;
    private Utils.QuestionOption[] playerScore;

    // Properties
    public int DisplayedQuestionsCounter { get { return displayedQuestionsCounter; } set { displayedQuestionsCounter = value; QuestionNumberText = value.ToString(); } }
    [Binding]
    public int Lives { get { return lives; } set { lives = value; NotifyPropertyChanged("Lives"); } }
    //[Binding]
    //public static int Level;
    [Binding]
    public string QuestionNumberText { get { return DisplayedQuestionsCounter.ToString() + "/" + Utils.QUESTIONS_NUM_IN_SIM; } set {questionNumberText = value + "/" + Utils.QUESTIONS_NUM_IN_SIM; NotifyPropertyChanged("QuestionNumberText"); }  }

    
    public const int NUMBER_OF_LEVELS = 3;
    public QuestionsManager questionsManager;
    
    //public static string currentLevelName = LevelsVM.chosenLevel;
    public static int currentLevel = LevelsVM.chosenLevelIdx;


    protected override void OnStart()
    {
        currentLevel = LevelsVM.chosenLevelIdx;
        Debug.Log("CURRENT LEVEL NAME: " + FromIdxToName());
        Debug.Log(currentLevel);
        DisplayedQuestionsCounter = 0;
        //SetModel();
        
        //if (!LevelsVM.isSet)
        //{
        //     currentLevel = LevelsVM.chosenLevelIdx;
        //    LevelsVM.isSet = true;
        //}

        SetQuestionsManager();
        //GameObject.Find("QuestionMenu").SetActive(false);
        // Get all the questions.
        //model.SetQuestionsByCategory(Utils.MIXED_HEBREW, toRnd:false);

       

        // Get only the quesions of the current level simulation.!

        Lives = MAX_NUMBER_OF_ERRORS;
    }

    private string FromIdxToName()
    {
        switch (currentLevel)
        {
            case 1:
                return "Level1";
            case 2:
                return "Level2";
            case 3:
                return "Level3";
            default:
                return LevelsVM.chosenLevel;
        }
    }

    private void ResetScore()
    {
        playerScore = new Utils.QuestionOption[model.NumOfQuestions];
        for (int i = 0; i < playerScore.Length; i++)
        {
            playerScore[i] = Utils.QuestionOption.NotAsked;
        }
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void DisplayQuestion(int questionNumber)
    {

        DisplayedQuestionsCounter++;
        questionsManager.DisplayQuestion(questionNumber);
    }

    // Set the model.
    protected override void SetModel()
    {
        base.SetModel();
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }

            string propertyName = GetPropertyName();

            // The model got the questions from the database.
            if (eventArgs.PropertyName.Equals("Questions"))
            {
                questionsManager.SetQuestions(model.Questions.ToArray());
            }
            else if (eventArgs.PropertyName.Equals(propertyName))
            {
                NotifyPropertyChanged(propertyName);
            }
        };
        model.SetQuestionsByLevel(FromIdxToName());
        ResetScore();
    }

    

    private void SetQuestionsManager()
    {
       // questionsManager = GameObject.Find("QuestionsManager").GetComponent<QuestionsManager>();
        questionsManager.IsSimulationQuestinos = true;
        //questionsManager.SetQuestions(model.Questions.ToArray());

        questionsManager.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }

            // The player answered a question.
            if (eventArgs.PropertyName.Equals("LastAnswerResults"))
            {

                Tuple<int, bool> result = questionsManager.LastAnswerResults;
                
                // Answer is correct.
                if (result.Item2)
                {
                    playerScore[result.Item1] = Utils.QuestionOption.Correct;
                    if (displayedQuestionsCounter.ToString().Equals(Utils.QUESTIONS_NUM_IN_SIM))
                    {
                        OnFinishLevel();
                    }
                }
                else
                {
                    Lives--;
                    playerScore[result.Item1] = Utils.QuestionOption.Wrong;
                }
            }

        };
    }

    public void OnFinishLevel()
    {
        if (currentLevel < (NUMBER_OF_LEVELS))
        {
            //currentLevel++;
            // Save the user level. 
            NotifyPropertyChanged("OpenedLevel");
        }
        else
        {
            NotifyPropertyChanged("FinishLastLevel");
        }




    }
    protected override ErrorTypes[] GetErrorTypes() { return new ErrorTypes[]{ ErrorTypes.SaveScore, ErrorTypes.LoadQuestions }; }
    public string GetPropertyName()
    {
        return "IsUserSaved";
    }
    public void OnExitSimulation()
    {
        model.UpdateUserScore(playerScore);
        model.UpdateUserLevel(currentLevel - 1);
        model.SaveUser();
        
    }
}
