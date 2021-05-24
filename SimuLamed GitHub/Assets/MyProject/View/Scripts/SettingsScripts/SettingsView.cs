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
            viewModel = GameObject.Find("View").GetComponent<SettingsVM>();
            viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
            {
                if (this == null) { return; }

                if (eventArgs.PropertyName.Equals(viewModel.GetPropertyName()))
                {
                    GoToOtherScene(Utils.MENU_SCENE);
                }
            };


            //viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
            //{
            //    if (eventArgs.PropertyName.Equals(viewModel.GetPropertyName()))
            //    {
            //        GoToOtherScene(Utils.MENU_SCENE);
            //    }
            //};

            //if (viewModel == null)
            //{
            //    viewModel = GameObject.Find("View").GetComponent<SettingsVM>();
            //    viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
            //    {
            //        if (eventArgs.PropertyName.Equals(viewModel.GetPropertyName()))
            //        {
            //            GoToOtherScene(Utils.MENU_SCENE);
            //        }
            //    };
            //}
        }

        // On click event handler for clicking the back button.
       // public void OnClickBack() { viewModel.SaveSettings(); }
    }

}
