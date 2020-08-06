using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogMovement : MonoBehaviour
{
    private Rigidbody2D rd;
    public Transform left;
    public Transform right;
    public float speed;
    private float leftx, rightx;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd = GetComponent<Rigidbody2D>();
        rd.velocity = new Vector2(-speed, rd.velocity.y);
        leftx = left.position.x;
        rightx = right.position.x;
        Destroy(left.gameObject);
        Destroy(right.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < leftx)
        {
            rd.velocity = new Vector2(speed, rd.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
            

        if (transform.position.x > rightx)
        {
            rd.velocity = new Vector2(-speed, rd.velocity.y);
            transform.localScale =new Vector3(1, 1, 1);
        }
        if(Mathf.Abs(rd.velocity.x) < 3f)
        {
            rd.velocity = new Vector2(speed, rd.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
            
    }
    void Death()
    {
        Destroy(gameObject);
    }
    public void JumpOn()
    {
        AudioManage.instance.PlaySound(AudioManage.instance.enemySound, AudioManage.instance.enemyDeath);
        anim.SetTrigger("death");
    }
}
