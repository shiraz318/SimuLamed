using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCrossLane : BaseAlertTrigger
{
    private const string ALERT_MESSAGE = "מעבר נתיב בכביש דו סטרי!";

    protected override string GetAlertMessage()
    {
        return ALERT_MESSAGE;
    }
}
