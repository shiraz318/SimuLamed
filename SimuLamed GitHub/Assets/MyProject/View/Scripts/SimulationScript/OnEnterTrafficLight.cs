using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterTrafficLight : MonoBehaviour
{
    public LightAnimator lightAnimator;
    private BoxCollider boxCollider;
    private AlertDisplayer alertDisplayer;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
       // lightAnimator = GameObject.Find("Lights").GetComponent<LightAnimator>();
        alertDisplayer = GameObject.Find("Alert").GetComponent<AlertDisplayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("TRIGGER");
        if (other.gameObject.CompareTag("Player"))
        {
            // Ignore collision with the player.
           // Physics.IgnoreCollision(GameObject.Find("Car").GetComponent<BoxCollider>(), boxCollider);
            //Debug.Log(lightAnimator.litColor);
            if (lightAnimator.currentLight.Equals(Lights.Red) || lightAnimator.currentLight.Equals(Lights.RedAndYellow) || lightAnimator.currentLight.Equals(Lights.Yellow))
            {
                alertDisplayer.DisplayAlert("חציית צומת מרומזר באור שאינו ירוק!");
               // Debug.Log("RED LIGHT!!!!!");
            }

        }


    }
}
