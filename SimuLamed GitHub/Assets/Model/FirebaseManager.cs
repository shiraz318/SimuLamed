using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private const string projectId = "simulamed-49311-default-rtdb";

//    https://simulamed-49311-default-rtdb.firebaseio.com/
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";

    public delegate void PostUserCallback();

    /// <summary>
    /// Adds a user to the Firebase Database
    /// </summary>
    /// <param name="user"> User object that will be uploaded </param>
    /// <param name="userId"> Id of the user that will be uploaded </param>
    /// <param name="callback"> What to do after the user is uploaded successfully </param>
    public static void PostUser(User user, string userId, PostUserCallback callback)
    {
        Proyecto26.RestClient.Put<User>($"{databaseURL}users/{userId}.json", user).Then(response =>
        {
            callback();
        });
    }
}
