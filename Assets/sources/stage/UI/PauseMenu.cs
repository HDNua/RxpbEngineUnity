using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI;
    bool Paused { get; set; }

    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    void Start()
    {
        PauseUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RequestPauseToggle();
        }

        if (Paused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    #endregion



    #region MyRegion
    /// <summary>
    /// 일시정지를 요청합니다.
    /// </summary>
    public void RequestPause()
    {
        Paused = true;
    }
    /// <summary>
    /// 일시정지 종료를 요청합니다.
    /// </summary>
    public void RequestPauseEnd()
    {
        Paused = false;
    }
    /// <summary>
    /// 일시정지 상태를 전환합니다.
    /// </summary>
    public void RequestPauseToggle()
    {
        Paused = !Paused;
    }

    #endregion
}