using Assets;
using Assets.ViewModel;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.View.Scripts.SettingsScripts
{
    public class OnClickSettings : MonoBehaviour
    {
        private SceneLoader sceneLoader;
        private SettingsVM viewModel;

        private void Start()
        {
            
            sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
            if (viewModel == null)
            {
                viewModel = GetComponentsInParent<SettingsVM>()[0];
            }
        }

        

        // On click event handler for clicking the back button.
        public void OnClickBack()
        {
            viewModel.Back(() => sceneLoader.LoadNextScene(Utils.MENU_SCENE));
        }
    }

}
