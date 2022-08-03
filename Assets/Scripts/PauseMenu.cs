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
            if (paused) Resume();

            else Pause();
        }
    }

    public void Resume()
    {
        //Hiding cursor and UI
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        optionsUI.SetActive(false);
        gameUI.SetActive(true);

        //Unfreezing the game
        Time.timeScale = 1f;
        paused = false;
    }

    public void Pause()
    {
        //Showing the cursor again so the player can click the menu buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Showing pause menu UI
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);

        //Freezing the game
        Time.timeScale = 0f;
        paused = true;
    }

    public void mouseSens(float value)
    {
        //I use this function to set the mouse sensitivity to the slider value
        sens.sensX = value;
        sens.sensY = value;
    }

    public void MainMenu()
    {
        //Unfreezing the game when the player decides to go back to the main menu
        Time.timeScale = 1f;
        paused = false;
        SceneManager.LoadScene(0);
    }
}
