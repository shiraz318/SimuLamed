using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public static bool isQuitMenu = false;

    public GameObject pauseMenuUI;
    public GameObject quitMenuUI;

   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isQuitMenu)
            {

                if (gameIsPaused)
                {

                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            QuitMenu();
        }
    }

    private void QuitMenu()
    {
        if (gameIsPaused)
        {
            pauseMenuUI.SetActive(false);
        }
        quitMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        isQuitMenu = true;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false; 
    }

    public void OnClickNoButton()
    {
        quitMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        isQuitMenu = false;
    }


}
