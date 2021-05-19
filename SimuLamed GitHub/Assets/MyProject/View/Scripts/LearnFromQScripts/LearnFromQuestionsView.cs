using Assets;
using UnityEngine;
using UnityEngine.UI;

public class LearnFromQuestionsView : BaseView
{
   // private SceneLoader sceneLoader;
    private LearnFromQVM viewModel;

    private void Start()
    {
      //  sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        if (viewModel == null)
        {
            viewModel = GetComponentsInParent<LearnFromQVM>()[0];
        }
    }

    // On click event handler for clicking any subject button.
    public void OnClickSubject(Button button)
    {
        viewModel.SelectedSubject = button.name;
        GoToOtherScene(Utils.QUESTIONS_SCENE);
        //sceneLoader.LoadNextScene(Utils.QUESTIONS_SCENE);
    }

    // On click event handler for clicking the back button.
    public void OnClickBack()
    { 
        viewModel.Back(() => GoToOtherScene(Utils.MENU_SCENE));
    }
}
