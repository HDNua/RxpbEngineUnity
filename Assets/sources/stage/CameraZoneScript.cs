using System;
using UnityEngine;
using System.Collections;



[Obsolete("CameraZone5Script로 대체되었습니다.")]
/// <summary>
/// 카메라 존에 대한 스크립트입니다.
/// </summary>
public class CameraZoneScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public PlayerController player;
    public Map map;


    #endregion










    #region 필드를 정의합니다.
    public float x;
    public float y;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;


    #endregion









    #region 프로퍼티를 정의합니다.


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {

    }


    #endregion










    #region Collider2D 메서드를 재정의합니다.
    /// <summary>
    /// 충돌체가 트리거 내부로 진입했습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            map.UpdateCameraZone(this);
            Debug.Log(string.Format("CameraZone changed to {0}", this));
        }
    }


    #endregion










    #region 메서드를 정의합니다.


    #endregion
}
