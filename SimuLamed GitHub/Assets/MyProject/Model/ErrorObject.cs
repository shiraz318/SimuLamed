using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.model
{
    public enum ErrorTypes
    {
        None,
        SignIn,
        SignUp,
        ResetPassword,
        SaveScore,
        Statistics,
        LoadQuestions
    }
    public class ErrorObject
    {
        private string message;
        public string Message { get { return message; } set { SetMessage(value); } }
        public ErrorTypes ErrorType { get; set; }

        public ErrorObject(string message, ErrorTypes errorType)
        {
            Message = message;
            ErrorType = errorType;
        }
        private void SetMessage(string value)
        {
            if (value.Equals("EMAIL EXISTS"))
            {
                message = "כתובת אימייל נמצאת בשימוש";
            }
            else if (value.Equals("EMAIL NOT FOUND"))
            {
                message = "כתובת אימייל לא קיימת";
            }
            else if (value.Equals("INVALID PASSWORD"))
            {
                message = "סיסמא שגויה";
            }
            else if (value.Equals("WEAK PASSWORD "))
            {
                message = "סיסמא חלשה. אנא הזן לפחות 6 תווים";
            }
            else if (ErrorType.Equals(ErrorTypes.SaveScore))
            {
                message = Utils.FAIL_SAVE_SCORE_MESSAGE_H;
            }
            else if (ErrorType.Equals(ErrorTypes.Statistics))
            {
                message = "תקלה! לא הצלחנו לטעון את הסטטיסטיקה שלך";
            }
            else if (ErrorType.Equals(ErrorTypes.LoadQuestions))
            {
                message = "תקלה! לא הצלחנו לטעון את השאלות";
            }
            else
            {
                message = value;
            }

        }
    }
}
