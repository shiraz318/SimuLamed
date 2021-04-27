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
        void SignUpUser(string username, string password, string email, Utils.OnSuccessFunc onSuccess, Utils.OnFailureFunc onFailure);
        void SignInUser(string password, string email, Utils.OnSuccessSignInFunc onSuccess, Utils.OnFailureFunc onFailure);

        void ResetPassword(string email, Utils.OnSuccessFunc onSuccess, Utils.OnFailureFunc onFailure);
        void UploadDataset(List<Question> questions);
        void GetQuestionsByCategory(string userIdToken, string category, Action<Question[]> onSuccess ,Utils.OnFailureFunc onFailure);
        void GetAllQuestions(string userIdToken, Action<Dictionary<string, Question>> onSuccess, Utils.OnFailureFunc onFailure);
        void SaveUser(User currentUser, Utils.OnSuccessFunc onSuccess, Utils.OnFailureFunc onFailure);
    }
}
