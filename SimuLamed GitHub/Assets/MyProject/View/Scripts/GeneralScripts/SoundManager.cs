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
    public AudioSource hintClicked;
    public AudioSource passLevel;
    public AudioSource failLevel;
    public AudioSource pauseSimulation;
    public AudioSource quitSimulation;
    public AudioSource displayQuestionInSimulation;
  //  public AudioSource carEngine;


    public static bool muteCar;
    public bool IsMute { get { return PlayerPrefs.GetInt(Utils.MUTE_SOUND) != 0; } }

    // On click regular button event handler.
    public void OnClickButton()
    {
        PlaySound(buttonClicked);
    }
    // On click correct answer event handler.
    public void OnClickCorrectAns()
    {
        PlaySound(correctAnswerClicked);
    }
    // On click wrong answer event handler.
    public void OnClickWrongAns()
    {
        PlaySound(wrongAnswerClicked);
    }
    // Called when the user got a new hint.
    public void GotNewHint()
    {
        PlaySound(gotNewHint);
    }
    // On click the hint button event handler.
    public void OnClickHint()
    {
        PlaySound(hintClicked);
    }
    // Called when the user passed a level.
    public void PassLevel()
    {
        PlaySound(passLevel);
    }
    // Called when the user failed the level.
    public void FailLevel()
    {
        PlaySound(failLevel);
    }
    // Called when a question is being displayed in the simulation.
    public void DisplayQuestion()
    {
        PlaySound(displayQuestionInSimulation);
    }
    // Called when the user wants to quit the simulation.
    public void QuitSimulation()
    {
        PlaySound(quitSimulation);
    }
    // Called when the user wants to pause the simulation.
    public void PauseSimulation()
    {
        PlaySound(pauseSimulation);
    }

    //public void UpdateEngineSound(float currentSpeed, float topSpeed)
    //{
    //    carEngine.pitch = currentSpeed / topSpeed;
    //    if ()
    //    PlaySound(carEngine);
    //}
    // Play the given sound if the user did not mute sounds.
    private void PlaySound(AudioSource sound)
    {
        if (PlayerPrefs.GetInt(Utils.MUTE_SOUND) == 0)
        {
            sound.Play();
        }
    }


}
