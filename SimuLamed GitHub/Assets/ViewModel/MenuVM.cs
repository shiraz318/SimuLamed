using Assets.model;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class MenuVM : MonoBehaviour, INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private IAppModel model;

    private string username = "";
    [Binding]
    public string Username
    {
        get { return username; }
        set { username = value; NotifyPropertyChanged("Username"); }
    }


    private void Start()
    {
        model = AppModel.Instance;
        Username = model.GetCurrentUsername();
    }

    // Log out.
    public void LogOut()
    {
        model.ResetCurrentUser();
    }

    // On property changed.
    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
