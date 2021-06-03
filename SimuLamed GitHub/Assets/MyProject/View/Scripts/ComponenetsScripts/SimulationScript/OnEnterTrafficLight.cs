using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterTrafficLight : MonoBehaviour
{
    public LightAnimator lightAnimator;
    private AlertDisplayer alertDisplayer;
    private const string ALERT_MESSAGE = "חציית צומת מרומזר באור שאינו ירוק!";

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
            if (lightAnimator.currentLight.Equals(Lights.Red) || 
                lightAnimator.currentLight.Equals(Lights.RedAndYellow) ||
                lightAnimator.currentLight.Equals(Lights.Yellow))
            {
                alertDisplayer.DisplayAlert(ALERT_MESSAGE);
            }

        }
    }
}
