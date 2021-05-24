using Assets.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model
{
    [Serializable]
    public class UserState
    {
        public int[] correctAnswers; // array that contains the indexes of questions that the user answered correctly.
        public int numOfHints; // number of hints the user has left.
        public int openLevel; // the highest level the user opened.

        private bool[] score; // array in size of number of questions that contains in each index if the user answered correctly or not.
        private int correctAnsInARowCounter; // counts the number of correct answeres in a row for hint purpess.
        

        public UserState(int[] correctAnswers, int numOfHints, int openLevel)
        {
            this.correctAnswers = correctAnswers;
            this.numOfHints = numOfHints;
            this.openLevel = openLevel;

        }

        // Init the score.
        public void InitScore(int numOfQuestions)
        {
            correctAnsInARowCounter = 0;
            
            score = new bool[numOfQuestions];

            // For each correct answer, mark this index in the score array as true.
            foreach (int correctAnsIdx in correctAnswers)
            {
                if (correctAnsIdx >= 0)
                {
                    score[correctAnsIdx] = true;
                }
            }

        }

        // Update the score according to the given playerScore.
        public void UpdateScore(Utils.QuestionOption[] playerScore)
        {
            for (int i = 0; i < score.Length; i++)
            {
                if (playerScore[i].Equals(Utils.QuestionOption.Correct))
                {
                    score[i] = true;
                }
                else if (playerScore[i].Equals(Utils.QuestionOption.Wrong))
                {
                    score[i] = false;
                }
            }

        }

        // Set the score according to the given question number and whether the user was correct or not.
        public void SetScore(int questionNum, bool isAnsCorrect)
        {
            // The user was correct.
            if (isAnsCorrect)
            {
                correctAnsInARowCounter = (correctAnsInARowCounter + 1) % (Utils.HINTS_PER_CORRECT_ANS + 1);
            }
            // The user was wrong.
            else
            {
                correctAnsInARowCounter = 0;
            }

            score[questionNum] = isAnsCorrect;
        }


        // Returns true if user deserve a new hint and false otherwise.
        public bool IsDeserveNewHint()
        {
            bool isDeserve = correctAnsInARowCounter == Utils.HINTS_PER_CORRECT_ANS;
            if (isDeserve)
            {
                correctAnsInARowCounter = 0;
            }
            return isDeserve;
        }

        // Set correct answers according to the true indexes in the score array.
        public void SetCorrectAns()
        {
            correctAnswers = score.Select((b, i) => b == true ? i : -1).Where(i => i != -1).ToArray();
            if (correctAnswers.Length == 0)
            {
                correctAnswers = new int[] { -1 };
            }
        }
        public void InitCorrectAnsInARowCounter()
        {
            correctAnsInARowCounter = 0;
        }
    }
}
