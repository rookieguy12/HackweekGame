using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove : MonoBehaviour
{
    public Transform cam;

    /*[Header("BackStartPoint")]
    public float backStartPointX, backStartPointY;*/
    [Header("移动比例，以及是否移动Y")]
    public float moverateX;
    public float moverateY;
    public bool moveY;

    float camStartPointX, camStartPointY;
    float xAdjustment, yAdjustment;
    float tempmoverateY;
    void Start()
    {
        //backStartPointX = transform.position.x;
        //backStartPointY = transform.position.y;
        camStartPointX = cam.position.x;
        camStartPointY = cam.position.y;
        xAdjustment = transform.position.x - camStartPointX * moverateX;
        if (moveY)
        {
            tempmoverateY = moverateY;
        }
        else
        {
            tempmoverateY = 0;
        }
        yAdjustment = transform.position.y - camStartPointY * tempmoverateY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(cam.position.x * moverateX + xAdjustment, cam.position.y * tempmoverateY + yAdjustment);
    }
}
