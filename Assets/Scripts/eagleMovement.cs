using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eagleMovement : MonoBehaviour
{
    private Rigidbody2D rd;
    public Transform up;
    public Transform down;
    public float speed;
    private float upy, downy;
    public bool fly;
    private Animator anim;
    public Transform head;
    public LayerMask ground;
    float durationTime = 5f;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd = GetComponent<Rigidbody2D>();
        rd.velocity = new Vector2(rd.velocity.x, -speed);
        upy = up.position.y;
        downy = down.position.y;
        Destroy(up.gameObject);
        Destroy(down.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fly)
        {
            if (transform.position.y < downy)
            {
                rd.velocity = new Vector2(rd.velocity.x, speed);
            }


            if (transform.position.y > upy)
            {
                rd.velocity = new Vector2(rd.velocity.x, -speed);
            }
            if (rd.velocity.x != 0)
            {
                rd.velocity = new Vector2(0, rd.velocity.y);
            }
            if (rd.velocity.y == 0)
            {
                rd.velocity = new Vector2(0, speed);
            }
        }
        else
        {
            time += Time.deltaTime;
            transform.localScale = new Vector3(-1, 1, 1);
            rd.velocity = new Vector2(speed, rd.velocity.y);
            if (rd.velocity.y < 0)
            {
                rd.velocity = new Vector2(rd.velocity.x, 0.5f);
            }
            if (Physics2D.OverlapCircle(head.position, 1.2f, ground))
            {
                AudioManage.instance.PlaySound(AudioManage.instance.enemySound, AudioManage.instance.enemyDeath);
                anim.SetBool("death", true);
            }
            if (time > durationTime)
            {
                AudioManage.instance.PlaySound(AudioManage.instance.enemySound, AudioManage.instance.enemyDeath);
                anim.SetBool("death", true);
            }
        }


    }
    void Death()
    {
        Destroy(gameObject);
    }

}