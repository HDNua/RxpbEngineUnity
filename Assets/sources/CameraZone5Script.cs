using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 카메라 존 스크립트입니다.
/// </summary>
public class CameraZone5Script : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public CameraZoneParent _cameraZoneParent;
//    public CameraFollowScript _cameraFollow;
//    public Map _map;

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
    CameraFollowScript _cameraFollow;


    Camera _mainCamera;


    BoxCollider2D _boxZone;
    PolygonCollider2D _slopeZone;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        _mainCamera = Camera.main;

        if (_cameraZoneParent == null)
        {
            Console.WriteLine();
        }

        /// _player = _cameraZoneParent.Player; /// GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _cameraFollow = _cameraZoneParent.CameraFollow;
        /// _camZ = _mainCamera.transform.position.z;


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
            /**
            Vector2[] slopePoints = _slopeZone.points;
            Vector2[] points = new Vector2[]
            {
                slopePoints[0], // 왼쪽 위
                slopePoints[1], // 오른쪽 위
                slopePoints[3], // 왼쪽 아래
                slopePoints[2], // 오른쪽 아래
            };
            */
            Vector2[] points = GetTetragonPoints(_slopeZone.points);
            float originX = transform.localPosition.x, originY = transform.localPosition.y;

            _top = (originY + Mathf.Max(points[0].y, points[1].y)) * 0.02008f;
            _left = (originX + Mathf.Min(points[0].x, points[2].x)) * 0.02008f;
            _right = (originX + Mathf.Max(points[1].x, points[3].x)) * 0.02008f;
            _bottom = (originY + Mathf.Min(points[2].y, points[3].y)) * 0.02008f;
        }
        //
        else
        {
            throw new Exception("둘 다 아님");
        }


        float frustumHeight = Mathf.Abs(2.0f * _mainCamera.transform.position.z * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        float frustumWidth = Mathf.Abs(frustumHeight * _mainCamera.aspect);
        float cameraWidthHalf = frustumWidth / 2;
        float cameraHeightHalf = frustumHeight / 2;


        // 상하좌우 값 조정
        _top -= cameraHeightHalf;
        _left += cameraWidthHalf;
        _right -= cameraWidthHalf;
        _bottom += cameraHeightHalf;


        // 마지막 상하좌우값 조정
        if (_top < _bottom)
        {
            float mid = (_top + _bottom) / 2;
            _top = _bottom = mid;
        }
        if (_right < _left)
        {
            float mid = (_left + _right) / 2;
            _left = _right = mid;
        }
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
    /// <summary>
    /// 임의의 사각형 좌표로부터, "왼쪽 위, 오른쪽 위, 왼쪽 아래, 오른쪽 아래"로 정렬된 좌표 배열을 획득합니다.
    /// </summary>
    /// <param name="points">사각형의 꼭짓점 좌표 배열입니다.</param>
    /// <returns>"왼쪽 위, 오른쪽 위, 왼쪽 아래, 오른쪽 아래"로 정렬된 좌표 배열을 획득합니다.</returns>
    Vector2[] GetTetragonPoints(Vector2[] points)
    {
        Vector2[] ret = new Vector2[4];

        // 절차:
        // 1. 네 꼭짓점의 평균값을 구한다.
        // 2. (꼭짓점 좌표 - 평균점 좌표)의 결과로 해당 꼭짓점을 알 수 있다.
        // (1) (-,-) : 왼쪽 아래
        // (2) (+,-) : 오른쪽 아래
        // (3) (-,+) : 왼쪽 위
        // (4) (+,+) : 오른쪽 위
        Vector2 center = (points[0] + points[1] + points[2] + points[3]) / 4;
        int dst;
        for (int i = 0; i < 4; ++i)
        {
            Vector2 dif = points[i] - center;
            if (dif.x < 0 && dif.y < 0) dst = 2;
            else if (dif.x > 0 && dif.y < 0) dst = 3;
            else if (dif.x < 0 && dif.y > 0) dst = 0;
            else if (dif.x > 0 && dif.y > 0) dst = 1;
            else throw new Exception("타당하지 않은 사각형 좌표 배열입니다.");

            // 결과 배열의 값을 업데이트합니다.
            ret[dst] = points[i];
        }
        return ret;
    }


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("필요없을 것 같습니다.")]
    PlayerController _player;


    [Obsolete("필요없을 것 같습니다.")]
    float _cameraWidth;
    [Obsolete("필요없을 것 같습니다.")]
    float _cameraHeight;
    [Obsolete("필요없을 것 같습니다.")]
    float _cameraWidthHalf;
    [Obsolete("필요없을 것 같습니다.")]
    float _cameraHeightHalf;


    [Obsolete("필요없을 것 같습니다.")]
    float _camZ;


    [Obsolete("이전 정의입니다.")]
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start_dep()
    {
        // 필드를 초기화합니다.
        _mainCamera = Camera.main;

        if (_cameraZoneParent == null)
        {
            Console.WriteLine();
        }

        _player = _cameraZoneParent.Player; /// GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _cameraFollow = _cameraZoneParent.CameraFollow;
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


        _cameraHeight = Mathf.Abs(2.0f * _mainCamera.transform.position.z * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        _cameraWidth = Mathf.Abs(_cameraHeight * _mainCamera.aspect);
        _cameraWidthHalf = _cameraWidth / 2;
        _cameraHeightHalf = _cameraHeight / 2;


        // 상하좌우 값 조정
        _top -= _cameraHeightHalf;
        _left += _cameraWidthHalf;
        _right -= _cameraWidthHalf;
        _bottom += _cameraHeightHalf;


        // 마지막 상하좌우값 조정
        if (_top < _bottom)
        {
            float mid = (_top + _bottom) / 2;
            _top = _bottom = mid;
        }
        if (_right < _left)
        {
            float mid = (_left + _right) / 2;
            _left = _right = mid;
        }

        Console.WriteLine(_player);
        Console.WriteLine(_camZ);
    }


    #endregion
}
