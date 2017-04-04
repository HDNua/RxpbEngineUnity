using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 세이버 공격 스크립트입니다.
/// </summary>
public class ZSaberScript : AttackScript
{
    /// <summary>
    /// 
    /// </summary>
    public ZController _player;



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

    #endregion



    #region Collider2D의 기본 메서드를 재정의합니다.
    /// <summary>
    /// 충돌체가 트리거 내부로 진입했습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // 적과 충돌했습니다.
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            Vector3 mean = (transform.position + enemy.transform.position) / 2;

            // 적이 무적 상태라면
            if (enemy.Invencible)
            {
                // 반사 효과를 생성합니다.
                MakeReflectedParticle(_player.FacingRight, mean);                
            }
            // 그 외의 경우
            else
            {
                // 타격 효과를 생성하고 대미지를 입힙니다.
                MakeHitParticle(_player.FacingRight, mean);
                enemy.Hurt(damage);
            }
        }
    }

    #endregion
}
