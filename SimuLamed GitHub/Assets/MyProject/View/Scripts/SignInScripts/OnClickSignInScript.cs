using Assets;
using UnityEngine;

public class OnClickSignInScript : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    // On click forgot password button.
    public void OnClickForgotPassword()
    {
        sceneLoader.LoadNextScene(Utils.FORGOT_PASSWORD_SCENE);
    }

    // On click new user button.
    public void OnClickNewUser()
    {
        sceneLoader.LoadNextScene(Utils.SIGN_UP_SCENE);
    }

    // On click sign in button.
    public void OnClickSignIn()
    {
        SignInVM viewModel = GameObject.Find("View").GetComponent<SignInVM>();
        viewModel.SignIn(() => sceneLoader.LoadNextScene(Utils.MENU_SCENE));
    }

    // On click quit button.
    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
