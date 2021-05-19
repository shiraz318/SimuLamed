using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterView : BaseView
{
    private RegisterViewModel viewModel;

    private void Start()
    {
        viewModel = GameObject.Find("View").GetComponent<RegisterViewModel>();
    }
    public void OnRegisterAciton(string nextSceneName)
    {
        OnClickButton();
        viewModel.OnAction(() => GoToOtherSceneNoSound(nextSceneName));
    }

}
