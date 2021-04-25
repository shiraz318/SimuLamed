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
        public int[] correctAnswers;
        public int numOfHints;

        private Score score;

        private bool[] lastAnswers;
        private int lastAnswersIndex;

        // Constructor.
        public UserState(int[] correctAnswers, int numOfHints)
        {
            this.correctAnswers = correctAnswers;
            this.numOfHints = numOfHints;

        }

        // Init the score of this user state.
        public void InitScore(int numOfQuestions)
        {
            InitLastAns();
            score = new Score(numOfQuestions);
            foreach (int correctAnsNum in correctAnswers)
            {
                score.SetQuestionScore(correctAnsNum, true);
            }

        }

        // Set the score according to the given question number and whether the user was correct or not.
        public void SetScore(int questionNum, bool isAnsCorrect)
        {
            // The user was correct.
            if (isAnsCorrect)
            {
                // The user just got a new hint - need to reset his last answers and start the count from zero.
                if (lastAnswersIndex == 0)
                {
                    lastAnswers = new bool[Utils.HINTS_PER_CORRECT_ANS];
                }

                lastAnswers[lastAnswersIndex] = isAnsCorrect;
                lastAnswersIndex = (lastAnswersIndex + 1) % Utils.HINTS_PER_CORRECT_ANS;
            }
            // The user was wrong.
            else
            {
                ResetLastAns();
            }

            score.SetQuestionScore(questionNum, isAnsCorrect);
        }

        // Reset last answers and it's index.
        private void ResetLastAns()
        {
            lastAnswers = new bool[Utils.HINTS_PER_CORRECT_ANS];
            lastAnswersIndex = 0;
        }

        // Returns true if user deserve a new hint and false otherwise.
        public bool IsDeserveNewHint()
        {
            bool isDeserveNewHint = lastAnswers[Utils.HINTS_PER_CORRECT_ANS - 1];
            
            // Reset the count.
            if (isDeserveNewHint)
            {
                ResetLastAns();
            }
            return isDeserveNewHint;
        }

        // Set correct answers according to the true indexes in the score object.
        public void SetCorrectAns()
        {
            correctAnswers = score.GetTrueScore();
        }
        public void InitLastAns()
        {
            lastAnswers = new bool[Utils.HINTS_PER_CORRECT_ANS];
            lastAnswersIndex = 0;
        }
    }
}
