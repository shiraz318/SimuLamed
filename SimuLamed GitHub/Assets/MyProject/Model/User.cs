using Assets;
using Assets.model;
using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;


[Serializable] 
public class User
{
    //public string username;
    //public string email;
    //public string localId;
    //public string idToken;
    public UserDetails details;
    public UserState state;
    
    //public User(string username, string email, int[] correctAnswers, int numOfHints, int openLevel)
    //{
    //    this.username = username;
    //    this.email = email;
    //    this.state = new UserState(correctAnswers, numOfHints, openLevel);
    //}
    public User (UserDetails details, UserState state)
    {
        this.details = details;
        this.state = state;
    }

    public void InitUserScore(int numOfQuestions)
    {
        state.InitScore(numOfQuestions);
    }
   

    public void SetScore(int questionNum, bool isAnsCorrect)
    {
        state.SetScore(questionNum, isAnsCorrect);
    }

    public void SetCorrectAns()
    {
        state.SetCorrectAns();
    }

    public void AddHint()
    {
        state.numOfHints++;
    }
    public void DecreaseHint()
    {
        state.numOfHints--;
    }
    public bool IsDeserveNewHint()
    {
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