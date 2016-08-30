using UnityEngine;
using System;



/// <summary>
/// 적 캐릭터를 정의합니다.
/// </summary>
public class EnemyScript : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    /// <summary>
    /// 충돌체입니다.
    /// </summary>
    Collider2D _collider;
    /// <summary>
    /// 충돌체입니다.
    /// </summary>
    public Collider2D Collider { get { return _collider; } }


    #endregion










    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 캐릭터의 체력입니다.
    /// </summary>
    public int _health;
    /// <summary>
    /// 캐릭터와 충돌했을 때 플레이어가 입을 대미지입니다.
    /// </summary>
    public int _damage;


    /// <summary>
    /// 캐릭터가 사용할 효과음 집합입니다.
    /// </summary>
    public AudioClip[] audioClips;
    /// <summary>
    /// 캐릭터가 사용할 효과 집합입니다.
    /// </summary>
    public GameObject[] effects;


    /// <summary>
    /// 적이 사망할 때 드롭 가능한 아이템의 목록입니다.
    /// </summary>
    public ItemScript[] _items;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 캐릭터가 사용할 효과음을 사용 가능한 형태로 보관합니다.
    /// </summary>
    AudioSource[] _soundEffects;
    /// <summary>
    /// 캐릭터가 사용할 효과음을 사용 가능한 형태로 보관합니다.
    /// </summary>
    public AudioSource[] SoundEffects { get { return _soundEffects; } }


    /// <summary>
    /// 사망했다면 참입니다.
    /// </summary>
    bool _isDead;
    /// <summary>
    /// 무적 상태라면 참입니다.
    /// </summary>
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
        _soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            _soundEffects[i] = gameObject.AddComponent<AudioSource>();
            _soundEffects[i].clip = audioClips[i];
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










    #region 메서드를 정의합니다.
    /// <summary>
    /// 캐릭터를 죽입니다.
    /// </summary>
    public virtual void Dead()
    {
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 자신의 위치에 아이템을 생성합니다.
    /// </summary>
    /// <param name="item">생성할 아이템입니다.</param>
    /// <returns>생성된 아이템을 반환합니다.</returns>
    protected ItemScript CreateItem(ItemScript item)
    {
        if (UnityEngine.Random.Range(0, 100) < item.Probability)
        {
            // 아이템 객체를 생성합니다.
            ItemScript ret = (ItemScript)Instantiate(item, transform.position, transform.rotation);

            // 속성을 업데이트합니다.
            ret.IsDropped = true;

            // 아이템을 반환합니다.
            return ret;
        }
        return null;
    }


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
