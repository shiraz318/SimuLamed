﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerQuestion : MonoBehaviour
{
    public int questionNumber;
    private static ScreensManager screensManager;



    // private static SimulationVM viewModel;

    private BoxCollider boxCollider; 

    private void Start()
    {
        screensManager = GameObject.Find("Screens").GetComponent<ScreensManager>();
        boxCollider = GetComponent<BoxCollider>();
        


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Ignore collision with the player.
            Physics.IgnoreCollision(GameObject.Find("Car").GetComponent<BoxCollider>(), boxCollider);

            Debug.Log("QUESTION NUMBER" + questionNumber.ToString());
            boxCollider.isTrigger = false;
            screensManager.DisplayQuestion(questionNumber);
            
            
            // The view model gets the question from the model and set it's property that are bindable to the questions manager/presenter.
            // Also, when the values of the view model are changed - may be adding a propery of bool for isQuestionReady -
            // it's means we need to:
            // 1. freeze the game
            // 2. make the question presenter canvas visable and interactable

            // When the player answer a question - we need to keep track of his success of failure.

           
            Debug.Log("PRESENTING QUESTION NUMBER:" + questionNumber);
        }


    }

}
