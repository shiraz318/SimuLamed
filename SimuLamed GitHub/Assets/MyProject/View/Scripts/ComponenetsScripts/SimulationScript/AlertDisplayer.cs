using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertDisplayer : MonoBehaviour
{
    public GameObject alert;
    public TMP_Text alertText;
    
    private static SoundManager soundManager;

    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Display an alert with a given message.
    public void DisplayAlert(string message)
    {
        soundManager.DisplayAlert();
        alertText.text = message;
        StartCoroutine("Show");
    }

    // Pop the alert for several seconds.
    IEnumerator Show()
    {
        alert.SetActive(true);
        yield return new WaitForSeconds(3f);
        alert.SetActive(false);
    }

}
