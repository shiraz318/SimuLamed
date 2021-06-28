using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWrongDirCollision : BaseAlertTrigger
{
    private const string ALERT_MESSAGE = "נסיעה בניגוד לכיוון התנועה!";

    protected override void OnTriggerPlayer()
    {
        if (otherCollider == null) { return; }

        Vector3 toTarget = (transform.position - otherCollider.transform.position).normalized;

        if (Vector3.Dot(toTarget, otherCollider.transform.forward) >= 0)
        {
            base.OnTriggerPlayer();
        }

    }

    protected override string GetAlertMessage()
    {
        return ALERT_MESSAGE;
    }
}
