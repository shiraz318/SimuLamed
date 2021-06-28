using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;


public class LivesCotroller : MonoBehaviour
{    

    [SerializeField]
    private Image[] lives;

    private SimulationVM viewModel;

    void Start()
    {
        viewModel = GameObject.Find(Utils.VIEW).GetComponent<SimulationVM>();
        viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals(nameof(viewModel.Lives)))
            {
                ChangeLives(viewModel.Lives);
            }
        };
    }

    // Change the number of lives on the screen.
    private void ChangeLives(int lives_left)
    {
        if (lives_left >= 0 && lives_left < Utils.MAX_NUMBER_OF_ERRORS)
        {
            // Disable the number of lives needed.
            lives[lives_left].gameObject.SetActive(false);
        }
    }

 
}
