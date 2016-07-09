using UnityEngine;
using System.Collections;



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
    /// 충돌이 감지되었습니다.
    /// </summary>
    /// <param name="collision">충돌 정보 객체입니다.</param>
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            
        }
    }


    #endregion



    #region 메서드를 정의합니다.


    #endregion
}
