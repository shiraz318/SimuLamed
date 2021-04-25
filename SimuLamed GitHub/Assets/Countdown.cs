using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text countdownTextField;

    void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        Time.timeScale = 0f;
        countdownTextField.text = "3";
        yield return new WaitForSecondsRealtime(1.0f);
        countdownTextField.text = "2";
        yield return new WaitForSecondsRealtime(1.0f);
        countdownTextField.text = "1";
        yield return new WaitForSecondsRealtime(1.0f);
        countdownTextField.text = "Go!";
        Time.timeScale = 1;
        // start the game here

        yield return new WaitForSeconds(1.0f);
        
        countdownTextField.text = "";
        yield return null;
    }

}
