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



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    protected virtual void Awake()
    {
        // 효과음을 초기화 합니다.
        soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].clip = audioClips[i];
        }
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {

    }

    #endregion



    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 적 캐릭터에게 대미지를 입힙니다.
    /// </summary>
    /// <param name="enemy">입힐 대미지입니다.</param>
    protected void Attack(EnemyScript enemy)
    {
        enemy.Hurt(damage);
    }

    #endregion
}
