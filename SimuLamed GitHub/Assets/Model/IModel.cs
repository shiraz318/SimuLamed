﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.model
{
    public interface IModel : INotifyPropertyChanged
    {
        // Properties.
        Question[] Questions { get; set; }
        ErrorObject Error { get; set; }
        int NumOfQuestions { get; }



        // Methods.
        void SignIn(string password, string email, Utils.OnSuccessFunc onSuccess);
        void SignUp(string username, string password, string email, Utils.OnSuccessFunc onSuccess);
        void ResetPassword(string email, Utils.OnSuccessFunc onSuccess);
        
        string GetCurrentUsername();
        void ResetCurrentUser();

        void SetQuestionsByCategory(string category);
        void SetUserScore(int questionNum, bool isAnsCorrect);
        void SaveUser(Utils.OnSuccessFunc onSuccess);
        int GetNumOfQuestionsByCategory(string category);
        int GetNumOfCorrectAnswersByCategory(string category);
    }
}
