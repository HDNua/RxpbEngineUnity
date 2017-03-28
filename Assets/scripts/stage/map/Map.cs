using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 맵 정보를 보관합니다.
/// </summary>
public class Map : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.

    #endregion

    


    
    #region 필드를 정의합니다.
    /// <summary>
    /// 카메라 존 집합의 부모 개체입니다.
    /// </summary>
    CameraZoneParent _cameraZoneParent;

    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    StageManager _stageManager;

    #endregion





    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 맵 개체입니다.
    /// </summary>
    public static Map Instance
    {
        get { return GameObject.FindGameObjectWithTag("Map").GetComponent<Map>(); }
    }

    /// <summary>
    /// 현재 활동중인 플레이어를 획득합니다.
    /// </summary>
    public PlayerController Player
    {
        get
        {
            // return _stageManager._player;
            return StageManager.Instance._player;
        }
    }

    /// <summary>
    /// 카메라 존 집합의 부모 개체입니다.
    /// </summary>
    public CameraZoneParent CameraZoneParent { get { return _cameraZoneParent; } }
    
    #endregion
    




    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        /**
        // 예외 메시지 리스트를 생성합니다.
        List<string> exceptionList = new List<string>();

        // 빈 필드가 존재하는 경우 예외 메시지를 추가합니다.
        if (_stageManager == null)
            exceptionList.Add("Map.StageManager == null");

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

        // 필드를 초기화합니다.
        _stageManager = StageManager.Instance;
        _cameraZoneParent = GetComponentInChildren<CameraZoneParent>();
    }
    
    #endregion
    




    #region 메서드를 정의합니다.
    /// <summary>
    /// 플레이어를 전환합니다.
    /// </summary>
    /// <param name="player">전환할 플레이어입니다.</param>
    public void UpdatePlayer(PlayerController player)
    {
        _stageManager._player = player;
    }


    #endregion

    



    #region 구형 정의를 보관합니다.


    #endregion
}
