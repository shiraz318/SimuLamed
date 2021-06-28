using Assets;
using Assets.model;
using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;


[Serializable] 
public class User
{
    public string username;
    public UserState state; // contains the user state.
    
    public User (string username, UserState state)
    {
        this.username = username;
        this.state = state;
    }
    public User(string username)
    {
        this.username = username;
        this.state = new UserState();

    }
}