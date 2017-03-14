using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 보스 적 캐릭터를 정의합니다.
/// </summary>
public abstract class EnemyBossScript : EnemyScript
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    /// <summary>
    /// Rigidbody2D 요소를 가져옵니다.
    /// </summary>
    protected Rigidbody2D _Rigidbody
    {
        get { return GetComponent<Rigidbody2D>(); }
    }
    /// <summary>
    /// Animator 요소를 가져옵니다.
    /// </summary>
    protected Animator _Animator
    {
        get { return GetComponent<Animator>(); }
    }
    /// <summary>
    /// SpriteRenderer 요소를 가져옵니다.
    /// </summary>
    protected SpriteRenderer _Renderer
    {
        get { return GetComponent<SpriteRenderer>(); }
    }

    #endregion





    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    /// <summary>
    /// 자신의 밑에 지면이 존재하는지 검사하기 위해 사용합니다.
    /// </summary>
    public Transform _groundCheck;
    /// <summary>
    /// 자신이 진행하는 방향에 벽이 존재하는지 검사하기 위해 사용합니다.
    /// </summary>
    public Transform _pushCheck;
    /// <summary>
    /// 무엇이 벽인지를 결정합니다. 기본값은 "Wall, MapBlock"입니다.
    /// </summary>
    public LayerMask _whatIsWall;
    /// <summary>
    /// 무엇이 땅인지를 결정합니다. 기본값은 "Ground, TiledGeometry"입니다.
    /// </summary>
    public LayerMask _whatIsGround;

    /// <summary>
    /// 걷는 속도입니다.
    /// </summary>
    public float _walkSpeed = 5;
    /// <summary>
    /// 점프 시작 속도입니다.
    /// </summary>
    public float _jumpSpeed = 16;
    /// <summary>
    /// 점프한 이후로 매 시간 속도가 깎이는 양입니다.
    /// </summary>
    public float _jumpDecSize = 0.8f;
    /// <summary>
    /// 대쉬 속도입니다.
    /// </summary>
    public float _dashSpeed = 12;

    #endregion





    #region 캐릭터의 운동 상태 필드를 정의합니다.
    /// <summary>
    /// 지상에 있다면 true입니다.
    /// </summary>
    public bool _landed = false;
    /// <summary>
    /// 지상에서 이동하고 있다면 true입니다.
    /// </summary>
    bool _moving = false;
    /// <summary>
    /// 벽을 밀고 있다면 true입니다.
    /// </summary>
    bool _pushing = false;
    /// <summary>
    /// 점프 상태라면 true입니다.
    /// </summary>
    bool _jumping = false;
    /// <summary>
    /// 떨어지고 있다면 true입니다.
    /// </summary>
    bool _falling = false;
    /// <summary>
    /// 지상에서 대쉬 중이라면 true입니다.
    /// </summary>
    bool _dashing = false;

    /// <summary>
    /// 플레이어의 최대 체력을 확인합니다.
    /// </summary>
    public int _maxHealth = 40;
    /// <summary>
    /// 위험 상태로 바뀌는 체력의 값입니다.
    /// </summary>
    public int _dangerHealth = 10;

    #endregion





    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.
    /// <summary>
    /// 캐릭터가 움직이는 속도를 정의합니다.
    /// </summary>
    public float _movingSpeed = 1;

    /// <summary>
    /// 지상에 있다면 true입니다.
    /// </summary>
    public bool Landed
    {
        get { return _landed; }
        protected set { _Animator.SetBool("Landed", _landed = value); }
    }
    /// <summary>
    /// 지상에서 이동하고 있다면 true입니다.
    /// </summary>
    protected bool Moving
    {
        get { return _moving; }
        set { _Animator.SetBool("Moving", _moving = value); }
    }
    /// <summary>
    /// 벽을 밀고 있다면 true입니다.
    /// </summary>
    protected bool Pushing
    {
        get { return _pushing; }
        set { _Animator.SetBool("Pushing", _pushing = value); }
    }
    /// <summary>
    /// 점프 상태라면 true입니다.
    /// </summary>
    protected bool Jumping
    {
        get { return _jumping; }
        set { _Animator.SetBool("Jumping", _jumping = value); }
    }
    /// <summary>
    /// 떨어지고 있다면 true입니다.
    /// </summary>
    protected bool Falling
    {
        get { return _falling; }
        set { _Animator.SetBool("Falling", _falling = value); }
    }
    /// <summary>
    /// 지상에서 대쉬 중이라면 true입니다.
    /// </summary>
    protected bool Dashing
    {
        get { return _dashing; }
        set { _Animator.SetBool("Dashing", _dashing = value); }
    }


    /// <summary>
    /// 최대 체력입니다.
    /// </summary>
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    /// <summary>
    /// 플레이어의 체력이 가득 찼는지 확인합니다.
    /// </summary>
    /// <returns>체력이 가득 찼다면 true입니다.</returns>
    public bool IsHealthFull()
    {
        return (_health == _maxHealth);
    }


    /// <summary>
    /// 플레이어의 속도(RigidBody2D.velocity)입니다.
    /// </summary>
    public Vector2 _Velocity
    {
        get { return _Rigidbody.velocity; }
        set
        {
            _Rigidbody.velocity = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    bool _appearEnded = false;
    /// <summary>
    /// 
    /// </summary>
    public bool AppearEnded
    {
        get { return _appearEnded; }
        protected set { _appearEnded = value; }
    }

    /// <summary>
    /// 후방 지형 검사를 위한 위치 객체입니다.
    /// </summary>
    public Transform groundCheckBack;
    /// <summary>
    /// 전방 지형 검사를 위한 위치 객체입니다.
    /// </summary>
    public Transform groundCheckFront;
    /// <summary>
    /// 지형 검사 범위를 표현하는 실수입니다.
    /// </summary>
    public float groundCheckRadius = 0.1f;
    /// <summary>
    /// 현재 플레이어와 닿아있는 땅 지형의 집합입니다.
    /// </summary>
    HashSet<EdgeCollider2D> _groundEdgeSet = new HashSet<EdgeCollider2D>();

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        /*
        // 필드를 초기화합니다.
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();

        // 자신과 가장 가까운 바닥으로 y 좌표를 옮깁니다.
        RaycastHit2D groundRay = Physics2D.Raycast
            (_groundCheck.position, Vector2.down, 10f, _whatIsGround);
        Vector2 newPos = transform.position;
        newPos.y -= Mathf.Abs(_collider2D.bounds.min.y - groundRay.point.y);
        transform.position = newPos;

        MoveLeft();
        */
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        /*
        // 사용할 변수를 선언합니다.
        float posX = transform.position.x;
        float boundLeft = SpawnZone.Left;
        float boundRight = SpawnZone.Right;


        // 영역을 넘어서면 방향을 전환하여 원래대로 복귀합니다.
        if (posX < boundLeft)
        {
            MoveRight();
        }
        else if (boundRight < posX)
        {
            MoveLeft();
        }
        */
    }
    /// <summary>
    /// FixedTimestep에 설정된 값에 따라 일정한 간격으로 업데이트 합니다.
    /// 물리 효과가 적용된 오브젝트를 조정할 때 사용됩니다.
    /// (Update는 불규칙한 호출이기 때문에 물리엔진 충돌검사가 제대로 되지 않을 수 있습니다.)
    /// </summary>
    protected override void FixedUpdate()
    {
        // 기존 사용자 입력을 확인합니다.
        // 점프 중이라면
        if (Jumping)
        {
            if (_Velocity.y <= 0)
            {
                Fall();
            }
            else
            {
                _Velocity = new Vector2(_Velocity.x, _Velocity.y - _jumpDecSize);
            }
        }
        // 떨어지고 있다면
        else if (Falling)
        {
            if (Landed)
            {
                Land();
            }
            else
            {
                float vy = _Velocity.y - _jumpDecSize;
                _Velocity = new Vector2(_Velocity.x, vy > -16 ? vy : -16);
            }
        }
        // 그 외의 경우
        else
        {
            if (Landed == false)
            {
                Fall();
            }
        }
    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    protected override void LateUpdate()
    {
        base.LateUpdate();
        UpdateState();

        // 색상을 업데이트합니다.
        UpdateColor();
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

        // 사망 시 아이템 드롭 루틴입니다.
        int dropItem = UnityEngine.Random.Range(0, _items.Length);
        if (_items[dropItem] != null)
        {
            CreateItem(_items[dropItem]);
        }

        // 캐릭터가 사망합니다.
        base.Dead();

        // 개체 제거를 요청합니다.
        Invoke("RequestDestroy", 3f);
    }

    #endregion





    #region 추상 메서드를 선언합니다.
    /// <summary>
    /// 등장 액션입니다.
    /// </summary>
    public abstract void Appear();
    /// <summary>
    /// 전투 시작 액션입니다.
    /// </summary>
    public abstract void Fight();

    #endregion





    #region 상태 메서드를 정의합니다.
    /// <summary>
    /// 플레이어가 체력을 회복합니다.
    /// </summary>
    public void Heal()
    {
        Health += 10;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    #endregion





    #region 행동 메서드를 정의합니다.
    ///////////////////////////////////////////////////////////////////
    // 기본
    /// <summary>
    /// 플레이어가 지상에 착륙할 때의 상태를 설정합니다.
    /// </summary>
    protected virtual void Land()
    {
        StopJumping();
        StopFalling();
    }
    /// <summary>
    /// 왼쪽으로 이동합니다.
    /// </summary>
    protected void MoveLeft()
    {
        if (FacingRight)
            Flip();

        Moving = true;

        _Rigidbody.velocity = new Vector2(-_movingSpeed, 0);
    }
    /// <summary>
    /// 오른쪽으로 이동합니다.
    /// </summary>
    protected void MoveRight()
    {
        if (FacingRight == false)
            Flip();

        Moving = true;

        _Rigidbody.velocity = new Vector2(_movingSpeed, 0);
    }
    /// <summary>
    /// 이동을 중지합니다.
    /// </summary>
    protected virtual void StopMoving()
    {
        _Velocity = new Vector2(0, _Velocity.y);
        Moving = false;
    }
    /// <summary>
    /// 개체 제거를 요청합니다.
    /// </summary>
    protected void RequestDestroy()
    {
        Destroy(gameObject);
    }
    
    ///////////////////////////////////////////////////////////////////
    // 점프 및 낙하
    /// <summary>
    /// 플레이어를 점프하게 합니다.
    /// </summary>
    protected virtual void Jump()
    {
        // 개체의 운동 상태를 갱신합니다.
        _Velocity = new Vector2(_Velocity.x, _jumpSpeed);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = true;
    }
    /// <summary>
    /// 플레이어의 점프를 중지합니다.
    /// </summary>
    protected virtual void StopJumping()
    {
        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = false;
    }
    /// <summary>
    /// 플레이어를 낙하시킵니다.
    /// </summary>
    protected virtual void Fall()
    {
        // 개체의 운동 상태를 갱신합니다.
        if (_Velocity.y > 0)
        {
            _Velocity = new Vector2(_Velocity.x, 0);
        }

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = false;
        Falling = true;
    }
    /// <summary>
    /// 플레이어의 낙하를 중지합니다.
    /// </summary>
    protected virtual void StopFalling()
    {
        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Falling = false;
    }

    #endregion




    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 애니메이션이 재생 중인지 확인합니다.
    /// </summary>
    /// <param name="stateName">재생 중인지 확인하려는 애니메이션의 이름입니다.</param>
    /// <returns>애니메이션이 재생 중이라면 true를 반환합니다.</returns>
    protected bool IsAnimationPlaying(string stateName)
    {
        return _Animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    /// <summary>
    /// 
    /// </summary>
    public void RequestBossHeal()
    {

    }

    #endregion





    #region 캐릭터의 지상 착륙 상태를 업데이트합니다.
    /// <summary>
    /// 플레이어의 물리 상태를 갱신합니다.
    /// </summary>
    protected void UpdateState()
    {
        UpdateLanding();
    }
    /// <summary>
    /// 충돌이 시작되었습니다.
    /// </summary>
    /// <param name="collision">충돌 객체입니다.</param>
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        UpdatePhysicsState(collision);
    }
    /// <summary>
    /// 충돌이 유지되고 있습니다.
    /// </summary>
    /// <param name="collision">충돌 객체입니다.</param>
    protected void OnCollisionStay2D(Collision2D collision)
    {
        UpdatePhysicsState(collision);
    }
    /// <summary>
    /// 충돌이 끝났습니다.
    /// </summary>
    /// <param name="collision">충돌 객체입니다.</param>
    protected void OnCollisionExit2D(Collision2D collision)
    {
        UpdatePhysicsState(collision);
    }
    
    /// <summary>
    /// 플레이어가 땅과 접촉했는지에 대한 필드를 갱신합니다.
    /// </summary>
    /// <returns>플레이어가 땅에 닿아있다면 참입니다.</returns>
    bool UpdateLanding()
    {
        RaycastHit2D rayB = Physics2D.Raycast(groundCheckBack.position, Vector2.down, groundCheckRadius, _whatIsGround);
        RaycastHit2D rayF = Physics2D.Raycast(groundCheckFront.position, Vector2.down, groundCheckRadius, _whatIsGround);

        Debug.DrawRay(groundCheckBack.position, Vector2.down, Color.red);
        Debug.DrawRay(groundCheckFront.position, Vector2.down, Color.red);

        if (Handy.DebugPoint) // PlayerController.UpdateLanding
        {
            Handy.Log("PlayerController.UpdateLanding");
        }

        if (OnGround())
        {
            // 절차:
            // 1. 캐릭터에서 수직으로 내린 직선에 맞는 경사면의 법선 벡터를 구한다.
            // 2. 법선 벡터와 이동 방향 벡터가 이루는 각도가 예각이면 내려오는 것
            //    법선 벡터와 이동 방향 벡터가 이루는 각도가 둔각이면 올라가는 것
            /// Handy.Log("OnGround()");
            
            // 앞 부분 Ray와 뒤 부분 Ray의 경사각이 다른 경우
            if (rayB.normal.normalized != rayF.normal.normalized)
            {
                bool isTouchingSlopeFromB = rayB.normal.x == 0;
                /// Transform pos = isTouchingSlopeFromB ? groundCheckBack : groundCheckFront;
                RaycastHit2D ray = isTouchingSlopeFromB ? rayB : rayF;

                Vector2 from = FacingRight ? Vector2.right : Vector2.left;
                float rayAngle = Vector2.Angle(from, ray.normal);
                float rayAngleRad = Mathf.Deg2Rad * rayAngle;

                float sx = _movingSpeed * Mathf.Cos(rayAngleRad);
                float sy = _movingSpeed * Mathf.Sin(rayAngleRad);
                float vx = FacingRight ? sx : -sx;

                if (Jumping)
                {
                }
                // 예각이라면 내려갑니다.
                else if (rayAngle < 90)
                {
                    float vy = -sy;
                    _Velocity = new Vector2(vx, vy);
                }
                // 둔각이라면 올라갑니다.
                else if (rayAngle > 90)
                {
                    float vy = sy;
                    _Velocity = new Vector2(vx, vy);
                }
                // 90도라면
                else
                {
                }
            }
            else
            {
            }

            Landed = true;
        }
        else if (rayB || rayF)
        {
            RaycastHit2D ray;

            if (rayB && !rayF)
            {
                // difY = rayB.distance / transform.localScale.y;
                // pos.y -= difY;
                ray = rayB;
            }
            else if (!rayB && rayF)
            {
                // difY = rayF.distance / transform.localScale.y;
                // pos.y -= difY;
                ray = rayF;
            }
            else
            {
                // difY = Mathf.Min(rayB.distance, rayF.distance) / transform.localScale.y;
                // pos.y -= difY;
                ray = rayB.distance < rayF.distance ? rayB : rayF;
            }

            // 
            Vector3 pos = transform.position;
            float difY = ray.distance / transform.localScale.y;
            pos.y -= difY;
            if (Mathf.Abs(difY) < _jumpDecSize)
            {
                // transform.position = pos;
                _Velocity = new Vector2(_Velocity.x, 0);
                Landed = true;
            }
            else
            {
                Landed = false;
            }
        }
        else if (Jumping || Falling)
        {
            Landed = false;
        }
        else
        {
            Landed = false;
        }
        return Landed;
    }
    /// <summary>
    /// 플레이어의 물리 상태를 갱신합니다.
    /// </summary>
    /// <param name="collision">충돌 정보를 담고 있는 객체입니다.</param>
    void UpdatePhysicsState(Collision2D collision)
    {
        int layer = collision.collider.gameObject.layer;

        // 땅과 접촉한 경우의 처리입니다.
        if (IsSameLayer(layer, _whatIsGround))
        {
            EdgeCollider2D groundCollider = collision.collider as EdgeCollider2D;
            if (IsTouchingGround(groundCollider))
            {
                _groundEdgeSet.Add(groundCollider);
            }
            else
            {
                _groundEdgeSet.Remove(groundCollider);
            }
        }

        // 벽과 접촉한 경우의 처리입니다.
        if (IsSameLayer(layer, _whatIsWall))
        {
            bool isTouchingWall = IsTouchingWall(collision);
            Pushing = isTouchingWall;
        }
    }
    /// <summary>
    /// 레이어가 어떤 레이어 마스크에 포함되는지 확인합니다.
    /// </summary>
    /// <param name="layer">확인할 레이어입니다.</param>
    /// <param name="layerMask">레이어 마스크입니다.</param>
    /// <returns>레이어가 인자로 넘어온 레이어 마스크에 포함된다면 true입니다.</returns>
    bool IsSameLayer(int layer, LayerMask layerMask)
    {
        return ((1 << layer) & layerMask) != 0;
    }
    /// <summary>
    /// 땅에 닿았는지 확인합니다. 측면에서 닿은 것은 포함하지 않습니다.
    /// </summary>
    /// <returns>땅과 닿아있다면 true입니다.</returns>
    bool OnGround()
    {
        // 땅과 닿아있는 경우 몇 가지 더 검사합니다.
        if (_Collider.IsTouchingLayers(_whatIsGround))
        {
            float playerBottom = _Collider.bounds.min.y;
            foreach (EdgeCollider2D edge in _groundEdgeSet)
            {
                float groundTop = edge.bounds.max.y;
                float groundBottom = edge.bounds.min.y;

                // 평면인 경우
                if (groundBottom == groundTop)
                {
                    if (playerBottom >= groundTop)
                    {
                        Debug.DrawLine(edge.points[0] * 0.02008f, edge.points[1] * 0.02008f, Color.red);
                        return true;
                    }
                }
                // 경사면인 경우
                else
                {
                    if (groundBottom <= playerBottom && playerBottom <= groundTop)
                    {
                        Debug.DrawLine(edge.points[0] * 0.02008f, edge.points[1] * 0.02008f, Color.blue);
                        return true;
                    }
                }
            }
            return false;
        }
        return false;
    }
    /// <summary>
    /// 땅에 닿았는지 확인합니다. 측면에서 닿은 것은 포함하지 않습니다.
    /// </summary>
    /// <param name="groundCollider">확인하려는 collider입니다.</param>
    /// <returns>땅에 닿았다면 참입니다.</returns>
    bool IsTouchingGround(EdgeCollider2D groundCollider)
    {
        // 땅과 닿아있는 경우 몇 가지 더 검사합니다.
        if (_Collider.IsTouching(groundCollider))
        {
            Bounds groundBounds = groundCollider.bounds;
            if (groundBounds.min.y == groundBounds.max.y)
            {
                float playerBot = _Collider.bounds.min.y;
                float groundTop = groundBounds.max.y;
                if (playerBot >= groundTop)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                float playerBot = _Collider.bounds.min.y;
                // float groundTop = groundBounds.max.y;
                float groundBottom = groundBounds.min.y;
                if (groundBottom <= playerBot) // && playerBot <= groundTop)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        // 땅과 닿아있지 않다면 거짓입니다.
        return false;
    }
    /// <summary>
    /// 벽에 닿았는지 확인합니다.
    /// </summary>
    /// <param name="collision">충돌 정보를 갖고 있는 객체입니다.</param>
    /// <returns>벽과 닿아있다면 true입니다.</returns>
    bool IsTouchingWall(Collision2D collision)
    {
        // 벽과 닿아있는 경우 몇 가지 더 검사합니다.
        if (_Collider.IsTouchingLayers(_whatIsWall))
        {
            return true;
            /*
            if (pushCheck.IsTouchingLayers(_whatIsWall))
            {
                return true;
            }
            else
            {
                return false;
            }
            */
        }
        // 벽과 닿아있지 않으면 거짓입니다.
        return false;
    }

    #endregion





    #region 구형 정의를 보관합니다.


    #endregion
}