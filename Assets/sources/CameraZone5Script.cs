using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 카메라 존 스크립트입니다.
/// </summary>
public class CameraZone5Script : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public CameraFollowScript _cameraFollow;

    public bool _isTopBounded;
    public bool _isLeftBounded;
    public bool _isRightBounded;
    public bool _isBottomBounded;

    public float _top;
    public float _left;
    public float _right;
    public float _bottom;


    #endregion










    #region 필드를 정의합니다.
    PlayerController _player;
    Camera _mainCamera;
    float _camZ;


    BoxCollider2D _boxZone;
    PolygonCollider2D _slopeZone;


    float _cameraWidth;
    float _cameraHeight;
    float _cameraWidthHalf;
    float _cameraHeightHalf;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        _mainCamera = Camera.main;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _camZ = _mainCamera.transform.position.z;


        // 
        _boxZone = GetComponent<BoxCollider2D>();
        _slopeZone = GetComponent<PolygonCollider2D>();

        if (_boxZone != null)
        {
            _top = (_boxZone.bounds.center.y + _boxZone.bounds.extents.y);
            _left = (_boxZone.bounds.center.x - _boxZone.bounds.extents.x);
            _right = (_boxZone.bounds.center.x + _boxZone.bounds.extents.x);
            _bottom = (_boxZone.bounds.center.y - _boxZone.bounds.extents.y);
        }
        else if (_slopeZone != null)
        {
            Vector2[] slopePoints = _slopeZone.points;
            Vector2[] points = new Vector2[]
            {
                slopePoints[0], // 왼쪽 위
                slopePoints[1], // 오른쪽 위
                slopePoints[3], // 왼쪽 아래
                slopePoints[2], // 오른쪽 아래
            };

            _top = Mathf.Max(points[0].y, points[1].y) * 0.02008f;
            _left = Mathf.Min(points[0].x, points[2].x) * 0.02008f;
            _right = Mathf.Max(points[1].x, points[3].x) * 0.02008f;
            _bottom = Mathf.Min(points[2].y, points[3].y) * 0.02008f;
        }
        //
        else
        {
            throw new Exception("둘 다 아님");
        }


        float frustumHeight = Mathf.Abs(2.0f * _mainCamera.transform.position.z * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        float frustumWidth = Mathf.Abs(frustumHeight * _mainCamera.aspect);
        _cameraWidth = frustumWidth;
        _cameraHeight = frustumHeight;
        _cameraWidthHalf = frustumWidth / 2;
        _cameraHeightHalf = frustumHeight / 2;


        // 상하좌우 값 조정
        _top -= _cameraHeightHalf;
        _left += _cameraWidthHalf;
        _right -= _cameraWidthHalf;
        _bottom += _cameraHeightHalf;
    }


    #endregion









    #region Trigger 관련 메서드를 재정의합니다.
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") == true)
        {
            _cameraFollow.CurrentCameraZone = this;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
    }


    #endregion










    #region 메서드를 정의합니다.


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
