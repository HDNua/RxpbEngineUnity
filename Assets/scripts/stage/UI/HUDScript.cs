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
    /// 
    /// </summary>
    public GameObject _statusBoardNormal;
    /// <summary>
    /// 
    /// </summary>
    public GameObject _statusBoardWeapon;
    /// <summary>
    /// 
    /// </summary>
    public UnityEngine.UI.Text _statusText;


    /// <summary>
    /// 
    /// </summary>
    public GameObject _healthBoardBody;
    /// <summary>
    /// 
    /// </summary>
    public GameObject _healthBoardHead;
    /// <summary>
    /// 
    /// </summary>
    public GameObject _healthBar;


    /// <summary>
    /// 
    /// </summary>
    public GameObject _manaBoardBody;
    /// <summary>
    /// 
    /// </summary>
    public GameObject _manaBoardHead;
    /// <summary>
    /// 
    /// </summary>
    public GameObject _manaBar;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    StageManager _stageManager;


    /// <summary>
    /// 정상 상태라면 참입니다. 무기 장착 상태라면 거짓입니다.
    /// </summary>
    public bool _isStateNormal = true;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        _stageManager = _database.StageManager;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        PlayerController player = _stageManager._player;
        if (player != null)
        {
            // 체력을 업데이트 합니다.
            Vector3 healthScale = _healthBar.transform.localScale;
            healthScale.y = (float)player.Health / player.MaxHealth;
            _healthBar.transform.localScale = healthScale;


            // 상태 보드를 업데이트합니다.
            UpdateStatusBoard();
            // 무기 장착 상태라면 마나도 업데이트합니다.
            if (_isStateNormal == false)
            {
                Vector3 manaScale = _manaBar.transform.localScale;
                manaScale.y = (float)player.Health / player.MaxHealth;
                _manaBar.transform.localScale = manaScale;
            }
        }
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 상태 텍스트를 업데이트합니다.
    /// </summary>
    public void UpdateStatusText()
    {
        _statusText.text = string.Format("{0:D2}", GameManager.Instance.GameData.TryCount);
    }
    /// <summary>
    /// 상태 텍스트를 업데이트합니다.
    /// </summary>
    /// <param name="text">업데이트할 텍스트입니다.</param>
    public void UpdateStatusText(string text)
    {
        _statusText.text = text;
    }


    /// <summary>
    /// 상태 보드를 업데이트합니다.
    /// </summary>
    void UpdateStatusBoard()
    {
        if (_isStateNormal)
        {
            _manaBar.SetActive(false);
            _manaBoardBody.SetActive(false);
            _manaBoardHead.SetActive(false);
            _statusBoardWeapon.SetActive(false);
        }
        else
        {
            _manaBar.SetActive(true);
            _manaBoardBody.SetActive(true);
            _manaBoardHead.SetActive(true);
            _statusBoardWeapon.SetActive(true);
        }
    }


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
