using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    Animator _animator;
    AudioSource _audioSource;

    #endregion 컨트롤러용 Unity 객체



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public AudioClip[] bgmClips;
    public AudioClip[] seClips;

    #endregion Unity 공용 필드



    #region 필드를 정의합니다.
    AudioSource[] _bgm;
    AudioSource[] _se;
    public AudioSource[] bgm { get { return _bgm; } }
    public AudioSource[] se { get { return _se; } }

    #endregion 필드



    #region MonoBehaviour가 정의하는 기본 메서드를 재정의합니다.
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _bgm = new AudioSource[bgmClips.Length];
        for (int i = 0, len = bgmClips.Length; i < len; ++i)
        {
            _bgm[i] = gameObject.AddComponent<AudioSource>();
            _bgm[i].clip = bgmClips[i];
        }

        _se = new AudioSource[seClips.Length];
        for (int i = 0, len = seClips.Length; i < len; ++i)
        {
            _se[i] = gameObject.AddComponent<AudioSource>();
            _se[i].clip = seClips[i];
        }

        _audioSource.clip = _bgm[0].clip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    void Update()
    {

    }
    void FixedUpdate()
    {

    }

    #endregion MonoBehaviour 기본 메서드 재정의



    #region 행동 메서드를 정의합니다.


    #endregion 행동 메서드



    #region 요청 메서드를 정의합니다.


    #endregion 요청 메서드



    #region 프레임 이벤트 핸들러를 정의합니다.


    #endregion 프레임 이벤트 핸들러



    #region 요청을 수행하기 위한 보조 메서드를 정의합니다.


    #endregion 보조 메서드 정의
}