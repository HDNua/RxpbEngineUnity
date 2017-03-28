using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 카메라 존의 부모 개체입니다.
/// </summary>
public class CameraZoneParent : MonoBehaviour
{
    /// <summary>
    /// 장면 관리자입니다.
    /// </summary>
    StageManager1P _stageManager;
    /// <summary>
    /// 데이터베이스입니다.
    /// </summary>
    DataBase _database;
    /// <summary>
    /// CameraFollow 스크립트입니다.
    /// </summary>
    CameraFollow1PScript _cameraFollow;

    /// <summary>
    /// 카메라 존의 부모 개체입니다.
    /// </summary>
    public static CameraZoneParent Instance
    {
        get { return GameObject.FindGameObjectWithTag("CameraZoneParent")
                .GetComponent<CameraZoneParent>(); }
    }

    /// <summary>
    /// 현재 행동중인 플레이어를 가져옵니다.
    /// </summary>
    public PlayerController Player
    {
        get { return _stageManager._player; }
    }
    /// <summary>
    /// CameraFollow 객체입니다.
    /// </summary>
    public CameraFollow1PScript CameraFollow
    {
        get { return _cameraFollow; }
    }

    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        /*
        // 예외 메시지 리스트를 생성합니다.
        List<string> exceptionList = new List<string>();

        // 빈 필드가 존재하는 경우 예외 메시지를 추가합니다.
        if (_stageManager == null)
            exceptionList.Add("CameraZoneParent.StageManager == null");
        if (_database == null)
            exceptionList.Add("CameraZoneParent.DataBase == null");

        // 예외 메시지가 하나 이상 존재하는 경우 예외를 발생하고 중지합니다.
        if (exceptionList.Count > 0)
        {
            foreach (string msg in exceptionList)
            {
                Handy.Log("CameraZoneParent Error: {0}", msg);
            }
            throw new Exception("데이터베이스 필드 정의 부족");
        }
        */

        // 필드를 초기화합니다.
        _database = DataBase.Instance;
        _stageManager = StageManager1P.Instance;
        _cameraFollow = _database.CameraFollow;
    }
}
