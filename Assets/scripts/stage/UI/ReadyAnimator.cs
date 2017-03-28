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
    StageManager _stageManager;

    #endregion


    


    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        _stageManager = StageManager.Instance;
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
        if (_stageManager is StageManager1P)
        {
            StageManager1P stageManager = (StageManager1P)_stageManager;

            // HUD를 활성화 합니다.
            stageManager.EnableHUD();

            // 플레이어 소환을 요청합니다.
            stageManager._player.transform.position = _stageManager._PlayerSpawnPosition.position;
            stageManager._player.RequestSpawn();
        }
        else if (_stageManager is StageManager2P)
        {
            StageManager2P stageManager = (StageManager2P)_stageManager;

            // 
            Vector3 mainSpawnPos = stageManager._PlayerSpawnPosition.position;
            stageManager.MainPlayer.transform.position = mainSpawnPos;
            stageManager.MainPlayer.RequestSpawn();
            stageManager.SubPlayer.transform.position = new Vector3
                (mainSpawnPos.x - 1, mainSpawnPos.y);
            stageManager.SubPlayer.RequestSpawn();
        }
        else
        {
            throw new Exception("알 수 없는 스테이지 관리자 형식");
        }
    }


    #endregion
}