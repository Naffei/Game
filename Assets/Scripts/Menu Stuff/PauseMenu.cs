using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject PauseUI;
    public GameObject SettingUI;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resume Game
    public void Resume()
    {
        PauseUI.SetActive(false);
        SettingUI.SetActive(false);
        Time.timeScale = 1;
        GamePaused = false;
    }

    // Pause Game
    void Pause()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0;
        GamePaused = true;
    }

    // Return to Menu
    public void LoadMainMenu()
    {
        Debug.Log("Returning to Menu");
        SceneManager.LoadScene("Menu");
    }

    // Restart Game
    public void RestartGame()
    {
        Debug.Log("Restarting Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Exit Applicaiton
    public void ExitGame()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }
}
