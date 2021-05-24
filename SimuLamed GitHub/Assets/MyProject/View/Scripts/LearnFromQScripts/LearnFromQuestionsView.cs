using Assets;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class LearnFromQuestionsView : BaseView
{
    private LearnFromQVM viewModel;

    private new void Start()
    {
        base.Start();
        Debug.Log("LFQ START");
        viewModel = GameObject.Find("View").GetComponent<LearnFromQVM>();
        viewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (this == null) { return; }
            if (eventArgs.PropertyName.Equals(viewModel.GetPropertyName()))
            {
                Debug.Log("GO TO OTHER SCNEN");
                GoToOtherScene(Utils.MENU_SCENE);
            }
        };
    }
    //private void Start()
    //{
    //    viewModel = GameObject.Find("View").GetComponent<LearnFromQVM>();
    //    //viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
    //    //{
    //    //    if (eventArgs.PropertyName.Equals(viewModel.GetPropertyName()))
    //    //    {
    //    //        GoToOtherScene(Utils.MENU_SCENE);
    //    //    }
    //    //};
    //    //if (viewModel == null)
    //    //{
    //    //    viewModel = GetComponentsInParent<LearnFromQVM>()[0];
    //    //    viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
    //    //    {
    //    //        if (eventArgs.PropertyName.Equals(viewModel.GetPropertyName()))
    //    //        {
    //    //            GoToOtherScene(Utils.MENU_SCENE);
    //    //        }
    //    //    };
    //    //}
    //}
    // On click event handler for clicking any subject button.
    //public void OnClickSubject(Button button)
    //{
    //    //viewModel.SelectedSubject = button.name;
    //    //GoToOtherScene(Utils.QUESTIONS_SCENE);
    //    //sceneLoader.LoadNextScene(Utils.QUESTIONS_SCENE);
    //}

    //// On click event handler for clicking the back button.
    //public void OnClickBack()
    //{
    //    viewModel.SaveUser();
    //}
}
