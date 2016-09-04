using System;
using UnityEngine;



/// <summary>
/// 준비 애니메이션을 관리합니다.
/// </summary>
public class ReadyAnimator : MonoBehaviour
{
    #region 필드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    public StageManager _stageManager;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        /// _stageManager = GetComponentInParent<StageManager>();
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