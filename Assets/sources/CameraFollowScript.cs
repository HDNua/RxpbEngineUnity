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
    public CameraZone _startCameraZone;

    public CameraZone _currentCameraZone_Unity = null;

    public float _unit;

    public GameObject _cameraZoneBorderParent;


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
    CameraZone _currentCameraZone;
    /// <summary>
    /// 카메라 존 집합입니다.
    /// </summary>
    CameraZone[] _cameraZones;


    /// <summary>
    /// 메인 카메라의 Z 좌표입니다.
    /// </summary>
    float _camZ;


    /// <summary>
    /// 카메라 존 변경 중이라면 참입니다.
    /// </summary>
    bool _transitioning = false;
    /// <summary>
    /// 카메라 존 변경 시작으로부터 경과한 시간을 나타냅니다.
    /// </summary>
    float _transitioningTime = 0f;


    #endregion









    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 현재 플레이어가 위치한 카메라 존입니다.
    /// </summary>
    public CameraZone CurrentCameraZone
    {
        get { return _currentCameraZone; }
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
        CameraZone[] cameraZones = _cameraZoneParent.GetComponentsInChildren<CameraZone>();
        _cameraZones = new CameraZone[cameraZones.Length];
        foreach (CameraZone cameraZone in cameraZones)
        {
            _cameraZones[cameraZone._cameraZoneID] = cameraZone;
        }

        CameraZoneBorder[] borders = _cameraZoneBorderParent.GetComponentsInChildren<CameraZoneBorder>();
        foreach (CameraZoneBorder border in borders)
        {
            border._from = _cameraZones[border._fromID];
            border._to = _cameraZones[border._toID];
        }


        _startCameraZone = _cameraZones[0];
        if (_startCameraZone == null)
            throw new Exception("시작 카메라 존이 설정되지 않았습니다.");

        /// _currentCameraZone = _startCameraZone;
        UpdateCameraZone(_startCameraZone, false);
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
    /// 뷰 포트를 업데이트합니다.
    /// </summary>
    void UpdateViewport()
    {
        // 카메라 존 변경 중이라면
        if (_transitioning)
        {
            MoveViewportToPlayer();
        }
        // 그 외의 경우
        else
        {
            SetViewportPosition(_player.transform.localPosition.x, _player.transform.localPosition.y);
        }
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

        // 실제로 카메라의 위치를 플레이어의 위치로 맞춥니다.
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
        float dstX = Mathf.Clamp(curX, xMin, xMax);
        float dstY = Mathf.Clamp(curY, yMin, yMax);

        Vector3 dstPos = new Vector3(dstX, dstY, _camZ);
        Vector3 camPos = _camera.transform.position;
        Vector3 difPos = dstPos - camPos;

        float difX = difPos.x < 0 ? -_unit : _unit;
        float difY = difPos.y < 0 ? -_unit : _unit;
        if (Mathf.Abs(difPos.x) < _unit) difX = 0;
        if (Mathf.Abs(difPos.y) < _unit) difY = 0;


        Vector3 newCamPos = camPos + new Vector3(difX, difY, 0);
        _camera.transform.position = newCamPos;
        if (difX == 0f && difY == 0f)
        {
            _transitioning = false;
        }
    }


    /// <summary>
    /// 카메라 존을 업데이트합니다.
    /// </summary>
    /// <param name="cameraZone"></param>
    public void UpdateCameraZone(CameraZone cameraZone, bool beginTransition)
    {
        _currentCameraZone = cameraZone;
        _currentCameraZone_Unity = _currentCameraZone;


        // 카메라 전이 애니메이션을 시작합니다.
        if (beginTransition)
        {
            _transitioning = true;
            _transitioningTime = 0f;
        }
    }
    /// <summary>
    /// 플레이어가 특정 카메라 존 안에 있는지를 확인합니다.
    /// </summary>
    /// <param name="cameraZone">플레이어가 포함되었는지를 확인할 카메라 존입니다.</param>
    /// <returns>카메라 존 안에 플레이어가 있다면 참입니다.</returns>
    public bool IsInCameraZone(CameraZone cameraZone)
    {
        return _currentCameraZone.GetInstanceID() == cameraZone.GetInstanceID();
    }



    #endregion









    #region 구형 정의를 보관합니다.
    [Obsolete("UpdateCameraZone() 메서드로 대체되었습니다. 다음 커밋에서 삭제할 예정입니다.")]
    /// <summary>
    /// 현재 플레이어가 위치한 카메라 존입니다.
    /// </summary>
    public CameraZone CurrentCameraZone_dep
    {
        set { _currentCameraZone = value; }
    }


    [Obsolete("MoveViewportToPlayer1()에서 사용합니다.")]
    public float _smoothTime;

    [Obsolete("이건 꼭 구형 정의는 아니에요. 나름 멋진 효과입니다.")]
    /// <summary>
    /// 플레이어로 뷰 포트를 맞춥니다.
    /// </summary>
    void MoveViewportToPlayer1()
    {
        float curX = _player.transform.localPosition.x;
        float curY = _player.transform.localPosition.y;
        float xMin = _currentCameraZone._isLeftBounded ? _currentCameraZone._left : float.MinValue;
        float xMax = _currentCameraZone._isRightBounded ? _currentCameraZone._right : float.MaxValue;
        float yMin = _currentCameraZone._isBottomBounded ? _currentCameraZone._bottom : float.MinValue;
        float yMax = _currentCameraZone._isTopBounded ? _currentCameraZone._top : float.MaxValue;
        float dstX = Mathf.Clamp(curX, xMin, xMax);
        float dstY = Mathf.Clamp(curY, yMin, yMax);
        Vector3 dstPos = new Vector3(dstX, dstY, _camZ);

        _transitioningTime = Mathf.Clamp(_transitioningTime + Time.deltaTime, 0f, _smoothTime);
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, dstPos, _transitioningTime / _smoothTime);

        // 카메라 존 전이 시간이 끝났다면 전이 상태를 해제합니다.
        if (_transitioningTime == _smoothTime)
        {
            _transitioning = false;
        }
    }


    #endregion
}
