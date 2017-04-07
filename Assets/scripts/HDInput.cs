using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 한 도영의 입력 클래스입니다.
/// </summary>
public static class HDInput
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool IsAnyKeyDown()
    {
        if (Input.anyKeyDown)
            return true;
        else if (IsLeftKeyDown() || IsRightKeyDown() || IsUpKeyDown() || IsDownKeyDown())
            return true;
        return false;
    }

    /// <summary>
    /// 키가 눌렸는지 확인합니다.
    /// </summary>
    /// <param name="axisName">상태를 확인할 키의 이름입니다.</param>
    /// <returns>키가 눌렸다면 true를 반환합니다.</returns>
    public static bool IsKeyDown(string axisName)
    {
        return Input.GetButtonDown(axisName);
    }
    /// <summary>
    /// 키가 계속 눌린 상태인지 확인합니다.
    /// </summary>
    /// <param name="axisName">눌린 상태인지 확인할 키의 이름입니다.</param>
    /// <returns>키가 눌린 상태라면 true를 반환합니다.</returns>
    public static bool IsKeyPressed(string axisName)
    {
        return Input.GetButton(axisName);
    }
    /// <summary>
    /// 왼쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>왼쪽 키가 눌려있다면 참입니다.</returns>
    public static bool IsLeftKeyPressed()
    {
        return Input.GetAxis("Horizontal") == -1;
    }
    /// <summary>
    /// 오른쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>오른쪽 키가 눌려있다면 참입니다.</returns>
    public static bool IsRightKeyPressed()
    {
        return Input.GetAxis("Horizontal") == 1;
    }
    /// <summary>
    /// 위쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>위쪽 키가 눌려있다면 참입니다.</returns>
    public static bool IsUpKeyPressed()
    {
        return Input.GetAxis("Vertical") == 1;
    }
    /// <summary>
    /// 아래쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>아래쪽 키가 눌려있다면 참입니다.</returns>
    public static bool IsDownKeyPressed()
    {
        return Input.GetAxis("Vertical") == -1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsAxisDown(ref bool axisDown, string axisName, int value)
    {
        if (axisDown == false)
        {
            var axisValue = Input.GetAxis(axisName);
            if (axisValue == value)
            {
                axisDown = true;

                Debug.Log("TEST: " + axisName + ", " + value + ", " + axisValue);
                return true;
            }
        }
        else
        {
            var axisValue = Input.GetAxis(axisName);
            if (axisValue != value)
            {
                Debug.Log("Key Released because of " + axisName + ", " + value + ", " + axisValue);
                axisDown = false;
            }
        }
        return false;
    }
    /// <summary>
    /// 왼쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>왼쪽 키가 눌려있다면 참입니다.</returns>
    public static bool IsLeftKeyDown()
    {
        /// return Input.GetAxis("Horizontal") == -1;
        return IsAxisDown(ref _axisLeftDown, "Horizontal", -1);
    }
    /// <summary>
    /// 오른쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>오른쪽 키가 눌려있다면 참입니다.</returns>
    public static bool IsRightKeyDown()
    {
        /// return Input.GetAxis("Horizontal") == 1;
        return IsAxisDown(ref _axisRightDown, "Horizontal", 1);
    }
    /// <summary>
    /// 위쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>위쪽 키가 눌려있다면 참입니다.</returns>
    public static bool IsUpKeyDown()
    {
        /// return Input.GetAxis("Vertical") == 1;
        return IsAxisDown(ref _axisUpDown, "Vertical", 1);
    }
    /// <summary>
    /// 아래쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>아래쪽 키가 눌려있다면 참입니다.</returns>
    public static bool IsDownKeyDown()
    {
        /// return Input.GetAxis("Vertical") == -1;
        return IsAxisDown(ref _axisDownDown, "Vertical", -1);
    }


    public static bool _axisLeftDown = false;
    public static bool _axisRightDown = false;
    public static bool _axisUpDown = false;
    public static bool _axisDownDown = false;
}
