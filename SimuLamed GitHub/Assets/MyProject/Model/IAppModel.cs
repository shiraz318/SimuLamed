using Assets.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.model
{
    public interface IAppModel : INotifyPropertyChanged
    {
        // Properties.
        Question[] Questions { get; set; } 
        ErrorObject Error { get; set; }
        int NumOfQuestions { get; set; }
        int HintsNumber { get; set; }
        int OpenLevel { get; set; }
        string CurrentUsername { get;}
        QuestionType SelectedSubject { get; set; }



        // Methods.
        void SignIn(string password, string email);
        void SignUp(string username, string password, string email);
        void ResetPassword(string email);
        void ResetCurrentUser();
        void SetQuestionsByCategory(string category, bool toRnd);
        void SetUserScore(int questionNum, bool isAnsCorrect);
        void SaveUser();
        int GetNumOfQuestionsByCategory(string category);
        int GetNumOfCorrectAnswersByCategory(string category);
        void DecreaseHint();
        void InitUserLastAns();
        void UpdateUserScore(Utils.QuestionOption[] playerScore);
        void SetFromNumToType(ErrorTypes errorType);
        void SetQuestionsByLevel(string level);
        //void SetQuestionsByLevel(string level);
        void UpdateUserLevel(int level);
    }
}
