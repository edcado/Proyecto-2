using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;
using System;

public class MainMenu : MonoBehaviour
{
    private VideoPlayer video;
    [SerializeField] private VideoClip idleClip;
    public int frameCount;
    public GameObject mainMenu;

    public void Start()
    {
        video = FindObjectOfType<VideoPlayer>();
        video.Play();
        video.loopPointReached += ChangeVideo;
    }

    private void ChangeVideo(VideoPlayer source)
    {
        source.clip = idleClip;
        source.Play();
        mainMenu.SetActive(true);
        video.loopPointReached -= ChangeVideo;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Lobby");
    }
}
