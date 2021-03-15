using Assets.model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadDataset : MonoBehaviour
{
    IDatabaseHandler databaseHandler;
    void Start()
    {
        databaseHandler = FirebaseManager.Instance;
        List<Question> q = QuestionsCreator.GenerateQuestions();
        databaseHandler.UploadDataset(q);
        
    }

}
