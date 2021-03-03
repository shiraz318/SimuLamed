using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;



namespace Assets.model
{
    public class QuestionsCreator
    {
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
            string filePath = @"D:\unity_installation\Unity Projects\SimuLamed\SimuLamed GitHub\Assets\Dataset\Dataset.csv";
            StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("windows-1255"));

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
            string[] answers = new string[5];
            QuestionDifficulty questionDifficulty;
            try
            {
                questionDifficulty = (QuestionDifficulty)int.Parse(line[0]);
            }
            catch 
            {
                questionDifficulty = QuestionDifficulty.Medium;
            }

            string questionString = line[1];
            QuestionType questionType = GetQuestionType(line[3]);
            answers[0] = line[4];
            answers[1] = line[5];
            answers[2] = line[6];
            answers[3] = line[7];
            answers[4] = line[8];
            string imageUrl = line[9];


            return new Question(rowNumber, questionType, questionDifficulty, questionString, answers, imageUrl);
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
    }
}
