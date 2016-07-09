using UnityEngine;
using System.Collections;



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
    public Camera _MainCamera;
    /// <summary>
    /// 카메라 존 배열입니다.
    /// </summary>
    public BoxCollider2D[] _CameraZones;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 현재 플레이어가 속한 카메라 존입니다.
    /// </summary>
    BoxCollider2D _cameraZone;


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
        _cameraZone = _CameraZones[0];

        float czLeft = _cameraZone.bounds.min.x;
        float czRight = _cameraZone.bounds.max.x;
        float viewCenterX = _MainCamera.transform.position.x;

        float czTop = _cameraZone.bounds.max.y;
        float czBottom = _cameraZone.bounds.min.y;
        float viewCenterY = _MainCamera.transform.position.y;


        // 필드를 업데이트 합니다.
        czWidth = viewCenterX - czLeft;
        czHorMin = czLeft + czWidth;
        czHorMax = czRight - czWidth;

        czHeight = viewCenterY - czBottom;
        czVerMin = czBottom + czHeight;
        czVerMax = czTop - czHeight;
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










    #region 요청을 수행하기 위한 보조 메서드를 정의합니다.
    /// <summary>
    /// 뷰 포트를 가운데로 맞춥니다.
    /// </summary>
    void SetViewportCenter()
    {
        /*
//        float czLeft = cameraZone.bounds.min.x;
//        float czRight = cameraZone.bounds.max.x;
        float playerX = _player.transform.position.x;
//        float czTop = cameraZone.bounds.max.y;
//        float czBottom = cameraZone.bounds.min.y;
        float playerY = _player.transform.position.y;


        bool movePos = false;
        var newPos = _MainCamera.transform.position;
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
            _MainCamera.transform.position = newPos;
        }
        */

        Vector3 playerPos = _player.transform.position;
        _MainCamera.transform.position = new Vector3(playerPos.x, playerPos.y, _MainCamera.transform.position.z);
    }


    #endregion
}
