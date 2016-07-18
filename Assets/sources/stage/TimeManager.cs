using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// UnityEngine.Time 객체 관리자입니다.
/// </summary>
public class TimeManager : MonoBehaviour
{
    #region 필드를 정의합니다.
    /// <summary>
    /// 화면이 정지해있다면 참입니다.
    /// </summary>
    bool _isStopped = false;


    #endregion










    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// PauseMenu가 화면 정지를 요청했습니다.
    /// </summary>
    public bool PauseMenuRequested { get; set; }
    /// <summary>
    /// StageManager가 화면 정지를 요청했습니다.
    /// </summary>
    public bool StageManagerRequested { get; set; }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        _isStopped = (Time.timeScale == 0);
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        UpdateTimeScale();
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// Time.timeScale을 업데이트합니다.
    /// </summary>
    void UpdateTimeScale()
    {
        if (_isStopped)
        {
            if (!PauseMenuRequested && !StageManagerRequested)
            {
                Time.timeScale = 1;
                _isStopped = false;
            }
        }
        else
        {
            if (PauseMenuRequested || StageManagerRequested)
            {
                Time.timeScale = 0;
                _isStopped = true;
            }
        }
    }


    #endregion
}
