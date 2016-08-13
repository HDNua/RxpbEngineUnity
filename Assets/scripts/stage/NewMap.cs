using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 맵 정보를 보관합니다.
/// </summary>
public class NewMap : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public StageManager _sceneManager;


    #endregion










    #region 필드를 정의합니다.


    #endregion









    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    public PlayerController Player { get { return _sceneManager._player; } }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {

    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 플레이어를 전환합니다.
    /// </summary>
    /// <param name="player">전환할 플레이어입니다.</param>
    public void UpdatePlayer(PlayerController player)
    {
        _sceneManager._player = player;
    }


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("그냥 StageManager 필드 쓰세요. 다음 커밋에서 지웁니다.")]
    public DataBase _database;


    #endregion
}
