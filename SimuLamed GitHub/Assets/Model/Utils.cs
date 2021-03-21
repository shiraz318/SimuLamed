using Assets.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Utils
    {
        // Delegates.
        public delegate void OnSuccessFunc();
        public delegate void OnSuccessSignInFunc(User user);
        public delegate void OnFailureFunc(string errorMessage);

        // Constants.
        public const string INAVLID_EMAIL_MESSAGE = "INVALID EMAIL ADDRESS";
        public const string UNVERIFIED_EMAIL_MESSAGE = "PLEASE VERIFY YOUR EMAIL";
        public const string EMPTY_FIELD_MESSAGE = "PLEASE FILL ALL FIELDS";
        public const string EMPTY_EMAIL_MESSAGE = "PLEASE ENTER YOUR EMAIL";
        public const string FAIL_SAVE_SCORE_MESSAGE = "WE COULD NOT SAVE YOUR PROGRESS";

        // Statics.
        public static Color32 greenColor = new Color32(14, 255, 0, 255);
        public static Color32 redColor = new Color32(255, 0, 0, 255);
        public static Color32 colorProgress1 = new Color32(231, 27, 29, 255);
        public static Color32 colorProgress2 = new Color32(241, 95, 28, 255);
        public static Color32 colorProgress3 = new Color32(243, 237, 38, 255);
        public static Color32 colorProgress4 = new Color32(156, 221, 0, 255);

        //private static Dictionary<int, QuestionType> fromQuestionNumToType;


        // Functions.
        //public static void SetFromQuestionNumToType(Dictionary<int, QuestionType> dic)
        //{
        //    fromQuestionNumToType = dic;
        //}

        //public static QuestionType FromQuestionNumToType(int questionNum)
        //{
        //    return fromQuestionNumToType[questionNum];
        //}


    }
}
