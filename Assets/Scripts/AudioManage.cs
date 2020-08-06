using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManage : MonoBehaviour
{
    public static AudioManage instance;
    [Header("Game detecter sounds")]
    public AudioClip winSound;
    public AudioClip fallSound;

    [Space]
    [Header("BGM")]
    public AudioClip bgm;
    public AudioClip bgmInHidenRoom;

    [Space]
    [Header("Player sounds")]
    public AudioClip getCherry;
    public AudioClip getDiamond;
    public AudioClip jump;
    public AudioClip getHit;
    

    [Space]
    [Header("Enemy sounds")]
    public AudioClip enemyDeath;

    [Space]
    [Header("Collection sounds")]
    public AudioClip cherrySound;
    public AudioClip gemSound;

    [Space]
    [Header("Different Audio Sources")]
    public AudioSource playerSound;
    public AudioSource collectionSound;
    public AudioSource enemySound;
    public AudioSource gamejudgeSound;
    void Awake()
    {
        gamejudgeSound = GetComponent<AudioSource>();
        gamejudgeSound.clip = bgm;
        gamejudgeSound.Play();
        playerSound = gameObject.AddComponent<AudioSource>();
        collectionSound = gameObject.AddComponent<AudioSource>();
        enemySound = gameObject.AddComponent<AudioSource>();
        instance = this;
    }
    public void PlaySound(AudioSource audiomanager,AudioClip audioClip)
    {
        audiomanager.clip = audioClip;
        if (!audiomanager.isPlaying)
        {
            audiomanager.Play();
        }
    }
}
