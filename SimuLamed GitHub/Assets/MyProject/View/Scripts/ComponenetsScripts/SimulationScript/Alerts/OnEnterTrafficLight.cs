using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterTrafficLight : BaseAlertTrigger
{
    [SerializeField]
    private LightAnimator lightAnimator;
    private const string ALERT_MESSAGE = "חציית צומת מרומזר באור שאינו ירוק!";

    protected override void OnTriggerPlayer()
    {
        if (lightAnimator.currentLight.Equals(Lights.Red) ||
                lightAnimator.currentLight.Equals(Lights.RedAndYellow) ||
                lightAnimator.currentLight.Equals(Lights.Yellow))
        {
            base.OnTriggerPlayer();
        }
    }

    protected override string GetAlertMessage()
    {
        return ALERT_MESSAGE;
    }
}
