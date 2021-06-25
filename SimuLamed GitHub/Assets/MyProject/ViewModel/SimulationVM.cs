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
public class SimulationVM : SaveViewModel
{
    // Private fields.
    private const int MAX_NUMBER_OF_ERRORS = 4;
    private int displayedQuestionsCounter;
    private int lives;
    private string questionNumberText;
    private Utils.QuestionOption[] playerScore;
    // Properties
    public int DisplayedQuestionsCounter 
    { 
        get { return displayedQuestionsCounter; } 
        set { displayedQuestionsCounter = value; QuestionNumberText = value.ToString(); } 
    }
    [Binding]
    public int Lives { get { return lives; } set { lives = value; NotifyPropertyChanged("Lives"); } }
    [Binding]
    public string QuestionNumberText 
    { 
        get { return DisplayedQuestionsCounter.ToString() + "/" + Utils.QUESTIONS_NUM_IN_SIM; } 
        set 
        { 
            questionNumberText = value + "/" + Utils.QUESTIONS_NUM_IN_SIM;
            NotifyPropertyChanged("QuestionNumberText"); 
        }  
    }


    public const int NUMBER_OF_LEVELS = 3;
    public QuestionsManager questionsManager;
    public static int currentLevel = LevelsVM.chosenLevelIdx;


    // Convert the index of the current level to the current level name.
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

    // Resets the score of the player.
    private void ResetScore()
    {
        playerScore = new Utils.QuestionOption[model.NumOfQuestions];
        for (int i = 0; i < playerScore.Length; i++)
        {
            playerScore[i] = Utils.QuestionOption.NotAsked;
        }
    }

    // Set the current level index.
    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    // Display the given question's number question.
    public void DisplayQuestion(int questionNumber)
    {
        DisplayedQuestionsCounter++;
        questionsManager.DisplayQuestion(questionNumber);
    }

    // Set the question manager properties.
    private void SetQuestionsManager()
    {
        questionsManager.IsSimulationQuestinos = true;
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
                    // Finish answering the amount of necessery questions.
                    if (displayedQuestionsCounter.ToString().Equals(Utils.QUESTIONS_NUM_IN_SIM))
                    {
                        OnFinishLevel();
                    }
                }
                // Answer is wrong.
                else
                {
                    Lives--;
                    playerScore[result.Item1] = Utils.QuestionOption.Wrong;
                }
            }

        };
    }

    // Called when the current level is finished.
    public void OnFinishLevel()
    {
        if (currentLevel < (NUMBER_OF_LEVELS))
        {
            // Save the user level. 
            NotifyPropertyChanged("OpenedLevel");
        }
        else
        {
            NotifyPropertyChanged("FinishLastLevel");
        }
    }
    
    // Get the property name of finishing the main action of the view model.
    public string GetOnFinishActionPropertyName()
    {
        return "IsUserSaved";
    }

    // Called when the user exit the simulation.
    public void OnExitSimulation()
    {
        model.UpdateUserScore(playerScore);
        model.UpdateUserLevel(currentLevel);
        model.SaveUser();
        //model.SaveUser();
    }




    // Override methods.
    protected override void OnStart()
    {
        IsSaveingFailed = false;
        currentLevel = LevelsVM.chosenLevelIdx;
        DisplayedQuestionsCounter = 0;
        SetQuestionsManager();
        Lives = MAX_NUMBER_OF_ERRORS;
    }
    protected override void SetModel()
    {
        base.SetModel();
        model.SetQuestionsByLevel(FromIdxToName());
        ResetScore();
        //model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        //{
        //    if (this == null) { return; }

        //    string propertyName = GetPropertyName();

        //    // The model got the questions from the database.
        //    if (eventArgs.PropertyName.Equals("Questions"))
        //    {
        //        questionsManager.SetQuestions(model.Questions.ToArray());
        //    }
        //    else if (eventArgs.PropertyName.Equals(propertyName))
        //    {
        //        NotifyPropertyChanged(propertyName);
        //    }
        //};
        // Get the questions of the current level from the database.
    }
    protected override void AdditionalModelSettings(PropertyChangedEventArgs eventArgs)
    {
        //string propertyName = GetOnFinishActionPropertyName();
        questionsManager.IsQuestionSet = false;
        // The model got the questions from the database.
        if (eventArgs.PropertyName.Equals("Questions"))
        {
            questionsManager.IsQuestionSet = true;
            questionsManager.SetQuestions(model.Questions.ToArray());
        }
        base.AdditionalModelSettings(eventArgs);
        //else if (eventArgs.PropertyName.Equals(propertyName))
        //{
        //    NotifyPropertyChanged(propertyName);
        //} 
        //else if (eventArgs.PropertyName.Equals("IsSaveingFailed"))
        //{
        //    IsSaveingFailed = true;
        //}
    }
    protected override ErrorTypes[] GetErrorTypes() { return new ErrorTypes[]{ ErrorTypes.SaveScore, ErrorTypes.LoadQuestions }; }
}
