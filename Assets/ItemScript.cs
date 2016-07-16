using System;
using UnityEngine;
using System.Collections;



[RequireComponent(typeof(BoxCollider2D))]
/// <summary>
/// 아이템 스크립트입니다.
/// </summary>
public class ItemScript : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    BoxCollider2D _triggerBox;


    #endregion



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public string _itemType;
    public int _itemValue;


    #endregion










    #region 필드를 정의합니다.


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        _triggerBox = GetComponent<BoxCollider2D>();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        
    }


    #endregion










    #region Trigger 관련 메서드를 정의합니다.
    /// <summary>
    /// 충돌체가 트리거 내부로 진입했습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 닿았다면 플레이어가 획득합니다.
        if (other.CompareTag("Player"))
        {
            // 사용할 변수를 선언합니다.
            GameObject pObject = other.gameObject;
            PlayerController player = pObject.GetComponent<PlayerController>();

            // 아이템 효과를 발동합니다.
            player.Heal(_itemValue);

            // 먹은 아이템을 삭제합니다.
            Destroy(gameObject);
        }
    }


    #endregion









    #region 메서드를 정의합니다.


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("구형 정의 테스트입니다.")]
    /// <summary>
    /// 구형 정의 테스트 함수입니다.
    /// </summary>
    void Function()
    {
        Console.WriteLine("Hello, world!");
    }


    #endregion
}
