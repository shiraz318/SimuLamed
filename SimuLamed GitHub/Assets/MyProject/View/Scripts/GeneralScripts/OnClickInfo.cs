using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickInfo : MonoBehaviour
{
    public GameObject infoScreen;

    // On click the information button event handler.
    public void OnClickInfoButton()
    {
        if (infoScreen.activeSelf)
        {
            infoScreen.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            infoScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

}
