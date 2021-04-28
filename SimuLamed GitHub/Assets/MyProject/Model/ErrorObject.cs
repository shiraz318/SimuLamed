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
        Statistics
    }
    public class ErrorObject
    {
        public string Message { get; set; }
        public ErrorTypes ErrorType { get; set; }

        public ErrorObject(string message, ErrorTypes errorType)
        {
            Message = message;
            ErrorType = errorType;
        }
    }
}
