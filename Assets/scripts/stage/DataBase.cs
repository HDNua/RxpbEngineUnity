using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// Scene 데이터베이스입니다.
/// </summary>
public class DataBase : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    // 기본 데이터입니다.
    public PlayerController _playerX;
    public PlayerController _playerZ;


    // Material입니다.
    public PhysicsMaterial2D _frictionlessWall;


    // 공용 맵 요소입니다.
    public NewMap _map;
    public CameraFollowScript _cameraFollow;
    public StageManager _stageManager;
    public PauseMenuManager _pauseMenu;
    public TimeManager _timeManager;
    public UIManager _userInterfaceManager;


    // 보스 전투 관리자입니다.
    public BossBattleManager _bossBattleManager;


    #endregion










    #region 필드를 정의합니다.


    #endregion










    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 게임 관리자입니다.
    /// </summary>
    public GameManager GameManager
    {
        get
        {
            return GameManager.Instance;
        }
    }


    /// <summary>
    /// 엑스 PlayerController입니다.
    /// </summary>
    public PlayerController PlayerX { get { return _playerX; } }
    /// <summary>
    /// 제로 PlayerController입니다.
    /// </summary>
    public PlayerController PlayerZ { get { return _playerZ; } }


    /// <summary>
    /// 맵 객체입니다.
    /// </summary>
    public NewMap Map
    {
        get { return _map; }
    }
    /// <summary>
    /// 마찰 없는 벽을 표현하는 Material입니다.
    /// </summary>
    public PhysicsMaterial2D FrictionlessWall
    {
        get { return _frictionlessWall; }
    }
    /// <summary>
    /// CameraFollow 객체입니다.
    /// </summary>
    public CameraFollowScript CameraFollow
    {
        get { return _cameraFollow; }
    }
    /// <summary>
    /// 스테이지 장면 관리자입니다.
    /// </summary>
    public StageManager StageManager
    {
        get { return _stageManager; }
    }
    /// <summary>
    /// UnityEngine.Time 관리자입니다.
    /// </summary>
    public TimeManager TimeManager
    {
        get { return _timeManager; }
    }


    /// <summary>
    /// 사용자 인터페이스 관리자입니다.
    /// </summary>
    public UIManager UIManager
    {
        get { return _userInterfaceManager; }
    }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 예외 메시지 리스트를 생성합니다.
        List<string> exceptionList = new List<string>();

        // 빈 필드가 존재하는 경우 예외 메시지를 추가합니다.
        if (_map == null)
            exceptionList.Add("DataBase.Map == null");

        // 예외 메시지가 하나 이상 존재하는 경우 예외를 발생하고 중지합니다.
        if (exceptionList.Count > 0)
        {
            foreach (string msg in exceptionList)
            {
                Handy.Log("DataBase Error: {0}", msg);
            }
            throw new Exception("데이터베이스 필드 정의 부족");
        }
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {

    }


    #endregion










    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    public void Test()
    {

    }


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("다음 커밋에서 삭제할 예정입니다.")]
    /// <summary>
    /// 카메라 존 집합의 부모 개체입니다.
    /// </summary>
    public CameraZoneParent _cameraZoneParent;


    [Obsolete("다음 커밋에서 삭제할 예정입니다.")]
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start_dep()
    {
        // 예외 메시지 리스트를 생성합니다.
        List<string> exceptionList = new List<string>();

        // 빈 필드가 존재하는 경우 예외 메시지를 추가합니다.
        if (_map == null)
            exceptionList.Add("DataBase.Map == null");
        if (_cameraZoneParent == null)
            exceptionList.Add("DataBase.CameraZoneParent == null");

        // 예외 메시지가 하나 이상 존재하는 경우 예외를 발생하고 중지합니다.
        if (exceptionList.Count > 0)
        {
            foreach (string msg in exceptionList)
            {
                Handy.Log("DataBase Error: {0}", msg);
            }
            throw new Exception("데이터베이스 필드 정의 부족");
        }


        // 필드를 정의합니다.
        _cameraZoneParent = _map.CameraZoneParent;
    }


    [Obsolete("다음 커밋에서 삭제할 예정입니다.")]
    /// <summary>
    /// 카메라 존의 부모 객체입니다.
    /// </summary>
    public CameraZoneParent CameraZoneParent
    {
        get { return _cameraZoneParent; }
    }


    #endregion
}
