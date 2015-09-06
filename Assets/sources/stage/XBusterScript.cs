using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// 버스터 공격 스크립트입니다.
/// </summary>
public class XBusterScript : AttackScript
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    Camera mainCamera;
    public Camera MainCamera { set { mainCamera = value; } }

    Collider2D _collider;
    Rigidbody2D _rigidbody;

    #endregion



    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    public LayerMask busterUnpassable;

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (mainCamera != null)
        {
            Vector3 camPos = mainCamera.transform.position;
            Vector3 bulPos = transform.position;
            if (Mathf.Abs(camPos.x - bulPos.x) > 10)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();

            if (enemy.Invencible)
            {

            }
            else
            {
                MakeHitParticle();
                enemy.Hurt(damage);
            }
            if (enemy.IsAlive())
            {
                Destroy(gameObject);
            }
        }
        // else if (_collider.IsTouchingLayers(whatIsGround))
        else if (_collider.IsTouchingLayers(busterUnpassable))
        {
            MakeHitParticle();
            Destroy(gameObject);
        }
    }

    #endregion



    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 피격 효과 객체를 생성합니다.
    /// </summary>
    /// <returns>피격 효과 객체입니다.</returns>
    GameObject MakeHitParticle()
    {
        GameObject hitParticle = Instantiate
            (effects[0], transform.position, transform.rotation)
            as GameObject;

        // 버스터 속도의 반대쪽으로 적절히 x 반전합니다.
        if (_rigidbody.velocity.x < 0)
        {
            Vector3 newScale = hitParticle.transform.localScale;
            newScale.x *= -1;
            hitParticle.transform.localScale = newScale;
        }

        // 효과음을 재생합니다.
        EffectScript hitEffect = hitParticle.GetComponent<EffectScript>();
        hitEffect.PlayEffectSound(SoundEffects[0].clip);

        // 생성한 효과 객체를 반환합니다.
        return hitParticle;
    }

    #endregion



    #region 구형 정의를 보관합니다.
    [Obsolete("busterUnpassable로 대체되었습니다.", true)]
    public LayerMask whatIsWall;

    #endregion
}