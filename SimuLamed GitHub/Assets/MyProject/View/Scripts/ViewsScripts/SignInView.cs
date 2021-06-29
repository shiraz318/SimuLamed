using Assets;
using Assets.ViewModel;
using UnityEngine;
using UnityEngine.Rendering;

public class SignInView : RegisterView
{

    public void Awake()
    {
        // Reset player prefab.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString(SettingsVM.SHOW_QUESTIONS, SettingsVM.DEFAULT_TO_SHOW_QUESTIONS == true ? SettingsVM.SHOW : string.Empty);
    }

    // On click quit button.
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
