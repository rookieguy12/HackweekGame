using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D ri;

    [Header("Position Points")]
    public Vector2 spawnPoint;
    public Transform enterRoomPoint;
    public Transform exitRoomPoint;



    [Header("Colliders")]
    public Collider2D disColl;

    [Space]
    [Header("speed & jumpforce")]
    public float moveForce = 5f;
    public float moveMaxSpeed = 9f;
    public float jumpvelocity = 20f;
    public float tick;

    [Space]
    [Header("PointCheckers")]
    public Transform celling;
    public Transform foot;
    public float checkRadius = 0.4f;

    [Space]
    [Header("LayerMasks")]
    public LayerMask ground;
    public LayerMask eagle;

    [Space]
    [Header("LifeCount")]
    public Text lifeCountText;


    [Header("状态检测")]
    public bool isGround, isJump;


    [Header("跳跃检测")]
    public bool jumpPressed, jumpHold;
    public int jumpcount;
    public float jumpHoldDuration = 0.2f;
    float jumpTime;




    int lifeCount;

    [SerializeField]
    bool qisile;

    [SerializeField]
    bool isHurt;
    // Start is called before the first frame update
    void Start()
    {
        ri = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lifeCount = 5;
    }
    // Update is called once per frame
    void Update()
    {
        JumpStatusCheck();//跳跃状态检测
        PlayerStatusCheck();//玩家状态检测
        if (!isHurt)
        {
            XMovement();
            YMovement();
            Crouch();
        }
        SwitchAnim();
    }
    public void PlayerStatusCheck()
    {
        isGround = Physics2D.OverlapCircle(foot.position, checkRadius, ground);
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
    //二段跳
    /*
    private void Jump()
    {
        if(isGround)
        {
            jumpCount = 1;
            isJump = false;
        }
        if(jumpPressed && isGround)
        {
            isJump = true;
            ri.add = new Vector2(ri.velocity.x, jumpForce);
            AudioManage.instance.PlaySound(AudioManage.instance.playerSound, AudioManage.instance.jump);
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount>0 && !isGround)
        {
            ri.velocity = new Vector2(ri.velocity.x, jumpHeight);
            AudioManage.instance.PlaySound(AudioManage.instance.playerSound, AudioManage.instance.jump);
            jumpCount--;
            jumpPressed = false;
        }

    }*/

    //切换动画
    private void SwitchAnim()
    {
        anim.SetFloat("runspeed", Mathf.Abs(ri.velocity.x));

        if(isGround)
        {
            anim.SetBool("isGround", true);
            anim.SetBool("isFalling", false);
            anim.SetBool("isJump", false);
        }
        else if(!isGround && ri.velocity.y > tick)
        {
            anim.SetBool("isGround", false);
            anim.SetBool("isJump", true);
        }
        else if(!isGround && ri.velocity.y < 0)
        {
            anim.SetBool("isGround", false);
            anim.SetBool("isJump", false);
            anim.SetBool("isFalling", true);
        }
        if(isHurt)
        {
            anim.SetBool("isHurt", true);
            if (Mathf.Abs(ri.velocity.x)<0.1)
            {
                isHurt = false;
                anim.SetBool("isHurt", false);
            }
        }
    }

    private void Crouch()
    {
            if(Input.GetKeyDown(KeyCode.S))
            {
                anim.SetBool("crouching", true);
                disColl.enabled = false;
                qisile = true;
            }
            /*else
            {
                anim.SetBool("crouching", false);
                disColl.enabled = true;
            }*/
            else if(Input.GetKeyUp(KeyCode.S))
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
            if(!qisile && !Physics2D.OverlapCircle(celling.position, 0.2f, ground))
            {
            anim.SetBool("crouching", false);
            disColl.enabled = true;
            qisile = false;
            }
    }

    //碰到Trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        //物品收集
        if (collision.CompareTag("cherry"))
        {
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("isCollected");
            AudioManage.instance.PlaySound(AudioManage.instance.playerSound, AudioManage.instance.cherrySound);
        }
        else if (collision.CompareTag("diamond")) 
        {
            collision.GetComponent<PolygonCollider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("isCollected");
            AudioManage.instance.PlaySound(AudioManage.instance.collectionSound, AudioManage.instance.gemSound);
        }
        else if (collision.CompareTag("spawnpoint"))//更新出生点
        {
            spawnPoint = collision.transform.position;
            collision.GetComponent<Collider2D>().enabled = false;
        }
        //碰到其它触碰器
        else if(collision.CompareTag("win"))
        {
            Destroy(collision.gameObject);
            Time.timeScale = 0.5f;
            AudioManage.instance.gamejudgeSound.loop = false;
            AudioManage.instance.PlaySound(AudioManage.instance.gamejudgeSound, AudioManage.instance.winSound);
            Invoke(nameof(CallNextLevel), 1f); 
        }
        else if (collision.CompareTag("fall"))
        {
            AudioManage.instance.gamejudgeSound.loop = false;
            AudioManage.instance.PlaySound(AudioManage.instance.gamejudgeSound, AudioManage.instance.fallSound);
            lifeCount--;
            lifeCount = Mathf.Clamp(lifeCount, 0, 5);
            lifeCountText.text = Convert.ToString(lifeCount);
            if (lifeCount != 0)
            {
                Invoke(nameof(BackPlace), 2f);
            }
            else
            {
                SceneController.sceneController.EndScene();
            }
        }
        else if (collision.CompareTag("hide"))
        {
            Invoke(nameof(CallHidenLevel), 0.3f);
        }

        else if (collision.CompareTag("exit"))
        {
            Invoke(nameof(CallNormalLevel), 0.3f);
        }
        else if (collision.CompareTag("end"))
        {
            Destroy(collision.gameObject);
            Time.timeScale = 0.5f;
            AudioManage.instance.gamejudgeSound.loop = false;
            AudioManage.instance.PlaySound(AudioManage.instance.gamejudgeSound, AudioManage.instance.winSound);
            Invoke(nameof(CallEndScene), 1f);
        }
    }
    //碰到碰撞体
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Frog") || collision.gameObject.CompareTag("Dog") || collision.gameObject.CompareTag("Eagle"))
        {
            
            if(anim.GetBool("isFalling"))
            {
                ri.velocity = new Vector2(ri.velocity.x, jumpvelocity);
                if (collision.gameObject.CompareTag("Frog"))
                {
                    collision.transform.GetComponent<BoxCollider2D>().enabled = false;
                    frogMovement frog = collision.gameObject.GetComponent<frogMovement>();
                    frog.JumpOn();
                }
                if (collision.gameObject.CompareTag("Dog"))
                {
                    collision.transform.GetComponent<CircleCollider2D>().enabled = false;
                    dogmovement dog = collision.gameObject.GetComponent<dogmovement>();
                    dog.JumpOn();   
                }
                if (collision.gameObject.CompareTag("Eagle"))
                {
                    eagleMovement eagle = collision.gameObject.GetComponent<eagleMovement>();
                    eagle.fly = true;
                }

            }
            else if(transform.position.x < collision.gameObject.transform.position.x)
            {
                ri.velocity = new Vector2(-10, ri.velocity.y);
                isHurt = true;
                lifeCount--;
                lifeCount = Mathf.Clamp(lifeCount, 0, 5);
                lifeCountText.text = Convert.ToString(lifeCount);
                if (lifeCount != 0)
                {
                    AudioManage.instance.PlaySound(AudioManage.instance.playerSound, AudioManage.instance.getHit);
                }
                else
                {
                    CallEndScene();
                }  
            }            
            else if(transform.position.x > collision.gameObject.transform.position.x)
            {
                ri.velocity = new Vector2(10, ri.velocity.y);
                isHurt = true;
                lifeCount--;
                lifeCount = Mathf.Clamp(lifeCount, 0, 5);
                lifeCountText.text = Convert.ToString(lifeCount);
                if (lifeCount != 0)
                {
                    AudioManage.instance.PlaySound(AudioManage.instance.playerSound, AudioManage.instance.getHit);
                }
                else
                {
                    CallEndScene();
                }
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
    void CallNextLevel()
    {
        SceneController.sceneController.NextLevel();
    }
    void CallHidenLevel()
    {
        ri.velocity = new Vector2(0, 0);
        transform.position = enterRoomPoint.position;
        AudioManage.instance.PlaySound(AudioManage.instance.gamejudgeSound, AudioManage.instance.bgmInHidenRoom);
        SceneController.sceneController.CameraToHidenRoom();
    }
    void CallNormalLevel()
    {
        ri.velocity = new Vector2(0, 0);
        transform.position = exitRoomPoint.position;
        AudioManage.instance.PlaySound(AudioManage.instance.gamejudgeSound, AudioManage.instance.bgm);
        SceneController.sceneController.CameraToNormalRoom();
    }
    void CallEndScene()
    {
        SceneController.sceneController.EndScene();
    }
}
