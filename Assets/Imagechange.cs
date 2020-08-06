using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Imagechange : MonoBehaviour
{
    Image image;
    float imageloadstep;
    GameObject a;
    void Start()
    {
        image = transform.Find("Image1").GetComponent<Image>();
        imageloadstep = 0.01f;
        StartCoroutine(loadchange());
    }

    IEnumerator loadchange()
    {
        while (image.fillAmount < 1)
        {
            image.fillAmount += imageloadstep;
            yield return new WaitForSeconds(0.1f);
        }
    }
    void Update()
    {
        
    }
}
