using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickInfo : MonoBehaviour
{
    public GameObject infoScreen;
    void Start()
    {
        //infoScreen.SetActive(false);
    }
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
