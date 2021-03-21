using Assets;
using Assets.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsSceneVM : MonoBehaviour
{
    private IModel model;
    public static Text errorText;
    public GameObject[] subjects;

    public SceneLoader sceneLoader;

    public void Start()
    {
        errorText = GameObject.FindWithTag("ErrorMessage").GetComponent<Text>() as Text;
        model = Model.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (model.Error.ErrorType == ErrorTypes.Statistics)
            {
                errorText.text = model.Error.Message;
            }
        };
        SetSubjects();



    }

    private void SetSubjects()
    {
        subjects = GameObject.FindGameObjectsWithTag("Subject");
        int numOfQuestions = 0;
        int allQuestion = 0;
        int allCorrectAnswers = 0;
        int correctAnswers = 0;
        foreach (GameObject subject in subjects)
        {
            string name = subject.name;
            if (name.Equals("Mixed"))
            {
                numOfQuestions = allQuestion;
                correctAnswers = allCorrectAnswers;

            } else
            {
                string category = Question.FromTypeToCategory(Question.FromCategoryToTypeEnglish(name));
                numOfQuestions = model.GetNumOfQuestionsByCategory(category);
                correctAnswers = model.GetNumOfCorrectAnswersByCategory(category);
                allQuestion += numOfQuestions;
                allCorrectAnswers += correctAnswers;
            }
           
            SetSubject(subject, numOfQuestions, correctAnswers);
        }
    }

   

    private void SetSubject(GameObject subject, int numOfQuestions, int correctAnswers)
    {
        float value = ((float)correctAnswers / (float)numOfQuestions);
        SetSlider(subject, value);
        SetPrecent(subject, value * 100);
        

            
    }

    private void SetPrecent(GameObject subject, float value)
    {
        subject.GetComponentInChildren<Text>().text = (Math.Round(value, 1).ToString() + "%");

    }

    private void SetSlider(GameObject subject, float value)
    {
        subject.GetComponentInChildren<Slider>().value = value;
        SetSliderColor(subject.GetComponentInChildren<Slider>(),GetColor(value));
    }
    private Color32 GetColor(float value)
    {
        if (value <= 25)
        {
            return Utils.colorProgress1;
        }
        else if (value <= 50)
        {
            return Utils.colorProgress2;
        }
        else if (value <= 75)
        {
            return Utils.colorProgress3;
        }
        return Utils.colorProgress4;

    }

    private void SetSliderColor(Slider slider, Color32 color)
    {
        var fill = (slider as Slider).GetComponentsInChildren<Image>().FirstOrDefault(t => t.name == "Fill");
        if (fill != null)
        {
            fill.color = color;// Color.Lerp(Color.red, Color.green, 1f);
        }
        //Color32 tempColor = slider.image.color;
        //tempColor = color;
        //slider.image.color = tempColor;
    }

    public void OnClickBack()
    {
        
        sceneLoader.LoadNextScene("MenuScene");

    }


}
