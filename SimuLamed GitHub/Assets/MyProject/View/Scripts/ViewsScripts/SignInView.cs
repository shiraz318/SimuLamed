using Assets;
using Assets.ViewModel;
using UnityEngine;
using UnityEngine.Rendering;

public class SignInView : RegisterView
{

    public void Awake()
    {
        SettingsVM.ResetSettings();
    }

    // On click quit button.
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
