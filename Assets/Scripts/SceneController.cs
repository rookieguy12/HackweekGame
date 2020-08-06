using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController sceneController;


    [Header("Restrict CameraScene")]
    public CinemachineConfiner confiner;
    public PolygonCollider2D NormalRoomRestriction;
    public PolygonCollider2D HidenRoomRestriction;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    public AudioMixer audioMixer;
    public Slider slider;
    void Awake()
    {
        if (sceneController == null)
        {
            sceneController = this;
        }
    }

    public void NextLevel()
    {   
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void CameraToHidenRoom()
    {
        confiner.m_BoundingShape2D = HidenRoomRestriction;
    }
    public void CameraToNormalRoom()
    {
        confiner.m_BoundingShape2D = NormalRoomRestriction;
    }
    public void EndScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(5);
    }
    public void StartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PaueseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void SetVolume()
    {
        audioMixer.SetFloat("MainVolume", slider.value);
    }
}
