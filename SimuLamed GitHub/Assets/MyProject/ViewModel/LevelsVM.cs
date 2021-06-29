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
    private bool isLoadingCircleOn;
    // Properties.
    public int OpenLevel { get { return AppModel.Instance.OpenLevel; } }
    [Binding]
    public bool IsLoadingCircleOn { get { return isLoadingCircleOn; } set { isLoadingCircleOn = value; NotifyPropertyChanged(); } }


    public static int chosenLevelIdx;


    void Awake()
    {
        isLoadingCircleOn = false;
    }

    // Set the level fields accordingly to the given level index.
    public void SetLevel(int levelIdx)
    {
        IsLoadingCircleOn = true;
        chosenLevelIdx = levelIdx;
    }
    

}
