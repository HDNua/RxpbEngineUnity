using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// Tiled로 작업한 지형의 스크립트입니다.
/// </summary>
public class TiledGeometryScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public PhysicsMaterial2D _material;


    #endregion










    #region 필드를 정의합니다.
    BoxCollider2D _flatGroundCollider;
    PolygonCollider2D _slopeGroundCollider;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // Collider 컴포넌트를 획득합니다.
        _flatGroundCollider = gameObject.GetComponent<BoxCollider2D>();
        _slopeGroundCollider = gameObject.GetComponent<PolygonCollider2D>();


        // 평평한 지형 충돌체의 처리입니다.
        EdgeCollider2D groundEdge, leftEdge, rightEdge, bottomEdge;
        if (_flatGroundCollider != null)
        {
            // 계산에 필요한 값을 먼저 획득합니다.
            BoxCollider2D box = _flatGroundCollider;
            Vector3 originScale = new Vector3(1, 1, 1);


            // edge 객체를 gameObject에 추가하고 이에 대한 참조를 획득합니다.
            GameObject childObject;

            // groundEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Ground");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            childObject.isStatic = true;
            groundEdge = childObject.AddComponent<EdgeCollider2D>();

            // leftEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            childObject.isStatic = true;
            leftEdge = childObject.AddComponent<EdgeCollider2D>();

            // rightEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            childObject.isStatic = true;
            rightEdge = childObject.AddComponent<EdgeCollider2D>();

            // bottomEdge
            childObject = new GameObject();
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            childObject.isStatic = true;
            bottomEdge = childObject.AddComponent<EdgeCollider2D>();


            // 주어진 정보를 바탕으로 꼭짓점을 계산합니다.
            Vector2[] points = new Vector2[4];
            Vector2 center = box.bounds.center;
            Vector2 extents = box.bounds.extents;
            points[0] = new Vector2(center.x - extents.x, center.y + extents.y); // 왼쪽 위
            points[1] = new Vector2(center.x + extents.x, center.y + extents.y); // 오른쪽 위
            points[2] = new Vector2(center.x - extents.x, center.y - extents.y); // 왼쪽 아래
            points[3] = new Vector2(center.x + extents.x, center.y - extents.y); // 오른쪽 아래


            // 스케일을 맞춥니다.
            for (int i = 0, len = points.Length; i < len; ++i)
            {
                points[i].x = points[i].x / 0.02008f / transform.localScale.x;
                points[i].y = points[i].y / 0.02008f / transform.localScale.y;
            }


            // 바닥, 왼쪽, 오른쪽 collider의 꼭짓점을 업데이트합니다.
            groundEdge.points = new Vector2[] { points[0], points[1] };
            leftEdge.points = new Vector2[] { points[0], points[2] };
            rightEdge.points = new Vector2[] { points[1], points[3] };
            bottomEdge.points = new Vector2[] { points[2], points[3] };
        }
        // 기울어진 지형 충돌체의 처리입니다.
        else if (_slopeGroundCollider != null)
        {
            // 계산에 필요한 값을 먼저 획득합니다.
            Vector2 edgeOrigin = transform.localPosition;

            PolygonCollider2D box = _slopeGroundCollider;
            Vector3 originScale = new Vector3(1, 1, 1);


            // edge 객체를 gameObject에 추가하고 이에 대한 참조를 획득합니다.
            GameObject childObject;

            // groundEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Ground");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            childObject.isStatic = true;
            groundEdge = childObject.AddComponent<EdgeCollider2D>();

            // leftEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            childObject.isStatic = true;
            leftEdge = childObject.AddComponent<EdgeCollider2D>();

            // rightEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            childObject.isStatic = true;
            rightEdge = childObject.AddComponent<EdgeCollider2D>();

            // bottomEdge
            childObject = new GameObject();
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            childObject.isStatic = true;
            bottomEdge = childObject.AddComponent<EdgeCollider2D>();


            // 주어진 정보를 바탕으로 꼭짓점을 계산합니다.
            Vector2[] slopePoints = box.points;
            Vector2[] points = new Vector2[4];
            points[0] = slopePoints[0] + edgeOrigin; // 왼쪽 위
            points[1] = slopePoints[1] + edgeOrigin; // 오른쪽 위
            points[2] = slopePoints[3] + edgeOrigin; // 왼쪽 아래
            points[3] = slopePoints[2] + edgeOrigin; // 오른쪽 아래


            // 스케일을 맞춥니다.
            for (int i = 0, len = points.Length; i < len; ++i)
            {
                points[i].x = points[i].x / transform.localScale.x;
                points[i].y = points[i].y / transform.localScale.y;
            }


            // 바닥, 왼쪽, 오른쪽 collider의 꼭짓점을 업데이트합니다.
            groundEdge.points = new Vector2[] { points[0], points[1] };
            leftEdge.points = new Vector2[] { points[0], points[2] };
            rightEdge.points = new Vector2[] { points[1], points[3] };
            bottomEdge.points = new Vector2[] { points[2], points[3] };

        }
        // 그 외의 경우 예외 처리합니다.
        else
        {
            throw new Exception("TiledGeometryScript가 FlatGround 또는 SlopeGround 둘 중 어느 것도 아닙니다.");
        }


        // 공통 속성을 업데이트합니다.
        groundEdge.sharedMaterial = _material;
        leftEdge.sharedMaterial = _material;
        rightEdge.sharedMaterial = _material;
        bottomEdge.sharedMaterial = _material;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        
    }


    #endregion










    #region 메서드를 정의합니다.


    #endregion

}
