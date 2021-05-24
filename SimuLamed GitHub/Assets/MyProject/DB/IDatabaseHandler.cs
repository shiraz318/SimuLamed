using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.model
{
    public interface IDatabaseHandler 
    {
        void SignInUser(string password, string email, Action<User> onSuccess, Action<string> onFailure);
        void ResetPassword(string email, Action onSuccess, Action<string> onFailure);
        void UploadDataset(List<Question> questions);
        void GetQuestionsByCategory(string userIdToken, string category, Action<Question[]> onSuccess , Action<string> onFailure);
        void PostUser(User user, Action onSuccess, Action<string> onFailure);
        void SignUp(string email, string password, Action<string,string> onSuccess, Action<string> onFailure);
        void SaveNewUser(User newUser, Action onSuccess, Action<string> onFailure);
        void GetNumberOfQuestions(string idToken, Action<int> onSuccess, Action<string> onFailure);
        void GetAllQuestions(string idToken, Action<Question[]> onSuccess, Action<string> onFailure);
        void GetQuestionsInLevel(string idToekn, string level, Action<Question[]> onSuccess, Action<string> onFailure);
    }
}
