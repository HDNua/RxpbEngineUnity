using UnityEngine;
using System.Collections;



/// <summary>
/// 지형에 대한 스크립트입니다.
/// </summary>
public class GeometryScript : MonoBehaviour
{
    #region 컨트롤러가 사용할 공용 형식 또는 값을 정의합니다.
    Collider2D _collider;


    #endregion










    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public BoxCollider2D[] plates;
    public EdgeCollider2D[] groundEdges;


    public EdgeCollider2D groundCollider;
    public EdgeCollider2D leftCollider;
    public EdgeCollider2D rightCollider;
    public EdgeCollider2D bottomCollider;


    /// <summary>
    /// 오른쪽 위에서 왼쪽 아래로 내려오는 Slope라면 참입니다.
    /// </summary>
    public bool isLeftDescending;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // Collider2D 요소를 획득합니다.
        _collider = GetComponent<Collider2D>();

        // Ground입니다.
        if (_collider is BoxCollider2D)
        {
            BoxCollider2D box = _collider as BoxCollider2D;
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


            // 바닥, 왼쪽, 오른쪽 collider의 위치와 스케일을 맞춥니다.
            groundCollider.transform.position = Vector3.zero;
            groundCollider.points = new Vector2[] { points[0], points[1] };
            leftCollider.transform.position = Vector3.zero;
            leftCollider.points = new Vector2[] { points[0], points[2] };
            rightCollider.transform.position = Vector3.zero;
            rightCollider.points = new Vector2[] { points[1], points[3] };
        }
        // Slope입니다.
        else if (plates != null)
        {
            Vector2[] uppers = new Vector2[plates.Length * 2];
            Vector2[] lowers = new Vector2[plates.Length * 2];


            if (isLeftDescending)
            {
                for (int i = 0, len = plates.Length; i < len; ++i)
                {
                    BoxCollider2D box = plates[i];
                    Vector2 center = box.bounds.center;
                    Vector2 extents = box.bounds.extents;
                    Vector2[] points = new Vector2[4];
                    points[0] = new Vector2(center.x - extents.x, center.y + extents.y);
                    points[1] = new Vector2(center.x + extents.x, center.y + extents.y);
                    points[2] = new Vector2(center.x - extents.x, center.y - extents.y);
                    points[3] = new Vector2(center.x + extents.x, center.y - extents.y);


                    for (int j = 0, jlen = points.Length; j < jlen; ++j)
                    {
                        points[j].x /= transform.localScale.x;
                        points[j].y /= transform.localScale.y;
                    }
                    uppers[2 * i] = points[1];
                    uppers[2 * i + 1] = points[0];
                    lowers[2 * i] = points[3];
                    lowers[2 * i + 1] = points[2];
                }


                // 지면 EdgeCollider의 위치와 스케일을 조정합니다.
                for (int i = 0, len = groundEdges.Length; i < len; ++i)
                {
                    groundEdges[i].transform.position = Vector3.zero;
                    groundEdges[i].points = new Vector2[]
                    { uppers[i], uppers[i + 1] };
                }


                // 나머지 EdgeCollider의 위치와 스케일을 조정합니다.
                bottomCollider.transform.position = Vector3.zero;
                bottomCollider.points = lowers;
                leftCollider.transform.position = Vector3.zero;
                leftCollider.points = new Vector2[] { uppers[0], lowers[0] };
                rightCollider.transform.position = Vector3.zero;
                rightCollider.points = new Vector2[]
                { uppers[uppers.Length - 1], lowers[lowers.Length - 1] };
            }
            else
            {
                for (int i = 0, len = plates.Length; i < len; ++i)
                {
                    BoxCollider2D box = plates[i];
                    Vector2 center = box.bounds.center;
                    Vector2 extents = box.bounds.extents;
                    Vector2[] points = new Vector2[4];
                    points[0] = new Vector2(center.x - extents.x, center.y + extents.y);
                    points[1] = new Vector2(center.x + extents.x, center.y + extents.y);
                    points[2] = new Vector2(center.x - extents.x, center.y - extents.y);
                    points[3] = new Vector2(center.x + extents.x, center.y - extents.y);


                    for (int j = 0, jlen = points.Length; j < jlen; ++j)
                    {
                        points[j].x /= transform.localScale.x;
                        points[j].y /= transform.localScale.y;
                    }
                    uppers[2 * i] = points[0];
                    uppers[2 * i + 1] = points[1];
                    lowers[2 * i] = points[2];
                    lowers[2 * i + 1] = points[3];
                }


                // 지면 EdgeCollider의 위치와 스케일을 조정합니다.
                for (int i = 0, len = groundEdges.Length; i < len; ++i)
                {
                    groundEdges[i].transform.position = Vector3.zero;
                    groundEdges[i].points = new Vector2[]
                    { uppers[i], uppers[i + 1] };
                }


                // 나머지 EdgeCollider의 위치와 스케일을 조정합니다.
                bottomCollider.transform.position = Vector3.zero;
                bottomCollider.points = lowers;
                leftCollider.transform.position = Vector3.zero;
                leftCollider.points = new Vector2[] { uppers[0], lowers[0] };
                rightCollider.transform.position = Vector3.zero;
                rightCollider.points = new Vector2[]
                { uppers[uppers.Length - 1], lowers[lowers.Length - 1] };
            }
        }
    }
    /**
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {

    }
    */


    #endregion










    #region 보조 메서드를 정의합니다.


    #endregion
}
