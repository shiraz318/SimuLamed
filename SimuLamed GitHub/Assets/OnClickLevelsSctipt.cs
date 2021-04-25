using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickLevelsSctipt : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }
    public void OnClickLevel(string level)
    {

        if (level.Contains("1"))
        {
            sceneLoader.LoadNextScene(Utils.SIMULATION_SCENE_1);
        }
        else if (level.Contains("2"))
        {
            sceneLoader.LoadNextScene(Utils.SIMULATION_SCENE_2);
        }
        else if (level.Contains("3"))
        {
            sceneLoader.LoadNextScene(Utils.SIMULATION_SCENE_3);
        }
    }
}
