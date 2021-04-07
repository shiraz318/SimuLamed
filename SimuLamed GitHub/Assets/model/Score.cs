using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.model
{
    [Serializable]
    public class Score
    {
        private bool[] score;


        public Score(int numOfQustions)
        {
            score = new bool[numOfQustions];

        }

        public void SetQuestionScore(int questionNum, bool isCorrect)
        {
            if (questionNum >= 0)
            {
                score[questionNum] = isCorrect;
            }
        }

        public int[] GetTrueScore()
        {
            return score.Select((b, i) => b == true ? i :-1).Where(i => i != -1).ToArray();
        }

        //public void SetCorrectAns()
        //{
        //    correctAns = GetTrueScore();
        //}
        //public void InitCorrectAns()
        //{
        //    foreach (int correctAnsNum in correctAns)
        //    {
        //        SetQuestionScore(correctAnsNum, true);
        //    }
        //}



    }
}
 