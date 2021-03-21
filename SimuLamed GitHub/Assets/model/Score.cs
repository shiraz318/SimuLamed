using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.model
{
    public class Score
    {
        private bool[] score;

        public Score(int numOfQustions)
        {
            score = new bool[numOfQustions];

        }

        public void SetQuestionScore(int questionNum, bool isCorrect)
        {
            score[questionNum] = isCorrect;
        }

        public List<int> GetTrueScore()
        {
            return score.Select((b, i) => b == true ? i : -1).Where(i => i != -1).ToList();
        }



    }
}
 