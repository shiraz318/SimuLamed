using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.model
{
    public interface IDatabaseHandler : INotifyPropertyChanged
    {
        void SignUpUser(string username, string oassword, string email, Action onSuccess);
        void SignInUser(string password, string email, Action onSuccess);
        ErrorObject Error { get; set; }
        List<Question> Questions { get; set; }

        void ResetPassword(string email, Action onSuccess);
        void ResetCurrentUser();
        string GetUsername();
        void UploadDataset(List<Question> questions);
        void SetQuestionsByCategory(string category);

    }
}
