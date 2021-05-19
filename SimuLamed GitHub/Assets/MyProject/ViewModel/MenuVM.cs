using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class MenuVM : BaseViewModel
{

    private string username = "";
    [Binding]
    public string Username
    {
        get { return username; }
        set { username = value; NotifyPropertyChanged("Username"); }
    }

    // Log out.
    public void LogOut()
    {
        model.ResetCurrentUser();
    }

    protected override void SetModel()
    {
        model = AppModel.Instance;
        Username = model.GetCurrentUsername();
    }
}
