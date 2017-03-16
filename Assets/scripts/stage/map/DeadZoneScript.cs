using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 사망 지역 스크립트입니다.
/// </summary>
public class DeadZoneScript : EnemyScript
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.


    #endregion










    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    public int _spikeDamage = 100;


    #endregion










    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        Health = 100;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected override void Update()
    {
        /// base.Update();
    }


    #endregion










    #region Collider2D의 기본 메서드를 재정의합니다.
    /// <summary>
    /// 충돌체가 여전히 트리거 내부에 있습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        // 트리거가 발동한 상대 충돌체가 플레이어라면 대미지를 입힙니다.
        if (other.CompareTag("Player"))
        {
            GameObject pObject = other.gameObject;
            PlayerController player = pObject.GetComponent<PlayerController>();


            // 플레이어가 무적 상태이거나 죽었다면
            if (player.Invencible || player.IsDead)
            {
                // 아무 것도 하지 않습니다.

            }
            // 그 외의 경우
            else
            {
                // 플레이어에게 대미지를 입힙니다.
                player.Hurt(_spikeDamage);
            }
        }
    }


    #endregion










    #region EnemyScript의 메서드를 오버라이드 합니다.


    #endregion










    #region 메서드를 정의합니다.


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
