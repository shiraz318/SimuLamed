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
    
    // APIs
    private const string SIGN_UP_API= "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + AUTH_KEY;
    private const string SEND_EMAIL_VER_API = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + AUTH_KEY;
    private const string SIGN_IN_API = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + AUTH_KEY;
    private const string RESET_PASSWORD_API = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + AUTH_KEY;
    private const string CHECK_EMAIL_VER_API = "https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=" + AUTH_KEY;
    
    // DB Fields.
    private const string EMAIL_FIELD = "\"email\"";
    private const string PASSWORD_FIELD = "\"password\"";
    private const string SIMULATION_LEVEL_FIELD = "\"simulationLevel\"";
    private const string QUESTION_CATEGORY_FIELD = "\"questionCategory\"";
    private const string QUESTION_NUMBER_FIELD = "\"questionNumber\"";
    
    // DB Collections names.
    private const string USERS_COLLECTION_NAME = "users";
    private const string QUESTIONS_COLLECTION_NAME = "questions";

    private const int TIMEOUT = 7;
    
    private string localId;
    private string idToken;



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

    // Reset the currnt user data.
    public void ResetUser()
    {
        localId = "";
        idToken = "";
    }

    // Put a given user to the database.
    public void PutUser(User user, Action onSuccess, Action<string> onFailure)
    {
        RestClient.Put<User>(GetRequestHelper(GetUrlAccessUserByLocalId(), user)).
            Then(response => { onSuccess(); }).
            Catch(error => { 
                onFailure(ExtractErrorMessage(error)); });
    }

    // Get a user from the database according to the given localId and idToken.
    private void GetUser(Action<User> onSuccess, Action<string> onFailure)
    {
        RestClient.Get<User>(GetRequestHelper(GetUrlAccessUserByLocalId())).
            Then(response => { onSuccess(response); }).
            Catch(error => { 
                onFailure(error.Message); });
    }

    // A general method for Post a given body to a given url while the response is of SignResponse type.
    public void SignResponsePost(string url, string body, Action<SignResponse> onSuccess, Action<string> onFailure)
    {
        RestClient.Post<SignResponse>(GetRequestHelper(url, body)).
            Then(response => onSuccess(response)).
            Catch(error => { 
                onFailure(ExtractErrorMessage(error));});
    }
     
    /*
     * A general method for getting an object from the database using the given url 
     * and act accordingly to the givne onResponse and onFailure.
     */
    private void GetRequest(string url, Action<ResponseHelper> onResponse, Action<string> onFailure)
    {
        RestClient.Get(GetRequestHelper(url)).
            Then(response => { onResponse(response); }).
            Catch(error => { onFailure(error.Message);
        });

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
            return ErrorObject.TIMEOUT_ERROR_MESSAGE;
        }
    }

    public RequestHelper GetRequestHelper(string url, object body)
    {
        RequestHelper request = GetRequestHelper(url);
        request.Body = body;
        return request;
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
        RequestHelper request = GetRequestHelper(url);
        request.BodyString = body;
        return request;
    }
   
    // Get the url string to access a user by his local id.
    private string GetUrlAccessUserByLocalId()
    {
        return $"{databaseURL}{USERS_COLLECTION_NAME}/{localId}.json?auth={idToken}";
    }
    
    // Get the url string to access quesitons.
    private string GetUrlAccessQuestionsCollection()
    {
        return $"{databaseURL}{QUESTIONS_COLLECTION_NAME}.json?auth={idToken}";
    }

    // Get the string for user data.
    private string GetUserDataStr(string email, string password)
    {
        return "{" + $"{EMAIL_FIELD}:\"{email}\",{PASSWORD_FIELD}:\"{password}\",\"returnSecureToken\":true" + "}";
    }

    // Sign up.
    public void SignUp(string username, string email, string password, Action onSuccess, Action<string> onFailure)
    {
        // Sign up.
        SignResponsePost(SIGN_UP_API, GetUserDataStr(email, password),
        (response) =>
            {
                idToken = response.idToken;
                localId = response.localId;

                // Put the new user into the DB and if puting the user is done successfully, send an email for verification.
                PutUser(new User(username), () => SendEmailForVerification(onSuccess, onFailure), onFailure);
            }
        , onFailure);   
    }


    // Send an email for verification.
    private void SendEmailForVerification(Action onSuccess, Action<string> onFailure)
    {
        string emailData = "{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"" + idToken + "\"}";
        SignResponsePost(SEND_EMAIL_VER_API, emailData, response => onSuccess(), onFailure);
    }

    // Sign in a given user.
    public void SignInUser(string password, string email, Action<User> onSuccess, Action<string> onFailure)
    {       
        // If Signing in is done successfully, check if the user verified his email.
        SignResponsePost(SIGN_IN_API, GetUserDataStr(email, password), response =>
        {
            CheckEmailVerification(response, onSuccess, onFailure);
        }, onFailure);
    }

    // Check if the given email has been verified.
    private void CheckEmailVerification(SignResponse response, Action<User> onSuccess, Action<string> onFailure)
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
                    localId = response.localId;
                    idToken = response.idToken;
                    GetUser(onSuccess, onFailure);
                }
                else
                {
                    onFailure(ErrorObject.UNVERIFIED_EMAIL_MESSAGE);
                }

        }).Catch(error => { onFailure(ExtractErrorMessage(error)); });

    }

    // Reset password.
    public void ResetPassword(string email, Action onSuccess, Action<string> onFailure)
    {
        string payload = "{"+ $"{EMAIL_FIELD}:\"{email}\",\"requestType\":\"PASSWORD_RESET\"" + "}";
        // Send to the user an email to reset his password.
        RestClient.Post(GetRequestHelper(RESET_PASSWORD_API, payload)).
            Then(response => { onSuccess(); }).
            Catch(error => { onFailure(ExtractErrorMessage(error)); });

    }


    // Get number of questions in the database.
    public void GetNumberOfQuestions(Action<int> onSuccess, Action<string> onFailure)
    {
        string getRequest = GetUrlAccessQuestionsCollection()+"&shallow=true";
        GetRequest(getRequest, response =>
        {
            Dictionary<string, bool> questions = new Dictionary<string, bool>();
            fsData questionsData = fsJsonParser.Parse(response.Text);
            serializer.TryDeserialize(questionsData, ref questions).AssertSuccessWithoutWarnings();
            onSuccess(questions.Count());
        }, onFailure);
    }
    
    // Get all the questions in the databae.
    public void GetAllQuestions(Action<Question[]> onSuccess, Action<string> onFailure)
    {
        string getRequest = GetUrlAccessQuestionsCollection() + $"&orderBy={QUESTION_NUMBER_FIELD}&startAt=0";
        GetQuestions(getRequest, onSuccess, onFailure);
    }

    // Set Questions by a given category from the database.
    public void GetQuestionsByCategory(string category, Action<Question[]> onSuccess, Action<string> onFailure)
    {
        if (category.Equals(Question.MIXED_HEBREW))
        {
            GetAllQuestions(onSuccess, onFailure);
        }
        else
        {
            string getRequest = GetUrlAccessQuestionsCollection() + $"&orderBy={QUESTION_CATEGORY_FIELD}&equalTo=\"{category}\"";
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
        }, 
        onFailure);

    }

    // Get all the questions that appears in the given level.
    public void GetQuestionsInLevel(string level, Action<Question[]> onSuccess, Action<string> onFailure)
    {
        string getRequest = GetUrlAccessQuestionsCollection() + $"&orderBy={SIMULATION_LEVEL_FIELD}&equalTo=\"{level}\"";
        GetQuestions(getRequest, onSuccess, onFailure);
    }




    // Upload the given question to the database.
    public void UploadDataset(List<Question> questions)
    {
        UploadDatasetHelper(questions, 0);

    }


    // Upload the given questions to the database reqursivly;
    private void UploadDatasetHelper(List<Question> questions, int num)
    {
        RestClient.Put<Question>($"{databaseURL}{QUESTIONS_COLLECTION_NAME}/{questions[num].questionNumber}.json", questions[num]).Then(response =>
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

}

