using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickLevelsSctipt : MonoBehaviour
{
    private SceneLoader sceneLoader;
    public GameObject loadingCircle;


    public static string ChosenLevel;
    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        loadingCircle.SetActive(false);
    }

    public void OnClickLevel(string level)
    {
        loadingCircle.SetActive(true);
        if (level.Contains("1"))
        {
            ChosenLevel = Utils.SIMULATION_SCENE_1;
        }
        else if (level.Contains("2"))
        {
            ChosenLevel = Utils.SIMULATION_SCENE_2;
        }
        else if (level.Contains("3"))
        {
            ChosenLevel = Utils.SIMULATION_SCENE_3;
        }
        sceneLoader.LoadLevel(ChosenLevel);
    }
}
