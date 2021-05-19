using Assets;
using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;


[Binding]
public class LearnFromQVM : BaseViewModel
{
    private string selectedSubject;
    public string SelectedSubject
    {
        get { return selectedSubject; }
        set { selectedSubject = value; model.SelectedSubject = Question.FromCategoryToTypeEnglish(value); }
    }

    // Save current user.
    public void Back(Utils.OnSuccessFunc onSuccess)
    {
        model.SaveUser(onSuccess);
    }

    protected override ErrorTypes GetErrorType()
    {
        return ErrorTypes.SaveScore;
    }
}
