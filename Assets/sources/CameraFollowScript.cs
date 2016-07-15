using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 카메라가 주인공을 따라갑니다.
/// </summary>
public class CameraFollowScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public NewMap _map;

    public BoxCollider2D _cameraViewBox;
    public GameObject _cameraZoneParent;


    Vector2 _cameraFollowVelocity;
    public float _smoothTimeX;
    public float _smoothTimeY;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// Scene에서 사용할 메인 카메라입니다.
    /// </summary>
    Camera _camera;
    /// <summary>
    /// PlayerController입니다.
    /// </summary>
    PlayerController _player;


    /// <summary>
    /// 현재 플레이어가 위치한 카메라 존입니다.
    /// </summary>
    CameraZone5Script _currentCameraZone;
    /// <summary>
    /// 카메라 존 집합입니다.
    /// </summary>
    CameraZone5Script[] _cameraZones;


    /// <summary>
    /// 메인 카메라의 Z 좌표입니다.
    /// </summary>
    float _camZ;


    #endregion









    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 현재 플레이어가 위치한 카메라 존을 업데이트 합니다.
    /// </summary>
    public CameraZone5Script CurrentCameraZone
    {
        set { _currentCameraZone = value; }
    }


    #endregion










    #region MonoBehaviour 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화 합니다.
    /// </summary>
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        _camZ = _camera.transform.position.z;


        // 일반 필드 초기화
        float frustumHeight = Mathf.Abs(2.0f * _camera.transform.position.z * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        float frustumWidth = Mathf.Abs(frustumHeight * _camera.aspect);
        _cameraViewBox.size = new Vector2(frustumWidth, frustumHeight);
        _cameraViewBox.transform.position = new Vector3
            (_camera.transform.position.x, _camera.transform.position.y, 0);

        _player = _map.Player; // 플레이어 필드 초기화


        // 카메라 존 초기화
        CameraZone5Script[] cameraZones = _cameraZoneParent.GetComponentsInChildren<CameraZone5Script>();
        _cameraZones = cameraZones;
        _currentCameraZone = _cameraZones[0];


    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        UpdateViewport();
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
    void UpdateViewport1()
    {
        Vector3 playerPos = _map.Player.transform.position;

        _camera.transform.position = new Vector3
            (playerPos.x, playerPos.y, _camera.transform.position.z);
    }


    /// <summary>
    /// 뷰 포트를 업데이트합니다.
    /// </summary>
    void UpdateViewport()
    {
        /// SetViewportPosition(_player.transform.localPosition.x, _player.transform.localPosition.y);
        MoveViewportToPlayer();
    }
    /// <summary>
    /// 뷰 포트의 위치를 업데이트합니다.
    /// </summary>
    /// <param name="curX">플레이어의 현재 X 좌표입니다.</param>
    /// <param name="curY">플레이어의 현재 Y 좌표입니다.</param>
    void SetViewportPosition(float curX, float curY)
    {
        float xMin = _currentCameraZone._isLeftBounded ? _currentCameraZone._left : float.MinValue;
        float xMax = _currentCameraZone._isRightBounded ? _currentCameraZone._right : float.MaxValue;
        float yMin = _currentCameraZone._isBottomBounded ? _currentCameraZone._bottom : float.MinValue;
        float yMax = _currentCameraZone._isTopBounded ? _currentCameraZone._top : float.MaxValue;
        float x = Mathf.Clamp(curX, xMin, xMax);
        float y = Mathf.Clamp(curY, yMin, yMax);


        Debug.Log(string.Format("x: [{0:F8}/{1:F8}/{2:F8}], y: [{3:F8}/{4:F8}/{5:F8}]", xMin, x, xMax, yMin, y, yMax));
        _camera.transform.position = new Vector3(x, y, _camZ);
    }
    /// <summary>
    /// 플레이어로 뷰 포트를 맞춥니다.
    /// </summary>
    void MoveViewportToPlayer()
    {
        float curX = _player.transform.localPosition.x;
        float curY = _player.transform.localPosition.y;
        float xMin = _currentCameraZone._isLeftBounded ? _currentCameraZone._left : float.MinValue;
        float xMax = _currentCameraZone._isRightBounded ? _currentCameraZone._right : float.MaxValue;
        float yMin = _currentCameraZone._isBottomBounded ? _currentCameraZone._bottom : float.MinValue;
        float yMax = _currentCameraZone._isTopBounded ? _currentCameraZone._top : float.MaxValue;
        float x = Mathf.Clamp(curX, xMin, xMax);
        float y = Mathf.Clamp(curY, yMin, yMax);
        Vector3 dstPos = new Vector3(x, y, _camZ);

        float posX, posY;
        posX = Mathf.Lerp(_camera.transform.position.x, x, Time.deltaTime);
        posY = Mathf.Lerp(_camera.transform.position.y, y, Time.deltaTime);




//        _camera.transform.position = Vector3.Lerp(_camera.transform.position, dstPos, Time.deltaTime);
        _camera.transform.position = dstPos;

        /*
        posX = Mathf.SmoothDamp(_camera.transform.position.x, x, ref _cameraFollowVelocity.x, _smoothTimeX);
        posY = Mathf.SmoothDamp(_camera.transform.position.y, y, ref _cameraFollowVelocity.y, _smoothTimeY);
        */
        //_camera.transform.position = new Vector3(posX, posY, _camZ);
    }


    #endregion









    #region 구형 정의를 보관합니다.


    #endregion
}
