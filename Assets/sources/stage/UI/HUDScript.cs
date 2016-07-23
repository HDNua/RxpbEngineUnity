using System;
using UnityEngine;



/// <summary>
/// HUD(Head Up Display) 스크립트입니다.
/// </summary>
public class HUDScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 데이터베이스입니다.
    /// </summary>
    public DataBase _database;


    /// <summary>
    /// 체력 게이지입니다.
    /// </summary>
    public GameObject _healthGage;
    /// <summary>
    /// 체력 바입니다.
    /// </summary>
    public GameObject _healthBar;
    /// <summary>
    /// 시도 횟수 보드입니다.
    /// </summary>
    public GameObject _tryCountBoard;
    /// <summary>
    /// 시도 횟수 텍스트입니다.
    /// </summary>
    public UnityEngine.UI.Text _tryCountText;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    StageManager _stageManager;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        /**
        Health.SetActive(false);
        HealthBar.SetActive(false);
        TryCountBoard.SetActive(false);
        TryCountText.enabled = false; // .SetActive(false);
        */


        // 필드를 초기화합니다.
        _stageManager = _database.StageManager;
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        PlayerController player = _stageManager._player;
        if (player != null)
        {
            Vector3 healthScale = _healthGage.transform.localScale;
            healthScale.y = (float)player.Health / player.MaxHealth;
            _healthGage.transform.localScale = healthScale;
        }
    }


    #endregion



    #region 메서드를 정의합니다.
    /// <summary>
    /// 시도 횟수 텍스트를 업데이트합니다.
    /// </summary>
    public void UpdateTryCountText()
    {
        _tryCountText.text = string.Format("{0:D2}", GameManager.Instance.GameData.TryCount);
    }


    #endregion
}
