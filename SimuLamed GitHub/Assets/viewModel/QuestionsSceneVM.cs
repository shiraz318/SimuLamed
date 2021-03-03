using Assets.model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsSceneVM : MonoBehaviour
{
    public TMP_Text subject;
    IDatabaseHandler databaseHandler;
    public SceneLoader sceneLoader;
    private string selectedQuestionCategory;
    private List<Question> questions;
    private int questionNum;

    void Start()
    {
        questions = new List<Question>();
        databaseHandler = FirebaseManager.Instance;
        selectedQuestionCategory = Question.FromTypeToCategory(LearningFromQuestionsSceneVM.selectedSubject);
        subject.text = selectedQuestionCategory;
        questions = databaseHandler.GetQuestionsByType(Question.FromCategoryToTypeHebrew(selectedQuestionCategory));
        questionNum = 0;



    }

    private void PresetQuestion()
    {
        Debug.Log(questionNum);
    }

    public void OnClickBack()
    {
        sceneLoader.LoadNextScene("LearningFromQuestionsScene");
    }
    
    public void OnClickAnswer(Button button)
    {
        string name = button.name;

        // Check if it was correct
        // Mark the correct answer with green and the wrong (if exists) with red.
        
    

    }

    public void OnClickNextQuestion()
    {
        questionNum++;
        PresetQuestion();
    }

}
