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
using Assets.Model;

// Singleton FirebaseManager.
public sealed class FirebaseManager : IDatabaseHandler
{
    private const string PROJECT_ID = "simulamed-49311-default-rtdb";
    private const string AUTH_KEY = "AIzaSyBS5WVLpACpe5AbRrZ2KmWcw92FFR65Vs0";
    private static readonly string databaseURL = $"https://{PROJECT_ID}.firebaseio.com/";
    private const string SIGN_UP_API= "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + AUTH_KEY;
    private const string SEND_EMAIL_VER_API = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + AUTH_KEY;
    private const string SIGN_IN_API = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + AUTH_KEY;
    private const string RESET_PASSWORD_API = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + AUTH_KEY;
    private const string CHECK_EMAIL_VER_API = "https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=" + AUTH_KEY;
    private const string EMAIL_FIELD = "email";
    private const string PASSWORD_FIELD = "password";
    private const string USERS_DB_NAME = "users";
    private const string QUESTIONS_DB_NAME = "questions";
    private const int TIMEOUT = 5;
    


    private static fsSerializer serializer = new fsSerializer();

    // Thread safety singleton using double check locking 
    private static readonly object padlock = new object();
    private static FirebaseManager instance = null;
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
    

    // Put a given user to the database.
    public void PutUser(User user, Action onSuccess, Action<string> onFailure)
    {

        RestClient.Put<User>(GetRequestHelper($"{databaseURL}users/{user.details.localId}.json?auth=" + user.details.idToken, user)).
            Then(response => { onSuccess(); }).
            Catch(error => { onFailure(ExtractErrorMessage(error)); });
    }


    // A general method for Post a given body to a given url while the response is of SignResponse type.
    public void SignResponsePost(string url, string body, Action<SignResponse> onSuccess, Action<string> onFailure)
    {
        RestClient.Post<SignResponse>(GetRequestHelper(url, body)).Then(response => onSuccess(response)).
            Catch(error =>
            {
                onFailure(ExtractErrorMessage(error));
            });
        //RestClient.Post<SignResponse>(url, body).Then(response => onSuccess(response)).
        //    Catch(error => { onFailure(ExtractErrorMessage(error)); });
    }

    public RequestHelper GetRequestHelper(string url, object body)
    {
        return new RequestHelper
        {
            Uri = url,
            Params = new Dictionary<string, string> {
            { "key", AUTH_KEY }
        },
            Body = body,
            Timeout = TIMEOUT
        };
    }
    public RequestHelper GetRequestHelper(string url)
    {
        return new RequestHelper
        {
            Uri = url,
            Params = new Dictionary<string, string> {
            { "key", AUTH_KEY }
        },
            Timeout = TIMEOUT
        };
    }



    public RequestHelper GetRequestHelper(string url, string body)
    {
        return new RequestHelper
        {
            Uri = url,
            Params = new Dictionary<string, string> {
                { "key", AUTH_KEY }
            },
            BodyString = body,
            Timeout = TIMEOUT
        };

    }


    // Sign up.
    public void SignUp(string email, string password, Action<string,string> onSuccess, Action<string> onFailure)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        SignResponsePost(SIGN_UP_API, userData, (response) => onSuccess(response.idToken, response.localId), onFailure);   
    }

    // Save a new user to the database.
    public void SaveNewUser(User newUser, Action onSuccess, Action<string> onFailure)
    {
        // If Posting the user is done successfully, send an email for verification.
        PutUser(newUser, () => SendEmailForVerification(newUser.details.idToken, onSuccess, onFailure), onFailure);
    }

    // Send an email for verification.
    private void SendEmailForVerification(string idToken, Action onSuccess, Action<string> onFailure)
    {
        string emailData = "{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"" + idToken + "\"}";
        SignResponsePost(SEND_EMAIL_VER_API, emailData, response => onSuccess(), onFailure);
    }

    // Create a new Error Object and set the Error property.
    private string ExtractErrorMessage(Exception error)
    {
        RequestException exception = (RequestException)error;
        string errorMessage = GetResponseError(exception);
        Debug.Log(errorMessage);
        return errorMessage;
    }

    // Extract the response message from a given RequestExcetion.
    private string GetResponseError(RequestException exception)
    {

        try
        {
            return exception.Response.Split(',')[1].Split(':')[1].Split('\"')[1].Replace('_', ' ');
        }
        catch
        {
            return "שגיאה! בדוק את חיבור הרשת שלך";
        }
    }

    // Sign in a given user.
    public void SignInUser(string password, string email, Action<User> onSuccess, Action<string> onFailure)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        
        // If Signing in is done successfully, check if the user verified his email.
        SignResponsePost(SIGN_IN_API, userData, response =>
        {
            CheckEmailVerification(response, email, onSuccess, onFailure);
        }, onFailure);
    }

    // Check if the given email has been verified.
    private void CheckEmailVerification(SignResponse response, string email, Action<User> onSuccess, Action<string> onFailure)
    {
        string emailVerification = "{\"idToken\":\"" + response.idToken + "\"}";

        // Check if the user verified his email.
        RestClient.Post(GetRequestHelper(CHECK_EMAIL_VER_API, emailVerification))
        .Then(emailResponse =>
        {
            fsData emailVerificationData = fsJsonParser.Parse(emailResponse.Text);
            EmailConfirmationInfo emailConfirmationInfo = new EmailConfirmationInfo();
            serializer.TryDeserialize(emailVerificationData, ref emailConfirmationInfo).AssertSuccessWithoutWarnings();

                // If the user verified his email he can sign in.
                if (emailConfirmationInfo.users[0].emailVerified)
            {
                GetUser(response.localId, response.idToken, onSuccess, onFailure);
            }
            else
            {
                onFailure(ErrorObject.UNVERIFIED_EMAIL_MESSAGE);
            }

        }).Catch(error => { onFailure(ExtractErrorMessage(error)); });

    }

    // Get a user from the database according to the given localId and idToken.
    private void GetUser(string localId, string idToken, Action<User> onSuccess, Action<string> onFailure)
    {
        RestClient.Get<User>(GetRequestHelper($"{databaseURL}users/{localId}.json?auth=" + idToken)).
            Then(response =>
            {
                response.details.SetIdToken(idToken); response.details.SetLocalId(localId);
                onSuccess(response);
            }).Catch(error =>
            { onFailure(error.Message); });
    }

    // Reset password.
    public void ResetPassword(string email, Action onSuccess, Action<string> onFailure)
    {
        string payload = "{\"email\":\"" + email + "\",\"requestType\":\"PASSWORD_RESET\"}";
        // Send to the user an email to reset his password.
        RestClient.Post(GetRequestHelper(RESET_PASSWORD_API, payload)).
            Then(response => { onSuccess(); }).Catch(error => { onFailure(ExtractErrorMessage(error)); });

    }

    /*
     * A general method for getting an object from the database using the given url 
     * and act accordingly to the givne onResponse and onFailure.
     */
    private void GetRequest(string url, Action<ResponseHelper> onResponse, Action<string> onFailure)
    {
        RestClient.Get(GetRequestHelper(url)).Then(response => { onResponse(response); }).
        Catch(error => { onFailure(error.Message); });

    }

    // Get number of questions in the database.
    public void GetNumberOfQuestions(string idToken, Action<int> onSuccess, Action<string> onFailure)
    {
        string getRequest = $"{databaseURL}questions.json?auth=" + idToken + "&shallow=true";
        GetRequest(getRequest, response =>
        {
            Dictionary<string, bool> questions = new Dictionary<string, bool>();
            fsData questionsData = fsJsonParser.Parse(response.Text);
            serializer.TryDeserialize(questionsData, ref questions).AssertSuccessWithoutWarnings();
            onSuccess(questions.Count());
        }, onFailure);
    }
    
    // Get all the questions in the databae.
    public void GetAllQuestions(string idToken, Action<Question[]> onSuccess, Action<string> onFailure)
    {
        string getRequest = $"{databaseURL}questions.json?auth=" + idToken + "&orderBy=\"questionNumber\"&startAt=0";
        GetQuestions(getRequest, onSuccess, onFailure);
    }

    // Set Questions by a given category from the database.
    public void GetQuestionsByCategory(string idToken, string category, Action<Question[]> onSuccess, Action<string> onFailure)
    {
        if (category.Equals(Utils.MIXED_HEBREW))
        {
            GetAllQuestions(idToken, onSuccess, onFailure);
        }
        else
        {
            string getRequest = $"{databaseURL}questions.json?auth=" + idToken + 
                "&orderBy=\"questionCategory\"&equalTo=\"" + category + "\"";
            GetQuestions(getRequest, onSuccess, onFailure);
        }
    }

    // Get questions using the given url and acting accordingly to the onSuccess and onFailure functions.
    private void GetQuestions(string url, Action<Question[]> onSuccess, Action<string> onFailure)
    {
        GetRequest(url, response =>
        {
            Dictionary<string, Question> questions = new Dictionary<string, Question>();
            fsData questionsData = fsJsonParser.Parse(response.Text);
            serializer.TryDeserialize(questionsData, ref questions).AssertSuccessWithoutWarnings();
            onSuccess(questions.Values.ToArray());
        }, onFailure);

    }

    // Get all the questions that appears in the given level.
    public void GetQuestionsInLevel(string idToekn, string level, Action<Question[]> onSuccess, Action<string> onFailure)
    {
        string getRequest = $"{databaseURL}questions.json?auth=" + idToekn + "&orderBy=\"simulationLevel\"&equalTo=\"" + level + "\"";
        GetQuestions(getRequest, onSuccess, onFailure);
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

}

