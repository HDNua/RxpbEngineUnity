using UnityEngine;
using System.Collections;

/// <summary>
/// 적 캐릭터를 정의합니다.
/// </summary>
public class EnemyScript : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    Collider2D _collider;

    #endregion



    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    public AudioClip[] audioClips;
    public GameObject[] effects;

    #endregion



    #region Unity를 통해 초기화한 속성을 사용 가능한 형태로 보관합니다.
    AudioSource[] soundEffects;
    public AudioSource[] SoundEffects { get { return soundEffects; } }

    #endregion



    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.
    public int hitPoint;

    /// <summary>
    /// 캐릭터가 죽었는지를 확인합니다.
    /// </summary>
    /// <returns></returns>
    bool IsDead()
    {
        return hitPoint == 0;
    }

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    protected virtual void Start()
    {
        _collider = GetComponent<Collider2D>();
        soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].clip = audioClips[i];
        }
    }
    protected virtual void Update()
    {
        if (IsDead())
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
    public void Hurt(int damage)
    {
        hitPoint -= damage;
    }

    #endregion



    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 캐릭터를 죽입니다.
    /// </summary>
    public virtual void Dead()
    {
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }

    #endregion
}
