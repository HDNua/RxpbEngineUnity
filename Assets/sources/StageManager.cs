using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

/// <summary>
/// 스테이지 장면 관리자입니다.
/// </summary>
public class StageManager : HDSceneManager
{
    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
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


        // 화면 정지 상태라면
        if (IsFrozen)
        {

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
    }
    /// <summary>
    /// 화면 동결을 해제합니다.
    /// </summary>
    public void UnFreeze()
    {
        IsFrozen = false;
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
    /// 플레이어의 체력을 회복합니다.
    /// </summary>
    /// <param name="player">체력을 회복할 플레이어입니다.</param>
    /// <param name="point">회복할 체력 양입니다.</param>
    public void RequestHeal(PlayerController player, int point)
    {
        Freeze();
        player.Heal(point);
        UnFreeze();
    }


    #endregion









    #region 구형 정의를 보관합니다.


    #endregion
}
