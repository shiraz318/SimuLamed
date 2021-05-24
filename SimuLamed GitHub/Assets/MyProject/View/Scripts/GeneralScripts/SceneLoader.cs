using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    private float transitionTime = 0.7f;

    // Load the given levelName level asyncronusly.
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelAnync(levelName));
    }

    // Load the given levelName.
    IEnumerator LoadLevelAnync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
        while (operation.progress < 1)
        {
            Debug.Log(operation.progress);
            yield return new WaitForEndOfFrame();
        }
    }

    // Load the given sceneName scene.
    public void LoadNextScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }
    IEnumerator LoadScene(string sceneName)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadScene(sceneName);
    }

    // Load the given sceneIndex scene.
    public void LoadNextScene(int sceneIndex)
    {
        StartCoroutine(LoadScene(sceneIndex));
    }
    IEnumerator LoadScene(int sceneIndex)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadScene(sceneIndex);
    }

}
