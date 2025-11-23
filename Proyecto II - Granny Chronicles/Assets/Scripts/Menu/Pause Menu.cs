using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private Player player;
    public GameObject pauseMenu;
    public static bool isPaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        /*if(player.pressedPause)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }*/
    }

    private void OnEnable()
    {
        Player.OnGamePaused += PauseEvent;
    }

    private void OnDisable()
    {
        Player.OnGamePaused -= PauseEvent;
    }

    public void PauseEvent()
    {
        if(isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void LoadCheckpoint()
    {
        player.Unalive();
        ResumeGame();

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
