using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;
using System.ComponentModel;
using Assets;
using Assets.model;

[Binding]
public class LevelsVM : BaseViewModel
{

    [Binding]
    public bool IsLevel1Open { get { return openLevel >= 0; } }
    [Binding]
    public bool IsLevel2Open { get { return openLevel >= 1; } }
    [Binding]
    public bool IsLevel3Open { get { return openLevel >= 2; } }

    private int openLevel;

    // Start is called before the first frame update
    void Awake()
    {
        model = AppModel.Instance;
        openLevel = model.GetOpenLevel();
        NotifyPropertyChanged("IsLevel1Open");
        NotifyPropertyChanged("IsLevel2Open");
        NotifyPropertyChanged("IsLevel3Open");
    }


}
