using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[Tiled2Unity.CustomTiledImporter]
/// <summary>
/// 카메라 존을 임포트합니다.
/// </summary>
public class HandyImporter_CameraZone : HandyImporter
{
    #region Tiled2Unity.ICustomTiledImporter 메서드를 선언합니다.
    /// <summary>
    /// 사용자 정의 속성을 다룹니다.
    /// </summary>
    /// <param name="gameObject">대상 GameObject입니다.</param>
    /// <param name="customProperties">사용자 정의 속성 딕셔너리입니다.</param>
    public override void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
    {
        // 자신이 관리할 객체가 아니라면 종료합니다.
        if (IsValid(customProperties) == false)
            return;

        // 사용할 변수를 선언합니다.
        CameraZone component = gameObject.GetComponent<CameraZone>();

        // 속성을 업데이트합니다.
        component._cameraZoneID = GetIntValue(customProperties, "_cameraZoneID");

        // 체크 포인트 속성을 업데이트합니다.
        bool isCheckpoint = GetBooleanValue(customProperties, "_Checkpoint");
        int checkpointIndex = GetIntValue(customProperties, "_CheckpointIndex");
        component._isCheckpoint = isCheckpoint;
        component._checkpointIndex = checkpointIndex;

        // 바운드 속성을 업데이트합니다.
        component._isTopBounded = GetBooleanValue(customProperties, "_isTopBounded");
        component._isTopFirst = GetBooleanValue(customProperties, "_isTopFirst");
        component._isLeftBounded = GetBooleanValue(customProperties, "_isLeftBounded");
        component._isLeftFirst = GetBooleanValue(customProperties, "_isLeftFirst");
        component._isRightBounded = GetBooleanValue(customProperties, "_isRightBounded");
        component._isRightFirst = GetBooleanValue(customProperties, "_isRightFirst");
        component._isBottomBounded = GetBooleanValue(customProperties, "_isBottomBounded");
        component._isBottomFirst = GetBooleanValue(customProperties, "_isBottomFirst");

        // 
        if (isCheckpoint)
        {
            // 
            component.name = "Checkpoint" + checkpointIndex;

            //
            StageManager stageManager = StageManager.Instance;
            stageManager._checkpointCameraZones[checkpointIndex] = gameObject.GetComponent<CameraZone>();
        }
    }
    /// <summary>
    /// 프리팹을 커스터마이징합니다.
    /// </summary>
    /// <param name="prefab">커스터마이징할 프리팹입니다.</param>
    public override void CustomizePrefab(GameObject prefab)
    {
        // Do Nothing
    }


    #endregion



    #region 생성자를 정의합니다.
    /// <summary>
    /// 카메라 존을 임포트합니다.
    /// </summary>
    public HandyImporter_CameraZone() : base("CameraZone")
    {

    }


    #endregion



    #region 메서드를 정의합니다.


    #endregion
}
