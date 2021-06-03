using Assets;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RegisterView : BaseView
{
    private RegisterViewModel viewModel;
    private string nextSceneName;

    private new void Start()
    {
        base.Start();
        viewModel = GameObject.Find("View").GetComponent<RegisterViewModel>();
        viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }

            // Register action is done successfully.
            if (eventArgs.PropertyName.Equals(viewModel.GetOnFinishActionPropertyName()))
            {
                GoToOtherSceneNoSound(nextSceneName);
            }
        };
    }
    
    // Called when a register action accures. 
    public void OnRegisterAciton(string nextSceneName)
    {
        this.nextSceneName = nextSceneName;
        OnClickButton();
        viewModel.OnRegisterAction();
    }

}
