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
        // Properties.
        public Question[] Questions { get ; set ; }
        public ErrorObject Error { get ; set ; }
        public int NumOfQuestions { get { return fromQuestionNumToType.Count; } }
        public int HintsNumber { get { return currentUser != null ? currentUser.state.numOfHints : 0; } }
        public QuestionType SelectedSubject { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        // Private members.
        private IDatabaseHandler databaseHandler;
        private User currentUser;
        private Dictionary<int, QuestionType> fromQuestionNumToType;

        // Singleton related fields.
        private static readonly object padlock = new object();
        private static AppModel instance = null;
        

        // Thread safety singleton using double check locking 
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
            databaseHandler = FirebaseManager.Instance;
            Error = new ErrorObject("", ErrorTypes.None);
            ResetCurrentUser();
            Questions = new Question[0];
            fromQuestionNumToType = new Dictionary<int, QuestionType>();
        }

        // Get the username of the current user.
        public string GetCurrentUsername()
        {
            return currentUser != null ? currentUser.details.username : "";
        }

        // Reset the current user.
        public void ResetCurrentUser()
        {
            currentUser = null;
        }

        // Reset password.
        public void ResetPassword(string email, Utils.OnSuccessFunc onSuccess)
        {
            onSuccess += delegate { ResetError(); };

            if (IsVaildEmailAddress(email))
            {
                databaseHandler.ResetPassword(email, onSuccess, (message) => SetError(message, ErrorTypes.ResetPassword));
            }
            else
            {
                SetError(Utils.INAVLID_EMAIL_MESSAGE_H, ErrorTypes.ResetPassword);
            }
        }
        
        // Sign in.
        public void SignIn(string password, string email, Utils.OnSuccessFunc onSuccess)
        {
            Utils.OnSuccessSignInFunc newOnSuccess = delegate (User user) 
            { 
                onSuccess(); 
                currentUser = user;
                NotifyPropertyChanged("HintsNumber");

                // Get all the questions from the database.
                databaseHandler.GetQuestionsByCategory(user.details.idToken, Utils.MIXED_HEBREW, onSuccess:(questions)=> 
                {
                    if (fromQuestionNumToType.Count == 0)
                    {
                        fromQuestionNumToType = questions.ToDictionary(x => x.questionNumber,
                            x => Question.FromCategoryToTypeHebrew(x.questionCategory));
                    }
                    
                    ResetError();
                    currentUser.InitUserScore(NumOfQuestions);
                },
                onFailure:(message) => { SetError(message, ErrorTypes.SignIn); }); 
            };

            if (IsVaildEmailAddress(email))
            {
                databaseHandler.SignInUser(password, email, newOnSuccess, (message) => SetError(message, ErrorTypes.SignIn));
            }
            else
            {
                SetError(Utils.INAVLID_EMAIL_MESSAGE_H, ErrorTypes.SignIn);
            }

        }

        // Sign up.
        public void SignUp(string username, string password, string email, Utils.OnSuccessFunc onSuccess)
        {
            onSuccess += delegate { ResetError(); };

            if (IsVaildEmailAddress(email))
            {
                databaseHandler.SignUpUser(username, password, email, onSuccess, (message) => SetError(message, ErrorTypes.SignUp));
            }
            else
            {
                SetError(Utils.INAVLID_EMAIL_MESSAGE_H, ErrorTypes.SignUp);
            }
        }

        // Check if a given string is a valid email address.
        private bool IsVaildEmailAddress(string email)
        {
            //TODO - FIX THIS FUNCTION - IT RETURNS TRUE FOR THE ADDRESS shiraz422@gmail.co
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

        // Set questions property by the given category.
        public void SetQuestionsByCategory(string category, bool toRnd)
        {
            // Get all questions of the given category from the database.
            databaseHandler.GetQuestionsByCategory(currentUser.details.idToken, category, 
                onSuccess:(questions) => 
                {
                    SetQuestions(questions, toRnd);
                    ResetError();
                },
                onFailure:(message) => { 
                    SetError(message, ErrorTypes.SignIn); 
                });
        }

        // Set the questions property to the given questions array and notify it.
        private void SetQuestions(Question[] questions, bool toRnd)
        {
            if (toRnd)
            {
                System.Random rnd = new System.Random();
                questions = questions.OrderBy(x => rnd.Next()).ToArray();

            }
            else
            {
                questions = questions.OrderBy(x => x.questionNumber).ToArray();

            }
            Questions = questions;
            NotifyPropertyChanged("Questions");
        }

        // Save current user to the database.
        public void SaveUser(Utils.OnSuccessFunc onSuccess)
        {
            onSuccess += delegate { ResetError(); };

            currentUser.SetCorrectAns();
            if (currentUser.state.correctAnswers.Length == 0) { currentUser.state.correctAnswers = new int[] { -1 }; }
            databaseHandler.SaveUser(currentUser, onSuccess , (message) => SetError(message, ErrorTypes.SaveScore)) ;
        }

        // Reset the error propery.
        private void ResetError()
        {
            SetError("", ErrorTypes.None);
        }

        // Set the current user score according to the given question number and wether the user was correct or not.
        public void SetUserScore(int questionNum, bool isAnsCorrect)
        {
            currentUser.SetScore(questionNum, isAnsCorrect);
            
            // The user deserve a new hint.
            if (currentUser.IsDeserveNewHint())
            {
                currentUser.AddHint();
                NotifyPropertyChanged("HintsNumber");
            }
            //CurrentUser.score.SetQuestionScore(questionNum, isAnsCorrect);
        }

        public void InitUserLastAns()
        {
            currentUser.InitLastAns();
        }

        // Set the Error propery according to the given message and error type.
        private void SetError(string message, ErrorTypes errorType)
        {
            if (message.Equals("EMAIL EXISTS"))
            {
                message = "כתובת אימייל נמצאת בשימוש";
            }
            else if (message.Equals("EMAIL NOT FOUND")) 
            {
                message = "כתובת אימייל לא קיימת";
            }
            else if (message.Equals("INVALID PASSWORD"))
            {
                message = "סיסמא שגויה";
            }
            else if (message.Equals("WEAK PASSWORD "))
            {
                message = "סיסמא חלשה. אנא הזן לפחות 6 תווים";
            }
            Error.Message = message;
            Error.ErrorType = errorType;
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
            //currentUser.correctAnswers.Contains(pair.Key)).Select(pair => pair.Key).Count();
            return numOfCorrectAnswers;
        }

        // Decrease a hint to the current user.
        public void DecreaseHint()
        {
            currentUser.DecreaseHint();
            NotifyPropertyChanged("HintsNumber");
        }
        public int GetOpenLevel()
        {
            return currentUser.GetOpenLevel();
        }
        
        public void UpdateUserScore(Utils.QuestionOption[] playerScore)
        {
            currentUser.state.UpdateScore(playerScore);
        }
        
        public void UpdateUserOpenLevel(int openLevel)
        {
            currentUser.state.openLevel = openLevel;
        }


        //public int GetNumOfQuestions()
        //{
        //    return NumOfQuestions;
        //}


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
