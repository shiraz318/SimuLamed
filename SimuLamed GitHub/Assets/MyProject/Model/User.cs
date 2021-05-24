using Assets;
using Assets.model;
using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;


[Serializable] 
public struct User
{
    public bool IsAssigned { get; set; }
    public UserDetails details; // contains the user details.
    public UserState state; // contains the user state.
    
    public User (UserDetails details, UserState state)
    {
        IsAssigned = true;
        this.details = details;
        this.state = state;
    }
    public void ResetUser()
    {
        IsAssigned = false;
        state = null;
        details.ResetDetails();
    }


    //// Init the user score.
    //public void InitUserScore(int numOfQuestions)
    //{
    //    state.InitScore(numOfQuestions);
    //}
   
    //// Set the user score according to the given question number and whether the user answered correctly on it or not.
    //public void SetScore(int questionNum, bool isAnsCorrect)
    //{
    //    state.SetScore(questionNum, isAnsCorrect);
    //}

    //// Set the correct answers of the user
    //public void SetCorrectAns()
    //{
    //    state.SetCorrectAns();
    //}

    //public void AddHint()
    //{
    //    state.numOfHints++;
    //}
    //public void DecreaseHint()
    //{
    //    state.numOfHints--;
    //}
    //public bool IsDeserveNewHint()
    //{
    //    return state.IsDeserveNewHint();

    //}
    //public int GetOpenLevel()
    //{
    //    return state.openLevel;
    //}
    //public void InitCorrectAnsInARowCounter()
    //{
    //    state.InitCorrectAnsInARowCounter();
    //}
    //public void AddLevel()
    //{
    //    state.openLevel++;
    //}
}