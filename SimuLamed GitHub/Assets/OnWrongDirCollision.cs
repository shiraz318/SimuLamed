﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWrongDirCollision : MonoBehaviour
{
    private AlertDisplayer alertDisplayer;
    private const string ALERT_MESSAGE = "נסיעה בניגוד לכיוון התנועה!";


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
            Vector3 toTarget = (transform.position - other.transform.position).normalized;
            
            if (Vector3.Dot(toTarget, other.transform.forward) < 0)
            {
                Debug.Log("from front");
            }
            else
            {
                Debug.Log("from back");
                alertDisplayer.DisplayAlert(ALERT_MESSAGE);
            }
        }
    }
}
