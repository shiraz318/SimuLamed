using Assets;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;

[RequireComponent(typeof(Button))]
public class OnClickAnswer : MonoBehaviour
{

    private static QuestionsManager questionManager;
    private static SoundManager soundManager;
    private TMP_Text ansText;
    private Image ansImage;
   
   public string CorrectAns { get { if (questionManager.IsQuestionSet) { return questionManager.CurrentQuestion.correctAns; } return ""; } }

    void Start()
    {
        ansText = GetComponentInChildren<TMP_Text>();
        ansImage = GetComponent<Image>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        SetQuestionManager();

        gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnClickAns(); });
    }

    // Set the question manager.
    private void SetQuestionManager()
    {
        questionManager = GameObject.Find("QuestionsManager").GetComponent<QuestionsManager>();
        questionManager.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }
            if (eventArgs.PropertyName.Equals("ResetAnsColor"))
            {
                ResetAnsColor();
            }
            else if (eventArgs.PropertyName.Equals("LastAnswerResults"))
            {
                // If this button contains the correct answer - change it's color.
                if (ansText.text.Equals(CorrectAns))
                {
                    ansImage.color = Utils.greenColor;
                }

                // User answered the wrong answer.
                //if (!questionManager.LastAnswerResults.Item2)
                //{
                //    // If this button contains the correct answer - change it's color.
                //    if (ansText.text.Equals(CorrectAns))
                //    {
                //        ansImage.color = Utils.greenColor;
                //    }
                //}
            }

        };
    }
    
    // Reset the answer button color.
    public void ResetAnsColor()
    {
        ansImage.color = Color.black;
    }

    // On click event handler for clicking an answer.
    private void OnClickAns()
    {
        bool isAnsCorrect = false;
        
        // This is the correct answer.
        if (ansText.text.Equals(CorrectAns))
        {
            soundManager.OnClickCorrectAns();
            isAnsCorrect = true;
            ansImage.color = Utils.greenColor;
        }
        // This is a wrong answer.
        else
        {
            soundManager.OnClickWrongAns();
            ansImage.color = Utils.redColor;
        }
        
        questionManager.OnClickAnswer(isAnsCorrect);
    }

}




//using Assets;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityWeld.Binding;

//[RequireComponent(typeof(Button))]
//public class OnClickAnswer : MonoBehaviour
//{

    
//    private QuestionsVM viewModel;
//    private TMP_Text ansText;
//    private Image ansImage;
   
//   public string CorrectAns { get { if (viewModel.IsQuestionSet) { return viewModel.GetCorrectAns(); } return ""; } }

//    // Start is called before the first frame update
//    void Start()
//    {
//        ansText = GetComponentInChildren<TMP_Text>();
//        ansImage = GetComponent<Image>();

//        SetViewModel();

//        gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnClickAns(); });
//    }

//    // Set the view model.
//    private void SetViewModel()
//    {
//        viewModel = GameObject.Find("View").GetComponent<QuestionsVM>();
//        viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
//        {
//            if (eventArgs.PropertyName.Equals("ResetAnsColor"))
//            {
//                ResetAnsColor();
//            }
//            else if (eventArgs.PropertyName.Equals("WrongAnswer"))
//            {
//                // If this button contains the correct answer - change it's color.
//                if (ansText.text.Equals(CorrectAns))
//                {
//                    ansImage.color = Utils.greenColor;
//                }
//            }

//        };
//    }
    
//    // Reset the answer button color.
//    public void ResetAnsColor()
//    {
//        ansImage.color = Color.black;
//    }

//    // On click event handler for clicking an answer.
//    private void OnClickAns()
//    {
//        bool isAnsCorrect = false;
        
//        // This is the correct answer.
//        if (ansText.text.Equals(CorrectAns))
//        {
//            isAnsCorrect = true;
//            ansImage.color = Utils.greenColor;
//        }
//        // This is a wrong answer.
//        else
//        {
//            ansImage.color = Utils.redColor;
//        }
//        viewModel.OnClickAnswer(isAnsCorrect);
//    }

//}




