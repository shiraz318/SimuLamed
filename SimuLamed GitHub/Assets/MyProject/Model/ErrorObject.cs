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

        // Hebrew Messages.
        public const string EMAIL_EXISTS_MESSAGE = "כתובת אימייל נמצאת בשימוש";
        public const string EMAIL_NOT_FOUND_MESSAGE = "כתובת אימייל לא קיימת";
        public const string INVALID_PASSWORD_MESSAGE = "סיסמא שגויה";
        public const string WEAK_PASSWORD__MESSAGE = "סיסמא חלשה. אנא הזן לפחות 6 תווים";
        public const string FAIL_SAVE_SCORE_MESSAGE = "קרתה תקלת רשת! לא הצלחנו לשמור את ההתקדמות שלך. אם תבחר לחזור ללא שמירה, התקדמותך תאבד.";
        public const string FAIL_LOAD_STATISTICS_MESSAGE = "תקלה! לא הצלחנו לטעון את הסטטיסטיקה שלך";
        //public const string FAIL_LOAD_QUESTIONS_MESSAGE = "תקלה! לא הצלחנו לטעון את השאלות";
        public const string FAIL_LOAD_QUESTION_MESSAGE = "תקלה! לא הצלחנו לטעון את השאלה";
        public const string INAVLID_EMAIL_MESSAGE = "כתובת אימייל לא חוקית";
        public const string UNVERIFIED_EMAIL_MESSAGE = "אנא אמת את המייל שלך";
        public const string EMPTY_FIELD_MESSAGE = "אנא מלא את כל השדות";
        public const string ERROR_IN_KEYS = " מקש נמצא בשימוש יותר מפעם אחת";
        public const string FAIL_LOAD_IMAGE_MESSAGE = "תקלה! לא הצלחנו לטעון את התמונה";


        private string message;
        public string Message { get { return message; } set { SetMessage(value); } }
        public ErrorTypes ErrorType { get; set; }

        public ErrorObject(string message, ErrorTypes errorType)
        {
            Message = message;
            ErrorType = errorType;
        }

        // Set the message of this error by the given value string.
        private void SetMessage(string value)
        {
            if (value.Equals("EMAIL EXISTS"))
            {
                message = EMAIL_EXISTS_MESSAGE;
            }
            else if (value.Equals("EMAIL NOT FOUND"))
            {
                message = EMAIL_NOT_FOUND_MESSAGE;
            }
            else if (value.Equals("INVALID PASSWORD"))
            {
                message = INVALID_PASSWORD_MESSAGE;
            }
            else if (value.Equals("WEAK PASSWORD "))
            {
                message = WEAK_PASSWORD__MESSAGE;
            }
            else if (ErrorType.Equals(ErrorTypes.SaveScore))
            {
                message = FAIL_SAVE_SCORE_MESSAGE;
            }
            else if (ErrorType.Equals(ErrorTypes.Statistics))
            {
                message = FAIL_LOAD_STATISTICS_MESSAGE;
            }
            else if (ErrorType.Equals(ErrorTypes.LoadQuestions))
            {
                message = FAIL_LOAD_QUESTION_MESSAGE;
            }
            else
            {
                message = value;
            }

        }
    }
}
