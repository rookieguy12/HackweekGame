using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour
{
    AudioSource ClickSound;
    void Start()
    {
        ClickSound = GetComponent<AudioSource>();
    }
    public void PlayClickSound()
    {
        ClickSound.Play();
    }
    public void StartLevels()
    {
        SceneManager.LoadScene(1);//1应为Loading
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void GotoStart()
    {
        SceneController.sceneController.StartScene();
    }
}
