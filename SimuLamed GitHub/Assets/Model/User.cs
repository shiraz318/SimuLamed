using Assets.model;
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

    public Score score;
    //public Score UserScore { get; set; }
    //public Score score;
    public int[] correctAnswers;
    //public CorrectAnswer[] correctAnswers;
    


    public User(string username, string email, int[] correctAnswers)
    {
        //this.correctAns = correctAns;
        this.correctAnswers = correctAnswers;
        this.username = username;
        this.email = email;
    }

    public void InitUserScore(int NumOfQuestions)
    {
        score = new Score(NumOfQuestions);
        //score.InitCorrectAns();
        foreach (int correctAnsNum in correctAnswers)
        {
            //if (correctAnsNum > -1)
            //{
                score.SetQuestionScore(correctAnsNum, true);
            //}
        }
    }

    public void SetScore(int questionNum, bool isAnsCorrect)
    {
        score.SetQuestionScore(questionNum, isAnsCorrect);
    }

    public void SetCorrectAns()
    {
        // score.SetCorrectAns();
        //return score.Select((b, i) => b == true ? i : -1).Where(i => i != -1).ToArray();


        correctAnswers = score.GetTrueScore();
        //correctAnswers = correctAnswersIndexes.Select((a,b) => new CorrectAnswer(a)).ToArray();
    }
}