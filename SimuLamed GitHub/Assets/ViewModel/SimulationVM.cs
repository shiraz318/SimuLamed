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

    private int lives;
    [Binding]
    public int Lives { get { return lives; } set { lives = value; NotifyPropertyChanged("Lives"); } }
    [Binding]
    public int Level { get; set; }
    private IAppModel model;

    private Score playerScore;
    
    
    
    void Start()
    {

        displayedQuestionsCounter = 0;
        SetModel();
        SetQuestionsManager();
        
        // Get all the questions.
        model.SetQuestionsByCategory(Utils.MIXED_HEBREW);
        Lives = MAX_NUMBER_OF_ERRORS;
        


    }
    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void DisplayQuestion(int questionNumber)
    {
        displayedQuestionsCounter++;
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
                playerScore.SetQuestionScore(result.Item1, result.Item2);
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
