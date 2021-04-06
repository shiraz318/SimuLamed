using Assets;
using UnityEngine;

public class OnClickForgotPwdScript : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    // On click Reset password button helper.
    public void OnClickResetPassword()
    {
        ForgotPasswordVM viewModel = GameObject.Find("View").GetComponent<ForgotPasswordVM>();
        viewModel.ResetPassword(() => sceneLoader.LoadNextScene(Utils.SIGN_IN_SCENE));
    }


}
