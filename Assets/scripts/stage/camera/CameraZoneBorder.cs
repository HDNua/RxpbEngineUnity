using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 카메라 존의 경계를 표현하는 객체입니다.
/// </summary>
public class CameraZoneBorder : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 카메라 존 전이 시작 ID입니다.
    /// </summary>
    public int _fromID;
    /// <summary>
    /// 카메라 존 전이 종료 ID입니다.
    /// </summary>
    public int _toID;
    
    /// <summary>
    /// 카메라 존 전이 시작 지점입니다.
    /// </summary>
    public CameraZone _from;
    /// <summary>
    /// 카메라 존 전이 종료 지점입니다.
    /// </summary>
    public CameraZone _to;
    
    /// <summary>
    /// 카메라 존 전이 애니메이션을 수행하려면 값을 참으로 설정합니다.
    /// </summary>
    public bool _beginTransition;

    #endregion





    #region 필드를 정의합니다.
    /// <summary>
    /// 데이터베이스 개체입니다.
    /// </summary>
    DataBase _database;
    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    StageManager _stageManager;

    /// <summary>
    /// 주인공 추적 카메라 개체입니다.
    /// </summary>
    CameraFollow _cameraFollow;
    
    /// <summary>
    /// 수평 방향 경계라면 참입니다.
    /// </summary>
    bool _isHorizontal;
    /// <summary>
    /// 경계 기준 값입니다.
    /// </summary>
    public float _border;
    
    /// <summary>
    /// 카메라 존 경계의 부모 개체입니다.
    /// </summary>
    CameraZoneBorderParent _parent;
    
    #endregion
    




    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        _parent = GetComponentInParent<CameraZoneBorderParent>();
        _database = DataBase.Instance;
        _stageManager = StageManager.Instance;
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        
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
        /// Vector3 playerPos = _cameraZoneParent.Player.transform.position;
        /// Vector3 playerPos = _sceneManager._player.transform.position;
        Vector3 playerPos = _stageManager.GetCurrentPlayerPosition();

        // 수평 방향의 전환이라면
        if (_isHorizontal)
        {
            // 오른쪽에 있는데 왼쪽으로 이동하는 경우
            if (playerPos.x < _border && _cameraFollow.IsInCameraZone(_to))
            {
                SwapCameraZone();
            }
            // 왼쪽에 있는데 오른쪽으로 이동하는 경우
            else if (playerPos.x > _border && _cameraFollow.IsInCameraZone(_from))
            {
                SwapCameraZone();
            }
        }
        // 수직 방향의 전환이라면
        else
        {
            /// EdgeCollider2D border = GetComponent<EdgeCollider2D>();
            /// Vector2[] points = border.points;


            // 위쪽에 있는데 아래쪽으로 이동하는 경우
            if (playerPos.y < _border && _cameraFollow.IsInCameraZone(_from))
            {
                SwapCameraZone();
            }
            // 아래쪽에 있는데 위쪽으로 이동하는 경우
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
            // Debug.Log(string.Format("CameraZone Updated from ({0}) to ({1}).", _to, _from));
            _cameraFollow.UpdateCameraZone(_from, _beginTransition);
        }
        else
        {
            // Debug.Log(string.Format("CameraZone Updated from ({0}) to ({1}).", _from, _to));
            _cameraFollow.UpdateCameraZone(_to, _beginTransition);
        }
    }

    #endregion





    #region 구형 정의를 보관합니다.
    [Obsolete("[v6.0.0] 다음 커밋에서 삭제할 예정입니다.")]
    /// <summary>
    /// 
    /// </summary>
    public StageManager1P _sceneManager;

    #endregion
}
