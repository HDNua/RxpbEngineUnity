using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 탄환을 정의합니다.
/// </summary>
public abstract class EnemyBulletScript : EnemyScript, IMovableEnemy
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    /// <summary>
    /// Rigidbody2D 요소를 가져옵니다.
    /// </summary>
    Rigidbody2D _rigidbody;

    /// <summary>
    /// Rigidbody2D 요소를 가져옵니다.
    /// </summary>
    protected Rigidbody2D _Rigidbody { get { return GetComponent<Rigidbody2D>(); } }

    #endregion





    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    /// <summary>
    /// 자신이 진행하는 방향에 벽이 존재하는지 검사하기 위해 사용합니다.
    /// </summary>
    public Transform _pushCheck;
    /// <summary>
    /// 무엇이 벽인지를 결정합니다. 기본값은 "Wall, MapBlock"입니다.
    /// </summary>
    public LayerMask _whatIsWall;

    /// <summary>
    /// 이동 속도를 정의합니다.
    /// </summary>
    public float _movingSpeed = 5;

    /// <summary>
    /// 탄환이 생존하는 기간입니다.
    /// </summary>
    public float _lifeTime = 5;

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

        // 필드를 초기화합니다.
        _rigidbody = GetComponent<Rigidbody2D>();

        // 생존 기간이 존재하는 탄환이면 _lifeTime 이후 폭발합니다.
        if (_lifeTime > 0)
            Invoke("Dead", _lifeTime);
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
        Vector3 camPos = Camera.main.transform.position;
        Vector3 bulPos = transform.position;
        if (Mathf.Abs(camPos.x - bulPos.x) > 10)
        {
            Destroy(gameObject);
        }
    }

    #endregion





    #region Collider2D의 기본 메서드를 재정의합니다.
    /// <summary>
    /// 충돌체가 트리거 내부로 진입했습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (_Collider.IsTouchingLayers(_whatIsWall))
        {
            Dead();
        }
    }
    /// <summary>
    /// 충돌체가 여전히 트리거 내부에 있습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    protected virtual void OnTriggerStay2D(Collider2D other)
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
                player.Hurt(Damage);
            }

            // 맞는 순간 폭발합니다.
            Dead();
        }
    }

    #endregion





    #region EnemyScript의 메서드를 오버라이드합니다.
    /// <summary>
    /// 캐릭터가 사망합니다.
    /// </summary>
    public override void Dead()
    {
        // 폭발 효과를 생성하고 효과음을 재생합니다.
        SoundEffects[0].Play();
        Instantiate(effects[0], transform.position, transform.rotation);

        // 캐릭터가 사망합니다.
        base.Dead();

        // 탄환 사망 후 1초 후에 제거합니다.
        Invoke("RequestDestroy", 1f);
    }
    /// <summary>
    /// 캐릭터에게 대미지를 입힙니다.
    /// </summary>
    /// <param name="damage">대미지 값입니다.</param>
    public override void Hurt(int damage)
    {
        base.Hurt(damage);

        // 무적 상태 코루틴을 시작합니다.
        StartCoroutine(CoroutineInvencible());
    }

    /// <summary>
    /// 탄환 발사 방향을 지정합니다.
    /// </summary>
    /// <param name="playerPos">현재 조작중인 플레이어의 위치입니다.</param>
    public abstract void MoveTo(Vector3 playerPos);

    #endregion





    #region IMovableEnemy를 구현합니다.
    /// <summary>
    /// 왼쪽으로 이동합니다.
    /// </summary>
    protected void MoveLeft()
    {
        if (FacingRight)
            Flip();
        _rigidbody.velocity = new Vector2(-_movingSpeed, _rigidbody.velocity.y);
    }
    /// <summary>
    /// 오른쪽으로 이동합니다.
    /// </summary>
    protected void MoveRight()
    {
        if (FacingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(_movingSpeed, _rigidbody.velocity.y);
    }

    #endregion
}
