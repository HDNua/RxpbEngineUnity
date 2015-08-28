using UnityEngine;
using System.Collections;

/// <summary>
/// 공격 스크립트입니다.
/// </summary>
public class AttackScript : MonoBehaviour
{
    // public PlayerController player;
    public GameObject[] effects;
    public int damage;
    public AudioClip[] audioClips;

    AudioSource[] soundEffects;
    public AudioSource[] SoundEffects { get { return soundEffects; } }

    void Awake()
    {
        // 효과음을 초기화 합니다.
        soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].clip = audioClips[i];
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }

    protected void Attack(EnemyScript enemy)
    {
        enemy.Hurt(damage);
    }
}
