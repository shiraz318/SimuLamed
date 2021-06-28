using Assets;
using UnityEngine;
using UnityEngine.Rendering;

public class SignInView : RegisterView
{

    public void Awake()
    {
        // Reset player prefab.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString(Utils.SHOW_QUESTIONS, Utils.DEFAULT_TO_SHOW_QUESTIONS == true ? "show" : string.Empty);
    }

    // On click quit button.
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
