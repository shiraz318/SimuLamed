using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class MenuVM : BaseViewModel
{
    // Properties.
    [Binding]
    public string Username { get { return AppModel.Instance.CurrentUsername; }
    }

    // Log out.
    public void LogOut() { model.ResetCurrentUser(); }
}
