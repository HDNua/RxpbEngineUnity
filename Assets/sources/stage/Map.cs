using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    PlayerController player;
    public PlayerController Player { set { player = value; } }

    #endregion 컨트롤러용 Unity 객체



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public Camera mainCamera;
    public BoxCollider2D[] cameraZones;

    #endregion Unity 공용 필드



    #region 필드를 정의합니다.
    BoxCollider2D cameraZone;
    float czWidth;
    float czHorMin;
    float czHorMax;
    float czHeight;
    float czVerMin;
    float czVerMax;

    #endregion 필드



    #region MonoBehaviour가 정의하는 기본 메서드를 재정의합니다.
    void Start()
    {
        cameraZone = cameraZones[0];

        float czLeft = cameraZone.bounds.min.x;
        float czRight = cameraZone.bounds.max.x;
        float viewCenterX = mainCamera.transform.position.x;

        float czTop = cameraZone.bounds.max.y;
        float czBottom = cameraZone.bounds.min.y;
        float viewCenterY = mainCamera.transform.position.y;

        czWidth = viewCenterX - czLeft;
        czHorMin = czLeft + czWidth;
        czHorMax = czRight - czWidth;

        czHeight = viewCenterY - czBottom;
        czVerMin = czBottom + czHeight;
        czVerMax = czTop - czHeight;
    }
    void Update()
    {
        if (player != null)
        {
            // 뷰 포트를 맞춥니다.
            SetViewportCenter();
        }
    }
    void FixedUpdate()
    {
    }

    #endregion MonoBehaviour 기본 메서드 재정의



    #region 요청을 수행하기 위한 보조 메서드를 정의합니다.
    /// <summary>
    /// 뷰 포트를 가운데로 맞춥니다.
    /// </summary>
    void SetViewportCenter()
    {
        float czLeft = cameraZone.bounds.min.x;
        float czRight = cameraZone.bounds.max.x;
        float playerX = player.transform.position.x;
        float czTop = cameraZone.bounds.max.y;
        float czBottom = cameraZone.bounds.min.y;
        float playerY = player.transform.position.y;

        bool movePos = false;
        var newPos = mainCamera.transform.position;
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

        if (movePos)
        {
            mainCamera.transform.position = newPos;
        }
    }

    #endregion 보조 메서드 정의
}
