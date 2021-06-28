using Assets;
using Assets.model;
using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;


[Serializable] 
public class User
{
  //  public bool IsAssigned { get; set; }
    //public UserDetails details; // contains the user details.
    public string username;
    public UserState state; // contains the user state.
    
    //public User (UserDetails details, UserState state)
    public User (string username, UserState state)
    {
//        IsAssigned = true;
        this.username = username;
        this.state = state;
    }
    
    //// Reset the user fiels.
    //public void ResetUser()
    //{
    //    IsAssigned = false;
    //    state = null;
    //    details.ResetDetails();
    //}

}