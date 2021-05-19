using Assets;
using Assets.ViewModel;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.View.Scripts.SettingsScripts
{
    public class SettingsView : BaseView
    {
        //private SceneLoader sceneLoader;
        private SettingsVM viewModel;

        private void Awake()
        {
            
           // sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
            if (viewModel == null)
            {
                viewModel = GameObject.Find("View").GetComponent<SettingsVM>();
            }
        }

        

        // On click event handler for clicking the back button.
        public void OnClickBack()
        {
            viewModel.Back(() => GoToOtherScene(Utils.MENU_SCENE));
        }
    }

}
