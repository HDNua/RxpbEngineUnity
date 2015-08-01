﻿using UnityEngine;
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

    public float walkSpeed = 8;
    public float jumpSpeed = 16;
    public float jumpDecSize = 0.8f;
    public float slideSpeed = 8;
    public float spawnSpeed = 16;
    public float dashSpeed = 16;

    #endregion Unity 공용 필드



    #region 주인공의 상태 필드를 정의합니다.
    bool facingRight = true;

    bool _landed = true;

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
    bool _slideBlocked = false;
    bool _slideRequested = false;
    bool _wallJumping = false;

    bool _riding = false;

    bool _spawning = true;

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

        // 준비 상태가 완료될 때까지 사용자 입력을 막습니다.
        BlockUserInput();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_dashing)
            {
                DashJump();
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
            else if (_jumpBlocked)
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
            else if (_attacking)
            {
                StopAttacking();
                Dash();
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

        // _landed = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, whatIsGround);
        // print(string.Format("{0} / {1}", landedMin, landedMax));
        // _landed = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
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
                print("TEST");
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
                Fall();
            }
            else if (_landed)
            {
                StopSliding();
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
            else
            {
                Slide();
            }
        }
        // 벽 점프 상태라면
        else if (_slideBlocked)
        {
            print("SLIDE BLOCKED");
        }
        /*
        // 대쉬 상태라면
        else if (_dashing)
        {

        }
        */
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

        /*
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Round(newPos.x);
        transform.position = newPos;
        */
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
        UnblockJumping();
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
        _animator.SetBool("Moving", _moving = true);
    }
    /// <summary>
    /// 플레이어를 오른쪽으로 이동합니다.
    /// </summary>
    void MoveRight()
    {
        if (facingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(walkSpeed, _rigidbody.velocity.y);
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

        float vx = facingRight ? dashSpeed : -dashSpeed;
        _rigidbody.velocity = new Vector2(vx, _rigidbody.velocity.y);
        _animator.SetBool("Dashing", _dashing = true);
    }
    /// <summary>
    /// 플레이어의 대쉬 상태를 종료하도록 요청합니다.
    /// </summary>
    void EndDash()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 10, _rigidbody.velocity.y);
    }
    /// <summary>
    /// 플레이어의 대쉬를 중지합니다. (사용자의 입력에 의함)
    /// </summary>
    void StopDashing()
    {
        UnblockMoving();
        UnblockDashing();

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
    // 기타
    /// <summary>
    /// 플레이어가 벽을 타도록 합니다.
    /// </summary>
    void Slide()
    {
        _animator.SetBool("Sliding", _sliding = true);
        StopMoving();
        StopJumping();
        StopFalling();
        UnblockJumping();
        _rigidbody.velocity = new Vector2(0, -slideSpeed);
    }
    /// <summary>
    /// 플레이어의 벽 타기를 중지합니다.
    /// </summary>
    void StopSliding()
    {
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
    /// <summary>
    /// 플레이어가 벽 점프를 합니다.
    /// </summary>
    void WallJump()
    {
        StopJumping();
        StopFalling();
        StopSliding();
        _animator.SetBool("Jumping", _jumping = true);
        _rigidbody.velocity = new Vector2(facingRight ? -0.6f : 0.6f, 1) * (jumpSpeed / Mathf.Sqrt(2));

        BlockJumping();
//        BlockSliding();
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
        // 막은 행동을 가능하게 합니다.
        UnblockAttacking();
        UnblockJumping();
        UnblockDashing();
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
        _animator.SetBool("AttackRequested", _attackRequested = false);
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
    public void Slide_beg()
    {
        audioSources[7].Play();
//        _animator.SetBool("Sliding", _sliding = true);
//        _animator.SetBool("SlideRequested", _slideRequested = false);
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
    /// 대쉬가 종료할 때 발생합니다.
    /// </summary>
    public void DashEnd()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 10, _rigidbody.velocity.y);
    }
    /// <summary>
    /// 대쉬가 사용자에 의해 중지될 때 발생합니다.
    /// </summary>
    public void DashEndFromRun_beg()
    {
        StopMoving();
        BlockMoving();
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
