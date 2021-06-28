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

        // Constants.
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

        // Scenes names.
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


        public const string ANS_1_NAME = "Ans1";
        public const string ANS_2_NAME = "Ans2";
        public const string ANS_3_NAME = "Ans3";
        public const string ANS_4_NAME = "Ans4";

        public const string RIGHT = "Right";
        public const string LEFT = "Left";
        public const string FORWARD = "Forward";
        public const string BACKWARDS = "Backwards";

        public const string SHOW_QUESTIONS = "ShowQuestions";
        public const string MUTE_SOUND = "MuteSound";


        // Default settings.
        public const string DEFAULT_FORWARD = "w";
        public const string DEFAULT_BACKWARDS = "s";
        public const string DEFAULT_RIGHT = "d";
        public const string DEFAULT_LEFT = "a";
        public const bool DEFAULT_TO_SHOW_QUESTIONS = true;
        public const bool DEFAULT_TO_MUTE_SOUND = false;
        
        
        public const string QUESTIONS_NUM_IN_SIM = "30";
        public const int HINTS_PER_CORRECT_ANS = 4;
        public const int INITIAL_NUMBER_OF_HINTS = 4;


        public const string SOUND_MANAGER = "SoundManager";
        public const string QUESTION_MANAGER = "QuestionsManager";
        public const string SCENE_LOADER =  "SceneLoader";
        public const string PLAYER_TAG = "Player";
        public const string VIEW = "View";
        public const string SCREENS = "Screens";

        public const int MAX_NUMBER_OF_ERRORS = 4;


    }
}
