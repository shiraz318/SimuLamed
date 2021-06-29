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
       
        
        public const string QUESTIONS_NUM_IN_SIM = "30";
        public const int HINTS_PER_CORRECT_ANS = 4;
        public const int INITIAL_NUMBER_OF_HINTS = 4;
        public const int MAX_NUMBER_OF_ERRORS = 4;


    }
}
