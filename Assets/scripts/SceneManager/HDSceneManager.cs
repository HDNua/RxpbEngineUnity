using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 장면 관리자입니다.
/// </summary>
public class HDSceneManager : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    /// <summary>
    /// 페이드 인/아웃 효과를 담당하는 개체입니다.
    /// </summary>
    public ScreenFader _fader;
    /// <summary>
    /// Scene에서 사용할 효과음의 리스트입니다.
    /// </summary>
    public AudioClip[] _audioClips;


    #endregion









    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// Scene에서 사용할 메인 카메라입니다.
    /// </summary>
    public Camera MainCamera
    {
        get { return Camera.main; }
    }


    #endregion










    #region Unity에서 초기화한 속성을 사용 가능한 형태로 보관합니다.
    /// <summary>
    /// Scene에서 사용할 효과음의 리스트입니다.
    /// </summary>
    AudioSource[] _audioSources;
    /// <summary>
    /// Scene에서 사용할 효과음의 리스트입니다.
    /// </summary>
    public AudioSource[] AudioSources { get { return _audioSources; } }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected virtual void Start()
    {
        // 효과음 리스트를 초기화 합니다.
        _audioSources = new AudioSource[_audioClips.Length];
        for (int i = 0, len = _audioClips.Length; i < len; ++i)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
            _audioSources[i].clip = _audioClips[i];
        }
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    protected virtual void Update()
    {

    }


    #endregion










    #region 메서드를 정의합니다.



    #endregion










    #region 구형 정의를 보관합니다.
    

    #endregion
}
