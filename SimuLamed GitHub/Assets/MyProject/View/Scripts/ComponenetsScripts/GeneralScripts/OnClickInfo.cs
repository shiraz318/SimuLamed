using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickInfo : MonoBehaviour
{
    public GameObject infoScreen;

    // On click the information button event handler.
    public void OnClickInfoButton()
    {
        bool isCurrentlyActicated = infoScreen.activeSelf;
       
        // If info screen is currently activate - deactivate it, else - activate it.
        SetInfoScreen(!isCurrentlyActicated);
    }

    // Set the info screen activation.
    private void SetInfoScreen(bool toActivate)
    {
        infoScreen.SetActive(toActivate);
    }

}
