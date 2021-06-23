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
    public UserDetails details; // contains the user details.
    public UserState state; // contains the user state.
    
    public User (UserDetails details, UserState state)
    {
//        IsAssigned = true;
        this.details = details;
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