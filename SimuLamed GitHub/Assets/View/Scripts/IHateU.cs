using Assets;
using Assets.model;
using Assets.ViewModel;
using System;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class IHateU : MonoBehaviour, INotifyPropertyChanged
{
    //public void GetInput()
    //{
    //	m_horizontalInput = Input.GetAxis("Horizontal");
    //	m_verticalInput = Input.GetAxis("Vertical");
    //}

    //private void Steer()
    //{
    //	m_steeringAngle = maxSteerAngle * m_horizontalInput;
    //	frontDriverW.steerAngle = m_steeringAngle;
    //	frontPassengerW.steerAngle = m_steeringAngle;
    //}

    //private void Accelerate()
    //{
    //	frontDriverW.motorTorque = m_verticalInput * motorForce;
    //	frontPassengerW.motorTorque = m_verticalInput * motorForce;
    //}

    //private void UpdateWheelPoses()
    //{
    //	UpdateWheelPose(frontDriverW, frontDriverT);
    //	UpdateWheelPose(frontPassengerW, frontPassengerT);
    //	UpdateWheelPose(rearDriverW, rearDriverT);
    //	UpdateWheelPose(rearPassengerW, rearPassengerT);
    //}

    //private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    //{
    //	Vector3 _pos = _transform.position;
    //	Quaternion _quat = _transform.rotation;

    //	_collider.GetWorldPose(out _pos, out _quat);

    //	_transform.position = _pos;
    //	_transform.rotation = _quat;
    //}

    //private void FixedUpdate()
    //{
    //	GetInput();
    //	Steer();
    //	Accelerate();
    //	UpdateWheelPoses();
    //}

    //private float m_horizontalInput;
    //private float m_verticalInput;
    //private float m_steeringAngle;

    //public WheelCollider frontDriverW, frontPassengerW;
    //public WheelCollider rearDriverW, rearPassengerW;
    //public Transform frontDriverT, frontPassengerT;
    //public Transform rearDriverT, rearPassengerT;
    //public float maxSteerAngle = 30;
    //public float motorForce = 1000;
    public event PropertyChangedEventHandler PropertyChanged;

    public void GetInput()
    {
        // SettingsVM sn =SettingsVM.instance;
        //SettingsVM[] sn1 = gameObject.GetComponentsInChildren<SettingsVM>();
       
        //string f = gameObject.GetComponent<SettingsVM>().Forward;
        

        float h = 0f;
        float v = 0f;
        Vector2 smoothedInput;

        string key = "";
        
        key = PlayerPrefs.GetString(Utils.FORWARD).Equals("")? FromStringToASCII(Utils.DEFAULT_FORWARD): PlayerPrefs.GetString(Utils.FORWARD);
        //if (key.Equals("")) 
        //{ 

        //    key = FromStringToASCII(Utils.DEFAULT_FORWARD);
        //}
        if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), key)))
        {
            v = 1f;
        }


        key = PlayerPrefs.GetString(Utils.BACKWARDS).Equals("") ? FromStringToASCII(Utils.DEFAULT_BACKWARDS) : PlayerPrefs.GetString(Utils.BACKWARDS);

        if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), key)))
        {
            v = -1f;
        }

        key = PlayerPrefs.GetString(Utils.LEFT).Equals("") ? FromStringToASCII(Utils.DEFAULT_LEFT) : PlayerPrefs.GetString(Utils.LEFT);
        
        if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), key)))
        {
            h = -1f;
        }

        key = PlayerPrefs.GetString(Utils.RIGHT).Equals("") ? FromStringToASCII(Utils.DEFAULT_RIGHT) : PlayerPrefs.GetString(Utils.RIGHT);
        if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), key)))
        {
            h = 1f;
        }

        smoothedInput = SmoothInput(h, v);

        //m_horizontalInput = Input.GetAxis("Horizontal");
        // m_verticalInput = Input.GetAxis("Vertical");
    }

    private Vector2 SmoothInput(float targetH, float targetV)
    {
        float sensitivity = 3f;
        float deadZone = 0.001f;

        m_horizontalInput = Mathf.MoveTowards(m_horizontalInput,
                      targetH, sensitivity * Time.deltaTime);

        m_verticalInput = Mathf.MoveTowards(m_verticalInput,
                      targetV, sensitivity * Time.deltaTime);

        return new Vector2(
               (Mathf.Abs(m_horizontalInput) < deadZone) ? 0f : m_horizontalInput,
               (Mathf.Abs(m_verticalInput) < deadZone) ? 0f : m_verticalInput);
    }


    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
        float drive = Mathf.PI * frontDriverW.radius * frontDriverW.rpm / 100;
        if (drive < 0)
        {
            drive *= -1;
        }
        Speed = Round(drive, 0).ToString();
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
       
        

    }

    //On property changed.
    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

    private string speed = "0";
    [Binding]
    public string Speed
    {
        get { return speed; }
        set { speed = value;
            NotifyPropertyChanged("Speed"); }
    }


    private string FromStringToASCII(string str)
    {
        char character = str.ToLower().ToCharArray()[0];
        int ascii = System.Convert.ToInt32(character);
        return ascii.ToString();
    }

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle = 30;
    public float motorForce = 1000;
}