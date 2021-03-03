using Assets.model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LearningFromQuestionsSceneVM : MonoBehaviour
{
    IDatabaseHandler databaseHandler;
    public SceneLoader sceneLoader;
    public static QuestionType selectedSubject;

    void Start()
    {
        databaseHandler = FirebaseManager.Instance;
    }

    public void OnClickSubject(Button button)
    {
        string name = button.name;
        selectedSubject = Question.FromCategoryToTypeEnglish(name);
        
        sceneLoader.LoadNextScene("QuestionsScene");
    }

    public void OnClickBack()
    {
        sceneLoader.LoadNextScene("MenuScene");
    }

}
