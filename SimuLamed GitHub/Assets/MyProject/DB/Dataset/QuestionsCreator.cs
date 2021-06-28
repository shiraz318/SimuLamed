using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Assets.model
{
    public class QuestionsCreator
    {
        private const string FILE_PATH = @"D:\unity_installation\Unity Projects\SimuLamed\SimuLamed GitHub\Assets\MyProject\DB\Dataset\Dataset.csv";
        private const int QUESTION_IDX = 1;
        private const int FIRST_ANS_IDX = 4;
        private const int NUM_OF_ANSWERES = 4;
        private const int IMAGE_IDX = 9;
        private const int SIMULATION_LEVEL_IDX = 10;
        private const int QUESTION_TYPE_IDX = 3;


        // Generate questions.
        public static List<Question> GenerateQuestions()
        {
            List<string[]> lines = ReadFromDataset();
            List<Question> questions = CreateQuestions(lines);
            return questions;
        }

        // Read from the dataset.
        private static List<string[]> ReadFromDataset()
        {
            StreamReader sr = new StreamReader(FILE_PATH, Encoding.GetEncoding("windows-1255"));

            var lines = new List<string[]>();
            bool isFirstRow = true;

            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(',');
                if (isFirstRow)
                {
                    isFirstRow = false;
                    continue;
                }

                /*
                 * Commas were replaced with '*' so we need to replace them back to commas.
                 * Remove '\' In the end of the data and replace double '\' in one '\'.
                 */
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = line[i].Replace('*', ',');
                    line[i] = line[i].Trim('\"').Replace("\"\"", "\"");
                }
                lines.Add(line);
            }
            return lines;
        }


        // Get the question type from the given string.
       private static QuestionType GetQuestionType(string type)
       {
            int number = int.Parse(type[type.Length - 1].ToString());
            return (QuestionType)number;
       }

        // Parse a given string array into a question object.
        private static Question ParseQuestion(string[] line, int rowNumber)
        {

            QuestionType questionType = GetQuestionType(line[QUESTION_TYPE_IDX]);
            string questionString = FixHebrew(line[QUESTION_IDX]);
            
            string[] answers = new string[5];
            for (int i = 0; i <= NUM_OF_ANSWERES; i++)
            {
                answers[i] = FixHebrew(line[FIRST_ANS_IDX + i]);
            }

            return new Question(rowNumber, questionType, questionString, answers, line[IMAGE_IDX], line[SIMULATION_LEVEL_IDX]);
        }

        // Create questions from the given list.
        private static List<Question> CreateQuestions(List<string[]> lines)
        {
            List<Question> questions = new List<Question>();
            int rowNumber = 0;
            foreach (string[] line in lines)
            {
                Question question = ParseQuestion(line, rowNumber);
                questions.Add(question);
                rowNumber++;
            }

            return questions;
        }

        // Replace open brackets with close brackets in the given string.
        private static string FixOneStringBracket(string text)
        {
            text = text.Replace("(", "&");
            text = text.Replace(")", "(");
            text = text.Replace("&", ")");
            return text;
        }


        // Revers a given string.
        private static string ReversString(string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        // Replace a given substring of a given string with it's revers string.
        private static string ReplaceWithRevers(string stringToReplace, string text)
        {
            if (!string.IsNullOrEmpty(stringToReplace))
            {
                return text.Replace(stringToReplace, ReversString(stringToReplace));
            }
            return text;
        }


        // Fix a given string and revers it's numbers and english letters.
        private static string FixOneStringNumbersAndEnglishLetters(string text)
        {
            //"כן – מהשעה 21:00 עד לשעה 05:30 בימי חול בלבד.  "
            string pattern = @"[\p{IsHebrew}]+";

            var hebrewMatchCollection = Regex.Matches(text, pattern);
            string hebrewPart = string.Join(" ", hebrewMatchCollection.Cast<Match>().Select(m => m.Value));

            var commaNumbersMatches = Regex.Matches(text, @"(\d{0,3},)(\d{3},)?\d{3}"); // for example: 1,000 
            var hourMatches = Regex.Matches(text, @"(2[0-3]|[0-1][0-9]):([0-5][0-9])"); // for example: 06:30

            // All substrings that are not hebrew letters.
            string[] notHebrews = Regex.Split(text, pattern);

            foreach (string notHebrew in notHebrews)
            {
                if (!string.IsNullOrEmpty(notHebrew))
                {
                    var englishPart = Regex.Replace(notHebrew, "[^a-zA-Z]", ""); // english letters
                    var simpleNumbersOnlyPart = Regex.Replace(notHebrew, "[^0-9]", ""); // only numbers

                    text = ReplaceWithRevers(englishPart, text);
                    text = ReplaceWithRevers(simpleNumbersOnlyPart, text);
                }

            }
            // Reverse hours part.
            foreach (Match hourMatch in hourMatches)
            {
                text = ReplaceWithRevers(hourMatch.Value, text);
            }
            // Reverse numbers with commas.
            foreach (Match commaMatch in commaNumbersMatches)
            {
                text = ReplaceWithRevers(commaMatch.Value, text);
            }
            return text;

        }

        // Fix a given string to fit hebrew presentation.
        private static string FixHebrew(string text)
        {
            // Fix brackets.
            text = FixOneStringBracket(text);
            // Fix numbers and english letters.
            text = FixOneStringNumbersAndEnglishLetters(text);
            return text;
        }
    }
}
