using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float throttle;
    public float steer;

    // Update is called once per frame
    //void Update()
    //{
    //    // char character :קלט
    //    // int alphaValue = character;
    //    // (KeyCode)Enum.Parse(typeof(KeyCode), alphaValue.ToString())
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        steer = Input.GetAxis("Horizontal");
    //        Debug.Log("Horizontal: " + steer);

    //    }

    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        steer = Input.GetAxis("Horizontal");
    //        Debug.Log("Horizontal: " + steer);

    //    }

    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        throttle = Input.GetAxis("Vertical");
    //        Debug.Log("Vertical: " + throttle);

    //    }

    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        throttle = Input.GetAxis("Vertical");
    //        Debug.Log("Vertical: " + throttle);

    //    }
        
        
           
    //}

    private void FixedUpdate()
    {
        float h = 0f;
        float v = 0f;
        Vector2 smoothedInput;

        if (Input.GetKey(KeyCode.W))
        {
            v = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            v = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            h = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            h = 1f;
        }

        smoothedInput = SmoothInput(h, v);

        float smoothedH = smoothedInput.x;
        float smoothedV = smoothedInput.y;

    }


    private Vector2 SmoothInput(float targetH, float targetV)
    {
        float sensitivity = 3f;
        float deadZone = 0.001f;

        steer = Mathf.MoveTowards(steer,
                      targetH, sensitivity * Time.deltaTime);

        throttle = Mathf.MoveTowards(throttle,
                      targetV, sensitivity * Time.deltaTime);

        return new Vector2(
               (Mathf.Abs(steer) < deadZone) ? 0f : steer,
               (Mathf.Abs(throttle) < deadZone) ? 0f : throttle);
    }
}
