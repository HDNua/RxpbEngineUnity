using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// Tiled로 작업한 지형의 스크립트입니다.
/// </summary>
public class TiledGeometryScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// TiledGeometry 집합의 부모 개체입니다.
    /// </summary>
    TiledGeometryParent _parent;
    /// <summary>
    /// Scene 데이터베이스입니다.
    /// </summary>
    DataBase _database;


    /// <summary>
    /// 평평한 지형 충돌체입니다.
    /// </summary>
    BoxCollider2D _flatGroundCollider;
    /// <summary>
    /// 경사면 지형 충돌체입니다.
    /// </summary>
    PolygonCollider2D _slopeGroundCollider;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 작업에 필요한 정보를 먼저 획득합니다.
        _parent = GetComponentInParent<TiledGeometryParent>();
        if (_parent == null)
        {
            throw new Exception("TiledGeometryError: Parent == null");
        }

        _database = _parent._database;
        PhysicsMaterial2D material = _database.FrictionlessWall;


        // Collider 컴포넌트를 획득합니다.
        _flatGroundCollider = gameObject.GetComponent<BoxCollider2D>();
        _slopeGroundCollider = gameObject.GetComponent<PolygonCollider2D>();


        // 평평한 지형 충돌체의 처리입니다.
        float scaleX = _database.Map.transform.localScale.x;
        float scaleY = _database.Map.transform.localScale.y;
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
            childObject.layer = LayerMask.NameToLayer("GeometryBottom");
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
                points[i].x = points[i].x / scaleX / transform.localScale.x;
                points[i].y = points[i].y / scaleY / transform.localScale.y;
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
            Vector2[] slopePoints = GetTetragonPoints(box.points);
            Vector2[] points = new Vector2[4];
            points[0] = slopePoints[0] + edgeOrigin; // 왼쪽 위
            points[1] = slopePoints[1] + edgeOrigin; // 오른쪽 위
            points[2] = slopePoints[2] + edgeOrigin; // 왼쪽 아래
            points[3] = slopePoints[3] + edgeOrigin; // 오른쪽 아래

            // 스케일을 맞춥니다.
            for (int i = 0, len = points.Length; i < len; ++i)
            {
                points[i].x = points[i].x / transform.localScale.x;
                points[i].y = points[i].y / transform.localScale.y + _database.Map.transform.position.y;
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
        groundEdge.sharedMaterial = material;
        leftEdge.sharedMaterial = material;
        rightEdge.sharedMaterial = material;
        bottomEdge.sharedMaterial = material;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
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
        // 임의의 사각형이 아닌, 직각사다리꼴과 같은 단순한 도형에 대해 꼭짓점을 구한다는 특성을 이용합니다.
        Vector2[] ret = new Vector2[4];
        List<Vector2> sorted = new List<Vector2>(points);

        // 배열을 정렬합니다. x 좌표는 오름차순, y 좌표는 내림차순으로 정렬합니다.
        // 이렇게 정렬하면 배열은 "왼쪽 위, 왼쪽 아래, 오른쪽 위, 오른쪽 아래"로 정렬됩니다.
        // 이것이 직각사다리꼴과 같은 단순한 도형에 대해 꼭짓점을 구한다는 특성을 이용한 것입니다.
        sorted.Sort(delegate (Vector2 a, Vector2 b)
        {
            if (a.x != b.x)
            {
                return (a.x < b.x) ? -1 : 1;
            }
            else if (a.y != b.y)
            {
                return (a.y > b.y) ? -1 : 1;
            }
            return 0;
        });

        // 반환하려는 순서대로 맞춥니다.
        ret[0] = sorted[0];
        ret[1] = sorted[2];
        ret[2] = sorted[1];
        ret[3] = sorted[3];
        return ret;
    }


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
