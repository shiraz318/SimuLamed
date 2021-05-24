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
    //[Binding]
    //public bool IsLevel1Open { get { return OpenLevel >= 0; } }
    //[Binding]
    //public bool IsLevel2Open { get { return OpenLevel >= 1; } }
    //[Binding]
    //public bool IsLevel3Open { get { return OpenLevel >= 2; } }
    public int OpenLevel { get { return AppModel.Instance.OpenLevel; } }
    [Binding]
    public bool IsLoadingCircleOn { get { return isLoadingCircleOn; } set { isLoadingCircleOn = value; NotifyPropertyChanged("IsLoadingCircleOn"); } }

    public static string chosenLevel;
    public static int chosenLevelIdx;
    public static bool isSet;

    //[Binding]
    //public bool IsLevelOpen(int level)
    //{
    //    return OpenLevel >= level - 1;
    //}

    void Awake()
    {
        isLoadingCircleOn = false;
        //model = AppModel.Instance;
        //NotifyPropertyChanged("IsLevel1Open");
        //NotifyPropertyChanged("IsLevel2Open");
        //NotifyPropertyChanged("IsLevel3Open");
    }
    //public void OnChooseLevel(string level, int idx)
    //{
    //    chosenLevel = level;
    //    chosenLevelIdx = idx;
    //}
    public void SetLevel(int levelIdx)
    {
        //isSet = false;
        IsLoadingCircleOn = true;
        chosenLevelIdx = levelIdx;
        chosenLevel = FromLevelIdxToName();
    }
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
    //public static int FromNameToIdx(string currentLevelName)
    //{
    //    if (currentLevelName.Equals("Level1"))
    //    {
    //        return 1;
    //    }
    //    else if (currentLevelName.Equals("Level2"))
    //    {
    //        return 2;
    //    }
    //    else
    //    {
    //        return 3;
    //    }
    //}
}
