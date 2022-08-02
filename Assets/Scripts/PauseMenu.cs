using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public FirstPerson sens;
    public GameObject pauseMenuUI;
    public GameObject gameUI;
    public GameObject optionsUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        optionsUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
    }

    public void mouseSens(float value)
    {
        sens.sensX = value;
        sens.sensY = value;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        paused = false;
        SceneManager.LoadScene(0);
    }
}
