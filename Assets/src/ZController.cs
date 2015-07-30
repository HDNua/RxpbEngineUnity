using UnityEngine;
using System.Collections;

public class ZController : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    Rigidbody2D _rigidbody;
    Animator _animator;

    #endregion 컨트롤러용 Unity 객체



    #region 필드를 정의합니다.
    AudioSource[] audioSources;


    #endregion 필드



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public AudioClip[] audioClips;

    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;
    public LayerMask whatIsGround;

    public float walkSpeed = 8;

    public float jumpSpeed = 16;
    public float jumpDecSize = 0.8f;

    public BoxCollider2D groundCheck2;

    public Transform pushCheck;
    public LayerMask whatIsWall;
    public float wallCheckRadius = 0.1f;



    public Transform groundCheckLeft;
    public Transform groundCheckRight;

    #endregion Unity 공용 필드



    #region 주인공의 상태 필드를 정의합니다. (deprecated)
    bool facingRight = true;

    bool jumping = false;
    bool falling = false;

    #endregion



    #region 주인공의 상태 필드를 정의합니다.
    bool _moving = false;
    bool _moveBlocked = false;

    bool _jumping = false;
    bool _jumpBlocked = false;
    bool _falling = false;

    bool _dashing = false;
    bool _dashBlocked = false;

    bool _attacking = false;
    bool _attackBlocked = false;
    bool _attackRequested = false;

    bool _sliding = false;
    bool _riding = false;

    #endregion



    #region MonoBehaviour가 정의하는 기본 메서드를 재정의합니다.
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        audioSources = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = audioClips[i];
        }

        /*
        var collider = GetComponent<BoxCollider2D>();
        groundCheckLeft.position = new Vector2(collider.bounds.min.x, collider.bounds.min.y);
        groundCheckRight.position = new Vector2(collider.bounds.max.x, collider.bounds.max.y);

        print("TEST");
        */
    }
    void _Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_animator.GetBool("ShotBlocked") == false)
            {
                RequestShot();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_animator.GetBool("JumpBlocked") == false)
            {
                RequestJump();
            }
        }
    }
    void _FixedUpdate()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        _animator.SetBool("Grounded", grounded);

        _animator.SetBool("JumpRequestExist", Input.GetKey(KeyCode.C));
        _animator.SetBool("Jumping", jumping);
        _animator.SetBool("Falling", falling);

        bool wallTouched = Physics2D.OverlapCircle(pushCheck.position, wallCheckRadius, whatIsWall);
        bool walkRequested = _animator.GetBool("WalkRequestExist");
        bool pushing = wallTouched && walkRequested;
        _animator.SetBool("Pushing", pushing);


        if (pushing)
        {
            RequestPush();
        }
        else if (_animator.GetBool("PushRequestExist"))
        {
            RequestPushEnd();
        }

        if (grounded == false)
        {
            if (jumping)
            {
                if (_animator.GetBool("JumpRequestExist") == false)
                {
                    RequestFall();
                }
                else
                {
                    float verSpeed = _rigidbody.velocity.y; // _animator.GetFloat("VerSpeed");
                    if (verSpeed > 0)
                    {
                        float vy = _rigidbody.velocity.y - jumpDecSize;
                        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, vy > 0 ? vy : 0);
                    }
                    else
                    {
                        RequestFall();
                    }
                }
            }
            else if (falling)
            {
                float verSpeed = _rigidbody.velocity.y; // _animator.GetFloat("VerSpeed");
                if (verSpeed > -jumpSpeed)
                {
                    float vy = _rigidbody.velocity.y - jumpDecSize;
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, vy > -jumpSpeed ? vy : -jumpSpeed);
                }
            }
            else
            {
                RequestFall();
            }
            _animator.SetFloat("VerSpeed", _rigidbody.velocity.y);
        }
        else
        {
            if (falling)
            {
                RequestFallEnd();
            }
        }

        if (_animator.GetBool("WalkBlocked") == false)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (facingRight == true)
                    Flip();
                _rigidbody.velocity = new Vector2(-walkSpeed, _rigidbody.velocity.y);
                RequestWalk();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (facingRight == false)
                    Flip();
                _rigidbody.velocity = new Vector2(walkSpeed, _rigidbody.velocity.y);
                RequestWalk();
            }
            else if (_animator.GetBool("WalkRequestExist"))
            {
                RequestWalkEnd();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_dashing)
            {
                DashJump();
            }
            else if (_jumping)
            {

            }
            else
            {
                Jump();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_jumping)
            {
                JumpAttack();
            }
            else if (_falling)
            {
                JumpAttack();
            }
            else if (_attackBlocked)
            {

            }
            else
            {
                Attack();
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_jumping)
            {
                AirDash();
            }
            else if (_dashing)
            {

            }
            else
            {
                Dash();
            }
        }

    }
    void FixedUpdate()
    {
        bool upPressed = Input.GetKey(KeyCode.UpArrow);
        bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
        bool rightPressed = Input.GetKey(KeyCode.RightArrow);
        bool jumpPressed = Input.GetKey(KeyCode.C);
        bool dashPressed = Input.GetKey(KeyCode.Z);
        bool attackPressed = Input.GetKey(KeyCode.X);

        bool landed = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        /*
        bool leftLanded = Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, whatIsGround);
        bool rightLanded = Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadius, whatIsGround);
        bool landed = leftLanded || rightLanded;
        print(landed);
        */

        // 점프 중이라면
        if (_jumping)
        {
            if (jumpPressed == false || _rigidbody.velocity.y <= 0)
            {
                StopJumping();
                Fall();
            }
            else
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y - 0.8f);
            }
        }
        // 떨어지는 중이라면
        else if (_falling)
        {
            if (landed)
            {
                StopFalling();
            }
            else
            {
                float vy = _rigidbody.velocity.y - 0.8f;
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, vy > -16 ? vy : -16);
            }
        }
        // 공격 중이라면
        else if (_attacking)
        {
            if (_attackBlocked == false)
            {
                // 공격 방향을 전환하는 조건을 나열합니다.
                if (leftPressed && facingRight)
                {
                    Flip();
                }
                else if (rightPressed && facingRight == false)
                {
                    Flip();
                }
            }
        }
        // 대쉬 중이라면
        else if (_dashing)
        {
            // 대쉬를 취소하는 조건을 나열합니다.
            if (leftPressed && facingRight)
            {
                StopDashing();
                MoveLeft();
            }
            else if (rightPressed && facingRight == false)
            {
                StopDashing();
                MoveRight();
            }
        }
        // 사다리를 타는 중이라면
        else if (_riding)
        {

        }
        // 벽을 타고 있다면
        else if (_sliding)
        {

        }
        // 그 외의 경우
        else
        {
            // 공중에 있다면 떨어지게 합니다.
            if (landed == false)
            {
                Fall();
            }
        }

        // 이동이 막혀있다면
        if (_moveBlocked)
        {
        }
        // 이동 가능한 상태라면
        else
        {
            if (leftPressed)
            {
                if (_dashing)
                {
                    if (facingRight)
                    {
                        MoveLeft();
                    }
                }
                else
                {
                    MoveLeft();
                }
            }
            else if (rightPressed)
            {
                if (_dashing)
                {
                    if (facingRight == false)
                    {
                        MoveRight();
                    }
                }
                else
                {
                    MoveRight();
                }
            }
            else
            {
                StopMoving();
            }
        }
    }

    #endregion MonoBehaviour 기본 메서드 재정의



    #region 행동 메서드를 정의합니다.
    void Idle()
    {
        _jumping = false;
        _falling = false;
        _animator.SetBool("Moving", false);
    }

    ///////////////////////////////////////////////////////////////////
    // 이동
    /// <summary>
    /// 플레이어를 왼쪽으로 이동합니다.
    /// </summary>
    void MoveLeft()
    {
        if (facingRight)
            Flip();
        _rigidbody.velocity = new Vector2(-walkSpeed, _rigidbody.velocity.y);
        _animator.SetBool("Moving", true);
    }
    /// <summary>
    /// 플레이어를 오른쪽으로 이동합니다.
    /// </summary>
    void MoveRight()
    {
        if (facingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(walkSpeed, _rigidbody.velocity.y);
        _animator.SetBool("Moving", true);
    }
    /// <summary>
    /// 플레이어의 이동을 중지합니다.
    /// </summary>
    void StopMoving()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _animator.SetBool("Moving", false);
    }
    /// <summary>
    /// 플레이어의 이동 요청을 막습니다.
    /// </summary>
    void BlockMoving()
    {
        _moveBlocked = true;
    }
    /// <summary>
    /// 플레이어가 이동을 요청할 수 있도록 합니다.
    /// </summary>
    void UnblockMoving()
    {
        _moveBlocked = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 점프 및 낙하
    /// <summary>
    /// 플레이어를 점프하게 합니다.
    /// </summary>
    void Jump()
    {
        _jumping = true;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 16);
        _animator.SetBool("Jumping", true);
    }
    /// <summary>
    /// 플레이어의 점프를 중지합니다.
    /// </summary>
    void StopJumping()
    {
        _animator.SetBool("Jumping", false);
    }
    /// <summary>
    /// 플레이어의 점프 요청을 막습니다.
    /// </summary>
    void BlockJumping()
    {
        _animator.SetBool("JumpBlocked", _jumpBlocked = true);
    }
    /// <summary>
    /// 플레이어가 점프할 수 있도록 합니다.
    /// </summary>
    void UnblockJumping()
    {
        _animator.SetBool("JumpBlocked", _jumpBlocked = false);
    }
    /// <summary>
    /// 플레이어를 낙하시킵니다.
    /// </summary>
    void Fall()
    {
        _jumping = false;
        _falling = true;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _animator.SetBool("Falling", true);
    }
    /// <summary>
    /// 플레이어의 낙하를 중지합니다.
    /// </summary>
    void StopFalling()
    {
        _animator.SetBool("Falling", false);
        _falling = false;

        if (_attacking)
        {
            var curState = _animator.GetCurrentAnimatorStateInfo(0);
            if (curState.IsName("Z_jumpShot_1inAir"))
            {
                var nTime = curState.normalizedTime;
                var fTime = nTime - Mathf.Floor(nTime);
                _animator.Play("Z_jumpShot_2ground", 0, fTime);

                StopMoving();
                BlockMoving();
                audioSources[5].Play();
            }
        }
    }

    ///////////////////////////////////////////////////////////////////
    // 공격
    /// <summary>
    /// 플레이어가 공격하게 합니다.
    /// </summary>
    void Attack()
    {
        StopMoving();
        BlockMoving();
        _animator.SetBool("Attacking", _attacking = true);
        _animator.SetBool("AttackRequested", _attackRequested = true);
    }
    /// <summary>
    /// 플레이어의 공격을 중지합니다.
    /// </summary>
    void StopAttacking()
    {
        _animator.SetBool("Attacking", _attacking = false);
        _animator.SetBool("AttackRequested", _attackRequested = false);
        UnblockMoving();
    }
    /// <summary>
    /// 플레이어의 공격 요청을 막습니다.
    /// </summary>
    void BlockAttacking()
    {
        _attackBlocked = true;
    }
    /// <summary>
    /// 플레이어가 공격할 수 있도록 합니다.
    /// </summary>
    void UnblockAttacking()
    {
        _attackBlocked = false;
    }
    /// <summary>
    /// 플레이어가 점프 공격하게 합니다.
    /// </summary>
    void JumpAttack()
    {
        _animator.SetBool("Attacking", _attacking = true);
        _animator.SetBool("AttackRequested", _attackRequested = true);
    }

    ///////////////////////////////////////////////////////////////////
    // 대쉬
    /// <summary>
    /// 플레이어가 대쉬하게 합니다.
    /// </summary>
    void Dash()
    {
        BlockMoving();
        BlockDashing();
    }
    /// <summary>
    /// 플레이어의 대쉬를 중지합니다.
    /// </summary>
    void StopDashing()
    {
        UnblockMoving();
        UnblockDashing();
    }
    /// <summary>
    /// 플레이어의 대쉬 요청을 막습니다.
    /// </summary>
    void BlockDashing()
    {
        _dashBlocked = true;
    }
    /// <summary>
    /// 플레이어가 대쉬할 수 있도록 합니다.
    /// </summary>
    void UnblockDashing()
    {
        _dashBlocked = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 기타
    /// <summary>
    /// 플레이어가 벽을 타도록 합니다.
    /// </summary>
    void Slide()
    {
        _animator.SetBool("Sliding", _sliding = true);
    }
    /// <summary>
    /// 플레이어의 벽 타기를 중지합니다.
    /// </summary>
    void StopSliding()
    {
        _animator.SetBool("Sliding", _sliding = false);
    }
    /// <summary>
    /// 플레이어가 사다리를 타도록 합니다.
    /// </summary>
    void RideLadder()
    {

    }

    ///////////////////////////////////////////////////////////////////
    // 조합
    /// <summary>
    /// 플레이어가 에어 대쉬하게 합니다.
    /// </summary>
    void AirDash()
    {

    }
    /// <summary>
    /// 플레이어가 대쉬 점프하게 합니다.
    /// </summary>
    void DashJump()
    {

    }

    #endregion



    #region 요청 메서드를 정의합니다.
    void RequestIdle()
    {
        _animator.SetBool("FallRequested", false);
        _animator.SetBool("JumpBlocked", false);
        _animator.SetBool("FallEnd", false);
        _animator.SetBool("ShotBlocked", false);
        _animator.SetBool("WalkBlocked", false);
    }
    void RequestShot()
    {
        if (_animator.GetBool("ShotBlocked") == false)
        {
            _animator.SetBool("ShotRequested", true);
            _animator.SetBool("ShotBlocked", true);
            _animator.SetBool("JumpBlocked", true);

            // 지상에 있을 경우
            if (_animator.GetBool("Grounded"))
            {
                RequestWalkBlock();
                RequestWalkEnd();
                print("JumpShot Requested");
            }
        }
    }
    void RequestJump()
    {
        if (_animator.GetBool("JumpBlocked") == false)
        {
            _animator.SetBool("JumpRequestExist", true);
            _animator.SetBool("JumpRequested", true);
            _animator.SetBool("JumpBlocked", true);
            _animator.SetBool("WalkBlocked", false);
            jumping = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
        }
    }
    void RequestFall()
    {
        jumping = false;
        falling = true;

        _animator.SetBool("JumpBlocked", true);
        _animator.SetBool("FallRequested", true);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -jumpDecSize);
    }
    void RequestFallEnd()
    {
        _animator.SetBool("FallEndRequested", true);
        falling = false;

        // 공중 공격 중이었다면
        if (_animator.GetBool("ShotBlocked"))
        {
            RequestWalkBlock();
            RequestWalkEnd();
        }

        var curState = _animator.GetCurrentAnimatorStateInfo(0);
        if (curState.IsName("Z_jumpShot_1inAir"))
        {
            var nTime = curState.normalizedTime;
            var fTime = nTime - Mathf.Floor(nTime);
            _animator.Play("Z_jumpShot_2ground", 0, fTime);
            RequestWalkEnd();
            RequestWalkBlock();
            _animator.SetBool("FallEndRequested", false);
            audioSources[5].Play();
        }
    }
    void RequestWalk()
    {
        _animator.SetBool("FallEnd", false);
        _animator.SetBool("WalkRequestExist", true);
    }
    void RequestWalkEnd()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _animator.SetBool("WalkRequestExist", false);
        _animator.SetBool("PushRequestExist", false);
        _animator.SetBool("PushRequested", false);
    }
    void RequestWalkBlock()
    {
        _animator.SetBool("WalkBlocked", true);
    }
    void RequestPush()
    {
        _animator.SetBool("PushRequested", true);
        _animator.SetBool("PushRequestExist", true);
    }
    void RequestPushEnd()
    {
        _animator.SetBool("PushRequestExist", false);
    }

    #endregion



    #region 프레임 이벤트 핸들러를 정의합니다.
    public void RemainIdle()
    {
        _animator.SetBool("ShotBlocked", false);
        _animator.SetBool("JumpBlocked", false);
        _animator.SetBool("WalkBlocked", false);
    }
    public void WalkBeg_beg()
    {
    }
    public void WalkBeg_end()
    {
    }
    public void Jump_beg()
    {
        _animator.SetBool("JumpRequested", false);
        audioSources[4].Play();
        audioSources[6].Play();
    }
    public void Fall_beg()
    {
        _animator.SetBool("FallRequested", false);
    }
    public void FallEndFromRun_beg()
    {
        RequestWalkBlock();
        _animator.SetBool("FallRequested", false);
        _animator.SetBool("FallEndRequested", false);
        _animator.SetBool("FallEnd", true);
        _animator.SetBool("JumpBlocked", false);
        audioSources[5].Play();
    }
    public void FallEndFromRun_end()
    {
        RequestIdle();
    }

    // 지상 공격 시에 발생하는 이벤트에 대한 핸들러입니다.
    /// <summary>
    /// 첫 번째 일반 지상 공격 시에 발생합니다.
    /// </summary>
    public void Attack_saber1()
    {
        // 받은 요청은 삭제합니다.
        _animator.SetBool("AttackRequested", _attackRequested = false);

        // 공격 시 불가능한 행동을 막습니다.
        BlockAttacking(); // 공격
        BlockJumping(); // 점프
        BlockDashing(); // 대쉬

        // 효과음을 재생합니다.
        audioSources[0].Play();
        audioSources[3].Play();
    }
    /// <summary>
    /// 첫 번째 일반 지상 공격이 종료할 때 발생합니다.
    /// </summary>
    public void Attack_saber1_end()
    {
        // 막은 행동을 가능하게 합니다.
        UnblockAttacking();
        UnblockJumping();
        UnblockDashing();
    }
    /// <summary>
    /// 두 번째 일반 지상 공격 시에 발생합니다.
    /// </summary>
    public void Attack_saber2()
    {
        // 받은 요청은 삭제합니다.
        _animator.SetBool("AttackRequested", _attackRequested = false);

        // 공격 시 불가능한 행동을 막습니다.
        BlockAttacking(); // 공격
        BlockJumping(); // 점프
        BlockDashing(); // 대쉬

        // 효과음을 재생합니다.
        audioSources[1].Play();
        audioSources[3].Play();
    }
    /// <summary>
    /// 두 번째 일반 지상 공격이 종료할 때 발생합니다.
    /// </summary>
    public void Attack_saber2_end()
    {
        // 막은 행동을 가능하게 합니다.
        UnblockAttacking();
        UnblockJumping();
        UnblockDashing();
    }
    /// <summary>
    /// 세 번째 일반 지상 공격 시에 발생합니다.
    /// </summary>
    public void Attack_saber3()
    {
        // 받은 요청은 삭제합니다.
        _animator.SetBool("AttackRequested", _attackRequested = false);

        // 공격 시 불가능한 행동을 막습니다.
        BlockAttacking(); // 공격
        BlockJumping(); // 점프
        BlockDashing(); // 대쉬

        // 효과음을 재생합니다.
        audioSources[2].Play();
        audioSources[3].Play();
    }
    /// <summary>
    /// 지상 공격이 종료할 때 발생합니다.
    /// </summary>
    public void AttackEndFromRun_beg()
    {
        UnblockAttacking();
    }
    /// <summary>
    /// 지상 공격 모션이 완전히 종료되어 대기 상태로 바뀔 때 발생합니다.
    /// </summary>
    public void AttackEndFromRun_end()
    {
        StopAttacking();
    }

    ///////////////////////////////////////////////////////////////////
    // 점프 공격
    /// <summary>
    /// 점프 공격 시에 발생합니다.
    /// </summary>
    public void JumpShot_beg()
    {
        BlockAttacking();
        audioSources[3].Play();
    }
    /// <summary>
    /// 점프 공격이 종료할 때 발생합니다.
    /// </summary>
    public void JumpShot_end()
    {
        UnblockAttacking();
        StopAttacking();
    }

    ///////////////////////////////////////////////////////////////////
    // 벽 타기
    /// <summary>
    /// 벽 타기 시에 발생합니다.
    /// </summary>
    public void SlideWall_beg()
    {
        _animator.SetBool("PushRequested", false);
        audioSources[7].Play();

    }
    /// <summary>
    /// 벽 타기가 종료할 때 발생합니다.
    /// </summary>
    public void SlideWall_end()
    {

    }

    #endregion 프레임 이벤트 핸들러



    #region 요청을 수행하기 위한 보조 메서드를 정의합니다.
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    #endregion 보조 메서드 정의
}
