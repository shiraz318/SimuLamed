using Assets.model;
using System;
using System.Collections.Generic;

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

    private Score score;
    //public Score UserScore { get; set; }
    //public Score score;
    public List<int> correctAns;
    


    public User(string username, string email, List<int> correctAns)
    {
        this.correctAns = correctAns;
        this.username = username;
        this.email = email;
    }

    public void InitUserScore(int NumOfQuestions)
    {
        score = new Score(NumOfQuestions);
        foreach (int correctAnsNum in correctAns)
        {
            score.SetQuestionScore(correctAnsNum, true);
        }
    }

    public void SetScore(int questionNum, bool isAnsCorrect)
    {
        score.SetQuestionScore(questionNum, isAnsCorrect);
    }

    public void SetCorrectAns()
    {
        correctAns = score.GetTrueScore();
    }
}