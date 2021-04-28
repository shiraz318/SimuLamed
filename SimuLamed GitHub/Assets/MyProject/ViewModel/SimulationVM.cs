using Assets;
using Assets.model;
using Assets.Model;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityWeld.Binding;

[Binding]
public class SimulationVM : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public QuestionsManager questionsManager;


    public const int NUMBER_OF_LEVELS = 3;
    private const int MAX_NUMBER_OF_ERRORS = 4;

    private int currentLevel;
    private int displayedQuestionsCounter;
    public int DisplayedQuestionsCounter { get { return displayedQuestionsCounter; } set { displayedQuestionsCounter = value; QuestionNumberText = value.ToString(); } }

    private int lives;
    [Binding]
    public int Lives { get { return lives; } set { lives = value; NotifyPropertyChanged("Lives"); } }
    [Binding]
    public int Level { get; set; }

    private string questionNumberText; 
    [Binding]
    public string QuestionNumberText { get { return DisplayedQuestionsCounter.ToString() + "/" + Utils.QUESTIONS_NUM_IN_SIM; } set {questionNumberText = value + "/" + Utils.QUESTIONS_NUM_IN_SIM; NotifyPropertyChanged("QuestionNumberText"); }  }
    private IAppModel model;

    private Utils.QuestionOption[] playerScore;
    
    
    
    void Awake()
    {
        DisplayedQuestionsCounter = 0;
        SetModel();
        ResetScore();

        SetQuestionsManager();
        
        // Get all the questions.
        model.SetQuestionsByCategory(Utils.MIXED_HEBREW, toRnd:false);
        Lives = MAX_NUMBER_OF_ERRORS;
        


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
    private void SetModel()
    {
        model = AppModel.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            // The model got the questions from the database.
            if (eventArgs.PropertyName.Equals("Questions"))
            {
                questionsManager.SetQuestions(model.Questions.ToArray());
            }
        };
    }
    private void SetQuestionsManager()
    {
        questionsManager.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            // The player answered a question.
            if (eventArgs.PropertyName.Equals("LastAnswerResults"))
            {

                Tuple<int, bool> result = questionsManager.LastAnswerResults;
                
                // Answer is correct.
                if (result.Item2)
                {
                    playerScore[result.Item1] = Utils.QuestionOption.Correct;
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
        if (Level <= (NUMBER_OF_LEVELS - 1))
        {
            Level++;
        }


        // Set the user score to && between the user score arrays.
        //model.UpdateUserScore(playerScore);

        // Save the user level. 
        NotifyPropertyChanged("LevelFinished");


    }
    public void OnExitSimulation()
    {

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
