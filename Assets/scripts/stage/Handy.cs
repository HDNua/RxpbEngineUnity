using System;
using UnityEngine;



/// <summary>
/// 한 도영의 편리한 라이브러리입니다.
/// </summary>
public class Handy
{
    /// <summary>
    /// 디버그 지점을 지정하여 함수를 편하게 찾을 수 있습니다.
    /// </summary>
    public static bool DebugPoint { get { return false; } }


    /// <summary>
    /// 디버그 스트림에 형식화된 문자열을 출력합니다.
    /// </summary>
    /// <param name="format">형식 문자열입니다.</param>
    /// <param name="args">형식 문자열의 인자 목록입니다.</param>
    public static void Log(string format, params object[] args)
    {
        Debug.Log(string.Format(format, args));
    }
}