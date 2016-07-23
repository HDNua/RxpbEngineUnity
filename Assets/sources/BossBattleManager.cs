using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 보스 전투 관리자입니다.
/// </summary>
public class BossBattleManager : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 데이터베이스입니다.
    /// </summary>
    public DataBase _database;


    /// <summary>
    /// 보스 캐릭터입니다.
    /// </summary>
    public EnemyScript _boss;


    #endregion










    #region 필드를 정의합니다.


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
        if (_boss.IsDead)
        {
            EndBattle();
        }
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 경고 화면을 표시합니다.
    /// </summary>
    void Warning()
    {
        _database.StageManager.StopBackgroundMusic();
        Debug.Log("warning");
    }
    /// <summary>
    /// 전투 전에 캐릭터 간 스크립트를 진행합니다.
    /// </summary>
    void BeginScript()
    {
        Debug.Log("begin script: blah blah blah");
    }
    /// <summary>
    /// 전투를 시작합니다.
    /// </summary>
    void BeginBattle()
    {
        Debug.Log("begin battle");
    }


    /// <summary>
    /// 전투를 끝냅니다.
    /// </summary>
    void EndBattle()
    {
        Debug.Log("end of game");
    }


    /// <summary>
    /// 보스 전투 시나리오 코루틴입니다.
    /// </summary>
    /// <returns>각 단계별로 경과되어야 하는 대기 시간을 반환합니다.</returns>
    IEnumerator BossBattleScenarioCoroutine()
    {
        Warning();
        yield return new WaitForSeconds(8);

        BeginScript();
        yield return new WaitForSeconds(1);

        BeginBattle();
        yield return new WaitForSeconds(2);


        // 보스 전투 시나리오 실행을 마칩니다.
        yield break;
    }


    #endregion










    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 보스 전투 시나리오 실행을 요청합니다.
    /// </summary>
    public void RequestBossBattleScenario()
    {
        StartCoroutine(BossBattleScenarioCoroutine());
    }





    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
