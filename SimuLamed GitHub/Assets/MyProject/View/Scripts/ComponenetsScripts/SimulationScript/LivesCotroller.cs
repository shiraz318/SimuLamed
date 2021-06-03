using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;


public class LivesCotroller : MonoBehaviour
{
    public Image live3;
    public Image live2;
    public Image live1; 
    public Image live0;

    private SimulationVM viewModel;

    void Start()
    {
        viewModel = GameObject.Find("View").GetComponent<SimulationVM>();
        viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Lives"))
            {
                ChangeLives(viewModel.Lives);
            }
        };
    }

    // Change the number of lives on the screen.
    private void ChangeLives(int lives)
    {
        if (lives < 0)
        {
            return;
        }
        // Disable the number of lives needed.s
        switch (lives)
        {
            case 3:
                live3.gameObject.SetActive(false);
                break;
            case 2:
                live2.gameObject.SetActive(false);
                break;
            case 1:
                live1.gameObject.SetActive(false);
                break;
            case 0:
                live0.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

 
}
