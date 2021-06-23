using Assets;
using Assets.model;
using Assets.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class MyCarController : MonoBehaviour, INotifyPropertyChanged
{
    // Start position variables.
    public List<Transform> optionalStartPositions; // a list of optional start positions for the car.
    private Vector3 carStartPosition;
    private Vector3 carStartRotation;

    // Sound related variables.
    private float pitch;
    private bool muteCar;
    private const float topSpeed = 180f;

    // Inputs variables.
    private float m_horizontalInput;
    private float m_verticalInput;

    // Physics of the car.
    public Rigidbody rigidBody;
    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;

    public float maxSteerAngle = 30;
    private float m_steeringAngle;
    public float motorForce = 1000;
    private float drive;
    private string speed = "0";



    // Properties.
    [Binding]
    public float Pitch { get { return pitch; } set { pitch = value; NotifyPropertyChanged("Pitch"); } }
    [Binding]
    public bool MuteCar { get { return muteCar; } set { muteCar = value; NotifyPropertyChanged("MuteCar"); } }

    [Binding]
    public Vector3 CarStartPosition { get { return carStartPosition; } set { carStartPosition = value; NotifyPropertyChanged("CarStartPosition"); } }
    [Binding]
    public Vector3 CarStartRotation { get { return carStartRotation; } set { carStartRotation = value; NotifyPropertyChanged("CarStartRotation"); } }
    [Binding]
    public string Speed
    {
        get { return speed; }
        set
        {
            speed = value;
            NotifyPropertyChanged("Speed");
        }
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

    private string GetKey(string keyName, string keyDefault)
    {
        return PlayerPrefs.GetString(keyName).Equals("") ? FromStringToASCII(keyDefault) : PlayerPrefs.GetString(keyName);
    }

    // Smooth the novement of the car.
    private Vector2 SmoothInput(float targetH, float targetV)
    {
        float sensitivity = 3f;
        float deadZone = 0.001f;

        m_horizontalInput = Mathf.MoveTowards(m_horizontalInput,
                      targetH, sensitivity * Time.unscaledDeltaTime);

        m_verticalInput = Mathf.MoveTowards(m_verticalInput,
                      targetV, sensitivity * Time.unscaledDeltaTime);

        return new Vector2(
               (Mathf.Abs(m_horizontalInput) < deadZone) ? 0f : m_horizontalInput,
               (Mathf.Abs(m_verticalInput) < deadZone) ? 0f : m_verticalInput);
    }


    // Set the front wheels.
    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;
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
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
        // calculate speed in km/h
        drive = rigidBody.velocity.magnitude * 3.6f;
        Speed = Round(drive, 0).ToString();
        UpdateEngineSound(drive, topSpeed);
    }

    // Updates the engine sound according to the given speed.
    private void UpdateEngineSound(float currentSpeed, float topSpeed)
    {
        Pitch = currentSpeed / topSpeed;
        MuteCar = SoundManager.muteCar;
         //GetComponent<AudioSource>().pitch = Pitch;
        //PlaySound(carEngine);
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
    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
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
    }
     
    // Convert the given string to ASCII.
    private string FromStringToASCII(string str)
    {
        char character = str.ToLower().ToCharArray()[0];
        int ascii = System.Convert.ToInt32(character);
        return ascii.ToString();
    }

    //On property changed.
    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }



}