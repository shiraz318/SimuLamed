using Assets.model;
using Proyecto26;
using System.ComponentModel;
using UnityEngine;
using FullSerializer;
using System;

// Singleton FirebaseManager.
public sealed class FirebaseManager : IDatabaseHandler
{

    // Constructor.
    private FirebaseManager() 
    {
        error = new ErrorObject("", ErrorTypes.None);
        ResetCurrentUser();
    }
    

    private const string projectId = "simulamed-49311-default-rtdb";
    private const string authKey = "AIzaSyBS5WVLpACpe5AbRrZ2KmWcw92FFR65Vs0";
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    private static fsSerializer serializer = new fsSerializer();


    public delegate void GetUserCallback(User user);
    public delegate void PostUserCallback();

    public event PropertyChangedEventHandler PropertyChanged;

    private ErrorObject error;
    public ErrorObject Error { get { return error; } set { error = value; NotifyPropertyChanged("Error"); } }

    public User currentUser;

    private static readonly object padlock = new object();
    private static FirebaseManager instance = null;

    // Thread safety singleton using double check locking 
    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new FirebaseManager();
                    }
                }
            }
            return instance;
        }
    }
    
    public string GetUsername()
    {
        return currentUser.username;
    }

    public void ResetCurrentUser()
    {
        currentUser = null;
    }


    // Post a given user to the database.
    private void PostUser(User user, string userId)
    {
        RestClient.Put<User>($"{databaseURL}users/{userId}.json", user);
    }

    // Sign up a given user.
    public void SignUpUser(string username, string password, string email, Action onSuccess)
    {

        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + authKey,
            bodyString: userData).Then(response =>
        {
            
            User user = new User(username, email);
            user.localId = response.localId;
            user.idToken = response.idToken;

            PostUser(user, response.localId);
            Debug.Log("SAVED USER");
            Error = new ErrorObject("", ErrorTypes.None);

            string emailData = "{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"" + user.idToken + "\"}";
            RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + authKey, emailData);
            onSuccess();


        }).Catch(error => 
        {
            RequestException exception = (RequestException)error;
            string errorMessage = GetResponseError(exception);
            Error = new ErrorObject(errorMessage, ErrorTypes.SignUp);

        });
    }

    // Extract the response message from a given RequestExcetion.
    private string GetResponseError(RequestException exception)
    {
        return exception.Response.Split(',')[1].Split(':')[1].Split('\"')[1].Replace('_', ' ');
    }


    // Sign in a given user.
    public void SignInUser(string password, string email, Action onSuccess)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + authKey,
            bodyString: userData).Then(response =>
            {
                string emailVerification = "{\"idToken\":\"" + response.idToken + "\"}";
                RestClient.Post("https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=" + authKey, emailVerification)
                .Then(emailResponse =>
                {
                    fsData emailVerificationData = fsJsonParser.Parse(emailResponse.Text);
                    EmailConfirmationInfo emailConfirmationInfo = new EmailConfirmationInfo();
                    serializer.TryDeserialize(emailVerificationData, ref emailConfirmationInfo).AssertSuccessWithoutWarnings();
                    
                    if (emailConfirmationInfo.users[0].emailVerified)
                    {
                        SetCurrentUser(response.localId, response.idToken, email, onSuccess);
                        Debug.Log("I AM IN!!!");
                        Error = new ErrorObject("", ErrorTypes.None);
                        
                    }
                    else
                    {
                        Error = new ErrorObject("PLEASE VERIFY YOUR EMAIL", ErrorTypes.SignIn);
                    }

                });

            }).Catch(error => {

                RequestException exception = (RequestException)error;
                string errorMessage = GetResponseError(exception);
                Error = new ErrorObject(errorMessage, ErrorTypes.SignIn);

            });
    }

    private void SetCurrentUser(string localId, string idToken, string email, Action onSuccess)
    {
        currentUser = new User("", "");
        currentUser.localId = localId;
        currentUser.idToken = idToken;
        currentUser.email = email;
        GetUsernameFromDatabase(onSuccess);
    }

    public void ResetPassword(string email, Action onSuccess)
    {
        string payload = "{\"email\":\"" + email + "\",\"requestType\":\"PASSWORD_RESET\"}";
        RestClient.Post("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + authKey,
            bodyString: payload).Then(response =>
            {
                Debug.Log("SENT EMAIL");
                Error = new ErrorObject("", ErrorTypes.None);
                onSuccess();

            }).Catch(error =>
            {
                RequestException exception = (RequestException)error;
                string errorMessage = GetResponseError(exception);
                Error = new ErrorObject(errorMessage, ErrorTypes.ResetPassword);

            });

    }


    private void GetUsernameFromDatabase(Action onSuccess)
    {
        RestClient.Get<User>($"{databaseURL}users/{currentUser.localId}.json").Then(response =>
        {
            currentUser.username = response.username;
            Debug.Log(currentUser.username);
            onSuccess();
        });
    }

    // Notify property changed.
    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }


    /// <summary>
    /// Adds a user to the Firebase Database
    /// </summary>
    /// <param name="user"> User object that will be uploaded </param>
    /// <param name="userId"> Id of the user that will be uploaded </param>
    /// <param name="callback"> What to do after the user is uploaded successfully </param>
    private void GetUser(string userId, GetUserCallback callback)
    {
        RestClient.Get<User>($"{databaseURL}users/{userId}.json").Then(user => { callback(user); });
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