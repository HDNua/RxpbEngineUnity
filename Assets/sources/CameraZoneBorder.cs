﻿using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 카메라 존의 경계를 표현하는 객체입니다.
/// </summary>
public class CameraZoneBorder : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public CameraZoneParent _cameraZoneParent;

    public CameraZone _from;
    public CameraZone _to;


    #endregion










    #region 필드를 정의합니다.
    CameraFollowScript _cameraFollow;

    bool _isHorizontal;
    float _border;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        _cameraFollow = _cameraZoneParent.CameraFollow;


        // 경계선 객체를 획득합니다.
        EdgeCollider2D border = GetComponent<EdgeCollider2D>();
        Vector2[] points = border.points;

        // X 좌표가 같다면 수평 방향을 관리하는 경계입니다.
        if (points[0].x == points[1].x)
        {
            _isHorizontal = true;
            _border = points[0].x + transform.position.x;
        }
        // Y 좌표가 같다면 수직 방향을 관리하는 경계입니다.
        else if (points[0].y == points[1].y)
        {
            _isHorizontal = false;
            _border = points[1].y + transform.position.y;
        }
        // 그 외의 경우 예외 처리합니다.
        else
            throw new Exception("타당하지 않은 CameraZoneBorder points 속성입니다.");
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        
    }


    #endregion









    #region Trigger 관련 메서드를 재정의합니다.
    /// <summary>
    /// 충돌체가 여전히 트리거 내부에 있습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        // 플레이어가 아니라면 동작하지 않습니다.
        if (other.CompareTag("Player") == false)
            return;

        // 사용할 변수를 먼저 획득합니다.
        Vector3 playerPos = _cameraZoneParent.Player.transform.position;
        EdgeCollider2D border = GetComponent<EdgeCollider2D>();
        Vector2[] points = border.points;


        // 수평 방향의 전환이라면
        if (_isHorizontal)
        {
            /**
            int curCamZoneID = _cameraFollow.CurrentCameraZone.GetInstanceID();
            int fromCamZoneID = _from.GetInstanceID();
            int toCamZoneID = _to.GetInstanceID();
            */

            if (playerPos.x < _border && _cameraFollow.IsInCameraZone(_to))
            {
                SwapCameraZone();
            }
            else if (playerPos.x > _border && _cameraFollow.IsInCameraZone(_from))
            {
                SwapCameraZone();
            }
            else
            {

            }
        }
        // 수직 방향의 전환이라면
        else
        {
            if (playerPos.y < _border && _cameraFollow.IsInCameraZone(_from))
            {
                SwapCameraZone();
            }
            else if (playerPos.y > _border && _cameraFollow.IsInCameraZone(_to))
            {
                SwapCameraZone();
            }
        }
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 카메라 존을 교체합니다.
    /// </summary>
    void SwapCameraZone()
    {
        if (_cameraFollow.IsInCameraZone(_to))
        {
            Debug.Log(string.Format("CameraZone Updated from ({0}) to ({1}).", _to, _from));
            _cameraFollow.UpdateCameraZone(_from);
        }
        else
        {
            Debug.Log(string.Format("CameraZone Updated from ({0}) to ({1}).", _from, _to));
            _cameraFollow.UpdateCameraZone(_to);
        }

        /**
        _cameraFollow.UpdateCameraZone(_to);
        CameraZone tmp = _to;
        _to = _from;
        _from = tmp;
        */
    }


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("OnTriggerStay2D()로 대체되었습니다.")]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.CompareTag("Player") == true)
        {
            _cameraFollow.CurrentCameraZone_dep = null; // this;
        }
        */
    }
    [Obsolete("OnTriggerStay2D()로 대체되었습니다.")]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {

    }


    #endregion
}