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

    public RectTransform groundCheckRect;
    public Transform groundCheckLeft;
    public Transform groundCheckRight;

    public Transform pushCheck;
    public float pushCheckRadius = 0.1f;
    public LayerMask whatIsWall;

    public float walkSpeed = 5;
    public float jumpSpeed = 16;
    public float jumpDecSize = 0.8f;
    public float slideSpeed = 4f;
    public float spawnSpeed = 16;
    public float dashSpeed = 10;

    public GameObject[] effects;
    public Transform dashFogPosition;
    public Transform dashBoostPosition;
    public Transform slideFogPosition;

    public GameObject slideFogEffect;

    public BoxCollider2D[] attackRange;

    #endregion Unity 공용 필드



    #region 주인공의 게임 상태 필드를 정의합니다.
    int hitPoint;
    int maxHitPoint;

    #endregion



    #region 주인공의 운동 상태 필드를 정의합니다.
    float movingSpeed = 5;

    bool facingRight = true;

    bool _landed = true;

    bool _moving = false;
    bool _moveBlocked = false;

    bool _jumping = false;
    bool _jumpBlocked = false;
    bool _falling = false;

    bool _dashing = false;
    bool _dashBlocked = false;
    bool _dashRequested = false;

    bool _attacking = false;
    bool _attackBlocked = false;
    bool _attackRequested = false;

    bool _sliding = false;
    bool _slideBlocked = false;
    bool _slideRequested = false;
    bool _wallJumping = false;

    bool _riding = false;

    bool _spawning = true;

    bool _dashJumping = false;
    bool _airDashing = false;
    bool _airDashBlocked = false;

    int _airDashCount = 0;

    #endregion



    #region 플레이어의 상태를 가져오는 프로퍼티를 정의합니다.
    public bool FacingRight { get { return facingRight; } }

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

        // 공격 범위 활성화를 취소합니다.
        foreach (var aRange in attackRange)
        {
            aRange.enabled = false;
        }

        // 준비 상태가 완료될 때까지 사용자 입력을 막습니다.
        BlockUserInput();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_jumpBlocked)
            {

            }
            else if (_dashing)
            {
                DashJump();
            }
            else if (_dashRequested)
            {
                if (_sliding)
                {
                    WallDashJump();
                }
                else
                {
                    DashJump();
                }
            }
            else if (_sliding)
            {
                if (_slideBlocked)
                {

                }
                else if (_falling)
                {

                }
                else if (_jumping)
                {

                }
                else
                {
                    WallJump();
                }
            }
            else if (_attacking)
            {
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
            if (_attackBlocked)
            {

            }
            else if (_airDashing)
            {
                StopAirDashing();
                JumpAttack();
            }
            else if (_jumping)
            {
                JumpAttack();
            }
            else if (_falling)
            {
                JumpAttack();
            }
            else
            {
                Attack();
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_dashBlocked)
            {

            }
            /*
            else if (_airDashBlocked)
            {

            }
            else if (_airDashing)
            {

            }
            */
            else if (_jumping)
            {
                if (_airDashCount == 0)
                {
                    StopJumping();
                    AirDash();
                }
            }
            else if (_falling)
            {
                if (_airDashCount == 0)
                {
                    StopFalling();
                    AirDash();
                }
            }
            else if (_attacking)
            {
                StopAttacking();
                Dash();
            }
            else if (_sliding)
            {

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
        bool centerLanded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, whatIsGround);
        bool leftLanded = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, groundCheckRadius, whatIsGround);
        bool rightLanded = Physics2D.Raycast(groundCheckRight.position, Vector2.down, groundCheckRadius, whatIsGround);
        _landed = leftLanded || centerLanded || rightLanded;
        _animator.SetBool("Landed", _landed);

        if (_spawning)
        {
            if (_landed)
            {
                Ready();
                _spawning = false;
            }
            else
            {

            }
            return;
        }

        bool upPressed = Input.GetKey(KeyCode.UpArrow);
        bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
        bool rightPressed = Input.GetKey(KeyCode.RightArrow);
        bool jumpPressed = Input.GetKey(KeyCode.C);
        bool dashPressed = Input.GetKey(KeyCode.Z);
        bool attackPressed = Input.GetKey(KeyCode.X);

        _dashRequested = dashPressed;

        bool sideTouched = Physics2D.OverlapCircle(pushCheck.position, pushCheckRadius, whatIsWall);
        bool leftMoving = leftPressed && (facingRight == false);
        bool rightMoving = rightPressed && facingRight;
        bool pushing = sideTouched && (leftMoving || rightMoving);
        _animator.SetBool("Pushing", pushing);

        // 점프 중이라면
        if (_jumping)
        {
            if (_slideBlocked)
            {

            }
            else if (pushing)
            {
                Slide();
            }
            else if (jumpPressed == false || _rigidbody.velocity.y <= 0)
            {
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
            if (_landed)
            {
                StopFalling();
                Land();
            }
            else if (pushing)
            {
                Slide();
            }
            else if (_airDashing)
            {
                if (dashPressed)
                {

                }
                else
                {
                    StopAirDashing();
                    Fall();
                }
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
            if (_landed == false)
            {
                StopDashing();
                Fall();
            }
            else if (dashPressed == false)
            {
                StopDashing();
            }
            /*
            else if (leftPressed && facingRight)
            {
                StopDashing();
                MoveLeft();
            }
            else if (rightPressed && facingRight == false)
            {
                StopDashing();
                MoveRight();
            }
            */
        }
        // 사다리를 타는 중이라면
        else if (_riding)
        {

        }
        // 벽을 타고 있다면
        else if (_sliding)
        {
            if (pushing == false)
            {
                StopSliding();
                Fall();
            }
            else if (_landed)
            {
                StopSliding();
                Fall();
            }
        }
        // 그 외의 경우
        else
        {
            // 공중에 있다면 떨어지게 합니다.
            if (_landed == false)
            {
                Fall();
            }
        }

        // 이동 요청이 막혀있다면
        if (_moveBlocked)
        {
            // 대쉬 상태라면
            if (_dashing)
            {
                // 점프 또는 낙하 상태라면
                if (_landed == false)
                {
                    if (leftPressed)
                    {
                        MoveLeft();
                    }
                    else if (rightPressed)
                    {
                        MoveRight();
                    }
                    else
                    {
                        StopMoving();
                    }
                }
                // 그 외의 경우
                else
                {

                }
            }
        }
        // 이동이 막혀있다면
        else if (pushing)
        {
            if (_landed)
            {
                StopMoving();
            }
            else if (_slideBlocked)
            {

            }
            else if (_airDashing)
            {
                StopAirDashing();
                Slide();
            }
            else
            {
                Slide();
            }
        }
        // 벽 점프 상태라면
        else if (_slideBlocked)
        {
        }
        // 이동 가능한 상태라면
        else
        {
            if (leftPressed)
            {
                MoveLeft();
            }
            else if (rightPressed)
            {
                MoveRight();
            }
            else
            {
                StopMoving();
            }
        }
    }

    #endregion MonoBehaviour 기본 메서드 재정의



    #region 외부에서 요청 가능한 행동 메서드를 정의합니다.
    /// <summary>
    /// 플레이어를 소환합니다.
    /// </summary>
    public void SpawnPlayer()
    {
        this.enabled = true;
        Spawn();
    }

    #endregion 공용 행동 메서드



    #region 행동 메서드를 정의합니다.
    ///////////////////////////////////////////////////////////////////
    // 기본
    /// <summary>
    /// 플레이어를 소환합니다.
    /// </summary>
    void Spawn()
    {
        _spawning = true;
        _rigidbody.velocity = new Vector2(0, -spawnSpeed);
        audioSources[9].Play();
    }
    /// <summary>
    /// 플레이어가 준비하게 합니다.
    /// </summary>
    void Ready()
    {
        _spawning = false;
        _rigidbody.velocity = Vector2.zero;
    }
    /// <summary>
    /// 준비 상태를 중지합니다.
    /// </summary>
    void StopReady()
    {
        UnblockUserInput();
    }
    /// <summary>
    /// 플레이어가 대기하게 합니다.
    /// </summary>
    void Idle()
    {
        StopMoving();
        StopJumping();
        StopSliding();
        StopAttacking();

        UnblockMoving();
        UnblockJumping();
        UnblockDashing();
        UnblockAttacking();
        UnblockSliding();
    }
    /// <summary>
    /// 사용자 입력을 방지합니다.
    /// </summary>
    void BlockUserInput()
    {
        BlockMoving();
        BlockJumping();
        BlockDashing();
        BlockAttacking();
    }
    /// <summary>
    /// 사용자가 입력할 수 있도록 합니다.
    /// </summary>
    void UnblockUserInput()
    {
        UnblockMoving();
        UnblockJumping();
        UnblockDashing();
        UnblockAttacking();
    }
    /// <summary>
    /// 플레이어가 지상에 착륙할 때의 상태를 설정합니다.
    /// </summary>
    void Land()
    {
        audioSources[5].Play();

        StopDashing();
        StopJumping();
        StopFalling();
        StopSliding();
        UnblockJumping();
        UnblockDashing();
        UnblockAttacking();
        UnblockSliding();
        ClearAirDashCount();
//        _airDashCount = 0;
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
        _rigidbody.velocity = new Vector2(-movingSpeed, _rigidbody.velocity.y);
        _animator.SetBool("Moving", _moving = true);
    }
    /// <summary>
    /// 플레이어를 오른쪽으로 이동합니다.
    /// </summary>
    void MoveRight()
    {
        if (facingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(movingSpeed, _rigidbody.velocity.y);
        _animator.SetBool("Moving", _moving = true);
    }
    /// <summary>
    /// 플레이어의 이동을 중지합니다.
    /// </summary>
    void StopMoving()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _animator.SetBool("Moving", _moving = false);
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
        BlockJumping();
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
        _animator.SetBool("Jumping", _jumping = true);
//        _canAirDash = true;
    }
    /// <summary>
    /// 플레이어의 점프를 중지합니다.
    /// </summary>
    void StopJumping()
    {
        _animator.SetBool("Jumping", _jumping = false);
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
        StopJumping();
        StopSliding();

        BlockJumping();

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _animator.SetBool("Falling", _falling = true);
    }
    /// <summary>
    /// 플레이어의 낙하를 중지합니다.
    /// </summary>
    void StopFalling()
    {
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
        _animator.SetBool("Falling", _falling = false);
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
        BlockAirDashing();
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
        BlockAirDashing();

        // 대쉬 효과 애니메이션을 추가합니다.
        GameObject dashFog = Instantiate(effects[0], dashFogPosition.position, dashFogPosition.rotation) as GameObject;
        if (facingRight == false)
        {
            var newScale = dashFog.transform.localScale;
            newScale.x = facingRight ? newScale.x : -newScale.x;
            dashFog.transform.localScale = newScale;
        }

        movingSpeed = dashSpeed;
        float vx = facingRight ? dashSpeed : -dashSpeed;
        _rigidbody.velocity = new Vector2(vx, _rigidbody.velocity.y);
        audioSources[11].Play();
        _animator.SetBool("Dashing", _dashing = true);
    }
    /// <summary>
    /// 플레이어의 대쉬 상태를 종료하도록 요청합니다.
    /// </summary>
    void EndDash()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 10, _rigidbody.velocity.y);
//        UnblockAirDashing();
    }
    /// <summary>
    /// 플레이어의 대쉬를 중지합니다. (사용자의 입력에 의함)
    /// </summary>
    void StopDashing()
    {
        UnblockMoving();
        UnblockDashing();
        UnblockAirDashing();

        movingSpeed = walkSpeed;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _animator.SetBool("Dashing", _dashing = false);
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
    // 벽 타기
    /// <summary>
    /// 플레이어가 벽을 타도록 합니다.
    /// </summary>
    void Slide()
    {
        _animator.SetBool("Sliding", _sliding = true);
        StopMoving();
        StopJumping();
        StopFalling();
        StopDashing();
        ClearAirDashCount();

        BlockDashing();
        UnblockJumping();
        _rigidbody.velocity = new Vector2(0, -slideSpeed);
    }
    /// <summary>
    /// 플레이어의 벽 타기를 중지합니다.
    /// </summary>
    void StopSliding()
    {
        if (slideFogEffect != null)
        {
            slideFogEffect.GetComponent<EffectScript>().RequestEnd();
            slideFogEffect = null;
        }

        UnblockDashing();
        _animator.SetBool("Sliding", _sliding = false);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
    }
    /// <summary>
    /// 플레이어의 벽 타기 요청을 막습니다.
    /// </summary>
    void BlockSliding()
    {
        _slideBlocked = true;
        _animator.SetBool("SlideBlocked", _slideBlocked);
    }
    /// <summary>
    /// 플레이어가 벽 타기할 수 있도록 합니다.
    /// </summary>
    void UnblockSliding()
    {
        _slideBlocked = false;
        _animator.SetBool("SlideBlocked", _slideBlocked);
    }

    ///////////////////////////////////////////////////////////////////
    // 사다리 타기
    /// <summary>
    /// 플레이어가 사다리를 타도록 합니다.
    /// </summary>
    void RideLadder()
    {
        ClearAirDashCount();

    }

    ///////////////////////////////////////////////////////////////////
    // 조합
    /// <summary>
    /// 플레이어가 벽 점프를 합니다.
    /// </summary>
    void WallJump()
    {
        StopJumping();
        StopFalling();
        StopSliding();

        GameObject wallKick = Instantiate(effects[3], slideFogPosition.position, slideFogPosition.rotation) as GameObject;

        _rigidbody.velocity = new Vector2(facingRight ? - 1.5f * movingSpeed : 1.5f * movingSpeed, jumpSpeed / Mathf.Sqrt(2));
        _animator.SetBool("Jumping", _jumping = true);
        BlockJumping();
    }
    /// <summary>
    /// 플레이어가 대쉬 점프하게 합니다.
    /// </summary>
    void DashJump()
    {
        StopJumping();
        StopFalling();
        StopSliding();

//        BlockMoving();
        BlockDashing();
        BlockJumping();

        movingSpeed = dashSpeed;
        _rigidbody.velocity = new Vector2(facingRight ? movingSpeed : -movingSpeed, jumpSpeed);
        _animator.SetBool("Jumping", _jumping = true);
        _animator.SetBool("Dashing", _dashing = true);

        _dashJumping = true;
    }
    /// <summary>
    /// 플레이어가 벽에서 대쉬 점프하게 합니다.
    /// </summary>
    void WallDashJump()
    {
        StopJumping();
        StopFalling();
        StopSliding();

        GameObject wallKick = Instantiate(effects[3], slideFogPosition.position, slideFogPosition.rotation) as GameObject;

        movingSpeed = dashSpeed;
        _rigidbody.velocity = new Vector2(facingRight ? -1.5f * movingSpeed : 1.5f * movingSpeed, jumpSpeed / Mathf.Sqrt(2));
        _animator.SetBool("Jumping", _jumping = true);

        ++_airDashCount;
        _dashJumping = true;
        BlockJumping();
        BlockDashing();
    }
    /// <summary>
    /// 플레이어가 에어 대쉬하게 합니다.
    /// </summary>
    void AirDash()
    {
        StopJumping();
        StopFalling();
        StopSliding();

        _rigidbody.velocity = new Vector2(facingRight ? dashSpeed : -dashSpeed, 0);
        _animator.SetBool("AirDashing", _airDashing = true);
        audioSources[11].Play();

        BlockMoving();
        ++_airDashCount;
//        _canAirDash = false;
    }
    /// <summary>
    /// 플레이어의 에어 대쉬를 중지합니다.
    /// </summary>
    void StopAirDashing()
    {
        UnblockMoving();
        _animator.SetBool("AirDashing", _airDashing = false);
    }
    /// <summary>
    /// 플레이어의 에어 대쉬 요청을 막습니다.
    /// </summary>
    void BlockAirDashing()
    {
        _airDashBlocked = true;
    }
    /// <summary>
    /// 플레이어가 에어 대쉬할 수 있도록 합니다.
    /// </summary>
    void UnblockAirDashing()
    {
        _airDashBlocked = false;
    }

    #endregion



    #region 요청 메서드를 정의합니다.


    #endregion



    #region 프레임 이벤트 핸들러를 정의합니다.
    public void Jump_beg()
    {
        audioSources[4].Play();
        audioSources[6].Play();
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
    /// 첫 번째 일반 지상 공격이 공격으로 인정되는 때에 발생합니다.
    /// </summary>
    public void Attack_saber1_run()
    {
        // 공격 범위를 활성화합니다.
        attackRange[0].enabled = true;
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
        // 공격 범위를 비활성화합니다.
        attackRange[0].enabled = false;

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
    /// 두 번째 일반 지상 공격이 공격으로 인정되는 때에 발생합니다.
    /// </summary>
    public void Attack_saber2_run()
    {
        // 공격 범위를 활성화합니다.
        attackRange[1].enabled = true;
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
        // 공격 범위를 비활성화합니다.
        attackRange[1].enabled = false;

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
    /// 세 번째 일반 지상 공격이 공격으로 인정되는 때에 발생합니다.
    /// </summary>
    public void Attack_saber3_run()
    {
        // 공격 범위를 활성화합니다.
        attackRange[2].enabled = true;
    }
    /// <summary>
    /// 지상 공격이 종료할 때 발생합니다.
    /// </summary>
    public void AttackEndFromRun_beg()
    {
        // 막은 행동을 가능하게 합니다.
        UnblockAttacking();
        UnblockJumping();
        UnblockDashing();

        // 공격 범위를 비활성화합니다.
        attackRange[2].enabled = false;
    }
    /// <summary>
    /// 지상 공격 모션이 완전히 종료되어 대기 상태로 바뀔 때 발생합니다.
    /// </summary>
    public void AttackEndFromRun_end()
    {
        StopAttacking();

        // 공격 범위를 모두 비활성화합니다.
        attackRange[0].enabled = false;
        attackRange[1].enabled = false;
        attackRange[2].enabled = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 점프 공격
    /// <summary>
    /// 점프 공격 시에 발생합니다.
    /// </summary>
    public void JumpShot_beg()
    {
        _animator.SetBool("AttackRequested", _attackRequested = false);
        BlockAttacking();

        audioSources[3].Play();
    }
    /// <summary>
    /// 점프 공격이 종료할 때 발생합니다.
    /// </summary>
    public void JumpShot_end()
    {
        UnblockAirDashing();
        UnblockAttacking();
        StopAttacking();
    }

    ///////////////////////////////////////////////////////////////////
    // 벽 타기
    /// <summary>
    /// 벽 타기 시에 발생합니다.
    /// </summary>
    public void Slide_beg()
    {
        GameObject slideFog = Instantiate(effects[2], slideFogPosition.position, slideFogPosition.rotation) as GameObject;
        slideFog.transform.SetParent(groundCheck.transform);
        if (facingRight == false)
        {
            var newScale = slideFog.transform.localScale;
            newScale.x = facingRight ? newScale.x : -newScale.x;
            slideFog.transform.localScale = newScale;
        }
        slideFogEffect = slideFog;

        audioSources[7].Play();
    }
    /// <summary>
    /// 벽 점프 시에 발생합니다.
    /// </summary>
    public void WallJump_beg()
    {
        BlockSliding();
        audioSources[7].Play();
        audioSources[8].Play();
    }
    /// <summary>
    /// 벽 점프가 종료할 때 발생합니다.
    /// </summary>
    public void WallJump_end()
    {
        UnblockSliding();
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }

    ///////////////////////////////////////////////////////////////////
    // 대쉬
    /// <summary>
    /// 대쉬 부스트가 시작할 때 발생합니다.
    /// </summary>
    public void DashRun()
    {
        GameObject dashBoost = Instantiate(effects[1], dashBoostPosition.position, dashBoostPosition.rotation) as GameObject;
        dashBoost.transform.SetParent(groundCheck.transform);
        if (facingRight == false)
        {
            var newScale = dashBoost.transform.localScale;
            newScale.x = facingRight ? newScale.x : -newScale.x;
            dashBoost.transform.localScale = newScale;
        }
    }
    /// <summary>
    /// 대쉬가 종료할 때 발생합니다.
    /// </summary>
    public void DashEnd()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 10, _rigidbody.velocity.y);
        audioSources[12].Play();
    }
    /// <summary>
    /// 대쉬가 사용자에 의해 중지될 때 발생합니다.
    /// </summary>
    public void DashEndFromRun_beg()
    {
        StopMoving();
        BlockMoving();
        audioSources[13].Play();
    }
    /// <summary>
    /// 대쉬 점프 모션이 사용자에 의해 완전히 중지되어 대기 상태로 바뀔 때 발생합니다.
    /// </summary>
    public void DashEndFromRun_end()
    {
        UnblockMoving();
        UnblockDashing();
        StopDashing();
    }
    /// <summary>
    /// 에어 대쉬 부스트가 시작할 때 발생합니다.
    /// </summary>
    public void AirDashRun()
    {
        GameObject dashBoost = Instantiate(effects[1], dashBoostPosition.position, dashBoostPosition.rotation) as GameObject;
        dashBoost.transform.SetParent(groundCheck.transform);
        if (facingRight == false)
        {
            var newScale = dashBoost.transform.localScale;
            newScale.x = facingRight ? newScale.x : -newScale.x;
            dashBoost.transform.localScale = newScale;
        }
    }
    /// <summary>
    /// 에어 대쉬가 중지될 때 발생합니다.
    /// </summary>
    public void AirDashEnd()
    {
        StopAirDashing();
        Fall();
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
    void ClearAirDashCount()
    {
        _airDashCount = 0;
    }

    #endregion 보조 메서드 정의
}
