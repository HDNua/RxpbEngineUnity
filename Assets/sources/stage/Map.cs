using UnityEngine;
using System.Collections;
using System;



/// <summary>
/// 맵을 정의합니다.
/// </summary>
public class Map : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    /// <summary>
    /// 플레이어 필드입니다.
    /// </summary>
    PlayerController _player;
    /// <summary>
    /// 플레이어 필드를 업데이트 합니다.
    /// </summary>
    public PlayerController Player { set { _player = value; } }


    #endregion 










    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 메인 카메라입니다.
    /// </summary>
    public Camera _mainCamera;
    /// <summary>
    /// 카메라 존 배열입니다.
    /// </summary>
    public CameraZoneScript[] _cameraZones;


    public BoxCollider2D _cameraBox;
    public PolygonCollider2D _cameraBoxZone;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 현재 플레이어가 속한 카메라 존입니다.
    /// </summary>
    CameraZoneScript _cameraZone;


    /// <summary>
    /// 카메라 존의 가로 길이입니다.
    /// </summary>
    float czWidth;
    /// <summary>
    /// 카메라 존의 가로 길이 최솟값입니다.
    /// </summary>
    float czHorMin;
    /// <summary>
    /// 카메라 존의 가로 길이 최댓값입니다.
    /// </summary>
    float czHorMax;
    /// <summary>
    /// 카메라 존의 세로 길이입니다.
    /// </summary>
    float czHeight;
    /// <summary>
    /// 카메라 존의 세로 길이 최솟값입니다.
    /// </summary>
    float czVerMin;
    /// <summary>
    /// 카메라 존의 세로 길이 최댓값입니다.
    /// </summary>
    float czVerMax;


    #endregion










    #region MonoBehaviour가 정의하는 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화 합니다.
    /// </summary>
    void Start()
    {
        _cameraZone = _cameraZones[0];
        UpdateCameraZoneBounds();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        if (_player != null)
        {
            // 뷰 포트를 맞춥니다.
            SetViewportCenter();
        }
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










    #region 메서드를 정의합니다.
    /// <summary>
    /// 뷰 포트를 가운데로 맞춥니다.
    /// </summary>
    void SetViewportCenter()
    {
        /*
        Vector3 playerPos = _player.transform.position;
        _mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y, _mainCamera.transform.position.z);
        return;
        */

        /*
        float czLeft = _cameraZone_dep.bounds.min.x;
        float czRight = _cameraZone_dep.bounds.max.x;
        float playerX = _player.transform.position.x;
        float czTop = _cameraZone_dep.bounds.max.y;
        float czBottom = _cameraZone_dep.bounds.min.y;
        float playerY = _player.transform.position.y;
        */


        float czLeft = _cameraZone.minX;
        float czRight = _cameraZone.maxX;
        float viewCenterX = _mainCamera.transform.position.x;
        float czTop = _cameraZone.minY;
        float czBottom = _cameraZone.maxY;
        float viewCenterY = _mainCamera.transform.position.y;
        float playerX = _player.transform.position.x;
        float playerY = _player.transform.position.y;


        // 테스트
        bool movePos = false;
        var newPos = _mainCamera.transform.position;
        if (czHorMin < playerX && playerX < czHorMax)
        {
            newPos.x = playerX;
            movePos = true;
        }
        if (czVerMin < playerY && playerY < czVerMax)
        {
            newPos.y = playerY;
            movePos = true;
        }


        // 화면이 움직였다면 업데이트합니다.
        if (movePos)
        {
            _mainCamera.transform.position = newPos;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void UpdateCameraZoneBounds()
    {
        float czLeft = _cameraZone.minX;
        float czRight = _cameraZone.maxX;
        float viewCenterX = _mainCamera.transform.position.x;
        float czTop = _cameraZone.minY;
        float czBottom = _cameraZone.maxY;
        float viewCenterY = _mainCamera.transform.position.y;


        // 필드를 업데이트 합니다.
        czWidth = viewCenterX - czLeft;
        czHorMin = czLeft + czWidth;
        czHorMax = czRight - czWidth;

        czHeight = viewCenterY - czBottom;
        czVerMin = czBottom + czHeight;
        czVerMax = czTop - czHeight;
    }


    #endregion










    #region public 메서드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cameraZone"></param>
    public void UpdateCameraZone(CameraZoneScript cameraZone)
    {
        _cameraZone = cameraZone;
        UpdateCameraZoneBounds();
    }


    #endregion









    #region 구형 정의를 보관합니다.
    [Obsolete()]
    /// <summary>
    /// 카메라 존 배열입니다.
    /// </summary>
    public BoxCollider2D[] _cameraZones_dep;
    [Obsolete()]
    /// <summary>
    /// 현재 플레이어가 속한 카메라 존입니다.
    /// </summary>
    BoxCollider2D _cameraZone_dep;


    [Obsolete()]
    /// <summary>
    /// MonoBehaviour 개체를 초기화 합니다.
    /// </summary>
    void Start_dep()
    {
        _cameraZone_dep = _cameraZones_dep[0];

        float czLeft = _cameraZone_dep.bounds.min.x;
        float czRight = _cameraZone_dep.bounds.max.x;
        float viewCenterX = _mainCamera.transform.position.x;

        float czTop = _cameraZone_dep.bounds.max.y;
        float czBottom = _cameraZone_dep.bounds.min.y;
        float viewCenterY = _mainCamera.transform.position.y;


        // 필드를 업데이트 합니다.
        czWidth = viewCenterX - czLeft;
        czHorMin = czLeft + czWidth;
        czHorMax = czRight - czWidth;

        czHeight = viewCenterY - czBottom;
        czVerMin = czBottom + czHeight;
        czVerMax = czTop - czHeight;
    }


    [Obsolete()]
    /// <summary>
    /// 뷰 포트를 가운데로 맞춥니다.
    /// </summary>
    void SetViewportCenter_dep()
    {
        /**
        Vector3 playerPos = _player.transform.position;
        _mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y, _mainCamera.transform.position.z);
        return;
        */

        float czLeft = _cameraZone_dep.bounds.min.x;
        float czRight = _cameraZone_dep.bounds.max.x;
        float playerX = _player.transform.position.x;
        float czTop = _cameraZone_dep.bounds.max.y;
        float czBottom = _cameraZone_dep.bounds.min.y;
        float playerY = _player.transform.position.y;


        bool movePos = false;
        var newPos = _mainCamera.transform.position;
        if (czHorMin < playerX && playerX < czHorMax)
        {
            newPos.x = playerX;
            movePos = true;
        }
        if (czVerMin < playerY && playerY < czVerMax)
        {
            newPos.y = playerY;
            movePos = true;
        }


        // 
        if (movePos)
        {
            _mainCamera.transform.position = newPos;
        }

    }


    #endregion
}
