using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// TiledGeometry의 부모 개체입니다.
/// </summary>
public class TiledGeometryParent : MonoBehaviour
{
    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    public static TiledGeometryParent Instance
    {
        get
        {
            return GameObject.FindGameObjectWithTag("TiledGeometryParent").GetComponent<TiledGeometryParent>();
        }
    }

    #endregion



    #region 구형 정의를 보관합니다.

    #endregion
}
