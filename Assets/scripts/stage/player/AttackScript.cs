using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 공격 스크립트입니다.
/// </summary>
public abstract class AttackScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 공격 개체가 사용할 효과 집합입니다.
    /// </summary>
    public GameObject[] effects;
    /// <summary>
    /// 공격의 대미지입니다.
    /// </summary>
    public int damage;
    /// <summary>
    /// 공격 개체가 사용할 효과음 집합입니다.
    /// </summary>
    public AudioClip[] audioClips;
    
    #endregion
    
    
        
    
    
    #region 필드를 정의합니다.
    /// <summary>
    /// 공격 효과음입니다.
    /// </summary>
    AudioSource[] _soundEffects;
    /// <summary>
    /// 공격 효과음입니다.
    /// </summary>
    public AudioSource[] SoundEffects { get { return _soundEffects; } }

    /// <summary>
    /// 
    /// </summary>
    public GameObject _ReflectedParticle { get { return effects[0]; } }
    /// <summary>
    /// 
    /// </summary>
    public GameObject _HitParticle { get { return effects[1]; } }

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected virtual void Awake()
    {
        // 효과음을 초기화 합니다.
        _soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            _soundEffects[i] = gameObject.AddComponent<AudioSource>();
            _soundEffects[i].clip = audioClips[i];
            _soundEffects[i].enabled = false;
        }
    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected virtual void Start()
    {

    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected virtual void Update()
    {

    }
    /// <summary>
    /// FixedTimestep에 설정된 값에 따라 일정한 간격으로 업데이트합니다.
    /// 물리 효과가 적용된 오브젝트를 조정할 때 사용됩니다.
    /// (Update는 불규칙한 호출이기 때문에 물리엔진 충돌검사가 제대로 되지 않을 수 있습니다.)
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




    #region 추상 메서드를 선언합니다.
    /// <summary>
    /// 피격 효과 객체를 생성합니다.
    /// </summary>
    /// <returns>피격 효과 객체입니다.</returns>
    protected virtual GameObject MakeHitParticle(bool right, Transform transform)
    {
        GameObject hitParticle = Instantiate
            (_HitParticle, transform.position, transform.rotation)
            as GameObject;

        // 버스터 속도의 반대쪽으로 적절히 x 반전합니다.
        if (right)
        {
            Vector3 newScale = hitParticle.transform.localScale;
            newScale.x *= -1;
            hitParticle.transform.localScale = newScale;
        }

        // 효과음을 재생합니다.
        EffectScript hitEffect = hitParticle.GetComponent<EffectScript>();
        hitEffect.PlayEffectSound(SoundEffects[1].clip);

        // 생성한 효과 객체를 반환합니다.
        return hitParticle;
    }
    /// <summary>
    /// 피격 효과 객체를 생성합니다.
    /// </summary>
    /// <returns>피격 효과 객체입니다.</returns>
    protected GameObject MakeReflectedParticle(bool right, Transform transform)
    {
        GameObject particle = Instantiate
            (_ReflectedParticle, transform.position, transform.rotation)
            as GameObject;

        // 버스터 속도의 반대쪽으로 적절히 x 반전합니다.
        if (right)
        {
            Vector3 newScale = particle.transform.localScale;
            newScale.x *= -1;
            particle.transform.localScale = newScale;
        }

        // 효과음을 재생합니다.
        EffectScript hitEffect = particle.GetComponent<EffectScript>();
        hitEffect.PlayEffectSound(SoundEffects[0].clip);

        // 생성한 효과 객체를 반환합니다.
        return particle;
    }

    #endregion
}
