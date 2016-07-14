using System;
using UnityEngine;



[RequireComponent(typeof(Animator))]
/// <summary>
/// 효과 스크립트입니다.
/// </summary>
public class EffectScript : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    AudioSource _audioSource = null;
    Animator _animator;

    #endregion










    #region 필드 및 프로퍼티를 정의합니다.
    bool _endRequested = false;
    bool _destroyRequested = false;
    public bool EndRequested
    {
        get { return _endRequested; }
        private set { _animator.SetBool("EndRequested", _endRequested = value); }
    }
    public bool DestroyRequested
    {
        get { return _destroyRequested; }
        private set { _destroyRequested = value; }
    }

    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (_destroyRequested == false)
            return;

        // 애니메이션이 재생중이거나 음원 재생중이라면
        if (_animator.enabled || _audioSource && _audioSource.isPlaying)
        {

        }
        // 모든 재생이 끝난 경우 파괴합니다.
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion










    #region 프레임 이벤트 핸들러를 정의합니다.
    void FE_EndEffect()
    {
        RequestDestroy();
    }

    #endregion










    #region 외부에서 호출 가능한 메서드를 정의합니다.
    /// <summary>
    /// 효과 객체를 x 반전합니다.
    /// </summary>
    public void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
    /// <summary>
    /// 효과 객체 파괴를 요청합니다.
    /// </summary>
    public void RequestDestroy()
    {
        _animator.enabled = false;
        GetComponent<SpriteRenderer>().color = Color.clear;
        DestroyRequested = true;
    }
    /// <summary>
    /// 효과 객체 종료를 요청합니다.
    /// </summary>
    public void RequestEnd()
    {
        if (_animator.parameters.Length > 0)
        {
            EndRequested = true;
        }
        else
        {
            RequestDestroy();
        }
    }
    /// <summary>
    /// AudioSource 컴포넌트의 clip을 설정합니다. AudioSource가 없으면 생성합니다.
    /// </summary>
    /// <param name="audioClip">붙일 clip입니다.</param>
    public void AttachSound(AudioClip audioClip)
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = audioClip;
    }
    /// <summary>
    /// AudioSource에 설정된 효과음을 재생합니다.
    /// </summary>
    public void PlayEffectSound()
    {
        _audioSource.Play();
    }
    /// <summary>
    /// AudioSource의 clip을 설정하고 재생합니다. AudioSource가 없으면 생성합니다.
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlayEffectSound(AudioClip audioClip)
    {
        AttachSound(audioClip);
        PlayEffectSound();
    }

    #endregion
}