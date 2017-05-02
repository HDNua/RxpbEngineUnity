using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 
/// </summary>
public class HitBoxScript : MonoBehaviour
{
    /*
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    public LayerMask _groundLayer;

    /// <summary>
    /// 
    /// </summary>
    public bool _mustBeCrouched = false;

    #endregion



    #region 필드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    BoxCollider2D _Collider;

    #endregion



    #region MonoBehaviour를 재정의합니다.
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        _Collider = GetComponent<BoxCollider2D>();
    }

    #endregion



    #region 테스트.
    /// <summary>
    /// 
    /// </summary>
    /// <param name="otherCollider"></param>
    private void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (_Collider.IsTouchingLayers(_groundLayer))
        {
            float otherBottom = ((EdgeCollider2D)otherCollider).points[0].y;
            float selfBottom = _Collider.bounds.min.y;
            if (selfBottom < otherBottom)
            {
                float selfTop = _Collider.bounds.max.y;
                _mustBeCrouched = otherBottom < selfTop;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="otherCollider"></param>
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (_Collider.IsTouchingLayers(_groundLayer)
            && otherCollider.gameObject.layer == _groundLayer)
        {
            float otherBottom = ((EdgeCollider2D)otherCollider).points[0].y;
            float selfBottom = _Collider.bounds.min.y;
            if (selfBottom < otherBottom)
            {
                float selfTop = _Collider.bounds.max.y;
                _mustBeCrouched = otherBottom < selfTop;
            }
        }

    }

    #endregion
    */
}
