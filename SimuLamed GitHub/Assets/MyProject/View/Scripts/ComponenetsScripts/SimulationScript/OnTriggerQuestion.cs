using Assets;
using Assets.MyProject.View.Scripts;
using Assets.ViewModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnTriggerQuestion : MonoBehaviour
{
    private static ScreensManager screensManager;
    private BoxCollider boxCollider;

    [SerializeField]
    private GameObject questionMark;
    [SerializeField]
    private int questionNumber;

    private void Awake()
    {
        screensManager = GameObject.Find(GameObjectNames.SCREENS).GetComponent<ScreensManager>();
        boxCollider = GetComponent<BoxCollider>();
        string toShowQuestions = PlayerPrefs.GetString(SettingsVM.SHOW_QUESTIONS);
       
        // Show question mark if the user did not disable this option.
        questionMark.SetActive(toShowQuestions.Equals(string.Empty)? false:true);
    }

    // On trigger enter event handler.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameObjectNames.PLAYER_TAG))
        {
            // Ignore collision with the player.
            Physics.IgnoreCollision(GameObject.Find(GameObjectNames.CAR_TAG).GetComponent<BoxCollider>(), boxCollider);
          
            // The next time the player collide with this box collider, he will not trigger it.
            boxCollider.isTrigger = false;
            questionMark.SetActive(false);
            screensManager.DisplayQuestion(questionNumber);
        }


    }

}
