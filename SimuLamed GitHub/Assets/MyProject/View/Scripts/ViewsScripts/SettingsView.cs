using Assets;
using Assets.ViewModel;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.View.Scripts.SettingsScripts
{
    public class SettingsView : BaseView
    {
        private SettingsVM viewModel;

        private void Awake()
        {
            viewModel = GameObject.Find(Utils.VIEW).GetComponent<SettingsVM>();
            viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
            {
                if (this == null) { return; }

                // If the view model finished it's main action - go to the next scene.
                if (eventArgs.PropertyName.Equals(viewModel.GetOnFinishActionPropertyName()))
                {
                    GoToOtherScene(Utils.MENU_SCENE);
                }
            };

        }
    }

}
