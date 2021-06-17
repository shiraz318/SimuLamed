using Assets;
using Assets.MyProject.View;
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
    // Private fields.
    private static QuestionsManager questionManager;
    private static SoundManager soundManager;
    private TMP_Text ansText;
    private Image ansImage;
   
   // Properties.
   public string CorrectAns 
    { 
        get 
        { 
            if (questionManager.IsQuestionSet)
            {
                return questionManager.CurrentQuestion.GetCorrectAns(); 
                //return questionManager.CurrentQuestion.correctAns; 
            }
            return ""; 
        }
    }

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
            // The user answered a question.
            else if (eventArgs.PropertyName.Equals("LastAnswerResults"))
            {
                // If this button contains the correct answer - change it's color.
                if (ansText.text.Equals(CorrectAns))
                {
                    ansImage.color = MyColors.greenColor;
                }
            }

        };
    }
    
    // Reset the answer button color.
    public void ResetAnsColor()
    {
        ansImage.color = MyColors.blackColor;
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
            ansImage.color = MyColors.greenColor;
        }
        // This is a wrong answer.
        else
        {
            soundManager.OnClickWrongAns();
            ansImage.color = MyColors.redColor;
        }
        
        questionManager.OnClickAnswer(isAnsCorrect);
    }

}




