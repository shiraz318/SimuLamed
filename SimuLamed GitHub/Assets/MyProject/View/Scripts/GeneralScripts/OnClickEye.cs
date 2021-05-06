using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickEye : MonoBehaviour
{
    public InputField password;
    public Sprite showImage;
    public Sprite hideImage;
    public Image eyeImage;

    public void MyOnClickEye()
    {
        if (password.contentType == InputField.ContentType.Password)
        {
            password.contentType = InputField.ContentType.Standard;
            eyeImage.sprite = showImage;
        }
        else
        {
            password.contentType = InputField.ContentType.Password;
            eyeImage.sprite = hideImage;

        }
        this.password.ForceLabelUpdate();
    }
   
}
