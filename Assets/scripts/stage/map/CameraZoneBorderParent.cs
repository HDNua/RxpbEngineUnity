using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 카메라 존 경계의 부모입니다.
/// </summary>
public class CameraZoneBorderParent : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public static CameraZoneBorderParent Instance
    {
        get
        {
            return GameObject.FindGameObjectWithTag("CameraZoneBorderParent")
                .GetComponent<CameraZoneBorderParent>();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public DataBase _database;

    /// <summary>
    /// 
    /// </summary>
    public CameraFollowScript CameraFollow
    {
        get { return _database.CameraFollow; }
    }
    /// <summary>
    /// 
    /// </summary>
    public StageManager1P StageManager
    {
        get { return StageManager1P.Instance; }
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {

    }
}
