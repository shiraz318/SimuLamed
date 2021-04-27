using Assets;
using Assets.model;
using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The user class, which gets uploaded to the Firebase Database
/// </summary>

[Serializable] // This makes the class able to be serialized into a JSON
public class User
{

    public string username;
    public string email;
    public string localId;
    public string idToken;
    

    public UserState state;

    
    //public int numOfHints;

    //public Score score;
    ////public Score UserScore { get; set; }
    ////public Score score;
    //public int[] correctAnswers;
    //private bool[] lastAnswers;
    //private int lastAnswersIndex;
    


    public User(string username, string email, int[] correctAnswers, int numOfHints, int openLevel)
    {
        //this.correctAns = correctAns;
       // this.correctAnswers = correctAnswers;
        this.username = username;
        this.email = email;
        //lastAnswers = new bool[Utils.HINTS_PER_CORRECT_ANS];
        //lastAnswersIndex = 0;
        //this.numOfHints = numOfHints;
        this.state = new UserState(correctAnswers, numOfHints, openLevel);
    }

    public void InitUserScore(int numOfQuestions)
    {
        state.InitScore(numOfQuestions);
        //score = new Score(numOfQuestions);
        ////score.InitCorrectAns();
        //foreach (int correctAnsNum in correctAnswers)
        //{
        //    //if (correctAnsNum > -1)
        //    //{
        //        score.SetQuestionScore(correctAnsNum, true);
        //    //}
        //}
    }
    public int GetNumOfQuestions()
    {
        return state.GetNumOfQuestions();
    }
   

    public void SetScore(int questionNum, bool isAnsCorrect)
    {
        state.SetScore(questionNum, isAnsCorrect);
        //if (isAnsCorrect)
        //{
        //    lastAnswers[lastAnswersIndex] = isAnsCorrect;
        //    lastAnswersIndex = (lastAnswersIndex + 1) % Utils.HINTS_PER_CORRECT_ANS;
        //} else
        //{
        //    lastAnswers = new bool[Utils.HINTS_PER_CORRECT_ANS];
        //}
        
        //score.SetQuestionScore(questionNum, isAnsCorrect);
    }

    public void SetCorrectAns()
    {
        // score.SetCorrectAns();
        //return score.Select((b, i) => b == true ? i : -1).Where(i => i != -1).ToArray();

        state.SetCorrectAns();
        //correctAnswers = score.GetTrueScore();
        //correctAnswers = correctAnswersIndexes.Select((a,b) => new CorrectAnswer(a)).ToArray();
    }

    public void AddHint()
    {
        state.numOfHints++;
        //numOfHints++;
    }
    public void DecreaseHint()
    {
        state.numOfHints--;
       // numOfHints--;
    }
    public bool IsDeserveNewHint()
    {
        //return lastAnswers[Utils.HINTS_PER_CORRECT_ANS - 1];
        return state.IsDeserveNewHint();

    }
    public int GetOpenLevel()
    {
        return state.openLevel;
    }
    public void InitLastAns()
    {
        state.InitLastAns();
    }
    public void AddLevel()
    {
        state.openLevel++;
    }
}