using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[Tiled2Unity.CustomTiledImporter]
/// <summary>
/// 컴포넌트를 추가합니다.
/// </summary>
public class HandyImporter_MapParents : HandyImporter
{
    #region Tiled2Unity.ICustomTiledImporter 메서드를 선언합니다.
    /// <summary>
    /// 사용자 정의 속성을 다룹니다.
    /// </summary>
    /// <param name="gameObject">대상 GameObject입니다.</param>
    /// <param name="customProperties">사용자 정의 속성 딕셔너리입니다.</param>
    public override void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
    {
        if (customProperties.ContainsKey("AddComponent") == false)
        {
            string goName = gameObject.name;
            Debug.LogWarning(goName);

            if (goName.Contains("Parent"))
            {
                switch (goName)
                {
                    case "CameraZoneParent":
                        gameObject.AddComponent<CameraZoneParent>();
                        break;

                    case "CameraZoneBorderParent":
                        gameObject.AddComponent<CameraZoneBorderParent>();
                        break;

                    case "TiledGeometryParent":
                        gameObject.AddComponent<TiledGeometryParent>();
                        break;

                    case "InvisibleWallParent":
                        gameObject.AddComponent<InvisibleWallParent>();
                        break;
                }
                gameObject.tag = goName;
            }
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
    /// 컴포넌트를 추가합니다.
    /// </summary>
    public HandyImporter_MapParents() : base("AddComponent")
    {

    }


    #endregion
}
