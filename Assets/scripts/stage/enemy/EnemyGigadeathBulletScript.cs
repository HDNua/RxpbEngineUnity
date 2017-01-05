using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 기가데스의 탄환을 정의합니다.
/// </summary>
public class EnemyGigadeathBulletScript : EnemyScript, IFlippableEnemy
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    /// <summary>
    /// Rigidbody2D 요소를 가져옵니다.
    /// </summary>
    Rigidbody2D _rigidbody;
    /// <summary>
    /// BoxCollider2D 요소를 가져옵니다.
    /// </summary>
    Collider2D _collider2D;


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
    /// <summary>
    /// 캐릭터가 오른쪽을 보고 있다면 참입니다.
    /// </summary>
    bool _facingRight = false;
    /// <summary>
    /// 캐릭터가 오른쪽을 보고 있다면 참입니다.
    /// </summary>
    public bool FacingRight
    {
        get { return _facingRight; }
        set
        {
            if (_facingRight != value)
            {
                Flip();
            }
        }
    }


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
        _collider2D = GetComponent<Collider2D>();

        // 초기화를 마무리합니다. 탄환은 _lifeTime 이후 폭발합니다.
        MoveLeft();

        Invoke("Dead", _lifeTime);
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_collider2D.IsTouchingLayers(_whatIsWall))
        {
            Dead();
            CancelInvoke("Dead");
        }
    }
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
                player.Hurt(Damage);
            }
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
    }


    #endregion










    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 왼쪽으로 이동합니다.
    /// </summary>
    void MoveLeft()
    {
        if (_facingRight)
            Flip();
        _rigidbody.velocity = new Vector2(-_movingSpeed, 0);
    }
    /// <summary>
    /// 오른쪽으로 이동합니다.
    /// </summary>
    void MoveRight()
    {
        if (_facingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(_movingSpeed, 0);
    }
    /// <summary>
    /// 방향을 바꿉니다.
    /// </summary>
    public void Flip()
    {
        if (_facingRight)
        {
            _rigidbody.transform.localScale = new Vector3
                (-_rigidbody.transform.localScale.x, _rigidbody.transform.localScale.y);
        }
        else
        {
            _rigidbody.transform.localScale = new Vector3
                (-_rigidbody.transform.localScale.x, _rigidbody.transform.localScale.y);
        }
        _facingRight = !_facingRight;
    }


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
