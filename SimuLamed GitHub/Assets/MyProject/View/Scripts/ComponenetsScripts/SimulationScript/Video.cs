using Assets;
using Assets.ViewModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Video : MonoBehaviour
{  //Raw Image to Show Video Images [Assign from the Editor]
    //public RawImage image;

    //Video To Play [Assign from the Editor]
    public VideoClip videoToPlay;
    public GameObject screenVideo;
    //public Text speedText;
    public Text speed;
    //public Button viewButton;
    //public GameObject lives;
    public GameObject defScreen;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    //Audio
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        //SetComponents(false);
        speed.gameObject.SetActive(false);
        defScreen.SetActive(false);
        StartCoroutine(playVideo());
        
    }

    //private void SetComponents(bool state)
    //{
    //    speedText.gameObject.SetActive(state);
    //    speed.gameObject.SetActive(state);
    //    viewButton.gameObject.SetActive(state);
    //    lives.SetActive(state);
    //}


    IEnumerator playVideo()
    {
        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;

        //We want to play from video clip not from url
        videoPlayer.source = VideoSource.VideoClip;

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            yield return null;
        }

        Debug.Log("Done Preparing Video");


        //Assign the Texture from Video to RawImage to be displayed
        //image.texture = videoPlayer.texture;


        if (PlayerPrefs.GetInt(SettingsVM.MUTE_SOUND) == 0)
        {
            //Set Audio Output to AudioSource
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

            //Assign the Audio from Video to AudioSource to be played
            videoPlayer.EnableAudioTrack(0, true);
            videoPlayer.SetTargetAudioSource(0, audioSource);


            //Play Sound
            audioSource.Play();
        }
        //Play Video
        videoPlayer.Play();



        yield return new WaitForSeconds(5.0f);
        screenVideo.SetActive(false);
        //SetComponents(true);
        speed.gameObject.SetActive(true);
        defScreen.SetActive(true);



    }
}
