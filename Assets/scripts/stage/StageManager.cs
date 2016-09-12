using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



/// <summary>
/// 스테이지 장면 관리자입니다.
/// </summary>
public class StageManager : HDSceneManager
{
    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    /// <summary>
    /// Scene 데이터베이스입니다.
    /// </summary>
    public DataBase _database;


    /// <summary>
    /// 준비 애니메이션 관리자입니다.
    /// </summary>
    public ReadyAnimator _ready;
    /// <summary>
    /// 현재 조작중인 플레이어입니다.
    /// </summary>
    public PlayerController _player;


    /// <summary>
    /// 사망 효과 파티클에 대한 스크립트입니다.
    /// </summary>
    public DeadEffectScript _deadEffect;


    /// <summary>
    /// 체크포인트 소환 위치 집합입니다.
    /// </summary>
    public Transform[] _checkpointSpawnPositions;
    /// <summary>
    /// 체크포인트 카메라 존 집합입니다.
    /// </summary>
    public CameraZone[] _checkpointCameraZones;


    /// <summary>
    /// 사용자 인터페이스 관리자입니다.
    /// </summary>
    public UIManager _userInterfaceManager;


    /// <summary>
    /// 테스트 시간입니다.
    /// </summary>
    public float test = 0.1f;
    /// <summary>
    /// 테스트 대미지 값입니다.
    /// </summary>
    public int TestDamageValue = 10;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 맵 객체입니다.
    /// </summary>
    NewMap _map;
    /// <summary>
    /// UnityEngine.Time 관리자입니다.
    /// </summary>
    TimeManager _timeManager;


    /// <summary>
    /// 배경 음악 AudioSource입니다.
    /// </summary>
    AudioSource _bgmSource;


    /// <summary>
    /// 엑스에 대한 PlayerController니다.
    /// </summary>
    PlayerController _playerX;
    /// <summary>
    /// 제로에 대한 PlayerController입니다.
    /// </summary>
    PlayerController _playerZ;


    /// <summary>
    /// 플레이어가 소환되는 위치입니다.
    /// </summary>
    Transform _playerSpawnPosition;


    /// <summary>
    /// 플레이어를 조종할 수 없는 상태라면 참입니다.
    /// </summary>
    bool _isFrozen;


    /// <summary>
    /// 게임이 종료되었다면 참입니다.
    /// </summary>
    bool _gameEnded = false;


    /// <summary>
    /// 
    /// </summary>
    int _gameEndValue = -1;


    #endregion









    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 엑스에 대한 PlayerController니다.
    /// </summary>
    public PlayerController PlayerX
    {
        get { return _playerX; }
    }
    /// <summary>
    /// 제로에 대한 PlayerController입니다.
    /// </summary>
    public PlayerController PlayerZ
    {
        get { return _playerZ; }
    }


    /// <summary>
    /// 플레이어를 조종할 수 없는 상태라면 참입니다.
    /// </summary>
    public bool IsFrozen
    {
        get { return _isFrozen; }
        protected set { _isFrozen = value; }
    }


    /// <summary>
    /// 플레이어가 소환되는 위치입니다.
    /// </summary>
    public Transform PlayerSpawnPosition
    {
        get { return _playerSpawnPosition; }
        set { _playerSpawnPosition = value; }
    }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        base.Start();


        // 필드를 초기화합니다.
        _map = _database.Map;
        _timeManager = _database.TimeManager;
        _bgmSource = GetComponent<AudioSource>();


        // 불러온 캐릭터를 잠깐 사용 불가능하게 합니다.
        _playerX = _database.PlayerX;
        _playerZ = _database.PlayerZ;
        _playerX.gameObject.SetActive(false);
        _playerZ.gameObject.SetActive(false);


        // 맵 데이터를 초기화합니다.
        /// _player.transform.position = _playerSpawnPos.transform.position;
        _playerSpawnPosition = _checkpointSpawnPositions[_database.GameManager.SpawnPositionIndex];


        // 페이드인 효과를 처리합니다.
        _fader.FadeIn();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    protected override void Update()
    {
        /// Handy: 하는 일이 없어서 삭제해도 될 것 같습니다.
        /// base.Update();

        if (_gameEnded)
        {
            if (_fader.FadeOutEnded)
            {
                switch (_gameEndValue)
                {
                    case 2:
                        LoadingSceneManager.LoadLevel("CS03_GaiaFound");
                        break;

                    default:
                        LoadingSceneManager.LoadLevel("StageSelect");
                        break;
                }

                // RestartLevel();
            }

            return;
        }


        // 페이드 인 효과가 종료되는 시점에
        if (_fader.FadeInEnded)
        {
            // 준비 애니메이션 재생을 시작합니다.
            _ready.gameObject.SetActive(true);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            _timeManager.TimeScale = _timeManager.TimeScale == test ? 1 : test;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            _player.Hurt(TestDamageValue);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            while (_player.IsHealthFull() == false)
            {
                _player.Heal();
            }
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            _userInterfaceManager.RequestPauseToggle();
        }
    }


    #endregion










    #region 기능 메서드를 정의합니다.
    /// <summary>
    /// 플레이어를 변경합니다.
    /// </summary>
    /// <param name="newPlayer">변경 이후의 플레이어입니다.</param>
    public void ChangePlayer(PlayerController newPlayer)
    {
        // 이전 플레이어를 비활성화합니다.
        _player.gameObject.SetActive(false);

        // 새 플레이어를 소환합니다.
        newPlayer.transform.position = _player.transform.position;
        newPlayer.RequestSpawn();
        if (_player.FacingRight == false)
        {
            newPlayer.RequestFlip();
        }

        // 관리자 객체의 필드가 새 플레이어를 가리키도록 합니다.
        _map.UpdatePlayer(_player = newPlayer);
    }


    /// <summary>
    /// 화면을 동결시킵니다.
    /// </summary>
    public void Freeze()
    {
        IsFrozen = true;
        _timeManager.StageManagerRequested = true;
    }
    /// <summary>
    /// 화면 동결을 해제합니다.
    /// </summary>
    public void Unfreeze()
    {
        IsFrozen = false;
        _timeManager.StageManagerRequested = false;
    }


    /// <summary>
    /// 스테이지를 재시작합니다.
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    #endregion










    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 아이템의 효과를 발동합니다.
    /// </summary>
    /// <param name="player">플레이어 객체입니다.</param>
    /// <param name="item">플레이어가 사용한 아이템입니다.</param>
    public void ActivateItem(PlayerController player, ItemScript item)
    {
        switch (item.Type)
        {
            case "EndGame":
                Test_EndGameItemGet(player, item);
                break;

            case "1UP":
                GetItem1UP(player, item);
                break;

            case "ECan":
                GetItemECan(player, item);
                break;

            case "WCan":
                GetItemWCan(player, item);
                break;

            case "XCan":
                GetItemXCan(player, item);
                break;

            case "LifeUp":
                GetItemLifeUp(player, item);
                break;

            // 일반적인 경우의 처리입니다.
            default:
                Heal(player, item);
                break;
        }
    }
    /// <summary>
    /// 메인 메뉴로 복귀합니다.
    /// </summary>
    public void BackToMainMenu()
    {
        LoadingSceneManager.LoadLevel("Title");
    }




    /// <summary>
    /// 종료 아이템을 획득했습니다.
    /// </summary>
    /// <param name="player">플레이어 객체입니다.</param>
    /// <param name="item">플레이어가 사용한 아이템입니다.</param>
    private void Test_EndGameItemGet(PlayerController player, ItemScript item)
    {
        RequestStopBackgroundMusic();


        _gameEndValue = item.Value;


        _player.RequestBlockInput();
        _timeManager.StageManagerRequested = true;
        StartCoroutine(EndGameCoroutine());
    }
    /// <summary>
    /// 스테이지 종료 아이템 획득 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator EndGameCoroutine()
    {
        // 다음 커밋에서 삭제할 예정입니다.
        GameManager.Instance.SpawnPositionIndex = 0;



        AudioSource audioSource = AudioSources[3];
        audioSource.Play();

        float startTime = 0f;
        float soundLength = audioSource.clip.length;
        while (startTime < soundLength)
        {
            startTime += Time.unscaledDeltaTime;
        }
        _timeManager.StageManagerRequested = false;



        while (_player.Landed == false)
        {
            yield return new WaitForSeconds(0.1f);
        }

        audioSource = AudioSources[5];
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }



        _player.RequestReturn();
        yield return new WaitForSeconds(0.2f);
        AudioSources[7].Play();

        while (_player.Returning == false)
        {
            yield return new WaitForSeconds(0.1f);
        }



        _gameEnded = true;
        _fader.FadeOut();
        yield break;
    }
    /// <summary>
    /// 배경 음악 재생 중지를 요청합니다.
    /// </summary>
    void RequestStopBackgroundMusic()
    {
        _bgmSource.Stop();
        _database._bossBattleManager.GetComponent<AudioSource>().Stop();
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 플레이어의 체력을 회복합니다.
    /// </summary>
    /// <param name="player">아이템을 사용할 플레이어입니다.</param>
    /// <param name="item">획득한 아이템입니다.</param>
    void Heal(PlayerController player, ItemScript item)
    {
        // 움직임을 정지합니다.
        Freeze();

        // 체력이 회복되는 동안의 루프입니다.
        StartCoroutine(HealRoutine(player, item));
    }
    /// <summary>
    /// 1UP 아이템을 획득합니다.
    /// </summary>
    /// <param name="player">아이템을 사용할 플레이어입니다.</param>
    /// <param name="item">획득한 아이템입니다.</param>
    void GetItem1UP(PlayerController player, ItemScript item)
    {
        AudioSource specialSoundSource = AudioSources[item.SoundEffectIndexes[0]];
        specialSoundSource.Play();


        // 목숨을 하나 증가시킵니다.
        IncreaseTryCount();
    }
    /// <summary>
    /// 라이프 서브탱크 아이템을 획득합니다.
    /// </summary>
    /// <param name="player">아이템을 사용할 플레이어입니다.</param>
    /// <param name="item">획득한 아이템입니다.</param>
    void GetItemECan(PlayerController player, ItemScript item)
    {
        AudioSource specialSoundSource = AudioSources[item.SoundEffectIndexes[0]];
        specialSoundSource.Play();
    }
    /// <summary>
    /// 웨폰 서브탱크 아이템을 획득합니다.
    /// </summary>
    /// <param name="player">아이템을 사용할 플레이어입니다.</param>
    /// <param name="item">획득한 아이템입니다.</param>
    void GetItemWCan(PlayerController player, ItemScript item)
    {
        AudioSource specialSoundSource = AudioSources[item.SoundEffectIndexes[0]];
        specialSoundSource.Play();
    }
    /// <summary>
    /// 엑스트라 라이프탱크 아이템을 획득합니다.
    /// </summary>
    /// <param name="player">아이템을 사용할 플레이어입니다.</param>
    /// <param name="item">획득한 아이템입니다.</param>
    void GetItemXCan(PlayerController player, ItemScript item)
    {
        AudioSource specialSoundSource = AudioSources[item.SoundEffectIndexes[0]];
        specialSoundSource.Play();
    }
    /// <summary>
    /// 엑스트라 라이프탱크 아이템을 획득합니다.
    /// </summary>
    /// <param name="player">아이템을 사용할 플레이어입니다.</param>
    /// <param name="item">획득한 아이템입니다.</param>
    void GetItemLifeUp(PlayerController player, ItemScript item)
    {
        // 움직임을 정지합니다.
        Freeze();

        // 최대 체력이 회복되는 동안의 루프입니다.
        StartCoroutine(IncreaseMaxHealthRoutine(player, item));
    }


    /// <summary>
    /// 회복이 이루어지는 루틴입니다.
    /// </summary>
    /// <param name="player">플레이어 객체입니다.</param>
    /// <param name="item">사용한 아이템입니다.</param>
    /// <returns>Update()를 다시 호출하기 위해 함수를 종료할 때마다 null을 반환합니다.</returns>
    IEnumerator HealRoutine(PlayerController player, ItemScript item)
    {
        // 사용할 변수를 선언합니다.
        float time = 0f;
        float unitTime = 0.02f;
        AudioSource audioSource = AudioSources[item.SoundEffectIndexes[0]];


        // 체력을 회복하는 루프입니다.
        for (int i = 0, len = item._itemValue; i < len; ++i)
        {
            // 루프 진입시마다 시작 시간을 초기화합니다.
            time = 0f;

            // 체력이 가득 찼다면 반복문을 탈출합니다.
            if (player.IsHealthFull())
            {
                break;
            }

            // 체력을 회복하면서 체력 회복 효과음을 재생합니다.
            audioSource.Play();
            audioSource.time = 0;
            player.Heal();

            // 일정한 간격으로 체력을 회복합니다.
            while (time < unitTime)
            {
                time += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        // 정지한 움직임을 해제합니다.
        Unfreeze();

        /// Handy: 예전에는 매번 AudioSource를 생성했기 때문에 이게 필요했는데,
        /// 이제는 StageManager가 기본적으로 가지고 있으므로 삭제하지 않습니다.
        /// 음원 객체를 파괴합니다.
        /// Destroy(audioSource, audioSource.clip.length);

        // 코루틴을 종료합니다.
        yield break;
    }
    /// <summary>
    /// 최대 체력이 증가하는 루틴입니다.
    /// </summary>
    /// <param name="player">플레이어 객체입니다.</param>
    /// <param name="item">사용한 아이템입니다.</param>
    /// <returns>Update()를 다시 호출하기 위해 함수를 종료할 때마다 null을 반환합니다.</returns>
    IEnumerator IncreaseMaxHealthRoutine(PlayerController player, ItemScript item)
    {
        // 사용할 변수를 선언합니다.
        AudioSource specialSoundSource = AudioSources[item.SoundEffectIndexes[0]];
        AudioSource healSoundSource = AudioSources[item.SoundEffectIndexes[1]];
        float time = 0f;
        float firstWaitingTime = specialSoundSource.clip.length * 2;
        float unitTime = 0.02f;


        // 첫 번째 대기 루프입니다.
        specialSoundSource.Play();
        while (time < firstWaitingTime)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        // 최대 체력이 증가하는 루프입니다.
        for (int i = 0, len = item._itemValue; i < len; ++i)
        {
            // 루프 진입시마다 시작 시간을 초기화합니다.
            time = 0f;

            // 최대 체력을 증가시키면서 체력 회복 효과음을 재생합니다.
            healSoundSource.Play();
            healSoundSource.time = 0;
            player.IncreaseMaxHealth();

            // 일정한 간격으로 체력을 회복합니다.
            while (time < unitTime)
            {
                time += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        // 정지한 움직임을 해제합니다.
        Unfreeze();

        /// Handy: 예전에는 매번 AudioSource를 생성했기 때문에 이게 필요했는데,
        /// 이제는 StageManager가 기본적으로 가지고 있으므로 삭제하지 않습니다.
        /// 음원 객체를 파괴합니다.
        /// Destroy(audioSource, audioSource.clip.length);

        // 코루틴을 종료합니다.
        yield break;
    }


    /// <summary>
    /// HUD를 활성화합니다.
    /// </summary>
    public void EnableHUD()
    {
        _userInterfaceManager.ActivatePlayerHUD();
    }
    /// <summary>
    /// 시도 횟수를 증가시킵니다.
    /// </summary>
    void IncreaseTryCount()
    {
        GameManager.Instance.RequestIncreaseTryCount();
        _userInterfaceManager.UpdateTryCountText();
    }


    /// <summary>
    /// 현재 체크포인트의 카메라 존을 획득합니다.
    /// </summary>
    /// <param name="checkpointIndex">현재 체크포인트 인덱스입니다.</param>
    /// <returns>카메라 존입니다.</returns>
    public CameraZone GetCheckpointCameraZone(int checkpointIndex)
    {
        return _checkpointCameraZones[checkpointIndex];
    }


    /// <summary>
    /// 배경 음악 재생을 중지합니다.
    /// </summary>
    public void StopBackgroundMusic()
    {
        _bgmSource.Stop();
    }


    /// <summary>
    /// 플레이어의 움직임 방지를 요청합니다.
    /// </summary>
    public void RequestBlockMoving()
    {
        _player.RequestBlockInput();
    }
    /// <summary>
    /// 플레이어의 움직임 방지 중지를 요청합니다.
    /// </summary>
    internal void RequestUnblockMoving()
    {
        _player.RequestUnblockInput();
    }



    #endregion









    #region 구형 정의를 보관합니다.


    #endregion
}
