using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCrossLane : MonoBehaviour
{
    private AlertDisplayer alertDisplayer;

    // Start is called before the first frame update
    void Start()
    {
        alertDisplayer = GameObject.Find("Alert").GetComponent<AlertDisplayer>();

    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("TRIGGER");
        if (other.gameObject.CompareTag("Player"))
        {
            alertDisplayer.DisplayAlert("מעבר נתיב בכביש דו סטרי!");
        }


    }
}
