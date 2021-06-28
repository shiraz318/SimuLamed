using System;
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

        //public QuestionType questionType;
        //public QuestionDifficulty questionDifficulty;
        public string question;
        //public string ans1;
        //public string ans2;
        //public string ans3;
        //public string ans4;
        //public string correctAns;
        public string[] answers;
        public string imageUrl;
        public int questionNumber;
        public string questionCategory;
        public string simulationLevel;



        
        public Question(int questionNumber, QuestionType questionType, string question, string[] answers, string imageUrl, string simulationLevel)
        //public Question(int questionNumber, QuestionType questionType, string question, string[] answers, string imageUrl)
        {
            this.questionNumber = questionNumber;
            this.questionCategory = FromTypeToCategory(questionType);
            //this.questionDifficulty = questionDifficulty;
            this.question = question;
            this.answers = answers;
            //this.ans1 = answers[0];
            //this.ans2 = answers[1];
            //this.ans3 = answers[2];
            //this.ans4 = answers[3];
            //this.correctAns = answers[4];
            this.imageUrl = imageUrl;
            this.simulationLevel = simulationLevel;
        }
        public string GetCorrectAns()
        {
            return this.answers[4];
        }
        // Returns hebrew category accordingly to the given quesiton type.
        public static string FromTypeToCategory(QuestionType questionType)
        {
            switch (questionType)
            {
                case QuestionType.Safety:
                    return Utils.SAFETY_HEBREW;
                case QuestionType.TransactionRules:
                    return Utils.TRANSACTION_RULES_HEBREW;
                case QuestionType.Signs:
                    return Utils.SIGNS_HEBREW;
                case QuestionType.UnderstandingVehicle:
                    return Utils.UNDERSTANDING_VEHICLE_HEBREW;
                default:
                    return Utils.MIXED_HEBREW;
            }

        }

        // Returns question type accordingly to the given hebrew category.
        public static QuestionType FromCategoryToTypeHebrew(string category)
        {
            switch (category)
            {
                case Utils.SAFETY_HEBREW:
                    return QuestionType.Safety;
                case Utils.TRANSACTION_RULES_HEBREW:
                    return QuestionType.TransactionRules;
                case Utils.SIGNS_HEBREW:
                    return QuestionType.Signs;
                case Utils.UNDERSTANDING_VEHICLE_HEBREW:
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
                case Utils.SAFETY_ENGLISH:
                    return QuestionType.Safety;
                case Utils.TRANSACTION_RULES_ENGLISH:
                    return QuestionType.TransactionRules;
                case Utils.SIGNS_ENGLISH:
                    return QuestionType.Signs;
                case Utils.UNDERSTANDING_VEHICLE_ENGLISH:
                    return QuestionType.UnderstandingVehicle;
                default:
                    return QuestionType.All;
            }

        }

    }
}
