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
    /// <summary>
    /// 마찰이 없는 Material입니다. Collider가 미끄러질 수 있게 해줍니다.
    /// </summary>
    public PhysicsMaterial2D _frictionlessWall;

    /// <summary>
    /// 플레이어 리스트입
    /// </summary>
    public PlayerController[] _players;

    /// <summary>
    /// 공용 맵 요소입니다.
    /// </summary>
    public Map _map;
    /// <summary>
    /// 정지 메뉴 관리자입니다.
    /// </summary>
    public PauseMenuManager _pauseMenu;
    /// <summary>
    /// 시간 관리자입니다.
    /// </summary>
    public TimeManager _timeManager;
    /// <summary>
    /// 사용자 인터페이스 관리자입니다.
    /// </summary>
    public UIManager _userInterfaceManager;

    /// <summary>
    /// 보스 전투 관리자입니다.
    /// </summary>
    public BossBattleManager _bossBattleManager;

    /// <summary>
    /// 효과 개체 집합입니다.
    /// </summary>
    public EffectScript[] _effects;

    /// <summary>
    /// 카메라 추적 스크립트입니다.
    /// </summary>
    public CameraFollow1PScript _cameraFollow;

    #endregion





    #region Unity 개체에 대한 참조를 보관합니다.
    /// <summary>
    /// 데이터베이스 개체입니다.
    /// </summary>
    public static DataBase Instance
    {
        get { return GameObject.FindGameObjectWithTag("Database").GetComponent<DataBase>(); }
    }

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
    public PlayerController PlayerX
    {
        get
        {
            return _players[0];
        }
    }
    /// <summary>
    /// 제로 PlayerController입니다.
    /// </summary>
    public PlayerController PlayerZ
    {
        get
        {
            return _players[1];
        }
    }

    /// <summary>
    /// 맵 객체입니다.
    /// </summary>
    public Map Map
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
    public CameraFollow1PScript CameraFollow
    {
        get { return _cameraFollow; }
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
    
    /// <summary>
    /// 폭발 효과 개체를 가져옵니다.
    /// </summary>
    public EffectScript Explosion1Effect
    {
        get { return _effects[0]; }
    }
    /// <summary>
    /// 폭발 효과 개체를 가져옵니다.
    /// </summary>
    public EffectScript Explosion2Effect
    {
        get { return _effects[1]; }
    }
    /// <summary>
    /// 폭발 효과 개체를 가져옵니다.
    /// </summary>
    public EffectScript Explosion3Effect
    {
        get { return _effects[2]; }
    }
    /// <summary>
    /// 폭발 효과 개체를 가져옵니다.
    /// </summary>
    public EffectScript Explosion4Effect
    {
        get { return _effects[3]; }
    }
    /// <summary>
    /// 연속 폭발 효과 개체를 가져옵니다.
    /// </summary>
    public EffectScript MultipleExplosionEffect
    {
        get { return _effects[4]; }
    }

    /// <summary>
    /// 
    /// </summary>
    public CameraZoneParent CameraZoneParent; // { get; set; }
    public CameraZoneBorderParent CameraZoneBorderParent; //{ get; set; }
    public TiledGeometryParent TiledGeometryParent; // { get; set; }
    public InvisibleWallParent InvisibleWallParent; // { get; set; }

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Awake()
    {
        /*
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
        */
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

    #endregion
}
