using System;
using UnityEngine;
using System.Collections;



[Obsolete("다른 요소로 대체되었습니다.")]
/// <summary>
/// 카메라가 주인공을 따라갑니다.
/// </summary>
public class CameraFollowScript_dep : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public float smoothTimeX;
    public float smoothTimeY;


    public BoxCollider2D _cameraViewBox;

    public EdgeCollider2D[] _camEdges;


    public Map _map;



    public BoxCollider2D test;
    public PolygonCollider2D test2;


    #endregion










    #region 필드를 정의합니다.
    Camera _camera;

    Rect _pixelRect;
    Rect _rect;


    #endregion









    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    public Rect PixelRect
    {
        get { return _pixelRect; }
    }
    /// <summary>
    /// 
    /// </summary>
    public Rect Rect
    {
        get { return _rect; }
    }


    #endregion










    #region MonoBehaviour 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화 합니다.
    /// </summary>
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();

        _pixelRect = _camera.pixelRect;
        _rect = _camera.rect;


        // 필드 초기화
        /// _map = GetComponentInParent<Map>();


        // 따내기?
        float frustumHeight = Mathf.Abs(2.0f * _camera.transform.position.z * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        float frustumWidth = Mathf.Abs(frustumHeight * _camera.aspect);
        _cameraViewBox.size = new Vector2(frustumWidth, frustumHeight);
        _cameraViewBox.transform.position = new Vector3
            (_camera.transform.position.x, _camera.transform.position.y, 0);


        Bounds camBound = _cameraViewBox.bounds;

        // 엣지 업데이트
        _camEdges[0].points = new Vector2[]
        {
            new Vector2(-_camera.transform.position.x + camBound.center.x - camBound.extents.x, -_camera.transform.position.y + camBound.center.y - camBound.extents.y),
            new Vector2(-_camera.transform.position.x + camBound.center.x + camBound.extents.x, -_camera.transform.position.y + camBound.center.y - camBound.extents.y),
        };
        _camEdges[1].points = new Vector2[]
        {
            new Vector2(-_camera.transform.position.x + camBound.center.x - camBound.extents.x, -_camera.transform.position.y + camBound.center.y + camBound.extents.y),
            new Vector2(-_camera.transform.position.x + camBound.center.x + camBound.extents.x, -_camera.transform.position.y + camBound.center.y + camBound.extents.y),
        };
        _camEdges[2].points = new Vector2[]
        {
            new Vector2(-_camera.transform.position.x + camBound.center.x - camBound.extents.x, -_camera.transform.position.y + camBound.center.y - camBound.extents.y),
            new Vector2(-_camera.transform.position.x + camBound.center.x - camBound.extents.x, -_camera.transform.position.y + camBound.center.y + camBound.extents.y),
        };
        _camEdges[3].points = new Vector2[]
        {
            new Vector2(-_camera.transform.position.x + camBound.center.x + camBound.extents.x, -_camera.transform.position.y + camBound.center.y - camBound.extents.y),
            new Vector2(-_camera.transform.position.x + camBound.center.x + camBound.extents.x, -_camera.transform.position.y + camBound.center.y + camBound.extents.y),
        };



        test.transform.position = new Vector3(test.transform.position.x, test.transform.position.y);
        test2.transform.position = new Vector3(test2.transform.position.x, test2.transform.position.y);

    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {

    }
    /// <summary>
    /// FixedTimestep에 설정된 값에 따라 일정한 간격으로 업데이트 합니다.
    /// 물리 효과가 적용된 오브젝트를 조정할 때 사용됩니다.
    /// (Update는 불규칙한 호출이기 때문에 물리엔진 충돌검사가 제대로 되지 않을 수 있습니다.)
    /// </summary>
    void FixedUpdate()
    {

    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    void LastUpdate()
    {

    }


    #endregion










    #region Collider2D 메서드를 재정의합니다.


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 뷰 포트를 업데이트 합니다.
    /// </summary>
    void UpdateViewport()
    {
        Vector3 playerPos = _map.Player.transform.position;

        _camera.transform.position = new Vector3
            (playerPos.x, playerPos.y, _camera.transform.position.z);
    }


    #endregion









    #region 구형 정의를 보관합니다.


    #endregion
}
