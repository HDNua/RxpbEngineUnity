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

    public ReadyAnimator _ready;
    public PlayerController _player;
    public Transform _playerSpawnPos;
    public PlayerController _playerX;
    public PlayerController _playerZ;


    public DeadEffectScript _deadEffect;
    public HUDScript _HUD;


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
    /// 플레이어를 조종할 수 없는 상태라면 참입니다.
    /// </summary>
    bool _isFrozen;


    #endregion









    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 플레이어를 조종할 수 없는 상태라면 참입니다.
    /// </summary>
    public bool IsFrozen
    {
        get { return _isFrozen; }
        protected set { _isFrozen = value; }
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


        // 불러온 캐릭터를 잠깐 사용 불가능하게 합니다.
        _playerX.gameObject.SetActive(false);
        _playerZ.gameObject.SetActive(false);

        // 맵 데이터를 초기화합니다.
        _player.transform.position = _playerSpawnPos.transform.position;

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


        // 페이드 인 효과가 종료되는 시점에
        if (_fader.FadeInEnded)
        {
            // 준비 애니메이션 재생을 시작합니다.
            _ready.gameObject.SetActive(true);
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
                LoadingSceneManager.LoadLevel("CS03_GaiaFound");
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

            // 일반적인 경우의 처리입니다.
            default:
                Heal(player, item);
                break;
        }
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

        // 사용할 변수를 정의합니다.
        AudioSource seSource = gameObject.AddComponent<AudioSource>();
        seSource.clip = item.SoundEffect;

        // 체력이 회복되는 동안의 루프입니다.
        StartCoroutine(HealRoutine(player, item, seSource));
    }
    /// <summary>
    /// 1UP 아이템을 획득합니다.
    /// </summary>
    /// <param name="player">아이템을 사용할 플레이어입니다.</param>
    /// <param name="item">획득한 아이템입니다.</param>
    void GetItem1UP(PlayerController player, ItemScript item)
    {
        // 사용할 변수를 정의합니다.
        AudioSource seSource = gameObject.AddComponent<AudioSource>();
        seSource.clip = item.SoundEffect;


        // 효과음을 재생합니다.
        seSource.Play();
    }
    /// <summary>
    /// 라이프 서브탱크 아이템을 획득합니다.
    /// </summary>
    /// <param name="player">아이템을 사용할 플레이어입니다.</param>
    /// <param name="item">획득한 아이템입니다.</param>
    void GetItemECan(PlayerController player, ItemScript item)
    {
        // 사용할 변수를 정의합니다.
        AudioSource seSource = gameObject.AddComponent<AudioSource>();
        seSource.clip = item.SoundEffect;


        // 효과음을 재생합니다.
        seSource.Play();
    }
    /// <summary>
    /// 웨폰 서브탱크 아이템을 획득합니다.
    /// </summary>
    /// <param name="player">아이템을 사용할 플레이어입니다.</param>
    /// <param name="item">획득한 아이템입니다.</param>
    void GetItemWCan(PlayerController player, ItemScript item)
    {
        // 사용할 변수를 정의합니다.
        AudioSource seSource = gameObject.AddComponent<AudioSource>();
        seSource.clip = item.SoundEffect;


        // 효과음을 재생합니다.
        seSource.Play();
    }


    /// <summary>
    /// 회복이 이루어지는 루틴입니다.
    /// </summary>
    /// <param name="player">회복할 플레이어 객체입니다.</param>
    /// <param name="item">사용한 아이템입니다.</param>
    /// <param name="audioSource">효과음 재생을 위해 추가한 컴포넌트입니다.</param>
    /// <returns>Update()를 다시 호출하기 위해 함수를 종료할 때마다 null을 반환합니다.</returns>
    IEnumerator HealRoutine(PlayerController player, ItemScript item, AudioSource audioSource)
    {
        float time = 0f;
        float unitTime = 0.02f;

        for (int i = 0, len = item._itemValue; i < len; ++i)
        {
            time = 0f;

            // 체력이 가득 찼다면 반복문을 탈출합니다.
            if (player.IsHealthFull())
            {
                break;

                /**
                // 정지한 움직임을 해제합니다.
                Unfreeze();
                yield break;
                */
            }

            // 체력을 회복하면서 체력 회복 효과음을 재생합니다.
            audioSource.Play();
            audioSource.time = 0;
            player.Heal(); // player.Heal(1);

            while (time < unitTime)
            {
                time += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        // 정지한 움직임을 해제합니다.
        Unfreeze();
        // 음원 객체를 파괴합니다.
        Destroy(audioSource, audioSource.clip.length);
        // 코루틴을 종료합니다.
        yield break;
    }


    #endregion









    #region 구형 정의를 보관합니다.


    #endregion
}
