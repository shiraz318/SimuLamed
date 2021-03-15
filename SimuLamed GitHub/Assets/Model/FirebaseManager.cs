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

// Singleton FirebaseManager.
public sealed class FirebaseManager : IDatabaseHandler
{

    // Constructor.
    private FirebaseManager() 
    {
        Error = new ErrorObject("", ErrorTypes.None);
        ResetCurrentUser();
        Questions = new List<Question>();
    }
    

    private const string projectId = "simulamed-49311-default-rtdb";
    private const string authKey = "AIzaSyBS5WVLpACpe5AbRrZ2KmWcw92FFR65Vs0";
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    private static fsSerializer serializer = new fsSerializer();


    public delegate void GetUserCallback(User user);
    public delegate void PostUserCallback();

    public event PropertyChangedEventHandler PropertyChanged;
    
    public ErrorObject Error { get; set; }
    public List<Question> Questions { get; set; }
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
    
    // Get the current user's username.
    public string GetUsername()
    {
        if (currentUser != null)
        {
            return currentUser.username;
        }
        return "";
    }

    // Reset the current user.
    public void ResetCurrentUser()
    {
        currentUser = null;
    }

    // Post a given user to the database.
    private void PostUser(User user, string userId)
    {
        RestClient.Put<User>($"{databaseURL}users/{userId}.json?auth=" + user.idToken , user).Catch(error => {
            SetError(ExtractErrorMessage(error), ErrorTypes.SignUp);

        });
    }

    // Sign up a given user.
    public void SignUpUser(string username, string password, string email, Action onSuccess)
    {
        if (!isVaildEmailAddress(email))
        {
            SetError("INVALID EMAIL ADDRESS", ErrorTypes.SignUp);
            return;
        }

        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        
        // Sign up as a user.
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + authKey,
            bodyString: userData).Then(response =>
        {
            User user = new User(username, email);
            user.localId = response.localId;
            user.idToken = response.idToken;

            // Post user data to the database.
            PostUser(user, response.localId);
            SetError("", ErrorTypes.None);

            // Send an email to the user for verification.
            SendEmailForVerification(user.idToken);
            onSuccess();

        }).Catch(error => 
        {
            SetError(ExtractErrorMessage(error), ErrorTypes.SignUp);
        });
    }

    // Set the Error propery.
    private void SetError(string message, ErrorTypes errorType)
    {
        Error.Message = message;
        Error.ErrorType = errorType;
        NotifyPropertyChanged("Error");
    }

    // Send an email for verification.
    private void SendEmailForVerification(string idToken)
    {
        string emailData = "{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"" + idToken + "\"}";
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + authKey, emailData);
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
    public void SignInUser(string password, string email, Action onSuccess)
    {
        Debug.Log("IN SIGN IN FIREBASE MANAGER");
        if (!isVaildEmailAddress(email))
        {
            SetError("INVALID EMAIL ADDRESS", ErrorTypes.SignIn);
            return;
        }
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        
        

        // Sign in with password and email.
        RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + authKey,
            bodyString: userData).Then(response =>
            {
                Debug.Log("BEFORE CHECK EMAIL VERIFICATION");
                // Check for email verification.
                CheckEmailVerification(response, email, onSuccess);

            }).Catch(error => {

                SetError(ExtractErrorMessage(error), ErrorTypes.SignIn);
            });
    }

    // Check if the given email has been verified.
    private void CheckEmailVerification(SignResponse response, string email, Action onSuccess)
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
                SetCurrentUser(response.localId, response.idToken, email, onSuccess);
                SetError("", ErrorTypes.None);
                SetFromQuestionNumToType();


            }
            else
            {
                SetError("PLEASE VERIFY YOUR EMAIL", ErrorTypes.SignIn);
            }

        });
    }


    private void SetFromQuestionNumToType()
    {
        Dictionary<string, Question> questions = new Dictionary<string, Question>();

        string getRequest = $"{databaseURL}questions.json?auth=" + currentUser.idToken + "&orderBy=\"questionNumber\"&startAt=0";

        RestClient.Get(getRequest).Then(response =>
        {
            fsData questionsData = fsJsonParser.Parse(response.Text);
            serializer.TryDeserialize(questionsData, ref questions).AssertSuccessWithoutWarnings();
            Dictionary<int, QuestionType> dic = questions.Values.ToDictionary(x => x.questionNumber, x => Question.FromCategoryToTypeEnglish(x.questionCategory));

            Utils.SetFromQuestionNumToType(dic);

        }).Catch(error => {
            Debug.Log(error.Message); 
        });
    }

    // Check if a given string is a valid email address.
    private bool isVaildEmailAddress(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // Set the current user data.
    private void SetCurrentUser(string localId, string idToken, string email, Action onSuccess)
    {
        currentUser = new User("", "");
        currentUser.localId = localId;
        currentUser.idToken = idToken;
        currentUser.email = email;
        GetUsernameFromDatabase(onSuccess);
    }

    // Reset password.
    public void ResetPassword(string email, Action onSuccess)
    {
        if (!isVaildEmailAddress(email))
        {
            SetError("INVALID EMAIL ADDRESS", ErrorTypes.ResetPassword);
            return;
        }

        string payload = "{\"email\":\"" + email + "\",\"requestType\":\"PASSWORD_RESET\"}";
        
        // Send to the user a mail to reset his password.
        RestClient.Post("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + authKey,
            bodyString: payload).Then(response =>
            {
                SetError("", ErrorTypes.None);
                onSuccess();

            }).Catch(error =>
            {
                SetError(ExtractErrorMessage(error), ErrorTypes.ResetPassword);

            });

    }

    // Get the current user's username from the database.
    private void GetUsernameFromDatabase(Action onSuccess)
    {
        RestClient.Get<User>($"{databaseURL}users/{currentUser.localId}.json?auth=" + currentUser.idToken).Then(response =>
        {
            currentUser.username = response.username;
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



    private void SetQuestions(List<Question> questions)
    {
        Questions = questions;
        NotifyPropertyChanged("Questions");

    }

    // Set Questions by a given category from the database.
    public void SetQuestionsByCategory(string category)
    {
        Dictionary<string, Question> questions = new Dictionary<string, Question>();

        

        string getRequest = $"{databaseURL}questions.json?auth=" + currentUser.idToken;

        if (category.Equals("כל הנושאים"))
        {
            getRequest = getRequest + "&orderBy=\"questionNumber\"&startAt=0";
        }
        else
        {
            getRequest = getRequest + "&orderBy=\"questionCategory\"&equalTo=\"" + category + "\"";
         
        }

        RestClient.Get(getRequest).Then( response => {

            fsData questionsData = fsJsonParser.Parse(response.Text);
            serializer.TryDeserialize(questionsData, ref questions).AssertSuccessWithoutWarnings();
            System.Random rnd = new System.Random();

            SetQuestions(questions.Values.OrderBy(x => rnd.Next()).ToList());

        }).Catch(error => 
        { 
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