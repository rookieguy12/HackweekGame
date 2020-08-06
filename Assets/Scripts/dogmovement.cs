using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogmovement : MonoBehaviour
{
    private Rigidbody2D rig;
    public Transform zuo;
    public Transform you;
    private float zuox, youx;
    public float speed;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        zuox = zuo.position.x;
        youx = you.position.x;
        Destroy(zuo.gameObject);
        Destroy(you.gameObject);
        rig.velocity = new Vector2(speed, rig.velocity.y);
        transform.localScale = new Vector3(-1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < zuox)
        {
            rig.velocity = new Vector2(speed, speed);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (transform.position.x > youx)
        {
            rig.velocity = new Vector2(-speed, speed);
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Mathf.Abs(rig.velocity.x)<3f)
        {
            rig.velocity = new Vector2(speed, rig.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void Death()
    {
        Destroy(gameObject);
    }
    public void JumpOn()
    {
        AudioManage.instance.PlaySound(AudioManage.instance.enemySound , AudioManage.instance.enemyDeath);
        anim.SetTrigger("death");
    }
}
