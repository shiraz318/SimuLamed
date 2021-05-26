using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject firstCamera;
    public GameObject secondCamera;
    // Start is called before the first frame update
    void Start()
    {
        firstCamera.SetActive(true);
        secondCamera.SetActive(false);
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if(Input.GetKey(KeyCode.V))
    //    {
    //        if(firstCamera.activeSelf)
    //        {
    //            firstCamera.SetActive(false);
    //            secondCamera.SetActive(true);
    //        } else
    //        {
    //            firstCamera.SetActive(true);
    //            secondCamera.SetActive(false);
    //        }
    //    }
    //}

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
