using Assets;
using Assets.model;
using Assets.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class MyCarController : MonoBehaviour, INotifyPropertyChanged
{
    // Start position variables.
    [SerializeField]
    private List<Transform> optionalStartPositions; // a list of optional start positions for the car.
    private Vector3 carStartPosition;
    private Vector3 carStartRotation;

    // Sound related variables.
    private float pitch;
    private bool muteCar;
    private const float topSpeed = 180f;

    // Inputs variables.
    private float horizontalInput;
    private float verticalInput;

    // Physics of the car.
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private Transform carT;
    [SerializeField]
    private WheelCollider frontDriverW, frontPassengerW, rearDriverW, rearPassengerW;
    [SerializeField]
    private Transform frontDriverT, frontPassengerT, rearDriverT, rearPassengerT;

    [SerializeField]
    private float maxSteerAngle = 30;
    [SerializeField]
    private float motorForce = 1000;
    
    private float steeringAngle;
    private float drive;
    private string speed = "0";

    



    // Properties.
    [Binding]
    public float Pitch { get { return pitch; } set { pitch = value; NotifyPropertyChanged(); } }
    [Binding]
    public bool MuteCar { get { return muteCar; } set { muteCar = value; NotifyPropertyChanged(); } }

    [Binding]
    public Vector3 CarStartPosition { get { return carStartPosition; } set { carStartPosition = value; NotifyPropertyChanged(); } }
    [Binding]
    public Vector3 CarStartRotation { get { return carStartRotation; } set { carStartRotation = value; NotifyPropertyChanged(); } }

    [Binding]
    public string Speed
    {
        get { return speed; }
        set {  speed = value;  NotifyPropertyChanged(); }
    }
    public event PropertyChangedEventHandler PropertyChanged;


    private void Start()
    {
        // Set the car start position.
        System.Random rd = new System.Random();
        int randIndex = rd.Next(0, optionalStartPositions.Count - 1);

        CarStartPosition = optionalStartPositions[randIndex].position;
        CarStartRotation = optionalStartPositions[randIndex].eulerAngles;
        
    }
    

    // Get the current pressed key.
    public void GetInput()
    {
        float h = 0f;
        float v = 0f;
        Vector2 smoothedInput;

        string key = "";

        key = GetKey(Utils.FORWARD, Utils.DEFAULT_FORWARD);
      
        if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), key)))
        {
            v = 1f;
        }


        key = GetKey(Utils.BACKWARDS, Utils.DEFAULT_BACKWARDS);

        if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), key)))
        {
            v = -1f;
        }

        key = GetKey(Utils.LEFT, Utils.DEFAULT_LEFT);
        
        if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), key)))
        {
            h = -1f;
        }

        key = GetKey(Utils.RIGHT, Utils.DEFAULT_RIGHT);
        if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), key)))
        {
            h = 1f;
        }

        smoothedInput = SmoothInput(h, v);

      
    }

    // Get the key accordingly to the key.
    private string GetKey(string keyName, string keyDefault)
    {
        return PlayerPrefs.GetString(keyName).Equals("") ? FromStringToASCII(keyDefault) : PlayerPrefs.GetString(keyName);
    }

    // Smooth the novement of the car.
    private Vector2 SmoothInput(float targetH, float targetV)
    {
        float sensitivity = 3f;
        float deadZone = 0.001f;

        horizontalInput = Mathf.MoveTowards(horizontalInput,
                      targetH, sensitivity * Time.unscaledDeltaTime);

        verticalInput = Mathf.MoveTowards(verticalInput,
                      targetV, sensitivity * Time.unscaledDeltaTime);

        return new Vector2(
               (Mathf.Abs(horizontalInput) < deadZone) ? 0f : horizontalInput,
               (Mathf.Abs(verticalInput) < deadZone) ? 0f : verticalInput);
    }


    // Set the front wheels.
    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        frontDriverW.steerAngle = steeringAngle;
        frontPassengerW.steerAngle = steeringAngle;
    }

    // Round the given number with the givem number of digits.
    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    // Calculate the speed.
    private void Accelerate()
    {
        frontDriverW.motorTorque = verticalInput * motorForce;
        frontPassengerW.motorTorque = verticalInput * motorForce;
        
        // Calculate speed in km/h.
        drive = rigidBody.velocity.magnitude * 3.6f;
        Speed = Round(drive, 0).ToString();
        UpdateEngineSound(drive, topSpeed);
    }

    // Updates the engine sound according to the given speed.
    private void UpdateEngineSound(float currentSpeed, float topSpeed)
    {
        Pitch = currentSpeed / topSpeed;
        MuteCar = SoundManager.muteCar;
    }

    // Set the position of the wheels.
    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    // Set the position of the given wheel.
    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;

        collider.GetWorldPose(out pos, out quat);

        transform.position = pos;
        transform.rotation = quat;
    }

    // Check if the car is flipped.
    private void IsFlip()
    {
        if (Vector3.Dot(carT.up, Vector3.down) > 0.8)
        {
            carT.transform.Rotate(180f, 0.0f, 0.0f, Space.Self);
        }
         else if (carT.up.z >  0.8)
        {
            carT.transform.Rotate(0.0f, 0.0f, 90.0f,  Space.Self);
        }
    }


    private void Update()
    {
        // Get the current pressed key.
        GetInput();
        // Set the streer.
        Steer();
        // Set the speed.
        Accelerate();
        // Set the position of the wheels.
        UpdateWheelPoses();
        // Check if the car flipped.
        IsFlip();
        

    }

    // Convert the given string to ASCII.
    private string FromStringToASCII(string str)
    {
        char character = str.ToLower().ToCharArray()[0];
        int ascii = System.Convert.ToInt32(character);
        return ascii.ToString();
    }

    //On property changed.
    public void NotifyPropertyChanged([CallerMemberName] string propertyname = null)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }



}