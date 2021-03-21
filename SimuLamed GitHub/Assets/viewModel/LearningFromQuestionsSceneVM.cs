using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LearningFromQuestionsSceneVM : MonoBehaviour
{
    private IModel model;
    public SceneLoader sceneLoader;
    public static QuestionType selectedSubject;
    public static Text errorText;


    void Start()
    {
        errorText = GameObject.FindWithTag("ErrorMessage").GetComponent<Text>() as Text;
        model = Model.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Error"))
            {
                if (model.Error.ErrorType == ErrorTypes.SaveScore)
                {
                    errorText.text = model.Error.Message;
                }
            }
        };
    }

    public void OnClickSubject(Button button)
    {
        string name = button.name;
        selectedSubject = Question.FromCategoryToTypeEnglish(name);
        
        sceneLoader.LoadNextScene("QuestionsScene");
    }

    public void OnClickBack()
    {
        model.SaveUserScore(()=> sceneLoader.LoadNextScene("MenuScene"));
    }

}
