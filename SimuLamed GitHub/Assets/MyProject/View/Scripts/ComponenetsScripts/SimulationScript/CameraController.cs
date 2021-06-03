using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject firstCamera;
    public GameObject secondCamera;

    void Start()
    {
        firstCamera.SetActive(true);
        secondCamera.SetActive(false);
    }

    
    // Switch between the two cameras.
    public void OnClickSwitch()
    {
        if (firstCamera.activeSelf)
        {
            firstCamera.SetActive(false);
            secondCamera.SetActive(true);
        }
        else
        {
            firstCamera.SetActive(true);
            secondCamera.SetActive(false);
        }
    }
}
