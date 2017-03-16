using System;
using UnityEngine;
using System.Collections.Generic;



/// <summary>
/// 준비 애니메이션을 관리합니다.
/// </summary>
public class ReadyAnimator : MonoBehaviour
{
    #region 필드를 정의합니다.
    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    public StageManager _stageManager;


    #endregion


    


    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 예외 메시지 리스트를 생성합니다.
        List<string> exceptionList = new List<string>();

        // 빈 필드가 존재하는 경우 예외 메시지를 추가합니다.
        if (_stageManager == null)
            exceptionList.Add("ReadyAnimator.StageManager == null");

        // 예외 메시지가 하나 이상 존재하는 경우 예외를 발생하고 중지합니다.
        if (exceptionList.Count > 0)
        {
            foreach (string msg in exceptionList)
            {
                Handy.Log("ReadyAnimator Error: {0}", msg);
            }
            throw new Exception("데이터베이스 필드 정의 부족");
        }
    }


    #endregion


    


    #region 프레임 이펙트 핸들러를 정의합니다.
    /// <summary>
    /// 준비 효과음을 재생합니다.
    /// </summary>
    void FE_PlayReadyVoice()
    {
        _stageManager.AudioSources[0].Play();
    }
    /// <summary>
    /// 플레이어를 소환합니다.
    /// </summary>
    void FE_SpawnPlayer()
    {
        // HUD를 활성화 합니다.
        _stageManager.EnableHUD();
        
        // 플레이어 소환을 요청합니다.
        _stageManager._player.transform.position = _stageManager.PlayerSpawnPosition.position;
        _stageManager._player.RequestSpawn();
    }


    #endregion
}