using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 
/// </summary>
public class CameraZoneBorderParent : MonoBehaviour
{
    public DataBase _database;



    #region 프로퍼티를 정의합니다.
    public CameraFollowScript CameraFollow
    {
        get { return _database.CameraFollow; }
    }
    public StageManager StageManager
    {
        get { return _database.StageManager; }
    }


    #endregion



    // Use this for initialization
    void Start()
    {
        /**
        CameraZoneBorder[] children = GetComponentsInChildren<CameraZoneBorder>();
        foreach (CameraZoneBorder child in children)
        {
            child._cameraZoneParent = _database.CameraZoneParent;
        }
        */
    }
}
