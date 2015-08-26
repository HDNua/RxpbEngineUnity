using UnityEngine;
using System;
using System.Collections;

[Obsolete("PlayerController로 대체되었습니다.", true)]
public interface IPlayerController
{
    #region MyRegion



    #endregion



    #region MyRegion
    /// <summary>
    /// 플레이어를 스테이지에 소환합니다.
    /// </summary>
    void RequestSpawn();



    #endregion
}