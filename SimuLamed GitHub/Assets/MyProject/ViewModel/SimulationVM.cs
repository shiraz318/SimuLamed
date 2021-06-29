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
    
    private int displayedQuestionsCounter;
    private int lives;
    private string questionNumberText;
    private Utils.QuestionOption[] playerScore;
    public const string OPENED_LEVEL = "OpenedLevel";
    public const string FINISHED_LEVEL = "FinishLastLevel";


    // Properties


    public int DisplayedQuestionsCounter 
    { 
        get { return displayedQuestionsCounter; } 
        set { displayedQuestionsCounter = value; QuestionNumberText = value.ToString(); } 
    }
    [Binding]
    public int Lives 
    { 
        get { return lives; } set { lives = value; NotifyPropertyChanged(); } 
    }
    [Binding]
    public string QuestionNumberText 
    { 
        get { return DisplayedQuestionsCounter.ToString() + "/" + Utils.QUESTIONS_NUM_IN_SIM; } 
        set 
        { 
            questionNumberText = value + "/" + Utils.QUESTIONS_NUM_IN_SIM;
            NotifyPropertyChanged(); 
        }  
    }


    public const int NUMBER_OF_LEVELS = 3;
    public static int currentLevel = LevelsVM.chosenLevelIdx;
    
    [SerializeField]
    private QuestionsManager questionsManager;
    


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
            if (eventArgs.PropertyName.Equals(nameof(questionsManager.LastAnswerResults)))
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
            NotifyPropertyChanged(OPENED_LEVEL);
            currentLevel++;
        }
        else
        {
            NotifyPropertyChanged(FINISHED_LEVEL);
        }
       
    }
    
    // Get the property name of finishing the main action of the view model.
    public string GetOnFinishActionPropertyName()
    {
        return nameof(model.IsUserSaved);
    }

    // Called when the user exit the simulation.
    public void OnExitSimulation()
    {
        model.UpdateUserScore(playerScore);
        model.UpdateUserLevel(currentLevel);
        model.SaveUser();
    }



    // Override methods.
    protected override void OnStart()
    {
        IsSaveingFailed = false;
        currentLevel = LevelsVM.chosenLevelIdx;
        DisplayedQuestionsCounter = 0;
        SetQuestionsManager();
        Lives = Utils.MAX_NUMBER_OF_ERRORS;
        
        questionsManager.IsQuestionSet = false;
    }
    protected override void SetModel()
    {
        base.SetModel();
        model.SetQuestionsByLevel(currentLevel.ToString());
        ResetScore();
    }
    protected override void AdditionalModelSettings(PropertyChangedEventArgs eventArgs)
    {
        // The model got the questions from the database.
        if (eventArgs.PropertyName.Equals(nameof(model.Questions)))
        {
            
            questionsManager.IsQuestionSet = true;
            questionsManager.SetQuestions(model.Questions.ToArray());
        }
        base.AdditionalModelSettings(eventArgs);
    }
    protected override ErrorTypes[] GetErrorTypes() { return new ErrorTypes[]{ ErrorTypes.SaveScore, ErrorTypes.LoadQuestions }; }
}
