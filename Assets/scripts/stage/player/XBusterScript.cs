using System;
using UnityEngine;
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










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }
    /// <summary>
    /// FixedTimestep에 설정된 값에 따라 일정한 간격으로 업데이트합니다.
    /// 물리 효과가 적용된 오브젝트를 조정할 때 사용됩니다.
    /// (Update는 불규칙한 호출이기 때문에 물리엔진 충돌검사가 제대로 되지 않을 수 있습니다.)
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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


            // 적이 무적 상태라면
            if (enemy.Invencible)
            {
                // 반사 효과를 생성합니다.
                MakeReflectedParticle();
            }
            // 그 외의 경우
            else
            {
                // 타격 효과를 생성하고 대미지를 입힙니다.
                MakeHitParticle();
                enemy.Hurt(damage);
            }

            // 적이 살아있다면 탄환을 제거합니다.
            if (enemy.IsAlive())
            {
                Destroy(gameObject);
            }
        }
        // X 버스터가 통과할 수 없는 레이어와 충돌했습니다.
        else if (_collider.IsTouchingLayers(busterUnpassable))
        {
            // 타격 입자를 생성하고 탄환을 제거합니다.
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
    /// <summary>
    /// 피격 효과 객체를 생성합니다.
    /// </summary>
    /// <returns>피격 효과 객체입니다.</returns>
    public GameObject MakeReflectedParticle()
    {
        GameObject hitParticle = Instantiate
            (effects[1], transform.position, transform.rotation)
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
        hitEffect.PlayEffectSound(SoundEffects[1].clip);

        // 생성한 효과 객체를 반환합니다.
        return hitParticle;
    }


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("busterUnpassable로 대체되었습니다.", true)]
    public LayerMask whatIsWall;

    #endregion
}