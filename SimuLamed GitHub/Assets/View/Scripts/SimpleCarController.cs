using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;

    public WheelCollider frontLeftWheel, frontRightWheel;
    public WheelCollider backLeftWheel, backRightWheel;

    public Transform frontLeftTransform, frontRightTransform;
    public Transform backLeftTransform, backRightTransform;

    public float maxSteerAngle = 30f;
    public float motorForce = 70f;

    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        frontLeftWheel.steerAngle = steeringAngle;
        frontRightWheel.steerAngle = steeringAngle;
    }

    private void Accelerate()
    {
        frontLeftWheel.motorTorque = motorForce * verticalInput;
        frontRightWheel.motorTorque = motorForce * verticalInput;
        backRightWheel.motorTorque = motorForce * verticalInput;
        backLeftWheel.motorTorque = motorForce * verticalInput;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(backLeftWheel, backLeftTransform);
        UpdateWheelPose(backRightWheel, backRightTransform);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 position = transform.position;
        Quaternion quaternion = transform.rotation;

        collider.GetWorldPose(out position, out quaternion);
        
        transform.position = position;
        transform.rotation = quaternion;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }
}
