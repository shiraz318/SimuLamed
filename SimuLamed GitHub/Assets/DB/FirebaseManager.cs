using Assets.model;
using Proyecto26;
using System.ComponentModel;
using UnityEngine;
using FullSerializer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Assets;
using System.Threading.Tasks;

// Singleton FirebaseManager.
public sealed class FirebaseManager : IDatabaseHandler
{
    

    private const string projectId = "simulamed-49311-default-rtdb";
    private const string authKey = "AIzaSyBS5WVLpACpe5AbRrZ2KmWcw92FFR65Vs0";
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    
    private static fsSerializer serializer = new fsSerializer();


    public delegate void GetUserCallback(User user);
    public delegate void PostUserCallback();



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
    

    // Post a given user to the database.
    private void PostUser(User user, string userId, Utils.OnSuccessFunc onSuccess, Utils.OnFailureFunc onFailure)
    {
        RestClient.Put<User>($"{databaseURL}users/{userId}.json?auth=" + user.idToken , user).
            Then(response => { onSuccess();}).
            Catch(error => { onFailure(ExtractErrorMessage(error));

            });
    }

    // Sign up a given user.
    public void SignUpUser(string username, string password, string email, Utils.OnSuccessFunc onSuccess, Utils.OnFailureFunc onFailure)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        
        // Sign up as a user.
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + authKey,
            bodyString: userData).Then(response =>
        {
            User user = new User(username, email, new int[] { -1 }, Utils.INITIAL_NUMBER_OF_HINTS, 0)
            {
                localId = response.localId,
                idToken = response.idToken
            };

            // Post user data to the database and if post succeded - send an email to the user for verification.
            PostUser(user, response.localId, onSuccess: () => SendEmailForVerification(user.idToken, onSuccess, onFailure), onFailure);

        }).Catch(error => 
        {
            onFailure(ExtractErrorMessage(error));
        });
    }


    // Send an email for verification.
    private void SendEmailForVerification(string idToken, Utils.OnSuccessFunc onSuccess, Utils.OnFailureFunc onFailure)
    {
        string emailData = "{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"" + idToken + "\"}";
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + authKey, emailData).
            Then( response => { onSuccess();}).
            Catch(error => { onFailure(ExtractErrorMessage(error));});
    }

    // Create a new Error Object and set the Error property.
    private string ExtractErrorMessage(Exception error)
    {
        RequestException exception = (RequestException)error;
        string errorMessage = GetResponseError(exception);
        return errorMessage;
    }

    // Extract the response message from a given RequestExcetion.
    private string GetResponseError(RequestException exception)
    {
        return exception.Response.Split(',')[1].Split(':')[1].Split('\"')[1].Replace('_', ' ');
    }

    // Sign in a given user.
    public void SignInUser(string password, string email, Utils.OnSuccessSignInFunc onSuccess, Utils.OnFailureFunc onFailure)
    {
        Debug.Log("IN SIGN IN FIREBASE MANAGER");
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        

        // Sign in with password and email.
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + authKey,
            bodyString: userData).Then(response =>
            {
                Debug.Log("BEFORE CHECK EMAIL VERIFICATION");
                // Check for email verification.
                CheckEmailVerification(response, email, onSuccess, onFailure);

            }).Catch(error => {
                onFailure(ExtractErrorMessage(error));
            });
    }

    // Check if the given email has been verified.
    private void CheckEmailVerification(SignResponse response, string email, Utils.OnSuccessSignInFunc onSuccess, Utils.OnFailureFunc onFailure)
    {
        Debug.Log("IN CHECK EMAIL VERIFICATION");
        string emailVerification = "{\"idToken\":\"" + response.idToken + "\"}";
        
        // Check if the user verified his email.
        RestClient.Post("https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=" + authKey, emailVerification)
        .Then(emailResponse =>
        {
            Debug.Log("INSIDE CHECK EMAIL VERIFICATION RESPONSE");
            fsData emailVerificationData = fsJsonParser.Parse(emailResponse.Text);
            EmailConfirmationInfo emailConfirmationInfo = new EmailConfirmationInfo();
            serializer.TryDeserialize(emailVerificationData, ref emailConfirmationInfo).AssertSuccessWithoutWarnings();

            // If the user verified his email he can sign in.
            if (emailConfirmationInfo.users[0].emailVerified)
            {
                Debug.Log("EMAIL VERIFIED");
                CreateUser(response.localId, response.idToken, email, onSuccess, onFailure);

            }
            else
            {
                onFailure(Utils.UNVERIFIED_EMAIL_MESSAGE);
            }

        }).Catch(error => { onFailure(ExtractErrorMessage(error)); });
    }


    public void GetAllQuestions(string userIdToken, Action<Dictionary<string, Question>> onSuccess, Utils.OnFailureFunc onFailure)
    {
        Dictionary<string, Question> questions = new Dictionary<string, Question>();

        string getRequest = $"{databaseURL}questions.json?auth=" + userIdToken + "&orderBy=\"questionNumber\"&startAt=0";

        RestClient.Get(getRequest).Then(response =>
        {
            fsData questionsData = fsJsonParser.Parse(response.Text);
            serializer.TryDeserialize(questionsData, ref questions).AssertSuccessWithoutWarnings();

            onSuccess(questions);

        }).Catch(error => {
            onFailure(error.Message);
            Debug.Log(error.Message); 
        });
    }


    private void CreateUser(string localId, string idToken, string email, Utils.OnSuccessSignInFunc onSuccess, Utils.OnFailureFunc onFailure)
    {

        RestClient.Get<User>($"{databaseURL}users/{localId}.json?auth=" + idToken).Then(response =>
        {

            Dictionary<int, int> dic = new Dictionary<int, int>();
            //User user = new User(response.username, email, response.correctAnswers, response.numOfHints);
            User user = new User(response.username, email, response.state.correctAnswers, response.state.numOfHints, response.state.openLevel);
            user.localId = localId;
            user.idToken = idToken;
            onSuccess(user);

            //try
            //{
                

            //}
            //catch (Exception e)
            //{
            //    string a = e.Message;
            //}

        }).Catch(error=> { 
            onFailure(error.Message); 
        });
    }

    // Reset password.
    public void ResetPassword(string email, Utils.OnSuccessFunc onSuccess, Utils.OnFailureFunc onFailure)
    {

        string payload = "{\"email\":\"" + email + "\",\"requestType\":\"PASSWORD_RESET\"}";
        
        // Send to the user a mail to reset his password.
        RestClient.Post("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + authKey, bodyString: payload).
            Then(response => { onSuccess(); }).
            Catch(error => { onFailure(ExtractErrorMessage(error));});

    }


    // Set Questions by a given category from the database.
    public void GetQuestionsByCategory(string currentUserIdToken, string category, Action<Question[]> onSuccess, Utils.OnFailureFunc onFailure)
    {

        string getRequest = $"{databaseURL}questions.json?auth=" + currentUserIdToken;

        // כל הנושאים
        if (category.Equals(Utils.MIXED_HEBREW))
        {
            getRequest = getRequest + "&orderBy=\"questionNumber\"&startAt=0";
        }
        else
        {
            getRequest = getRequest + "&orderBy=\"questionCategory\"&equalTo=\"" + category + "\"";
        }

        RestClient.Get(getRequest).Then( response => {
            
            Dictionary<string, Question> questions = new Dictionary<string, Question>();
            fsData questionsData = fsJsonParser.Parse(response.Text);
            serializer.TryDeserialize(questionsData, ref questions).AssertSuccessWithoutWarnings();
            
            System.Random rnd = new System.Random();

            onSuccess(questions.Values.OrderBy(x => rnd.Next()).ToArray());

        }).Catch(error => 
        {
            onFailure(error.Message);
            Debug.Log(error);
        });


    }

    // Upload the given questions to the database reqursivly;
    private void UploadDatasetHelper(List<Question> questions, int num)
    {
        RestClient.Put<Question>($"{databaseURL}questions/{questions[num].questionNumber}.json", questions[num]).Then(response =>
        {
            if (num < questions.Count)
            {
                UploadDatasetHelper(questions, ++num);
            }
            else
            {
                return;
            }
        }).Catch(error => { Debug.Log(error.Message); });

    }

    // Upload the given question to the database.
    public void UploadDataset(List<Question> questions)
    {
        UploadDatasetHelper(questions, 0);

    }

    public void SaveUser(User currentUser, Utils.OnSuccessFunc onSuccess, Utils.OnFailureFunc onFailure)
    {
        PostUser(currentUser, currentUser.localId, onSuccess, onFailure);
        //JsonUtility.ToJson(currentUser.score.correctAns);
        //RestClient.Put<User>($"{databaseURL}users/{currentUser.localId}/email.json?auth=" + currentUser.idToken, userData).
        //    Then(response => { 
        //        onSuccess(); }).
        //    Catch(error => {
        //        onFailure(ExtractErrorMessage(error));

        //    });

        //foreach (int correctAns in currentUser.correctAnswers)
        //{
        //    Debug.Log(correctAns);
        //}

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