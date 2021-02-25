using System;

/// <summary>
/// The user class, which gets uploaded to the Firebase Database
/// </summary>

[Serializable] // This makes the class able to be serialized into a JSON
public class User
{
    public string username;
    public string email;
    //public GameInfo gameInfo;
    public string localId;
    public string idToken;


    public User(string username, string email)
    {
        this.username = username;
        this.email = email;
    }
}