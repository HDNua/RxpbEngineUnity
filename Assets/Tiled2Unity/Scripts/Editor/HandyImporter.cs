using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[Tiled2Unity.CustomTiledImporter]
/// <summary>
/// Tiled2Unity 도구를 이용하여 임포트를 수행합니다.
/// </summary>
public abstract class HandyImporter : Tiled2Unity.ICustomTiledImporter
{
    #region Tiled2Unity.ICustomTiledImporter 메서드를 선언합니다.
    /// <summary>
    /// 사용자 정의 속성을 다룹니다.
    /// </summary>
    /// <param name="gameObject">대상 GameObject입니다.</param>
    /// <param name="customProperties">사용자 정의 속성 딕셔너리입니다.</param>
    public abstract void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties);
    /// <summary>
    /// 프리팹을 커스터마이징합니다.
    /// </summary>
    /// <param name="prefab">커스터마이징할 프리팹입니다.</param>
    public abstract void CustomizePrefab(GameObject prefab);


    #endregion

    

    #region 필드 및 프로퍼티를 정의합니다.
    string _targetName;
    /// <summary>
    /// 타겟의 이름입니다.
    /// </summary>
    public string TargetName { get { return _targetName; } }


    #endregion


    
    #region 생성자를 정의합니다.
    /// <summary>
    /// Tiled2Unity 도구를 이용하여 임포트를 수행합니다.
    /// </summary>
    /// <param name="targetName">타겟 형식의 이름입니다.</param>
    public HandyImporter(string targetName)
    {
        _targetName = targetName;
    }


    #endregion


    
    #region 메서드를 정의합니다.
    /// <summary>
    /// 문자열 배열을 문자열로 변환하여 반환합니다.
    /// </summary>
    /// <param name="dict"></param>
    /// <returns>문자열 배열을 문자열로 변환하여 반환합니다.</returns>
    protected string StringArrayToString(IDictionary<string, string> dict)
    {
        string ret = "";
        foreach (string key in dict.Keys)
        {
            ret += string.Format("[{0}:{1}]", key, dict[key]);
        }
        return ret;
    }
    /// <summary>
    /// 형식화된 문자열을 디버그 스트림에 출력합니다.
    /// </summary>
    /// <param name="format">형식화된 문자열입니다.</param>
    /// <param name="args">형식화된 문자열의 인자입니다.</param>
    protected void Log(string format, params object[] args)
    {
        string front = "HI_" + _targetName + ": ";
        Debug.Log(front + string.Format(format, args));
    }


    /// <summary>
    /// 자신이 관리할 객체라면 참입니다.
    /// </summary>
    /// <param name="customProperties">사용자 정의 속성 딕셔너리입니다.</param>
    /// <returns>자신이 관리할 객체라면 참입니다.</returns>
    protected bool IsValid(IDictionary<string, string> customProperties)
    {
        if (!customProperties.ContainsKey("AddComponent"))
        {
            return false;
        }
        string componentName = customProperties["AddComponent"];
        if (componentName != _targetName)
        {
            return false;
        }

        // 자신이 관리할 객체임을 확인했습니다.
        return true;
    }
    /// <summary>
    /// 자신이 관리할 객체라면 참입니다.
    /// </summary>
    /// <param name="customProperties">사용자 정의 속성 딕셔너리입니다.</param>
    /// <param name="targetName">비교할 타겟 이름입니다.</param>
    /// <returns>자신이 관리할 객체라면 참입니다.</returns>
    protected bool IsValid(IDictionary<string, string> customProperties, string targetName)
    {
        if (!customProperties.ContainsKey("AddComponent"))
        {
            return false;
        }
        string componentName = customProperties["AddComponent"];
        if (componentName != targetName)
        {
            return false;
        }

        // 자신이 관리할 객체임을 확인했습니다.
        return true;
    }


    /// <summary>
    /// 사용자 정의 프로퍼티 딕셔너리로부터 부울 값을 획득합니다.
    /// </summary>
    /// <param name="customProperties">값을 획득할 딕셔너리입니다.</param>
    /// <param name="key">값을 획득하기 위한 키 값입니다.</param>
    /// <returns>소문자 문자열이 "true"라면 참, 아니면 거짓을 반환합니다.</returns>
    protected bool GetBooleanValue(IDictionary<string, string> customProperties, string key)
    {
        if (customProperties.ContainsKey(key) == false)
            return false;

        string value = customProperties[key];
        if (value.ToLower() == "true")
            return true;

        // 일반적으로 거짓을 반환합니다.
        return false;
    }
    /// <summary>
    /// 사용자 정의 프로퍼티 딕셔너리로부터 문자열 값을 획득합니다.
    /// </summary>
    /// <param name="customProperties">값을 획득할 딕셔너리입니다.</param>
    /// <param name="key">값을 획득하기 위한 키 값입니다.</param>
    /// <returns>키가 없으면 null을, 그 외의 경우 키에 대한 값을 반환합니다.</returns>
    protected string GetStringValue(IDictionary<string, string> customProperties, string key)
    {
        if (customProperties.ContainsKey(key) == false)
            return null;

        // 키에 대한 값을 반환합니다.
        return customProperties[key];
    }
    /// <summary>
    /// 사용자 정의 프로퍼티 딕셔너리로부터 정수 값을 획득합니다.
    /// </summary>
    /// <param name="customProperties">값을 획득할 딕셔너리입니다.</param>
    /// <param name="key">값을 획득하기 위한 키 값입니다.</param>
    /// <returns>키가 없거나 파싱에 실패하면 int.MinValue를, 그 외의 경우 정수를 반환합니다.</returns>
    protected int GetIntValue(IDictionary<string, string> customProperties, string key)
    {
        if (customProperties.ContainsKey(key) == false)
            return int.MinValue;
        
        try
        {
            // 키에 대한 값을 반환합니다.
            return int.Parse(customProperties[key]);
        }
        catch (Exception)
        {
            return int.MinValue;
        }
    }


    #endregion
}
