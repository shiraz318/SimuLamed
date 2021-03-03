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

    public class Question
    {

        //public QuestionType questionType;
        public QuestionDifficulty questionDifficulty;
        public string question;
        public string ans1;
        public string ans2;
        public string ans3;
        public string ans4;
        public string correctAns;
        public string imageUrl;
        public int questionNumber;
        public string questionCategory;


        public Question(int questionNumber, QuestionType questionType, QuestionDifficulty questionDifficulty, string question, string[] answers, string imageUrl)
        {
            this.questionNumber = questionNumber;
            this.questionCategory = FromTypeToCategory(questionType);
            this.questionDifficulty = questionDifficulty;
            this.question = question;
            this.ans1 = answers[0];
            this.ans2 = answers[1];
            this.ans3 = answers[2];
            this.ans4 = answers[3];
            this.correctAns = answers[4];
            this.imageUrl = imageUrl;
        }

        public static string FromTypeToCategory(QuestionType questionType)
        {
            switch (questionType)
            {
                case QuestionType.Safety:
                    return "בטיחות";
                case QuestionType.TransactionRules:
                    return "חוקי התנועה";
                case QuestionType.Signs:
                    return "תמרורים";
                case QuestionType.UnderstandingVehicle:
                    return "הכרת הרכב";
                default:
                    return "כל הנושאים";
            }

        }

        public static QuestionType FromCategoryToTypeHebrew(string category)
        {
            switch (category)
            {
                case "בטיחות":
                    return QuestionType.Safety;
                case "חוקי התנועה":
                    return QuestionType.TransactionRules;
                case "תמרורים":
                    return QuestionType.Signs;
                case "הכרת הרכב":
                    return QuestionType.UnderstandingVehicle;
                default:
                    return QuestionType.All;
            }

        }
        public static QuestionType FromCategoryToTypeEnglish(string category)
        {
            switch (category)
            {
                case "Safety":
                    return QuestionType.Safety;
                case "TransactionRules":
                    return QuestionType.TransactionRules;
                case "Signs":
                    return QuestionType.Signs;
                case "UnderstandingVehicle":
                    return QuestionType.UnderstandingVehicle;
                default:
                    return QuestionType.All;
            }

        }

    }
}
