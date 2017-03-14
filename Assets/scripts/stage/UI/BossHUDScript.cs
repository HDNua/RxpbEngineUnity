using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 보스 HUD 스크립트입니다.
/// </summary>
public class BossHUDScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 데이터베이스 개체입니다.
    /// </summary>
    public DataBase _database;

    /// <summary>
    /// 체력 바입니다.
    /// </summary>
    public GameObject _healthBar;
    /// <summary>
    /// 체력 바 보드의 머리 부분입니다.
    /// </summary>
    public GameObject _healthBoardHead;
    /// <summary>
    /// 체력 바 보드의 몸통 부분입니다.
    /// </summary>
    public GameObject _healthBoardBody;

    /// <summary>
    /// 
    /// </summary>
    public GameObject _healthTextBoard;
    /// <summary>
    /// 
    /// </summary>
    public UnityEngine.UI.Text _healthText;

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {

    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        EnemyBossScript boss = _database._bossBattleManager._boss;
        if (boss != null)
        {
            // 체력을 업데이트 합니다.
            Vector3 healthScale = _healthBar.transform.localScale;
            healthScale.y = (float)boss.Health / boss.MaxHealth;
            _healthBar.transform.localScale = healthScale;
        }
    }

    #endregion





    #region 구형 정의를 보관합니다.
    [Obsolete("뭔지 모르겠어요.")]
    /// <summary>
    /// 
    /// </summary>
    public GameObject _health;

    #endregion
}
