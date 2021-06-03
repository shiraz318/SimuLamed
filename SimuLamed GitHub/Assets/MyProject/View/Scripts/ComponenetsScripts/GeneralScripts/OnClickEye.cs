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

    // On click event handler for clicking the eye icon.
    public void MyOnClickEye()
    {
        // The password is hidden - need to show it and replace the eye icon.
        if (password.contentType == InputField.ContentType.Password)
        {
            password.contentType = InputField.ContentType.Standard;
            eyeImage.sprite = showImage;
        }
        // The password is shown - need to hide it and replace the eye icon.
        else
        {
            password.contentType = InputField.ContentType.Password;
            eyeImage.sprite = hideImage;

        }
        this.password.ForceLabelUpdate();
    }
   
}
