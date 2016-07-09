using UnityEngine;
using System.Collections;

/// <summary>
/// 공격 스크립트입니다.
/// </summary>
public class AttackScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    // public PlayerController player;
    public GameObject[] effects;
    public int damage;
    public AudioClip[] audioClips;


    #endregion



    
    
    
    
    
    
    
    #region 필드를 정의합니다.
    AudioSource[] soundEffects;
    public AudioSource[] SoundEffects { get { return soundEffects; } }


    #endregion




    
    
    
    
    
    
    
    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
    protected virtual void Start()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    protected virtual void Update()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    protected virtual void FixedUpdate()
    {

    }


    #endregion










    #region 메서드를 정의합니다.
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
