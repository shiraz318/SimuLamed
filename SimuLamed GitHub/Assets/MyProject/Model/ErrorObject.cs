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

        // Error Messages.
        public const string EMAIL_EXISTS_MESSAGE = "כתובת אימייל נמצאת בשימוש";
        public const string EMAIL_NOT_FOUND_MESSAGE = "כתובת אימייל לא קיימת";
        public const string INVALID_PASSWORD_MESSAGE = "סיסמא שגויה";
        public const string WEAK_PASSWORD__MESSAGE = "סיסמא חלשה. אנא הזן לפחות 6 תווים";
        public const string FAIL_SAVE_SCORE_MESSAGE = "קרתה תקלת רשת! לא הצלחנו לשמור את ההתקדמות שלך. אם תבחר לחזור ללא שמירה, התקדמותך תאבד.";
        public const string FAIL_LOAD_STATISTICS_MESSAGE = "תקלה! לא הצלחנו לטעון את הסטטיסטיקה שלך";
        public const string FAIL_LOAD_QUESTION_MESSAGE = "תקלה! לא הצלחנו לטעון את השאלה";
        public const string INAVLID_EMAIL_MESSAGE = "כתובת אימייל לא חוקית";
        public const string UNVERIFIED_EMAIL_MESSAGE = "אנא אמת את המייל שלך";
        public const string EMPTY_FIELD_MESSAGE = "אנא מלא את כל השדות";
        public const string ERROR_IN_KEYS = " מקש נמצא בשימוש יותר מפעם אחת";
        public const string KEY_IS_MISSING = "אנא הזן את כל המקשים";
        public const string FAIL_LOAD_IMAGE_MESSAGE = "תקלה! לא הצלחנו לטעון את התמונה";
        public const string TIMEOUT_ERROR_MESSAGE = "שגיאה! בדוק את חיבור הרשת שלך";


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
            switch (value)
            {
                case "EMAIL EXISTS":
                    message = EMAIL_EXISTS_MESSAGE;
                    return;
                case "EMAIL NOT FOUND":
                    message = EMAIL_NOT_FOUND_MESSAGE;
                    return;
                case "INVALID PASSWORD":
                    message = INVALID_PASSWORD_MESSAGE;
                    return;
                case "WEAK PASSWORD ":
                    message = WEAK_PASSWORD__MESSAGE;
                    return;
                default:
                    break;
            };

            switch (ErrorType)
            {
                case ErrorTypes.SaveScore:
                    message = FAIL_SAVE_SCORE_MESSAGE;
                    break;
                case ErrorTypes.Statistics:
                    message = FAIL_LOAD_STATISTICS_MESSAGE;
                    break;
                case ErrorTypes.LoadQuestions:
                    message = FAIL_LOAD_QUESTION_MESSAGE;
                    break;
                default:
                    message = value;
                    break;
            };

        }
    }
}
