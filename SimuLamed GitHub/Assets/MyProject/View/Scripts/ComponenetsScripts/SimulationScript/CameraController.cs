using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject firstCamera;
    [SerializeField]
    private GameObject secondCamera;

    void Start()
    {
        firstCamera.SetActive(true);
        secondCamera.SetActive(false);
    }


    // Get the current active camera.
    public GameObject GetCurrentCamera()
    {
        return firstCamera.activeSelf ? firstCamera : secondCamera;
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
