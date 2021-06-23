using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const int BUTTON_CLICKED_SOUND = 0;
    private const int WRONG_ANS_CLICKED_SOUND = 1;
    private const int CORRECT_ANS_CLICKED_SOUND = 2;
    private const int GET_NEW_HINT_SOUND = 3;
    private const int HINT_CLICKED_SOUND = 4;
    private const int PASS_LEVEL_SOUND = 5;
    private const int FAIL_LEVEL_SOUND = 6;
    private const int PAUSE_SIM_SOUND = 7;
    private const int QUIT_SIM_SOUND = 8;
    private const int DISPLAY_QUESTION_IN_SIM_SOUND = 9;
    private const int ALERT_SOUND = 10;

    //public AudioSource buttonClicked;
    //public AudioSource wrongAnswerClicked;
    //public AudioSource correctAnswerClicked;
    //public AudioSource gotNewHint;
    //public AudioSource hintClicked;
    //public AudioSource passLevel;
    //public AudioSource failLevel;
    //public AudioSource pauseSimulation;
    //public AudioSource quitSimulation;
    //public AudioSource displayQuestionInSimulation;
    //public AudioSource alert;

    public AudioSource[] sounds;


    public static bool muteCar;
    public bool IsMute { get { return PlayerPrefs.GetInt(Utils.MUTE_SOUND) != 0; } }

    // On click regular button event handler.
    public void OnClickButton()
    {
        PlaySound(sounds[BUTTON_CLICKED_SOUND]);
        //PlaySound(buttonClicked);
    }
   
    // On click correct answer event handler.
    public void OnClickCorrectAns()
    {
        PlaySound(sounds[CORRECT_ANS_CLICKED_SOUND]);
        //PlaySound(correctAnswerClicked);
    }
    
    // On click wrong answer event handler.
    public void OnClickWrongAns()
    {
        PlaySound(sounds[WRONG_ANS_CLICKED_SOUND]);
        //PlaySound(wrongAnswerClicked);
    }
    
    // Called when the user got a new hint.
    public void GotNewHint()
    {
        PlaySound(sounds[GET_NEW_HINT_SOUND]);
        //PlaySound(gotNewHint);
    }
    
    // On click the hint button event handler.
    public void OnClickHint()
    {
        PlaySound(sounds[HINT_CLICKED_SOUND]);
        //PlaySound(hintClicked);
    }
    
    // Called when the user passed a level.
    public void PassLevel()
    {
        PlaySound(sounds[PASS_LEVEL_SOUND]);
        //PlaySound(passLevel);
    }
    
    // Called when the user failed the level.
    public void FailLevel()
    {
        PlaySound(sounds[FAIL_LEVEL_SOUND]);
        //PlaySound(failLevel);
    }
   
    // Called when a question is being displayed in the simulation.
    public void DisplayQuestion()
    {
        PlaySound(sounds[DISPLAY_QUESTION_IN_SIM_SOUND]);
        //PlaySound(displayQuestionInSimulation);
    }
    
    // Called when the user wants to quit the simulation.
    public void QuitSimulation()
    {
        PlaySound(sounds[QUIT_SIM_SOUND]);
        //PlaySound(quitSimulation);
    }
    
    // Called when the user wants to pause the simulation.
    public void PauseSimulation()
    {
        PlaySound(sounds[PAUSE_SIM_SOUND]);
        //PlaySound(pauseSimulation);
    }
    
    // Called when an alert is being displayed.
    public void DisplayAlert()
    {
        PlaySound(sounds[ALERT_SOUND]);
        //PlaySound(alert);
    }

    // Play the given sound if the user did not mute sounds.
    private void PlaySound(AudioSource sound)
    {
        if (PlayerPrefs.GetInt(Utils.MUTE_SOUND) == 0)
        {
            sound.Play();
        }
    }


}
