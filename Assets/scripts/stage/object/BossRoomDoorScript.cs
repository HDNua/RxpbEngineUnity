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
    
    /// <summary>
    /// 단 한 번만 사용되는 문이라면 참입니다.
    /// </summary>
    public bool _isOneTimeDoor = false;
    
    /// <summary>
    /// 보스 방 문이라면 참입니다. 진입 시 BossBattleManager에게 스크립트 수행을 요청합니다.
    /// </summary>
    public bool _isBossRoomDoor = false;
    /// <summary>
    /// 보스 전투 관리자입니다.
    /// </summary>
    public BossBattleManager _bossBattleManager;
    
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
    /// 문이 개방되었다면 참입니다.
    /// </summary>
    bool _opened = false;
    /// <summary>
    /// 문이 개방되었다면 참입니다.
    /// </summary>
    bool Opened
    {
        get { return _opened; }
        set { _animator.SetBool("Opened", _opened = value); }
    }
    
    /// <summary>
    /// 문이 개방중이라면 참입니다.
    /// </summary>
    bool Opening
    {
        get;
        set;
    }
    
    /// <summary>
    /// 문을 개방한 플레이어 개체입니다.
    /// </summary>
    PlayerController _player = null;
    
    /// <summary>
    /// 단 한 번만 사용되는 문이 사용되었다면 참입니다.
    /// </summary>
    bool _oneTimeDoorUsed = false;

    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    StageManager _StageManager
    {
        get { return StageManager.Instance; }
    }

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
    /// 충돌체가 트리거 내부로 진입했습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        /**
        if (Opened || _oneTimeDoorUsed)
            return;

        if (other.CompareTag("Player"))
        {
            _player = other.GetComponent<PlayerController>();

            if (_player.Invencible)
            {

            }
            else
            {
                RequestOpen();
            }
        }
        */
    }
    /// <summary>
    /// 충돌체가 여전히 트리거 내부에 있습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Opened || _oneTimeDoorUsed)
            return;
        else if (Opening)
            return;

        // 
        if (other.CompareTag("Player"))
        {
            _player = other.GetComponent<PlayerController>();

            if (_player.Invencible)
            {

            }
            else
            {
                RequestOpen();
            }
        }
    }
    
    #endregion





    #region 메서드를 정의합니다.
    /// <summary>
    /// 문 개방을 요청합니다.
    /// </summary>
    public void RequestOpen()
    {
        if (_isOneTimeDoor)
        {
            _oneTimeDoorUsed = true;
        }

        // 
        _player.RequestBlockInput();
        _StageManager.RequestDisableAllEnemy();
        StartCoroutine(OpenCoroutine());
    }
    /// <summary>
    /// 문 폐쇄를 요청합니다.
    /// </summary>
    public void RequestClose()
    {
        // 한 번만 열리는 문인 경우의 처리입니다.
        if (_isOneTimeDoor)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        // 보스 방인 경우 시나리오를 진행합니다.
        if (_isBossRoomDoor)
        {
            _bossBattleManager.RequestBossBattleScenario();
        }

        // 문의 개방 상태를 모두 내립니다.
        Opened = false;
        Opening = false;

        // 문 닫히는 소리를 재생합니다.
        _audioSources[1].Play();

        // 문을 통과하기 이전의 원래 속도로 되돌립니다.
        _player.RequestChangeMovingSpeed(_player._walkSpeed);
        _player.RequestStopMoving();

        // 보스 방이 아닌 경우에만 입력 중지를 해제합니다.
        if (_isBossRoomDoor == false)
        {
            _player.RequestUnblockInput();
        }
        // 플레이어를 가리키는 포인터를 지웁니다.
        _player = null;

        // 문을 통과한 다음에만 적이 출현할 수 있도록 설정합니다.
        _StageManager.RequestEnableAllEnemy();
    }

    /// <summary>
    /// 문 개방 코루틴입니다.
    /// </summary>
    /// <returns>IEnumerator 반복자를 반환합니다.</returns>
    IEnumerator OpenCoroutine()
    {
        bool openSoundPlayed = false;
        float deltaTime = 0f;

        // 문 개방을 시작합니다.
        _audioSources[0].Play();
        Opened = true;
        Opening = true;

        // 플레이어가 이동하기 위해 문을 여는 과정입니다.
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

        // 플레이어가 이동을 마칠 때까지의 코드입니다.
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

        // 플레이어의 이동이 끝났습니다.
        RequestClose();
        yield break;
    }

    #endregion
}
