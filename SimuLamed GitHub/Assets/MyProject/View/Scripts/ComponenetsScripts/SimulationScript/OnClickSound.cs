using Assets;
using Assets.ViewModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickSound : MonoBehaviour
{
    [SerializeField]
    private Sprite soundImage;
    [SerializeField]
    private Sprite muteImage;
    [SerializeField]
    private Image voliumImage;
    
    private void Start()
    {
        voliumImage.sprite = PlayerPrefs.GetInt(SettingsVM.MUTE_SOUND) == 0 ? soundImage : muteImage;
    }

    // On click the sound icon in the simulation event handler.
    public void MyOnClickSound()
    {
        bool isMuteNow;
        // This click causes mute.
        if (PlayerPrefs.GetInt(SettingsVM.MUTE_SOUND) == 0)
        {
            isMuteNow = true;
            PlayerPrefs.SetInt(SettingsVM.MUTE_SOUND, 1);
            voliumImage.sprite = muteImage;
        }
        // This click causes turn on sound.
        else
        {
            isMuteNow = false;
            PlayerPrefs.SetInt(SettingsVM.MUTE_SOUND, 0);
            voliumImage.sprite = soundImage;
        }
        SettingsVM.toMuteSound = isMuteNow;
        SoundManager.muteCar = isMuteNow;
    }

}
