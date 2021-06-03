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
        SetInfoScreen(!isCurrentlyActicated, isCurrentlyActicated ? 1f : 0f);
    }

    // Set the info screen activation.
    private void SetInfoScreen(bool toActivate, float timeScale)
    {
        infoScreen.SetActive(toActivate);
        Time.timeScale = timeScale;
    }

}
