using Assets;
using Assets.MyProject.View.Scripts;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class LearnFromQuestionsView : BaseView
{
    private LearnFromQVM viewModel;

    private new void Start()
    {
        base.Start();
        viewModel = GameObject.Find(GameObjectNames.VIEW).GetComponent<LearnFromQVM>();
        viewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }
            
            // If the view model finished it's main action - go to the next scene.
            if (eventArgs.PropertyName.Equals(nameof(viewModel.IsUserSaved)))
            {
                GoToOtherScene(ScenesNames.MENU_SCENE);
            }
        };
    }

}
