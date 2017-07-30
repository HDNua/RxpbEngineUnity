using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 카메라 존의 부모 개체입니다.
/// </summary>
public class CameraZoneParent : MonoBehaviour
{
    #region 필드를 초기화합니다.
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

    #endregion



    #region 프로퍼티를 초기화합니다.
    /// <summary>
    /// 카메라 존의 부모 개체입니다.
    /// </summary>
    public static CameraZoneParent Instance
    {
        get
        {
            var go = GameObject.FindGameObjectWithTag("CameraZoneParent");
            if (go == null)
            {
                throw new Exception("It can't be...!!!");
            }

            return go.GetComponent<CameraZoneParent>();
        }
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

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        _database = DataBase.Instance;
        _stageManager = StageManager1P.Instance;
        _cameraFollow = _database.CameraFollow;
    }

    #endregion
}
