using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[Tiled2Unity.CustomTiledImporter]
/// <summary>
/// 카메라 존을 임포트합니다.
/// </summary>
public class HandyImporter_CameraZone : Tiled2Unity.ICustomTiledImporter
{
    /// <summary>
    /// 사용자 정의 속성을 다룹니다.
    /// </summary>
    /// <param name="gameObject">대상 GameObject입니다.</param>
    /// <param name="customProperties">사용자 정의 속성 딕셔너리입니다.</param>
    public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
    {
        if (!customProperties.ContainsKey("AddComponent"))
        {
            //            Debug.Log("HI_CameraZone: does not contains key 'AddComponent'");
            return;
        }
        string componentName = customProperties["AddComponent"];
        if (componentName != "CameraZone")
        {
            //            Debug.Log("HI_CameraZone: component name is " + componentName);
            return;
        }

        //        Debug.Log("HI_CameraZone: TiledGeometryScript.HandleCustomProperties() called");


        if (customProperties.ContainsKey("_isTopBounded"))
        {
            string test = customProperties["_isTopBounded"];
            Debug.Log("HI_CameraZone: _isTopBounded: " + test);
        }
        else
        {
            Debug.Log("HI_CameraZone: keys = " + StringArrayToString(customProperties));
        }
    }



    /// <summary>
    /// 프리팹을 커스터마이징합니다.
    /// </summary>
    /// <param name="prefab">커스터마이징할 프리팹입니다.</param>
    public void CustomizePrefab(GameObject prefab)
    {
        // Do Nothing
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    public string StringArrayToString(IDictionary<string, string> dict)
    {
        string ret = "";
        foreach (string key in dict.Keys)
        {
            ret += string.Format("[{0}:{1}]", key, dict[key]);
        }
        return ret;
    }
}
