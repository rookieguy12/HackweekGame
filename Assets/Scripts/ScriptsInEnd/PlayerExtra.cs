using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerExtra : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D ri;

    [Header("Position Points")]
    public Vector2 spawnPoint;

    [Header("Colliders")]
    public Collider2D disColl;

    [Space]
    [Header("speed & jumpforce")]
    public float moveForce;
    public float moveMaxSpeed;
    public float jumpvelocity;
    public float tick;

    [Space]
    [Header("PointCheckers")]
    public Transform celling;
    public Transform foot;
    public float checkRadius;

    [Space]
    [Header("LayerMasks")]
    public LayerMask ground;
    public LayerMask eagle;

    [Header("状态检测")]
    public bool isGround, isJump;


    [Header("跳跃检测")]
    public bool jumpPressed, jumpHold;
    public int jumpcount;
    public float jumpHoldDuration;
    float jumpTime;

    [SerializeField]
    bool qisile;

    [SerializeField]
    bool isHurt;
    // Start is called before the first frame update
    void Start()
    {
        ri = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        JumpStatusCheck();//跳跃状态检测
        BackPlaceCheck();
        PlayerStatusCheck();//玩家状态检测

        if (!isHurt)
        {
            XMovement();
            YMovement();
            Crouch();
        }
        SwitchAnim();
    }

    private void BackPlaceCheck()
    {
        if (Input.GetKey(KeyCode.R))
        {
            BackPlace();
        }
    }

    public void PlayerStatusCheck()
    {
        isGround = Physics2D.OverlapCircle(foot.position, checkRadius, ground) || Physics2D.OverlapCircle(foot.position, checkRadius, eagle);
    }


    public void JumpStatusCheck()
    {
        jumpPressed = Input.GetKeyDown(KeyCode.W);
        jumpHold = Input.GetKey(KeyCode.W);
        if (Input.GetKeyUp(KeyCode.W))
        {
            isJump = false;
        }
    }

    private void XMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        ri.AddForce(new Vector2(h * moveForce, 0), ForceMode2D.Impulse);
        ri.velocity = new Vector2(Mathf.Clamp(ri.velocity.x, -moveMaxSpeed, moveMaxSpeed), ri.velocity.y);
        if (h != 0)
        {
            transform.localScale = new Vector3(h, 1, 1);
        }
    }

    void YMovement()
    {
        if (isGround && !isJump && !(anim.GetBool("isFalling") || anim.GetBool("isJump")))
        {
            jumpcount = 2;
        }
        if (isGround && jumpPressed)
        {
            isJump = true;
            jumpcount--;
            jumpTime = jumpHoldDuration;
            ri.velocity = new Vector2(ri.velocity.x, jumpvelocity);
        }
        else if (!isGround && jumpPressed && jumpcount > 0)
        {
            isJump = true;
            jumpcount--;
            jumpTime = jumpHoldDuration;
            ri.velocity = new Vector2(ri.velocity.x, jumpvelocity);
        }
        if (jumpHold && isJump)
        {
            if (jumpTime > 0)
            {
                ri.velocity = new Vector2(ri.velocity.x, jumpvelocity);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJump = false;
            }
        }
    }

    //切换动画
    private void SwitchAnim()
    {
        anim.SetFloat("runspeed", Mathf.Abs(ri.velocity.x));
        if (isGround)
        {
            anim.SetBool("isGround", true);
            anim.SetBool("isFalling", false);
            anim.SetBool("isJump", false);
        }
        else if (!isGround && ri.velocity.y > tick)
        {
            anim.SetBool("isGround", false);
            anim.SetBool("isJump", true);
        }
        else if (!isGround && ri.velocity.y < 0)
        {
            anim.SetBool("isGround", false);
            anim.SetBool("isJump", false);
            anim.SetBool("isFalling", true);
        }
        if (isHurt)
        {
            anim.SetBool("isHurt", true);
            if (Mathf.Abs(ri.velocity.x) < 0.1)
            {
                isHurt = false;
                anim.SetBool("isHurt", false);
            }
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("crouching", true);
            disColl.enabled = false;
            qisile = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (!Physics2D.OverlapCircle(celling.position, checkRadius, ground))
            {
                anim.SetBool("crouching", false);
                disColl.enabled = true;
            }
            else
            {
                qisile = false;
            }
        }
        if (!qisile && !Physics2D.OverlapCircle(celling.position, 0.2f, ground))
        {
            anim.SetBool("crouching", false);
            disColl.enabled = true;
            qisile = false;
        }
    }

    //碰到Trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("spawnpoint"))//更新出生点
        {
            spawnPoint = collision.transform.position;
            collision.GetComponent<Collider2D>().enabled = false;
        }
        //碰到其它触碰器
        else if (collision.CompareTag("win"))
        {
            Destroy(collision.gameObject);
            Time.timeScale = 0.5f;
            AudioManage.instance.gamejudgeSound.loop = false;
            AudioManage.instance.PlaySound(AudioManage.instance.gamejudgeSound, AudioManage.instance.winSound);
            Invoke(nameof(CallStartScene), 1f);
        }
        else if (collision.CompareTag("fall"))
        {
            AudioManage.instance.gamejudgeSound.loop = false;
            AudioManage.instance.PlaySound(AudioManage.instance.gamejudgeSound, AudioManage.instance.fallSound);
            Invoke(nameof(BackPlace), 2f);
        }
    }
    //碰到碰撞体
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Eagle"))
        {
            if (anim.GetBool("isFalling"))
            {
                ri.velocity = new Vector2(ri.velocity.x, jumpvelocity);
                if (collision.gameObject.CompareTag("Eagle"))
                {
                    eagleMovement eagle = collision.gameObject.GetComponent<eagleMovement>();
                    eagle.fly = true;
                }

            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                ri.velocity = new Vector2(-10, ri.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                ri.velocity = new Vector2(10, ri.velocity.y);
                isHurt = true;
            }
        }
    }
    void BackPlace()
    {
        ri.velocity = new Vector2(0, 0);
        transform.position = spawnPoint;
        AudioManage.instance.PlaySound(AudioManage.instance.gamejudgeSound, AudioManage.instance.bgm);
        AudioManage.instance.gamejudgeSound.loop = true;
    }
    void CallStartScene()
    {
        SceneController.sceneController.StartScene();
    }
}
