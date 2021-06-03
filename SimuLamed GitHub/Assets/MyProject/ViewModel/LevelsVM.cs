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
    public bool IsLoadingCircleOn { get { return isLoadingCircleOn; } set { isLoadingCircleOn = value; NotifyPropertyChanged("IsLoadingCircleOn"); } }


    public static string chosenLevel;
    public static int chosenLevelIdx;
    public static bool isSet;


    void Awake()
    {
        isLoadingCircleOn = false;
    }

    // Set the level fields accordingly to the given level index.
    public void SetLevel(int levelIdx)
    {
        //isSet = false;
        IsLoadingCircleOn = true;
        chosenLevelIdx = levelIdx;
        chosenLevel = FromLevelIdxToName();
    }
    
    // Convert the index ot the chosen level to the level name.
    private string FromLevelIdxToName()
    {
        switch (chosenLevelIdx)
        {
            case 1:
                return "Level1";
            case 2:
                return "Level2";
            case 3:
                return "Level3";
            default:
                return "Level1";
        }
    }

}
