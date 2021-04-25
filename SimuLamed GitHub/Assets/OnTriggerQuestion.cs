using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerQuestion : MonoBehaviour
{
    public int questionNumber;
    private static SimulationVM viewModel;

    private void Start()
    {
        viewModel = GameObject.Find("View").GetComponent<SimulationVM>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("QUESTION NUMBER" + questionNumber.ToString());
            
            viewModel.DisplayQuestion(questionNumber);

            // The view model gets the question from the model and set it's property that are bindable to the questions manager/presenter.
            // Also, when the values of the view model are changed - may be adding a propery of bool for isQuestionReady -
            // it's means we need to:
            // 1. freeze the game
            // 2. make the question presenter canvas visable and interactable

            // When the player answer a question - we need to keep track of his success of failure.
            
            // We need to count how many times we presented a question to the user.
           
            Debug.Log("PRESENTING QUESTION NUMBER:" + questionNumber);
        }


    }

}
