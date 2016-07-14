using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// Tiled로 작업한 지형의 스크립트입니다.
/// </summary>
public class TiledGeometryScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
///    public bool isLeftDescending = true;


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
        if (_flatGroundCollider != null)
        {
            // 계산에 필요한 값을 먼저 획득합니다.
            /// float originX = transform.position.x, originY = transform.position.y;
            BoxCollider2D box = _flatGroundCollider;
            Vector3 originScale = new Vector3(1, 1, 1);


            // edge 객체를 gameObject에 추가하고 이에 대한 참조를 획득합니다.
            GameObject childObject;
            EdgeCollider2D groundEdge, leftEdge, rightEdge, bottomEdge;

            // groundEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Ground");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            groundEdge = childObject.AddComponent<EdgeCollider2D>();

            // leftEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            leftEdge = childObject.AddComponent<EdgeCollider2D>();

            // rightEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            rightEdge = childObject.AddComponent<EdgeCollider2D>();

            // bottomEdge
            childObject = new GameObject();
            // childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            bottomEdge = childObject.AddComponent<EdgeCollider2D>();


            // 주어진 정보를 바탕으로 꼭짓점을 계산합니다.
            Vector2[] points = new Vector2[4];
            Vector2 center = box.bounds.center;
            Vector2 extents = box.bounds.extents;
            /// points[0] = new Vector2(-originX + center.x - extents.x, -originY + center.y + extents.y); // 왼쪽 위
            /// points[1] = new Vector2(-originX + center.x + extents.x, -originY + center.y + extents.y); // 오른쪽 위
            /// points[2] = new Vector2(-originX + center.x - extents.x, -originY + center.y - extents.y); // 왼쪽 아래
            /// points[3] = new Vector2(-originX + center.x + extents.x, -originY + center.y - extents.y); // 오른쪽 아래
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
            /// float originX = transform.position.x, originY = transform.position.y;
            Vector2 edgeOrigin = transform.localPosition;

            PolygonCollider2D box = _slopeGroundCollider;
            Vector3 originScale = new Vector3(1, 1, 1);


            // edge 객체를 gameObject에 추가하고 이에 대한 참조를 획득합니다.
            GameObject childObject;
            EdgeCollider2D groundEdge, leftEdge, rightEdge, bottomEdge;

            // groundEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Ground");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            groundEdge = childObject.AddComponent<EdgeCollider2D>();

            // leftEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            leftEdge = childObject.AddComponent<EdgeCollider2D>();

            // rightEdge
            childObject = new GameObject();
            childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            rightEdge = childObject.AddComponent<EdgeCollider2D>();

            // bottomEdge
            childObject = new GameObject();
            // childObject.layer = LayerMask.NameToLayer("Wall");
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localScale = originScale;
            bottomEdge = childObject.AddComponent<EdgeCollider2D>();


            // 주어진 정보를 바탕으로 꼭짓점을 계산합니다.
            /// Vector2[] points = new Vector2[4];
            /// Vector2 center = box.bounds.center;
            /// Vector2 extents = box.bounds.extents;
            /// points[0] = new Vector2(-originX + center.x - extents.x, -originY + center.y + extents.y); // 왼쪽 위
            /// points[1] = new Vector2(-originX + center.x + extents.x, -originY + center.y + extents.y); // 오른쪽 위
            /// points[2] = new Vector2(-originX + center.x - extents.x, -originY + center.y - extents.y); // 왼쪽 아래
            /// points[3] = new Vector2(-originX + center.x + extents.x, -originY + center.y - extents.y); // 오른쪽 아래
            /**
            points[0] = new Vector2(center.x - extents.x, center.y + extents.y); // 왼쪽 위
            points[1] = new Vector2(center.x + extents.x, center.y + extents.y); // 오른쪽 위
            points[2] = new Vector2(center.x - extents.x, center.y - extents.y); // 왼쪽 아래
            points[3] = new Vector2(center.x + extents.x, center.y - extents.y); // 오른쪽 아래
            */
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
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        
    }


    #endregion










    #region 메서드를 정의합니다.
    [Obsolete("새로운 Start 함수로 대체되었습니다.")]
    public GameObject test;



    [Obsolete("새로운 Start 함수로 대체되었습니다.")]
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다. 추신: 쳐다보지 마세요!
    /// </summary>
    void Start_dep()
    {
        // 으아아 눈갱


        _flatGroundCollider = gameObject.GetComponent<BoxCollider2D>();
        _slopeGroundCollider = gameObject.GetComponent<PolygonCollider2D>();


        if (_flatGroundCollider != null)
        {
            BoxCollider2D box = _flatGroundCollider;
            EdgeCollider2D groundEdge, leftEdge, rightEdge; ///, bottomEdge;



            if (test == null)
                return;






            GameObject groundCloneObject = Instantiate(test);
            groundCloneObject.transform.parent = gameObject.transform;
            groundCloneObject.transform.localPosition = Vector3.zero;
            groundCloneObject.transform.localScale = new Vector3(1, 1, 1);


            GeometryScript geometryScript = groundCloneObject.GetComponent<GeometryScript>();
            EdgeCollider2D[] edges = groundCloneObject.GetComponentsInChildren<EdgeCollider2D>();





            GeometryScript originGeomScript = test.GetComponent<GeometryScript>();




            Console.WriteLine();




            leftEdge = edges[0];
            rightEdge = edges[1];
            groundEdge = edges[2];
            /// bottomEdge = edges[3];
            //            groundEdge.transform.localPosition = Vector3.zero;
            //            leftEdge.transform.localPosition = Vector3.zero;
            //            rightEdge.transform.localPosition = Vector3.zero;





            BoxCollider2D _what1 = groundCloneObject.GetComponent<BoxCollider2D>();
            _what1.transform.localPosition = Vector3.zero;
            _what1.size = _flatGroundCollider.size; // * 0.02008f; // geometryScript.transform.localScale.x;
            _what1.offset = _flatGroundCollider.offset;
            //            _what1.transform.localScale = 1f;





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
                points[i].x /= transform.localScale.x;
                points[i].y /= transform.localScale.y;
            }


            //            groundEdge.transform.localPosition = Vector3.zero;
            //            leftEdge.transform.localPosition = Vector3.zero;
            //            rightEdge.transform.localPosition = Vector3.zero;



            // 바닥, 왼쪽, 오른쪽 collider의 위치와 스케일을 맞춥니다.
            //            groundEdge.transform.position = Vector3.zero;
            groundEdge.points = new Vector2[] { points[0], points[1] };
            //            leftEdge.transform.position = Vector3.zero;
            leftEdge.points = new Vector2[] { points[0], points[2] };
            //            rightEdge.transform.position = Vector3.zero;
            rightEdge.points = new Vector2[] { points[1], points[3] };


            //            groundEdge.transform.localPosition = Vector3.zero;
            //            leftEdge.transform.localPosition = Vector3.zero;
            //            rightEdge.transform.localPosition = Vector3.zero;


        }
        else if (_slopeGroundCollider != null)
        {



        }
        else
        {
            throw new Exception("TiledGeometryScript가 FlatGround 또는 SlopeGround 둘 중 어느 것도 아닙니다.");
        }
    }


    #endregion

}
