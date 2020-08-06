using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterExtra : MonoBehaviour
{
    public float waitingTime = 5f;
    public float waitingCount;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            waitingCount += Time.deltaTime;
        }
        if (waitingCount > waitingTime)
        {
            SceneController.sceneController.NextLevel();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        waitingCount = 0f;
    }
}
