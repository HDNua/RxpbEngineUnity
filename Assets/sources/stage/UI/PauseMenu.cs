using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 정지 화면을 관리합니다.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// Scene 데이터베이스입니다.
    /// </summary>
    public DataBase _database;


    /// <summary>
    /// 정지 화면 UI 객체입니다.
    /// </summary>
    public GameObject PauseUI;


    #endregion










    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// UnityEngine.Time 객체 관리자입니다.
    /// </summary>
    TimeManager _timeManager;


    /// <summary>
    /// 정지된 상태라면 참입니다.
    /// </summary>
    bool _paused = false;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        _timeManager = _database.TimeManager;


        // 정지 화면을 비활성화합니다.
        PauseUI.SetActive(false);
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        // 정지 상태 토글 키를 받습니다.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RequestPauseToggle();
        }


        // 정지 상태라면
        if (_paused)
        {
            // 정지 화면을 활성화하고 시간을 멈춥니다.
            PauseUI.SetActive(true);
        }
        // 정지 상태가 아니라면
        else
        {
            // 정지 화면을 비활성화하고 다시 프로그램을 실행합니다.
            PauseUI.SetActive(false);
        }
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
            _timeManager.PauseMenuRequested = false;
        }
        else
        {
            _paused = true;
            _timeManager.PauseMenuRequested = true;
        }
    }


    #endregion



    #region 구형 정의를 보관합니다.
    [Obsolete("다음 커밋에서 삭제할 예정입니다.")]
    /// <summary>
    /// 일시정지를 요청합니다.
    /// </summary>
    public void RequestPause()
    {
        _paused = true;
        _timeManager.PauseMenuRequested = true;
    }
    [Obsolete("다음 커밋에서 삭제할 예정입니다.")]
    /// <summary>
    /// 일시정지 종료를 요청합니다.
    /// </summary>
    public void RequestPauseEnd()
    {
        _paused = false;
        _timeManager.PauseMenuRequested = false;
    }


    #endregion
}