using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[Tiled2Unity.CustomTiledImporter]
/// <summary>
/// 카메라 존 경계 컴포넌트의 속성을 임포트합니다.
/// </summary>
public class HandyImporter_CameraZoneBorder : HandyImporter
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
        CameraZoneBorder component = gameObject.GetComponent<CameraZoneBorder>();
        int fromID = GetIntValue(customProperties, "_fromID");
        int toID = GetIntValue(customProperties, "_toID");
        bool beginTransition = GetBooleanValue(customProperties, "_beginTransition");


        // 속성을 업데이트합니다.
        component._fromID = fromID;
        component._toID = toID;
        component._beginTransition = beginTransition;
        // Log("keys = {0} [{1}/{2}/{3}] ({4})", StringArrayToString(customProperties), fromID, toID, beginTransition, component);
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
    /// 카메라 존 경계 컴포넌트의 속성을 임포트합니다.
    /// </summary>
    public HandyImporter_CameraZoneBorder() : base("CameraZoneBorder")
    {

    }


    #endregion
}
