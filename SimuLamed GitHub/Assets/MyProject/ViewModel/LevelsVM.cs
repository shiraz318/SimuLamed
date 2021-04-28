using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;
using System.ComponentModel;
using Assets;
using Assets.model;

[Binding]
public class LevelsVM : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public bool IsLevel1Open { get { return openLevel >= 0; } }
    [Binding]
    public bool IsLevel2Open { get { return openLevel >= 1; } }
    [Binding]
    public bool IsLevel3Open { get { return openLevel >= 2; } }

    private int openLevel;
    private IAppModel model;

    // Start is called before the first frame update
    void Awake()
    {
        SetModel();
    }

    private void SetModel()
    {
        model = AppModel.Instance;
        openLevel = model.GetOpenLevel();
        NotifyPropertyChanged("IsLevel1Open");
        NotifyPropertyChanged("IsLevel2Open");
        NotifyPropertyChanged("IsLevel3Open");

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
