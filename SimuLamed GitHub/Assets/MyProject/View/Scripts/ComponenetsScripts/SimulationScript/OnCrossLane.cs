using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCrossLane : MonoBehaviour
{
    private AlertDisplayer alertDisplayer;
    private const string ALERT_MESSAGE = "מעבר נתיב בכביש דו סטרי!";


    // Start is called before the first frame update
    void Start()
    {
        alertDisplayer = GameObject.Find("Alert").GetComponent<AlertDisplayer>();
    }

    // On trigger enter event handler.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            alertDisplayer.DisplayAlert(ALERT_MESSAGE);
        }
    }
}
