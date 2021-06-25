using Assets.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.model
{
    public sealed class AppModel : IAppModel
    {
        // Private fields.
        private bool isSignedUp;
        private bool isSignedIn;
        private bool isResetPassword;
        private bool isUserSaved;
        private bool failToSave;
        private Question[] questions;
        private IDatabaseHandler databaseHandler;
        private User currentUser;
        private Dictionary<int, QuestionType> fromQuestionNumToType;

        // Properties.
        public Question[] Questions 
        { 
            get { return questions; } 
            set { questions = value; NotifyPropertyChanged("Questions"); } 
        }
        public ErrorObject Error { get ; set ; }
        public int NumOfQuestions { get; set; }
        public int HintsNumber 
        { 
            get { return currentUser != null? currentUser.state.numOfHints : 0; } 
            set{ currentUser.state.numOfHints = value; NotifyPropertyChanged("HintsNumber"); } 
        }
        public int OpenLevel 
        { 
            get { return currentUser.state.openLevel; } 
            set { currentUser.state.openLevel = value; } 
        }
        public QuestionType SelectedSubject { get; set; }
        public string CurrentUsername 
        { 
            get { return currentUser != null ? currentUser.details.username : ""; } 
        }
        public bool IsSignedUp 
        { 
            get { return isSignedUp; } 
            set { isSignedUp = value; NotifyPropertyChanged("IsSignedUp"); } 
        }
        public bool IsSignedIn 
        { 
            get { return isSignedIn; } 
            set { isSignedIn = value; NotifyPropertyChanged("IsSignedIn"); } 
        }
        public bool IsResetPassword 
        { 
            get { return isResetPassword; } 
            set { isResetPassword = value; NotifyPropertyChanged("IsResetPassword"); } 
        }
        public bool IsUserSaved 
        {
            get { return isUserSaved; } 
            set { isUserSaved = value; NotifyPropertyChanged("IsUserSaved"); } 
        }
        public bool IsSaveingFailed
        {
            get { return failToSave; }
            set { failToSave = value; NotifyPropertyChanged("IsSaveingFailed"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        // Thread safety singleton using double check locking 
        private static readonly object padlock = new object();
        private static AppModel instance = null;
        public static AppModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new AppModel();
                        }
                    }
                }
                return instance;
            }
        }


        // Private Constructor.
        private AppModel()
        {
            // Initialization.
            databaseHandler = FirebaseManager.Instance;
            Error = new ErrorObject("", ErrorTypes.None);
            currentUser = null;
            Questions = new Question[0];
            fromQuestionNumToType = new Dictionary<int, QuestionType>();
        }

        // Reset password.
        public void ResetPassword(string email)
        {
            // If email is valid - reset password.
            ActionIfEmailIsValid(email,
                () => databaseHandler.ResetPassword(email,
                    () => { ResetError(); IsResetPassword = true; },
                    (message) => SetError(message, ErrorTypes.ResetPassword)
                    ),
                ErrorTypes.ResetPassword);
        }
        
        /*
         * Set the dictionary fromQuestionNumToType and set the error object 
         * accordingly to the given errorType if something is wrong.
         */
        public void SetFromNumToType(ErrorTypes errorType)
        {
            databaseHandler.GetAllQuestions(currentUser.details.idToken, onSuccess: (questions) =>
                {
                    // This is the first time the dictionary is being set.
                    if (fromQuestionNumToType.Count == 0)
                    {
                        questions = questions.OrderBy(x => x.questionNumber).ToArray();
                        fromQuestionNumToType = questions.ToDictionary(x => x.questionNumber,
                            x => Question.FromCategoryToTypeHebrew(x.questionCategory));
                    }
                    ResetError();
                    NotifyPropertyChanged("FromQuestionNumToType");
                },
                onFailure: (message) => { SetError(message, errorType); });
        }

        // Sign in.
        public void SignIn(string password, string email)
        {
            // Define onSuccess function for when signing in is done successfully.
            Action<User> newOnSuccess = delegate (User user)
            {
                IsSignedIn = true;
                currentUser = user;
                NotifyPropertyChanged("HintsNumber");

                databaseHandler.GetNumberOfQuestions(user.details.idToken, (numOfQuestions) =>
                {
                    NumOfQuestions = numOfQuestions;
                    ResetError();
                    currentUser.state.InitScore(numOfQuestions);
                },
                (error) => { SetError(error, ErrorTypes.SignIn); });
            };
            
            // If email is valid - sign in.
            ActionIfEmailIsValid(email, 
                () => databaseHandler.SignInUser(password, email, newOnSuccess,
                    (message) => SetError(message, ErrorTypes.SignIn)), 
                ErrorTypes.SignIn);
        }

        // Create a new user.
        //private User CreateUser(string username, string email, string idToken, string localId)
        private User CreateUser(string username, string idToken, string localId)
        {
            UserDetails details = new UserDetails(username, localId, idToken);
           // UserDetails details = new UserDetails(username, email, localId, idToken);
            UserState state = new UserState(new int[] { -1 }, Utils.INITIAL_NUMBER_OF_HINTS, 1);
            return new User(details, state);
        }

        // Sign up.
        public void SignUp(string username, string password, string email)
        {
            Action<string> onFailure = (message) => SetError(message, ErrorTypes.SignUp);
            
            // If signing up is done successfully, save the new user to the database.
            Action<string,string> onSuccessSignUp = (string idToken, string localId) =>
            {
                // If saving the new user is done successfully, reset the error object and set IsSignedUp to true.
                databaseHandler.SaveNewUser(CreateUser(username, idToken, localId),
                //databaseHandler.SaveNewUser(CreateUser(username, email, idToken, localId),
                    ()=> { ResetError(); IsSignedUp = true; },
                    onFailure);
            };
            
            // If email is valid - sign up.
            ActionIfEmailIsValid(email,
                () => databaseHandler.SignUp(email, password, onSuccessSignUp, onFailure),
                ErrorTypes.SignUp);
        }

        // Activate onSuccess action if the given email is valid.
        private void ActionIfEmailIsValid(string email, Action onSuccess, ErrorTypes errorType)
        {
            if (IsVaildEmailAddress(email)) 
            {
                onSuccess(); 
            }
            else 
            { 
                SetError(ErrorObject.INAVLID_EMAIL_MESSAGE, errorType);
            }
        }

        // Check if a given string is a valid email address.
        private bool IsVaildEmailAddress(string email)
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

        // Set questions array to contain questions in the given level.
        public void SetQuestionsByLevel(string level)
        {
            databaseHandler.GetQuestionsInLevel(currentUser.details.idToken, level, 
                (questions) => { SetQuestions(questions, false); ResetError(); },
                (message) => { SetError(message, ErrorTypes.LoadQuestions); });
        }

        // Set questions array to contain questinos in the given category.
        public void SetQuestionsByCategory(string category, bool toRnd)
        {
            // Get all questions of the given category from the database.
            databaseHandler.GetQuestionsByCategory(currentUser.details.idToken, category, 
                // If getting the questions is done successfully, set the questions property and reset the error object.
                (questions) => { SetQuestions(questions, toRnd); ResetError(); },
                (message) => { SetError(message, ErrorTypes.LoadQuestions); });
        }

        // Set the questions array to the given questions array and randomize it accornding to toRnd.
        private void SetQuestions(Question[] inputQuestions, bool toRnd)
        {
            // Randomize the questions.
            if (toRnd)
            {
                System.Random rnd = new System.Random();
                inputQuestions = inputQuestions.OrderBy(x => rnd.Next()).ToArray();
            }
            else
            {
                inputQuestions = inputQuestions.OrderBy(x => x.questionNumber).ToArray();
            }
            Questions = inputQuestions;
        }

        // Save current user to the database.
        public void SaveUser()
        {
            // Set the correct answers of the current user.
            currentUser.state.SetCorrectAns();

            // If posting the user is done successfully, reset the error object and set IsUserSaved to true.
            databaseHandler.PutUser(currentUser,
                () => { ResetError(); IsUserSaved = true; },
                (message) => { IsSaveingFailed = true;  SetError(message, ErrorTypes.SaveScore); });
        }



        // Reset the error propery.
        private void ResetError()
        {
            SetError("", ErrorTypes.None);
        }

        // Set the current user score according to the given question number and wether the user was correct or not.
        public void SetUserScore(int questionNum, bool isAnsCorrect)
        {
            // Set the current user score.
            currentUser.state.SetScore(questionNum, isAnsCorrect);
            
            // The user deserve a new hint.
            if (currentUser.state.IsDeserveNewHint())
            {
                HintsNumber = HintsNumber + 1;
            }
        }

        // Initialize the user count of correct answers in a row.
        public void InitUserLastAns()
        {
            currentUser.state.InitCorrectAnsInARowCounter();
        }

        // Set the Error propery according to the given message and error type.
        private void SetError(string message, ErrorTypes errorType)
        {
            Error.ErrorType = errorType;
            Error.Message = message;
            NotifyPropertyChanged("Error");
        }

        // Get the number of questions in the database of the given category.
        public int GetNumOfQuestionsByCategory(string category)
        {
            int numOfQuestion = fromQuestionNumToType.Where(pair => 
            pair.Value == Question.FromCategoryToTypeHebrew(category)).Select(pair => pair.Key).Count();
            return numOfQuestion;
        }

        // Get the number of correct answers the current user answered of the given category.
        public int GetNumOfCorrectAnswersByCategory(string category)
        {
            int numOfCorrectAnswers = fromQuestionNumToType.Where(pair =>
            pair.Value == Question.FromCategoryToTypeHebrew(category) &&
            currentUser.state.correctAnswers.Contains(pair.Key)).Select(pair => pair.Key).Count();
            return numOfCorrectAnswers;
        }

        // Decrease a hint to the current user.
        public void DecreaseHint()
        {
            HintsNumber = HintsNumber - 1;
        }
        
        // Updata the user open level to max(given level, user prev open level)
        public void UpdateUserLevel(int level)
        {
            if (currentUser.state.openLevel < level)
            {
                currentUser.state.openLevel = level;
            }
        }

        // Update current user score according to the given player score.
        public void UpdateUserScore(Utils.QuestionOption[] playerScore)
        {
            currentUser.state.UpdateScore(playerScore);
        }

        // Reset the current user.
        public void ResetCurrentUser()
        {
            currentUser = null;
        }

        // Notify property changed.
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


    }

}
