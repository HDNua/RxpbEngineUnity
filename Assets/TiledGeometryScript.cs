using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 
/// </summary>
public class TiledGeometryScript : MonoBehaviour
{
    #region MyRegion
    public GameObject test;


    #endregion



    #region 필드를 정의합니다.
    BoxCollider2D _flatGroundCollider;
    PolygonCollider2D _slopeGroundCollider;


    EdgeCollider2D _tes2;


    #endregion



    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        _flatGroundCollider = gameObject.GetComponent<BoxCollider2D>();
        _slopeGroundCollider = gameObject.GetComponent<PolygonCollider2D>();


        if (_flatGroundCollider != null)
        {
            BoxCollider2D box = _flatGroundCollider;
            EdgeCollider2D groundEdge, leftEdge, rightEdge; ///, bottomEdge;

            if (box.size == new Vector2(96, 96) && box.offset == new Vector2(48, -48))
            {
                Console.WriteLine();
            }
            groundEdge = gameObject.AddComponent<EdgeCollider2D>();
            leftEdge = gameObject.AddComponent<EdgeCollider2D>();
            rightEdge = gameObject.AddComponent<EdgeCollider2D>();


            float originX = transform.position.x, originY = transform.position.y;
            Vector2[] points = new Vector2[4];
            Vector2 center = box.bounds.center;
            Vector2 extents = box.bounds.extents;
            points[0] = new Vector2(-originX + center.x - extents.x, -originY + center.y + extents.y); // 왼쪽 위
            points[1] = new Vector2(-originX + center.x + extents.x, -originY + center.y + extents.y); // 오른쪽 위
            points[2] = new Vector2(-originX + center.x - extents.x, -originY + center.y - extents.y); // 왼쪽 아래
            points[3] = new Vector2(-originX + center.x + extents.x, -originY + center.y - extents.y); // 오른쪽 아래


            // 스케일을 맞춥니다.
            for (int i = 0, len = points.Length; i < len; ++i)
            {
                points[i].x = points[i].x / 0.02008f / transform.localScale.x;
                points[i].y = points[i].y / 0.02008f / transform.localScale.y;
            }

            // 바닥, 왼쪽, 오른쪽 collider의 위치와 스케일을 맞춥니다.
//            groundEdge.transform.localPosition = Vector3.zero;
            groundEdge.points = new Vector2[] { points[0], points[1] };
//            leftEdge.transform.localPosition = Vector3.zero;
            leftEdge.points = new Vector2[] { points[0], points[2] };
//            rightEdge.transform.localPosition = Vector3.zero;
            rightEdge.points = new Vector2[] { points[1], points[3] };

            /*
            if (test == null)
                return;

            GameObject groundCloneObject = Instantiate(test);
            groundCloneObject.transform.parent = gameObject.transform;
            groundCloneObject.transform.localPosition = Vector3.zero;
            groundCloneObject.transform.localScale = new Vector3(1, 1, 1);

            GeometryScript geometryScript = groundCloneObject.GetComponent<GeometryScript>();
            geometryScript.UpdateStart();

            geometryScript.transform.position = Vector3.zero;
            */
        }
        else if (_slopeGroundCollider != null)
        {



        }
        else
        {
            throw new Exception("TiledGeometryScript가 FlatGround 또는 SlopeGround 둘 중 어느 것도 아닙니다.");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        
    }


    #endregion



    #region 메서드를 정의합니다.
    void Start_dep()
    {
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
