using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager
{
    private const string projectId = "simulamed-49311-default-rtdb";
    private const string authKey = "AIzaSyBS5WVLpACpe5AbRrZ2KmWcw92FFR65Vs0";
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";

    public delegate void GetUserCallback(User user);
    public delegate void PostUserCallback();
  
    public void PostUser(User user, string userId)
    {
        Proyecto26.RestClient.Put<User>($"{databaseURL}users/{userId}.json", user);
    }

    public void SignUpUser(User user)
    {
       string userData = "{\"email\":\"" + user.email + "\",\"password\":\"" + user.password + "\",\"returnSecureToken\":true}";
        Proyecto26.RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + authKey, userData).
            Then(response =>
        {
            PostUser(user, response.localId);
        }).Catch(error => {
            Debug.Log(error);
        });
    }


    
    /// <summary>
    /// Adds a user to the Firebase Database
    /// </summary>
    /// <param name="user"> User object that will be uploaded </param>
    /// <param name="userId"> Id of the user that will be uploaded </param>
    /// <param name="callback"> What to do after the user is uploaded successfully </param>
    public static void GetUser(string userId, GetUserCallback callback)
    {
        Proyecto26.RestClient.Get<User>($"{databaseURL}users/{userId}.json").Then(user => { callback(user); });
    }
}



//User user2 = new User("Shiraz", "9876", "Email");
//FirebaseManager.PostUser(user2, user2.email, () =>
//{
//    Debug.Log($"{user2.username} {user2.password} {user2.email}");
//});
//FirebaseManager.GetUser("1", user =>
//{
//    Debug.Log($"{user.username} {user.password} {user.email}");
//});