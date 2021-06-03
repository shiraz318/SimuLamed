using Assets;
using Assets.MyProject.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsView : BaseView
{
    // Private fields.
    private QuestionsVM viewModel;
    private static Image image;
    private static List<GameObject> answers;


    private void Awake()
    {
        image = GameObject.Find("ImageButton").GetComponentInChildren<Image>();
        SetViewModel();
        GameObject[] answersGameObject = GameObject.FindGameObjectsWithTag("AnsButton");
        answers = answersGameObject.ToList();
    }

    // Set the view model.
    private void SetViewModel()
    {
        viewModel = GameObject.Find("View").GetComponent<QuestionsVM>();
        viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }
            
            // If the user got a new hint - play the new hint sound.
            if (eventArgs.PropertyName.Equals("IsNewHint") && viewModel.IsNewHint)
            {
                soundManager.GotNewHint();
            }
        };
    }

    // Disable a wrong answer.
    private void DisableWrongAns()
    {
        string correctAns = viewModel.GetCorrectAns();

        // Shuffle the buttons so every time a differrent wrong answer will be disable.
        System.Random rnd = new System.Random();
        answers = answers.OrderBy(x => rnd.Next()).ToList();

        foreach (GameObject ans in answers)
        {
            Button button = ans.GetComponent<Button>();
            // This is a wrong answer.
            if (!correctAns.Equals(button.GetComponentInChildren<TMP_Text>().text))
            {
                button.interactable = false;
                button.image.color = MyColors.disabledAnsColor;
                break;
            }
        }
    }

    // On click event handler for clicking the hint button.
    public void OnClickHint()
    {
        soundManager.OnClickHint();
        DisableWrongAns();
        viewModel.OnClickHint();
    }

}

