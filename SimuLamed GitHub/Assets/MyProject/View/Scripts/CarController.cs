using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class CarController : MonoBehaviour
{
    public InputManager inputManager;
    public List<WheelCollider> throttleWheels;
    public List<WheelCollider> steeringWheels;
    public float strengthCoofficient = 20000f;
    public float maxTurn = 20f;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(WheelCollider wheel in throttleWheels)
        {
            wheel.motorTorque = strengthCoofficient * Time.deltaTime * inputManager.throttle;
        }
        foreach (WheelCollider wheel in steeringWheels)
        {
            wheel.steerAngle = maxTurn * inputManager.steer;
        }
    }
}
