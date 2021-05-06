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
        public enum QuestionOption
        {
            NotAsked,
            Wrong,
            Correct
        }


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
        
        public const string SAFETY_HEBREW = "בטיחות";
        public const string TRANSACTION_RULES_HEBREW = "חוקי התנועה";
        public const string UNDERSTANDING_VEHICLE_HEBREW = "הכרת הרכב";
        public const string SIGNS_HEBREW = "תמרורים";
        public const string MIXED_HEBREW = "כל הנושאים";

        public const string SAFETY_ENGLISH = "Safety";
        public const string TRANSACTION_RULES_ENGLISH = "TransactionRules";
        public const string SIGNS_ENGLISH = "Signs";
        public const string UNDERSTANDING_VEHICLE_ENGLISH = "UnderstandingVehicle";
        public const string MIXED_ENGLISH = "Mixed";
        
        
        public const string SIGN_IN_SCENE = "SignInScene";
        public const string SIGN_UP_SCENE = "SignUpScene";
        public const string MENU_SCENE = "MenuScene";
        public const string LEARNING_FROM_Q_SCENE = "LearningFromQuestionsScene";
        public const string STATISTICS_SCENE = "StatisticsScene";
        public const string QUESTIONS_SCENE = "QuestionsScene";
        public const string FORGOT_PASSWORD_SCENE = "ForgotPasswordScene";
        public const string SETTINGS_SCENE = "SettingsScene";
        public const string SIMULATION_SCENE_1 = "SimulationScene_1";
        public const string SIMULATION_SCENE_2 = "SimulationScene_2";
        public const string SIMULATION_SCENE_3 = "SimulationScene_3";
        public const string LEVELS_SCENE = "LevelsScene";
        //public const string SIMULATION_SCENE = "SimpleRoad";


        public const string ANS_1_NAME = "Ans1";
        public const string ANS_2_NAME = "Ans2";
        public const string ANS_3_NAME = "Ans3";
        public const string ANS_4_NAME = "Ans4";

        public const string RIGHT = "Right";
        public const string LEFT = "Left";
        public const string FORWARD = "Forward";
        public const string BACKWARDS = "Backwards";
        public const string SHOW_QUESTIONS = "ShowQuestions";
        public const string ERROR_IN_KEYS = "KEY ALREADY IN USE"; 


        public const string DEFAULT_FORWARD = "w";
        public const string DEFAULT_BACKWARDS = "s";
        public const string DEFAULT_RIGHT = "d";
        public const string DEFAULT_LEFT = "a";
        public const bool DEFAULT_TO_SHOW_QUESTIONS = true;
        
        
        public const string QUESTIONS_NUM_IN_SIM = "30";

        





        public const int HINTS_PER_CORRECT_ANS = 5;
        public const int INITIAL_NUMBER_OF_HINTS = 4;



        // Statics.
        public static Color32 greenColor = new Color32(14, 255, 0, 255);
        public static Color32 redColor = new Color32(255, 0, 0, 255);
        public static Color32 progress1Color = new Color32(255, 76, 76, 255);
        public static Color32 progress2Color = new Color32(255, 166, 61, 255);
        public static Color32 progress3Color = new Color32(248, 238, 92, 255);
        public static Color32 progress4Color = new Color32(141, 255, 95, 255);
        
        public static Color32 disabledAnsColor = new Color32(217, 177, 83, 255);//125

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
        public static void OnClickWithButtonCheck(string buttonWantedName, string buttonName, Action action)
        {
            if (buttonName.Equals(buttonWantedName))
            {
                action();
            }
        }




    }
}
