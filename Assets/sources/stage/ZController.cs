using UnityEngine;

/// <summary>
/// 제로에 대한 컨트롤러입니다.
/// </summary>
public class ZController : PlayerController
{
    #region 효과 객체를 보관합니다.
    GameObject dashBoostEffect = null;

    #endregion



    #region 플레이어의 상태 필드를 정의합니다.
    public Collider2D[] attackRange;

    bool _attackBlocked;
    bool _attacking;
    bool _attackRequested;

    bool dangerVoicePlayed = false;

    bool AttackBlocked
    {
        get { return _attackBlocked; }
        set { _attackBlocked = value; }
    }
    bool Attacking
    {
        get { return _attacking; }
        set { _animator.SetBool("Attacking", _attacking = value); }
    }
    bool AttackRequested
    {
        get { return _attackRequested; }
        set { _animator.SetBool("AttackRequested", _attackRequested = value); }
    }

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의합니다.
    protected override void Awake()
    {
        base.Awake(); // Initialize();
    }
    void Start()
    {
        // Initialize();
    }
    protected override void Update()
    {
        if (UpdateController() == false)
        {
            return;
        }

        // 화면 갱신에 따른 변화를 추적합니다.
        if (Dashing) // 대쉬 상태에서 잔상을 만듭니다.
        {
            // 대쉬 잔상을 일정 간격으로 만들기 위한 조건 분기입니다.
            if (DashAfterImageTime < DashAfterImageInterval)
            {
                DashAfterImageTime += Time.deltaTime;
            }
            // 실제로 잔상을 생성합니다.
            else
            {
                // GameObject dashAfterImage = I_nstantiate(effects[4], transform.position, transform.rotation) as GameObject;
                GameObject dashAfterImage = CloneObject(effects[4], transform);
                Vector3 daiScale = dashAfterImage.transform.localScale;
                if (FacingRight == false)
                    daiScale.x *= -1;
                dashAfterImage.transform.localScale = daiScale;
                dashAfterImage.SetActive(false);
                var daiRenderer = dashAfterImage.GetComponent<SpriteRenderer>();
                daiRenderer.sprite = _renderer.sprite;
                dashAfterImage.SetActive(true);
                DashAfterImageTime = 0;
            }
        }

        // 새로운 사용자 입력을 확인합니다.
        // 점프 키가 눌린 경우
        if (IsKeyDown("Jump")) // IsKeyDown(GameKey.Jump))
        {
            if (JumpBlocked)
            {
                // p_rint("TEST");
            }
            else if (Sliding)
            {
                // if (IsKeyPressed(GameKey.Dash))
                if (IsKeyPressed("Dash"))
                {
                    WallDashJump();
                }
                else
                {
                    WallJump();
                }
            }
            else if (Dashing)
            {
                DashJump();
            }
            else if (Landed && IsKeyPressed("Dash")) // else if (Landed && IsKeyPressed(GameKey.Dash))
            {
                DashJump();
            }
            else
            {
                Jump();
            }
        }
        // 대쉬 키가 눌린 경우
        else if (IsKeyDown("Dash")) // else if (IsKeyDown(GameKey.Dash))
        {
            if (Sliding)
            {

            }
            else if (Landed == false)
            {
                if (AirDashBlocked)
                {

                }
                else
                {
                    AirDash();
                }
            }
            else if (DashBlocked)
            {

            }
            else
            {
                Dash();
            }
        }
        // 캐릭터 변경 키가 눌린 경우
        else if (IsKeyDown("ChangeCharacter")) // else if (Input.GetKeyDown(KeyCode.F))
        {
            // sceneManager.ChangePlayer(sceneManager.PlayerX);
            stageManager.ChangePlayer(stageManager.PlayerX);
        }
    }
    void FixedUpdate()
    {
        UpdateState();

        if (FixedUpdateController() == false)
        {
            return;
        }

        /*
        // 소환 중이라면
        if (Spawning)
        {
            // 준비 중이라면
            if (Readying)
            {
                // 준비가 끝나서 대기 상태로 전환되었다면
                if (IsAnimationPlaying("Idle"))
                    // 준비를 완전히 종료합니다.
                    EndReady();
            }
            // 준비 중이 아닌데 지상에 착륙했다면
            else if (Landed)
            {
                // 준비 상태로 전환합니다.
                Ready();
            }
            return;
        }
        */

        // 기존 사용자 입력을 확인합니다.
        // 점프 중이라면
        if (Jumping)
        {
            if (Pushing)
            {
                if (SlideBlocked)
                {

                }
                else
                {
                    Slide();
                }
            }
            else if (IsKeyPressed("Jump") == false
                || _rigidbody.velocity.y <= 0)
            {
                Fall();
            }
            else
            {
                _rigidbody.velocity = new Vector2
                    (_rigidbody.velocity.x, _rigidbody.velocity.y - jumpDecSize);
            }
        }
        // 떨어지고 있다면
        else if (Falling)
        {
            if (Landed)
            {
                // StopFalling();
                Land();
            }
            else if (Pushing)
            {
                if (SlideBlocked)
                {

                }
                else
                {
                    Slide();
                }
            }
            else
            {
                float vy = _rigidbody.velocity.y - jumpDecSize;
                _rigidbody.velocity = new Vector2
                    (_rigidbody.velocity.x, vy > -16 ? vy : -16);
            }
        }
        // 대쉬 중이라면
        else if (Dashing)
        {
            if (AirDashing)
            {
                if (IsKeyPressed("Dash") == false)
                {
                    StopAirDashing();
                    Fall();
                }
                else if (Landed)
                {
                    StopAirDashing();
                    Fall();
                }
                else if (Pushing)
                {
                    StopAirDashing();
                    Slide();
                }
            }
            else if (Landed == false)
            {
                StopDashing();
                Fall();
            }
            else if (IsKeyPressed("Dash") == false)
            {
                StopDashing();
            }
        }
        // 벽을 타고 있다면
        else if (Sliding)
        {
            if (Pushing == false)
            {
                StopSliding();
                Fall();
            }
            else if (Landed)
            {
                StopSliding();
                Fall();
            }
        }
        // 벽을 밀고 있다면
        else if (Pushing)
        {
            if (Landed)
            {

            }
            else
            {
                Slide();
            }
        }
        // 그 외의 경우
        else
        {
            if (Landed == false)
            {
                Fall();
            }

            UnblockSliding();
        }

        // 방향 키 입력에 대해 처리합니다.
        // 대쉬 중이라면
        if (Dashing)
        {
            if (AirDashing)
            {

            }
            // 대쉬 중에 공중에 뜬 경우
            else if (Landed == false)
            {
                if (SlideBlocked)
                {

                }
                else if (IsLeftKeyPressed()) // else if (IsKeyPressed(GameKey.Left))
                {
                    MoveLeft();
                }
                else if (IsRightKeyPressed()) // else if (IsKeyPressed(GameKey.Right)
                {
                    MoveRight();
                }
                else
                {
                    StopMoving();
                }
            }
            else
            {

            }
        }
        // 움직임이 막힌 상태라면
        else if (MoveBlocked)
        {

        }
        // 벽 점프 중이라면
        else if (SlideBlocked)
        {

        }
        // 그 외의 경우
        else
        {
            if (IsLeftKeyPressed())
            {
                if (FacingRight == false && Pushing)
                {
                    StopMoving();
                }
                else
                {
                    if (Sliding)
                    {
                        StopSliding();
                    }
                    MoveLeft();
                }
            }
            else if (IsRightKeyPressed())
            {
                if (FacingRight && Pushing)
                {
                    StopMoving();
                }
                else
                {
                    if (Sliding)
                    {
                        StopSliding();
                    }
                    MoveRight();
                }
            }
            else
            {
                StopMoving();
            }
        }

        // 공격 키가 눌린 경우를 처리합니다.
        if (IsKeyDown("Attack"))
        {
            if (AttackBlocked)
            {

            }
            else if (AirDashing)
            {
                StopAirDashing();
                JumpAttack();
            }
            else if (Jumping)
            {
                JumpAttack();
            }
            else if (Falling)
            {
                JumpAttack();
            }
            else
            {
                Attack();
            }
        }
    }

    #endregion



    #region 제로에 대해 새롭게 정의된 행동 메서드의 목록입니다.
    ///////////////////////////////////////////////////////////////////
    // 공격
    /// <summary>
    /// 플레이어가 공격하게 합니다.
    /// </summary>
    void Attack()
    {
        StopMoving();
        BlockMoving();

        Attacking = true;
        AttackRequested = true;
    }
    /// <summary>
    /// 플레이어의 공격을 중지합니다.
    /// </summary>
    void StopAttacking()
    {
        Attacking = false;
        AttackRequested = false;
        UnblockMoving();
    }
    /// <summary>
    /// 플레이어의 공격 요청을 막습니다.
    /// </summary>
    void BlockAttacking()
    {
        AttackBlocked = true;
    }
    /// <summary>
    /// 플레이어가 공격할 수 있도록 합니다.
    /// </summary>
    void UnblockAttacking()
    {
        AttackBlocked = false;
    }
    /// <summary>
    /// 플레이어가 점프 공격하게 합니다.
    /// </summary>
    void JumpAttack()
    {
        Attacking = true;
        AttackRequested = true;
        BlockAirDashing();
    }
    #endregion



    #region PlayerController 행동 메서드를 재정의 합니다.
    ///////////////////////////////////////////////////////////////////
    // 기본
    /// <summary>
    /// 플레이어를 소환합니다.
    /// </summary>
    protected override void Spawn()
    {
        base.Spawn();
        SoundEffects[0].Play();
    }
    /// <summary>
    /// 플레이어가 지상에 착륙할 때의 상태를 설정합니다.
    /// </summary>
    protected override void Land()
    {
        base.Land();
        SoundEffects[2].Play();
    }

    ///////////////////////////////////////////////////////////////////
    // 점프 및 낙하
    /// <summary>
    /// 플레이어를 점프하게 합니다.
    /// </summary>
    protected override void Jump()
    {
        base.Jump();
        Voices[0].Play();
        SoundEffects[1].Play();
    }
    /// <summary>
    /// 플레이어의 낙하를 중지합니다.
    /// </summary>
    protected override void StopFalling()
    {
        base.StopFalling();
        if (_attacking)
        {
            var curState = _animator.GetCurrentAnimatorStateInfo(0);
            if (curState.IsName("JumpShot"))
            {
                var nTime = curState.normalizedTime;
                var fTime = nTime - Mathf.Floor(nTime);
                _animator.Play("JumpShotGround", 0, fTime);

                StopMoving();
                BlockMoving();
//                audioSources[5].Play();
            }
        }
    }

    ///////////////////////////////////////////////////////////////////
    // 대쉬
    /// <summary>
    /// 플레이어가 대쉬하게 합니다.
    /// </summary>
    protected override void Dash()
    {
        base.Dash();

        // 대쉬 효과 애니메이션을 추가합니다.
        // GameObject dashFog = I_nstantiate(effects[0], dashFogPosition.position, dashFogPosition.rotation) as GameObject;
        GameObject dashFog = CloneObject(effects[0], dashFogPosition);
        if (FacingRight == false)
        {
            var newScale = dashFog.transform.localScale;
            newScale.x = FacingRight ? newScale.x : -newScale.x;
            dashFog.transform.localScale = newScale;
        }
        SoundEffects[3].Play();
    }
    /// <summary>
    /// 플레이어의 대쉬를 중지합니다. (사용자의 입력에 의함)
    /// </summary>
    protected override void StopDashing()
    {
        base.StopDashing();
        if (dashBoostEffect != null)
        {
            dashBoostEffect.GetComponent<EffectScript>().RequestEnd();
            dashBoostEffect = null;
        }
    }
    /// <summary>
    /// 플레이어가 대쉬 점프하게 합니다.
    /// </summary>
    protected override void DashJump()
    {
        base.DashJump();

        SoundEffects[3].Stop();
        SoundEffects[1].Play();
        if (dashBoostEffect != null)
        {
            dashBoostEffect.GetComponent<EffectScript>().RequestEnd();
            dashBoostEffect = null;
        }
    }
    /// <summary>
    /// 플레이어가 에어 대쉬하게 합니다.
    /// </summary>
    protected override void AirDash()
    {
        base.AirDash();
        SoundEffects[3].Play();
    }
    /// <summary>
    /// 플레이어의 에어 대쉬를 중지합니다.
    /// </summary>
    protected override void StopAirDashing()
    {
        base.StopAirDashing();
        if (dashBoostEffect != null)
        {
            dashBoostEffect.GetComponent<EffectScript>().RequestEnd();
            dashBoostEffect = null;
        }
    }

    #endregion



    #region PlayerController 상태 메서드를 재정의 합니다.
    /// <summary>
    /// 플레이어가 사망합니다.
    /// </summary>
    protected override void Dead()
    {
        base.Dead();

        stageManager.deadEffect.RequestRun(stageManager.player);
        Voices[8].Play();
        SoundEffects[9].Play();
    }
    /// <summary>
    /// 플레이어가 대미지를 입습니다.
    /// </summary>
    /// <param name="damage">플레이어가 입을 대미지입니다.</param>
    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        if (IsAlive())
        {
            Voices[5].Play();
            SoundEffects[8].Play();
        }
        Invoke("EndHurt", GetCurrentAnimationLength());
    }
    protected override void EndHurt()
    {
        base.EndHurt();
        if (Danger && dangerVoicePlayed == false)
        {
            Voices[7].Play();
            dangerVoicePlayed = true;
        }
        else
        {
            dangerVoicePlayed = false;
        }
    }

    #endregion



    #region 프레임 이벤트 핸들러를 정의합니다.
    ///////////////////////////////////////////////////////////////////
    // 지상 공격
    /// <summary>
    /// 첫 번째 일반 지상 공격 시에 발생합니다.
    /// </summary>
    public void FE_Attack1()
        {
            // 받은 요청은 삭제합니다.
            AttackRequested = false;

            // 공격 시 불가능한 행동을 막습니다.
            BlockAttacking();
            BlockJumping();
            BlockDashing();

            // 효과음을 재생합니다.
            Voices[1].Play();
            SoundEffects[7].Play();
        }
    /// <summary>
    /// 첫 번째 일반 지상 공격이 공격으로 인정되는 때에 발생합니다.
    /// </summary>
    public void FE_Attack1_1()
    {
        // 공격 범위를 활성화합니다.
        attackRange[0].enabled = true;
    }
    /// <summary>
    /// 첫 번째 일반 지상 공격이 종료할 때 발생합니다.
    /// </summary>
    public void FE_Attack1_end()
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
        AttackRequested = false;

        // 공격 시 불가능한 행동을 막습니다.
        BlockAttacking(); // 공격
        BlockJumping(); // 점프
        BlockDashing(); // 대쉬

        // 효과음을 재생합니다.
        Voices[2].Play();
        SoundEffects[7].Play();
    }
    /// <summary>
    /// 두 번째 일반 지상 공격이 공격으로 인정되는 때에 발생합니다.
    /// </summary>
    public void Attack_saber2_run()
    {
        // 공격 범위를 활성화합니다.
        attackRange[1].enabled = true;
    }
    public void Attack_saber2_run2()
    {
        // 이전 공격 범위를 비활성화합니다.
        attackRange[1].enabled = false;
        // 공격 범위를 활성화합니다.
        attackRange[2].enabled = true;
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
        attackRange[2].enabled = false;

        // 받은 요청은 삭제합니다.
        _animator.SetBool("AttackRequested", _attackRequested = false);

        // 공격 시 불가능한 행동을 막습니다.
        BlockAttacking(); // 공격
        BlockJumping(); // 점프
        BlockDashing(); // 대쉬

        // 효과음을 재생합니다.
        Voices[3].Play();
        SoundEffects[7].Play();
    }
    /// <summary>
    /// 세 번째 일반 지상 공격이 공격으로 인정되는 때에 발생합니다.
    /// </summary>
    public void Attack_saber3_run()
    {
        // 공격 범위를 활성화합니다.
        attackRange[3].enabled = true;
    }
    public void Attack_saber3_run2()
    {
        // 이전 공격 범위를 비활성화합니다.
        //        attackRange[3].enabled = false;
        // 공격 범위를 활성화합니다.
        attackRange[4].enabled = true;
    }
    public void Attack_saber3_run3()
    {
        // 이전 공격 범위를 비활성화합니다.
        //        attackRange[4].enabled = false;
        // 공격 범위를 활성화합니다.
        attackRange[5].enabled = true;
    }
    public void Attack_saber3_run4()
    {
        // 이전 공격 범위를 비활성화합니다.
        //        attackRange[5].enabled = false;
        // 공격 범위를 활성화합니다.
        attackRange[6].enabled = true;
    }
    public void Attack_saber3_run5()
    {
        // 이전 공격 범위를 비활성화합니다.
        //        attackRange[6].enabled = false;
        // 공격 범위를 활성화합니다.
        attackRange[7].enabled = true;
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
        attackRange[3].enabled = false;
        attackRange[4].enabled = false;
        attackRange[5].enabled = false;
        attackRange[6].enabled = false;
        attackRange[7].enabled = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 대쉬
    /// <summary>
    /// 대쉬 준비 애니메이션이 시작할 때 발생합니다.
    /// </summary>
    void FE_DashBegBeg()
    {

    }
    /// <summary>
    /// 대쉬 부스트 애니메이션이 시작할 때 발생합니다.
    /// </summary>
    void FE_DashRunBeg()
    {
        // GameObject dashBoost = I_nstantiate(effects[1], dashBoostPosition.position, dashBoostPosition.rotation) as GameObject;
        GameObject dashBoost = CloneObject(effects[1], dashBoostPosition);
        dashBoost.transform.SetParent(groundCheck.transform);
        if (FacingRight == false)
        {
            var newScale = dashBoost.transform.localScale;
            newScale.x = FacingRight ? newScale.x : -newScale.x;
            dashBoost.transform.localScale = newScale;
        }
        dashBoostEffect = dashBoost;
    }
    /// <summary>
    /// 플레이어의 대쉬 상태를 종료하도록 요청합니다.
    /// </summary>
    void FE_DashRunEnd()
    {
        StopDashing();
        StopAirDashing();
    }
    /// <summary>
    /// 대쉬가 사용자에 의해 중지될 때 발생합니다.
    /// </summary>
    void FE_DashEndBeg()
    {
        StopMoving();
        SoundEffects[3].Stop();
        SoundEffects[4].Play();
    }
    /// <summary>
    /// 대쉬 점프 모션이 사용자에 의해 완전히 중지되어 대기 상태로 바뀔 때 발생합니다.
    /// </summary>
    void FE_DashEndEnd()
    {
    }

    ///////////////////////////////////////////////////////////////////
    // 벽 타기
    /// <summary>
    /// 벽 타기 시에 발생합니다.
    /// </summary>
    void FE_SlideBeg()
    {
        SoundEffects[6].Play();
    }
    /// <summary>
    /// 벽 점프 시에 발생합니다.
    /// </summary>
    void FE_WallJumpBeg()
    {
        SoundEffects[5].Play();
    }
    /// <summary>
    /// 벽 점프가 종료할 때 발생합니다.
    /// </summary>
    void FE_WallJumpEnd()
    {
        UnblockSliding();
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }

    ///////////////////////////////////////////////////////////////////
    // 점프 공격
    /// <summary>
    /// 점프 공격 시에 발생합니다.
    /// </summary>
    void FE_JumpShotBeg()
    {
        _animator.SetBool("AttackRequested", _attackRequested = false);
        BlockAttacking();
        SoundEffects[7].Play();
    }
    /// <summary>
    /// 점프 공격이 종료할 때 발생합니다.
    /// </summary>
    void FE_JumpShotEnd()
    {
        UnblockAirDashing();
        UnblockAttacking();
        StopAttacking();
    }

    #endregion
}