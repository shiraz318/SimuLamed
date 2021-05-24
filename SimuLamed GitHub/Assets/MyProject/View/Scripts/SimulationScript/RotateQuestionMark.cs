using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateQuestionMark : MonoBehaviour
{
    private float speed = 100f;

    // Update is called once per frame and rotate the question mark.
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
             
    }
}
