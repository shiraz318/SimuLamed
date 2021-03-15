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


        public static Color32 greenColor = new  Color32(14, 255, 0, 255);
        public static Color32 redColor = new Color32(255, 0, 0, 255);
        private static Dictionary<int, QuestionType> fromQuestionNumToType;

        public static void SetFromQuestionNumToType(Dictionary<int, QuestionType> dic)
        {
            fromQuestionNumToType = dic;
        }

        public static QuestionType FromQuestionNumToType(int questionNum)
        {
            return fromQuestionNumToType[questionNum];
        }


    }
}
