using Assets;
using Assets.MyProject.View.Scripts;
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
        viewModel = GameObject.Find(GameObjectNames.VIEW).GetComponent<LevelsVM>();
        sceneLoader = GameObject.Find(GameObjectNames.SCENE_LOADER).GetComponent<SceneLoader>();

        /*
         * If the highest open level of the user is bigger than this level index - this level is open. 
         * Otherwise its close.
         */
        SetEnable(viewModel.OpenLevel >= levelIdx);
        button.onClick.AddListener(OnClickIcon);
    }

    // Set this level icon to be enable or disable accordingly to the given toEnable.
    private void SetEnable(bool toEnable)
    {
        button.enabled = toEnable;
        backOpenImage.enabled = toEnable;
        openLevelImage.enabled = toEnable;
    }
    
    // On click the level icon event handler.
    public void OnClickIcon()
    {
        viewModel.SetLevel(levelIdx);
        sceneLoader.LoadLevel(FromIdxToSceneName());
    }
    
    // Convert the chosen index to the right scene name.
    private string FromIdxToSceneName()
    {
        switch (levelIdx)
        {
            case 1:
                return ScenesNames.SIMULATION_SCENE_1;
            case 2:
                return ScenesNames.SIMULATION_SCENE_2;
            case 3:
                return ScenesNames.SIMULATION_SCENE_3;
            default:
                return ScenesNames.SIMULATION_SCENE_1;
        }
    }

}
