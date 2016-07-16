using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[Tiled2Unity.CustomTiledImporter]
/// <summary>
/// 컴포넌트를 추가합니다.
/// </summary>
public class HandyImporter_CameraZoneBorder : Tiled2Unity.ICustomTiledImporter
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
        if (componentName != "CameraZoneBorder")
        {
            //            Debug.Log("HI_CameraZone: component name is " + componentName);
            return;
        }

        //        Debug.Log("HI_CameraZone: TiledGeometryScript.HandleCustomProperties() called");

        if (customProperties.ContainsKey("_from"))
        {
            string test = customProperties["_from"];
            Debug.Log("HI_CameraZoneBorder: _from: " + test);
        }
        else
        {
            Debug.Log("HI_CameraZoneBorder: keys = " + StringArrayToString(customProperties));
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



    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    void Log(string format, params object[] args)
    {
        Debug.Log(string.Format(format, args));
    }
}
