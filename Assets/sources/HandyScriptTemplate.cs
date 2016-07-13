using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 한 도영의 스크립트 템플릿입니다.
/// </summary>
public class HandyScriptTemplate : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public string _helloMessage;


    #endregion










    #region 필드를 정의합니다.
    int _count;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        _count = 0;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        Debug.Log(_helloMessage);
        _count++;
    }


    #endregion










    #region 메서드를 정의합니다.


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("구형 정의 테스트입니다.")]
    /// <summary>
    /// 구형 정의 테스트 함수입니다.
    /// </summary>
    void Function()
    {
        Console.WriteLine("Hello, world!");
    }


    #endregion
}
