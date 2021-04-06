using Assets;
using UnityEngine;

public class OnClickSignUpScript : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    // On click event handler for clicking on sign up button.
    public void OnClickSignUp()
    {
        SignUpVM viewModel = GameObject.Find("View").GetComponent<SignUpVM>();
        viewModel.SignUp(() => sceneLoader.LoadNextScene(Utils.SIGN_IN_SCENE));
    }


}
