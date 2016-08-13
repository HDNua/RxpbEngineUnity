using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 정지 화면을 관리합니다.
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// Scene 데이터베이스입니다.
    /// </summary>
    public DataBase _database;


    #endregion










    #region 필드 및 프로퍼티를 정의합니다.
    /// <summary>
    /// 정지된 상태라면 참입니다.
    /// </summary>
    bool _paused = false;


    /// <summary>
    /// UnityEngine.Time 관리자입니다.
    /// </summary>
    TimeManager TimeManager
    {
        get { return _database.TimeManager; }
    }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        /// _timeManager = _database.TimeManager;


        // 필드 초기화 이후 개체를 비활성화합니다.
        /// PauseUI.SetActive(false);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        /**
        // 정지 상태 토글 키를 받습니다.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RequestPauseToggle();
        }
        */

        /**
        // 정지 상태라면
        if (_paused)
        {
            // 정지 화면을 활성화합니다.
            /// PauseUI.SetActive(true);
            gameObject.SetActive(true);
        }
        // 정지 상태가 아니라면
        else
        {
            // 정지 화면을 비활성화합니다.
            /// PauseUI.SetActive(false);
            gameObject.SetActive(true);
        }
        */
    }


    #endregion










    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 일시정지 상태를 전환합니다.
    /// </summary>
    public void RequestPauseToggle()
    {
        if (_paused)
        {
            _paused = false;
            TimeManager.PauseMenuRequested = false;
            gameObject.SetActive(false);
        }
        else
        {
            _paused = true;
            TimeManager.PauseMenuRequested = true;
            gameObject.SetActive(true);
        }
    }


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}