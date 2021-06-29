﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.model
{
    public enum QuestionType
    {
        Safety,
        TransactionRules,
        Signs,
        UnderstandingVehicle,
        All
    }
    public enum QuestionDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    [Serializable]
    public class Question
    {

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


        private const int CORRECT_ANS_IDX = 4;

        public string question;
        public string[] answers;
        public string imageUrl;
        public int questionNumber;
        public string questionCategory;
        public string simulationLevel;



        
        public Question(int questionNumber, QuestionType questionType, string question, string[] answers, string imageUrl, string simulationLevel)
        {
            this.questionNumber = questionNumber;
            this.questionCategory = FromTypeToCategory(questionType);
            this.question = question;
            this.answers = answers;
            this.imageUrl = imageUrl;
            this.simulationLevel = simulationLevel;
        }
        
        // Get the correct answer.
        public string GetCorrectAns()
        {
            return this.answers[CORRECT_ANS_IDX];
        }
        
        // Returns hebrew category accordingly to the given question type.
        public static string FromTypeToCategory(QuestionType questionType)
        {
            switch (questionType)
            {
                case QuestionType.Safety:
                    return SAFETY_HEBREW;
                case QuestionType.TransactionRules:
                    return TRANSACTION_RULES_HEBREW;
                case QuestionType.Signs:
                    return SIGNS_HEBREW;
                case QuestionType.UnderstandingVehicle:
                    return UNDERSTANDING_VEHICLE_HEBREW;
                default:
                    return MIXED_HEBREW;
            }

        }

        // Returns question type accordingly to the given hebrew category.
        public static QuestionType FromCategoryToTypeHebrew(string category)
        {
            switch (category)
            {
                case SAFETY_HEBREW:
                    return QuestionType.Safety;
                case TRANSACTION_RULES_HEBREW:
                    return QuestionType.TransactionRules;
                case SIGNS_HEBREW:
                    return QuestionType.Signs;
                case UNDERSTANDING_VEHICLE_HEBREW:
                    return QuestionType.UnderstandingVehicle;
                default:
                    return QuestionType.All;
            }

        }

        // Returns question type accordingly to the given english category.
        public static QuestionType FromCategoryToTypeEnglish(string category)
        {
            switch (category)
            {
                case SAFETY_ENGLISH:
                    return QuestionType.Safety;
                case TRANSACTION_RULES_ENGLISH:
                    return QuestionType.TransactionRules;
                case SIGNS_ENGLISH:
                    return QuestionType.Signs;
                case UNDERSTANDING_VEHICLE_ENGLISH:
                    return QuestionType.UnderstandingVehicle;
                default:
                    return QuestionType.All;
            }

        }

    }
}
