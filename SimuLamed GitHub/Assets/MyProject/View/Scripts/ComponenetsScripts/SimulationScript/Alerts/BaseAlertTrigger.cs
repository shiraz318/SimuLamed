using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAlertTrigger : MonoBehaviour
{
    private AlertDisplayer alertDisplayer;
    protected Collider otherCollider;

    // Start is called before the first frame update
    void Start()
    {
        alertDisplayer = GameObject.Find("Alert").GetComponent<AlertDisplayer>();
    }

    protected abstract string GetAlertMessage();
    
    // On trigger alert collided with the player.
    protected virtual void OnTriggerPlayer() 
    {
        alertDisplayer.DisplayAlert(GetAlertMessage());
    }

    // On trigger enter event handler.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Utils.PLAYER_TAG))
        {
            otherCollider = other;
            OnTriggerPlayer();
        }
    }
}
