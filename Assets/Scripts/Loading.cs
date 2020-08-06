using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image loadingLine;
    public GameObject startButton;
    AsyncOperation asyncOperation;
    void Start()
    {
        asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncOperation.allowSceneActivation = false;
        StartCoroutine("LoadingBar");
    }
    IEnumerator LoadingBar()
    {
        for (int i = 0; i < 4; i++)
        {
            loadingLine.fillAmount += 0.25f;
            yield return new WaitForSeconds(0.5f);
        }
        if (Mathf.Approximately(asyncOperation.progress, 0.9f))
        {
            startButton.SetActive(true);
        }
    }
    public void StartGame()
    {
        asyncOperation.allowSceneActivation = true;
    }
}
