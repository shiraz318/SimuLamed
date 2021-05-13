using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource buttonClicked;
    public AudioSource wrongAnswerClicked;
    public AudioSource correctAnswerClicked;
    public AudioSource gotNewHint;
    public AudioSource passLevel;
    public AudioSource failLevel;

    
    public void OnClickButton()
    {
        PlaySound(buttonClicked);
    }
    public void OnClickCorrectAns()
    {
        PlaySound(correctAnswerClicked);
    }
    public void OnClickWrongAns()
    {
        PlaySound(wrongAnswerClicked);
    }
    public void GotNewHint()
    {
        PlaySound(gotNewHint);
    }
    public void PassLevel()
    {
        PlaySound(passLevel);
    }
    public void FailLevel()
    {
        PlaySound(failLevel);
    }

    public void PlaySound(AudioSource sound)
    {
        if (PlayerPrefs.GetInt(Utils.MUTE_SOUND) == 0)
        {
            sound.Play();
        }
    }

}
