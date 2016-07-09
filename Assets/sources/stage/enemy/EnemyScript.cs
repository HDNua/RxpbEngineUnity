using UnityEngine;
using System;



/// <summary>
/// 적 캐릭터를 정의합니다.
/// </summary>
public class EnemyScript : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    Collider2D _collider;
    public Collider2D Collider { get { return _collider; } }

    #endregion










    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public int _health;
    public int _damage;


    public AudioClip[] audioClips;
    public GameObject[] effects;


    #endregion










    #region 필드를 정의합니다.
    AudioSource[] soundEffects;
    public AudioSource[] SoundEffects { get { return soundEffects; } }


    bool _isDead;
    bool _invencible;


    #endregion










    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.
    /// <summary>
    /// 체력을 가져옵니다.
    /// </summary>
    public int Health
    {
        get { return _health; }
        protected set { _health = value; }
    }
    /// <summary>
    /// 대미지를 가져옵니다.
    /// </summary>
    public int Damage
    {
        get { return _damage; }
    }


    /// <summary>
    /// 캐릭터가 죽었다면 참입니다.
    /// </summary>
    public bool IsDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    /// <summary>
    /// 캐릭터가 무적 상태라면 참입니다.
    /// </summary>
    public bool Invencible
    {
        get { return _invencible; }
        protected set { _invencible = value; }
    }


    /// <summary>
    /// 캐릭터가 살아있는지 확인합니다.
    /// </summary>
    /// <returns>캐릭터가 살아있다면 참입니다.</returns>
    public bool IsAlive()
    {
        return (0 < Health);
    }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    /// <summary>
    /// MonoBehaviour 객체를 초기화합니다.
    /// </summary>
    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].clip = audioClips[i];
        }
    }
    /// <summary>
    /// MonoBehaviour 객체를 초기화합니다.
    /// </summary>
    protected virtual void Start()
    {

    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected virtual void Update()
    {
        if (IsAlive() == false)
        {
            Dead();
        }
    }


    #endregion










    #region 외부에서 접근 가능한 공용 메서드를 정의합니다.
    /// <summary>
    /// 캐릭터에게 대미지를 입힙니다.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Hurt(int damage)
    {
        _health -= damage;
    }


    #endregion










    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 캐릭터를 죽입니다.
    /// </summary>
    public virtual void Dead()
    {
        gameObject.SetActive(false);
    }


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
