using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.model
{
    public class Model : IModel
    {
        // Properties.
        public Question[] Questions { get ; set ; }
        public ErrorObject Error { get ; set ; }
        public int NumOfQuestions { get { return fromQuestionNumToType.Count; } }

        public event PropertyChangedEventHandler PropertyChanged;
        


        private IDatabaseHandler databaseHandler;
        private User currentUser;
        private Dictionary<int, QuestionType> fromQuestionNumToType;

        // Singleton related fields.
        private static readonly object padlock = new object();
        private static Model instance = null;
        


        // Thread safety singleton using double check locking 
        public static Model Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Model();
                        }
                    }
                }
                return instance;
            }
        }

        // Private Constructor.
        private Model()
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
            return currentUser != null ? currentUser.username : "";
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
                SetError(Utils.INAVLID_EMAIL_MESSAGE, ErrorTypes.ResetPassword);
            }
        }
        
        // Sign in.
        public void SignIn(string password, string email, Utils.OnSuccessFunc onSuccess)
        {
            Utils.OnSuccessSignInFunc newOnSuccess = delegate (User user) 
            { 
                onSuccess(); 
                currentUser = user; 
                databaseHandler.GetAllQuestions(user.idToken, onSuccess:(questions)=> 
                {
                    if (fromQuestionNumToType.Count == 0)
                    {
                        fromQuestionNumToType = questions.Values.ToDictionary(x => x.questionNumber,
                            x => Question.FromCategoryToTypeHebrew(x.questionCategory));
                    }
                    
                    ResetError();

                    currentUser.InitUserScore(NumOfQuestions);

                },
                onFailure:(message) => 
                { 
                    SetError(message, ErrorTypes.SignIn); 
                }); 
            };

            if (IsVaildEmailAddress(email))
            {
                databaseHandler.SignInUser(password, email, newOnSuccess, (message) => SetError(message, ErrorTypes.SignIn));
            }
            else
            {
                SetError(Utils.INAVLID_EMAIL_MESSAGE, ErrorTypes.SignIn);
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
                SetError(Utils.INAVLID_EMAIL_MESSAGE, ErrorTypes.SignIn);
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


        public void SetQuestionsByCategory(string category)
        {
            databaseHandler.GetQuestionsByCategory(currentUser.idToken, category, 
                onSuccess:(questions) => 
                {
                    SetQuestions(questions);
                    ResetError();
                },
                onFailure:(message) => { SetError(message, ErrorTypes.SignIn); });
        }

        private void SetQuestions(Question[] questions)
        {
            Questions = questions;
            NotifyPropertyChanged("Questions");
        }

        public void SaveUser(Utils.OnSuccessFunc onSuccess)
        {
            onSuccess += delegate { ResetError(); };

            currentUser.SetCorrectAns();

            databaseHandler.SaveUserCorrectAns(currentUser, onSuccess , (message) => SetError(message, ErrorTypes.SaveScore)) ;
        }

        private void ResetError()
        {
            SetError("", ErrorTypes.None);
        }

        public void SetUserScore(int questionNum, bool isAnsCorrect)
        {

            Debug.Log("EDIT USER'S SCORE");
            currentUser.SetScore(questionNum, isAnsCorrect);
            //CurrentUser.score.SetQuestionScore(questionNum, isAnsCorrect);
        }


        // Set the Error propery.
        private void SetError(string message, ErrorTypes errorType)
        {
            Error.Message = message;
            Error.ErrorType = errorType;
            NotifyPropertyChanged("Error");
        }

        public int GetNumOfQuestionsByCategory(string category)
        {
            int numOfQuestion = fromQuestionNumToType.Where(pair => 
            pair.Value == Question.FromCategoryToTypeHebrew(category)).Select(pair => pair.Key).Count();
            return numOfQuestion;
        }

        public int GetNumOfCorrectAnswersByCategory(string category)
        {
            int numOfCorrectAnswers = fromQuestionNumToType.Where(pair =>
            pair.Value == Question.FromCategoryToTypeHebrew(category) &&
            currentUser.correctAnswers.Contains(pair.Key)).Select(pair => pair.Key).Count();
            return numOfCorrectAnswers;
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
