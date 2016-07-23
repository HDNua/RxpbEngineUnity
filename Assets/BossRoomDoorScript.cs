using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 문(Door) 스크립트입니다.
/// </summary>
public class BossRoomDoorScript : MonoBehaviour
{

    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 문 개폐시 재생될 효과음 리스트입니다.
    /// </summary>
    public AudioClip[] _audioClips;


    /// <summary>
    /// 플레이어가 문을 지나가는 속도입니다.
    /// </summary>
    public float _transitioningSpeed = 1f;
    /// <summary>
    /// 플레이어가 문을 지나가는 시간입니다.
    /// </summary>
    public float _transitioningTime = 2f;


    /// <summary>
    /// 문을 여는 소리가 재생되기 시작할 시간입니다.
    /// </summary>
    public float _openSoundPlayTime = 1f;


    #endregion










    #region Unity를 통해 정의한 필드를 사용 가능한 형태로 보관합니다.
    /// <summary>
    /// 문 개폐시 재생될 효과음 리스트입니다.
    /// </summary>
    AudioSource[] _audioSources;
    /// <summary>
    /// 애니메이션 관리자입니다.
    /// </summary>
    Animator _animator;


    #endregion










    #region 필드 및 프로퍼티를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    bool _opened = false;
    /// <summary>
    /// 
    /// </summary>
    bool Opened
    {
        get { return _opened; }
        set { _animator.SetBool("Opened", _opened = value); }
    }


    /// <summary>
    /// 문을 개방한 플레이어 개체입니다.
    /// </summary>
    PlayerController _player = null;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 
        _animator = GetComponent<Animator>();

        // 
        _audioSources = new AudioSource[_audioClips.Length];
        for (int i = 0; i < _audioClips.Length; ++i)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = _audioClips[i];
            _audioSources[i] = audioSource;
        }
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBevahiour 개체 정보를 업데이트합니다.
    /// </summary>
    void Update()
    {

    }


    #endregion










    #region Trigger 관련 메서드를 재정의합니다.
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (Opened)
            return;

        if (other.CompareTag("Player"))
        {
            _player = other.GetComponent<PlayerController>();
            RequestOpen();
        }
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 문 개방을 요청합니다.
    /// </summary>
    public void RequestOpen()
    {
        _player.RequestBlockInput();
        StartCoroutine(OpenCoroutine());
    }
    /// <summary>
    /// 문 폐쇄를 요청합니다.
    /// </summary>
    public void RequestClose()
    {
        Opened = false;
        _audioSources[1].Play();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenCoroutine()
    {
        bool openSoundPlayed = false;
        float deltaTime = 0f;


        // 
        _audioSources[0].Play();
        Opened = true;
        while (_animator.GetCurrentAnimatorStateInfo(0).IsName("BossRoomDoor_4Opened") == false)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > _openSoundPlayTime && openSoundPlayed == false)
            {
                _audioSources[1].Play();
                openSoundPlayed = true;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (openSoundPlayed == false)
        {
            _audioSources[1].Play();
            openSoundPlayed = true;
        }


        // 
        _player.RequestChangeMovingSpeed(_transitioningSpeed);
        if (_player.FacingRight)
        {
            _player.RequestMoveRight();
        }
        else
        {
            _player.RequestMoveLeft();
        }
        yield return new WaitForSeconds(_transitioningTime);


        // 
        _player.RequestChangeMovingSpeed(_player._walkSpeed);
        _player.RequestStopMoving();
        _player.RequestUnblockInput();
        _player = null;
        RequestClose();
        yield break;
    }


    #endregion
}
