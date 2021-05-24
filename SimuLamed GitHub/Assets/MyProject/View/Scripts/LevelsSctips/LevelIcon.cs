using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIcon : MonoBehaviour
{

    public Button button;
    public Image backOpenImage;
    public Image openLevelImage;
    public int levelIdx;
    private static LevelsVM viewModel;
    private static SceneLoader sceneLoader;
  


    private void Start()
    {
        viewModel = GameObject.Find("View").GetComponent<LevelsVM>();
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        Debug.Log(viewModel.OpenLevel);
        
        if (viewModel.OpenLevel >= levelIdx - 1)
        {
            SetEnable(true);
            Debug.Log(" LEVEL " + levelIdx.ToString() + "  IS OPEN");
        }
        else
        {
            SetEnable(false);
        }
        button.onClick.AddListener(OnClickIcon);
    }
    private void SetEnable(bool toEnable)
    {
        button.enabled = toEnable;
        backOpenImage.enabled = toEnable;
        openLevelImage.enabled = toEnable;
    }
    public void OnClickIcon()
    {
        viewModel.SetLevel(levelIdx);
        sceneLoader.LoadLevel(FromIdxToSceneName());
    }
    private string FromIdxToSceneName()
    {
        switch (levelIdx)
        {
            case 1:
                return Utils.SIMULATION_SCENE_1;
            case 2:
                return Utils.SIMULATION_SCENE_2;
            case 3:
                return Utils.SIMULATION_SCENE_3;
            default:
                return Utils.SIMULATION_SCENE_1;
        }
    }

}
