using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickSound : MonoBehaviour
{
    public Sprite soundImage;
    public Sprite muteImage;
    public Image voliumImage;
    private void Start()
    {
        voliumImage.sprite = PlayerPrefs.GetInt(Utils.MUTE_SOUND) == 0 ? soundImage : muteImage;
    }
    public void MyOnClickSound()
    {
        // This click causes mute.
        if (PlayerPrefs.GetInt(Utils.MUTE_SOUND) == 0)
        {
            PlayerPrefs.SetInt(Utils.MUTE_SOUND, 1);
            voliumImage.sprite = muteImage;
        }// This click causes turn on sound.
        else
        {
            PlayerPrefs.SetInt(Utils.MUTE_SOUND, 0);
            voliumImage.sprite = soundImage;
        }
    }

}
