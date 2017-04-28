using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 제로에 대한 컨트롤러입니다.
/// </summary>
public class ZController : PlayerController
{
    #region 효과 객체를 보관합니다.
    /// <summary>
    /// 
    /// </summary>
    GameObject _dashBoostEffect = null;

    #endregion


    


    #region 플레이어의 상태 필드를 정의합니다.
    /// <summary>
    /// 제로의 공격 범위입니다.
    /// </summary>
    public GameObject[] _attackRange;

    /// <summary>
    /// 
    /// </summary>
    bool _attackBlocked;
    /// <summary>
    /// 
    /// </summary>
    bool _attacking;
    /// <summary>
    /// 
    /// </summary>
    bool _attackRequested;

    /// <summary>
    /// 
    /// </summary>
    bool dangerVoicePlayed = false;

    /// <summary>
    /// 
    /// </summary>
    bool AttackBlocked
    {
        get { return _attackBlocked; }
        set { _attackBlocked = value; }
    }
    /// <summary>
    /// 
    /// </summary>
    bool Attacking
    {
        get { return _attacking; }
        set { _Animator.SetBool("Attacking", _attacking = value); }
    }
    /// <summary>
    /// 
    /// </summary>
    bool AttackRequested
    {
        get { return _attackRequested; }
        set { _Animator.SetBool("AttackRequested", _attackRequested = value); }
    }

    /// <summary>
    /// 
    /// </summary>
    int AttackCount
    {
        get { return _attackCount; }
        set { _Animator.SetInteger("AttackCount", _attackCount = value); }
    }

    /// <summary>
    /// 
    /// </summary>
    public int _attackCount = 0;

    #endregion





    #region 추상 프로퍼티를 구현합니다.
    /// <summary>
    /// 
    /// </summary>
    protected override AudioSource VoiceDamaged { get { return Voices[5]; } }
    /// <summary>
    /// 
    /// </summary>
    protected override AudioSource VoiceBigDamaged { get { return Voices[6]; } }
    /// <summary>
    /// 
    /// </summary>
    protected override AudioSource VoiceDanger { get { return Voices[7]; } }

    /// <summary>
    /// 
    /// </summary>
    protected override AudioSource SoundHit { get { return SoundEffects[8]; } }



    /// <summary>
    /// 
    /// </summary>
    protected AudioSource VoiceAttack1 { get { return Voices[1]; } }
    /// <summary>
    /// 
    /// </summary>
    protected AudioSource VoiceAttack2 { get { return Voices[2]; } }
    /// <summary>
    /// 
    /// </summary>
    protected AudioSource VoiceAttack3 { get { return Voices[3]; } }

    /// <summary>
    /// 
    /// </summary>
    protected AudioSource SoundSaber { get { return SoundEffects[7]; } }

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); // Initialize();

        // 
        _DefaultPalette = ZColorPalette.DefaultPalette;
    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // Initialize();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if (UpdateController() == false)
        {
            return;
        }

        // 화면 갱신에 따른 변화를 추적합니다.
        if (Dashing) // 대쉬 상태에서 잔상을 만듭니다.
        {
            // 대쉬 잔상을 일정 간격으로 만들기 위한 조건 분기입니다.
            if (DashAfterImageTime < DASH_AFTERIMAGE_LIFETIME)
            {
                DashAfterImageTime += Time.deltaTime;
            }
            // 실제로 잔상을 생성합니다.
            else
            {
                GameObject dashAfterImage = CloneObject(effects[4], transform);
                Vector3 daiScale = dashAfterImage.transform.localScale;
                if (FacingRight == false)
                    daiScale.x *= -1;
                dashAfterImage.transform.localScale = daiScale;
                dashAfterImage.SetActive(false);

                // 
                var daiRenderer = dashAfterImage.GetComponent<SpriteRenderer>();
                daiRenderer.sprite = _Renderer.sprite;
                dashAfterImage.SetActive(true);
                DashAfterImageTime = 0;
                
                // 
                UpdateEffectColor(dashAfterImage, ZColorPalette.DefaultPalette, ZColorPalette.DashEffectColorPalette);
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        // 새로운 사용자 입력을 확인합니다.
        // 점프 키가 눌린 경우
        if (IsKeyDown("Jump"))
        {
            if (JumpBlocked)
            {
                
            }
            else if (Sliding)
            {
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
            else if (Landed && IsKeyPressed("Dash"))
            {
                DashJump();
            }
            else
            {
                Jump();
            }
        }
        // 대쉬 키가 눌린 경우
        else if (IsKeyDown("Dash"))
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
        /*
        // 캐릭터 변경 키가 눌린 경우
        else if (IsKeyDown("ChangeCharacter"))
        {
            stageManager.ChangePlayer(stageManager.PlayerX);
        }
        */
        // 공격 키가 눌린 경우
        else if (IsKeyDown("Attack"))
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
            else if (Crouching)
            {
                CrouchAttack();
            }
            else if (Sliding)
            {
                SlideAttack();
            }
            else
            {
                Attack();
            }
        }
    }
    /// <summary>
    /// FixedTimestep에 설정된 값에 따라 일정한 간격으로 업데이트 합니다.
    /// 물리 효과가 적용된 오브젝트를 조정할 때 사용됩니다.
    /// (Update는 불규칙한 호출이기 때문에 물리엔진 충돌검사가 제대로 되지 않을 수 있습니다.)
    /// </summary>
    void FixedUpdate()
    {
        /// UpdateState();

        if (FixedUpdateController() == false)
        {
            return;
        }

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
                || _Rigidbody.velocity.y <= 0)
            {
                Fall();
            }
            else
            {
                _Rigidbody.velocity = new Vector2
                    (_Rigidbody.velocity.x, _Rigidbody.velocity.y - _jumpDecSize);
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
                float vy = _Rigidbody.velocity.y - _jumpDecSize;
                _Rigidbody.velocity = new Vector2
                    (_Rigidbody.velocity.x, vy > -16 ? vy : -16);
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
                StopDashing(false);
                Fall();
            }
            else if (IsKeyPressed("Dash") == false)
            {
                StopDashing(true);
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
                else if (IsLeftKeyPressed())
                {
                    MoveLeft();
                }
                else if (IsRightKeyPressed())
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
        // 앉은 상태라면
        else if (Crouching)
        {
            if (IsDownKeyPressed() == false)
            {
                StopCrouching();
            }
            else if (IsLeftKeyPressed() && FacingRight)
            {
                Flip();
            }
            else if (IsRightKeyPressed() && FacingRight == false)
            {
                Flip();
            }
            else
            {
                StopMoving();
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
            if (IsDownKeyPressed() && Landed)
            {
                Crouch();
                /// StopMoving();
            }
            else if (MoveRequested)
            {

            }
            else if (IsLeftKeyPressed())
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


        // 

    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    protected override void LateUpdate()
    {
        base.LateUpdate();
        UpdateState();

        // 제로의 색상을 업데이트합니다.
        UpdateColor();
    }

    #endregion





    #region PlayerController 행동 메서드를 위한 코루틴을 정의합니다.
    /// <summary>
    /// 대쉬 코루틴 필드입니다.
    /// </summary>
    Coroutine _dashCoroutine = null;
    /// <summary>
    /// 대쉬 코루틴입니다.
    /// </summary>
    /// <returns>행동 단위가 끝날 때마다 null을 반환합니다.</returns>
    IEnumerator CoroutineDash()
    {
        // DashBeg
        {
            yield return new WaitForSeconds(0.1f);
        }

        // DashRun
        if (DashJumping == false)
        {
            GameObject dashBoost = CloneObject(effects[1], dashBoostPosition);
            dashBoost.transform.SetParent(groundCheck.transform);
            if (FacingRight == false)
            {
                var newScale = dashBoost.transform.localScale;
                newScale.x = FacingRight ? newScale.x : -newScale.x;
                dashBoost.transform.localScale = newScale;
            }
            _dashBoostEffect = dashBoost;

            yield return new WaitForSeconds(0.3f);
        }

        // DashEnd (사용자 입력 중지가 아닌 기본 대쉬 중지 행동입니다.)
        if (DashJumping == false)
        {
            StartDashEnd();

            /*
            StopDashing(false);
            StopAirDashing();
            StopMoving();
            SoundEffects[3].Stop();
            SoundEffects[4].Play();
            */
        }

        // 코루틴을 중지합니다.
        yield break;
    }
    /// <summary>
    /// 대쉬 종료를 시작합니다.
    /// </summary>
    void StartDashEnd()
    {
        StopDashing(false);
        _dashCoroutine = StartCoroutine(CoroutineDashEnd());
    }
    /// <summary>
    /// 대쉬 종료 코루틴입니다.
    /// </summary>
    IEnumerator CoroutineDashEnd()
    {
        StopAirDashing();
        StopMoving();
        SoundEffects[3].Stop();
        SoundEffects[4].Play();
        BlockMoving();

        yield return new WaitForSeconds(DASH_END_TIME);
        UnblockMoving();

        yield break;
    }

    /// <summary>
    /// 에어 대쉬 코루틴 필드입니다.
    /// </summary>
    Coroutine _airDashCoroutine = null;
    /// <summary>
    /// 에어 대쉬 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineAirDash()
    {
        // AirDashBeg == AirDashRun
        {
            GameObject dashBoost = CloneObject(effects[1], dashBoostPosition);
            dashBoost.transform.SetParent(groundCheck.transform);
            if (FacingRight == false)
            {
                var newScale = dashBoost.transform.localScale;
                newScale.x = FacingRight ? newScale.x : -newScale.x;
                dashBoost.transform.localScale = newScale;
            }
            _dashBoostEffect = dashBoost;

            yield return new WaitForSeconds(0.3f);
        }

        // AirDashEnd
        {
            StopAirDashing();
        }

        yield break;
    }

    /// <summary>
    /// 벽 타기 코루틴 필드입니다.
    /// </summary>
    Coroutine _slideCoroutine = null;
    /// <summary>
    /// 벽 타기 코루틴입니다.
    /// </summary>
    /// <returns>행동 단위가 끝날 때마다 null을 반환합니다.</returns>
    IEnumerator CoroutineSlide()
    {
        // SlideBeg
        {
            SoundEffects[6].Play();
        }

        // 코루틴을 중지합니다.
        yield break;
    }

    /// <summary>
    /// 벽 점프 코루틴 필드입니다.
    /// </summary>
    Coroutine _wallJumpCoroutine = null;
    /// <summary>
    /// 벽 점프 코루틴입니다.
    /// </summary>
    /// <returns>행동 단위가 끝날 때마다 null을 반환합니다.</returns>
    IEnumerator CoroutineWallJump()
    {
        // WallJumpBeg
        {
            SoundEffects[5].Play();
        }

        // WallJumpEnd
        {
            // UnblockSliding();
            // _Velocity = new Vector2(0, _Velocity.y);
        }

        // 코루틴을 중지합니다.
        yield break;
    }

    public float _time = 0f;
    public const float FRAME_INTERVAL_36 = 0.027777f;

    public float SLIDE_ATTACK_INTERVAL_2 = 2 * FRAME_INTERVAL_36;
    public float SLIDE_ATTACK_INTERVAL_1 = 1 * FRAME_INTERVAL_36;

    public float CROUCH_ATTACK_TIME_1 = 1 * FRAME_INTERVAL_36;
    public float JUMP_ATTACK_TIME_2 = 2 * FRAME_INTERVAL_36;
    public float JUMP_ATTACK_TIME_1 = 1 * FRAME_INTERVAL_36;

    public float _ATTACK1_RUN_INDEX = 1 * FRAME_INTERVAL_36; // 0.03
    public float _ATTACK1_END_INDEX = 2 * FRAME_INTERVAL_36; // 0.07

    public float _ATTACK2_RUN_INDEX1 = 1 * FRAME_INTERVAL_36; // 0.01
    public float _ATTACK2_RUN_INDEX2 = 2 * FRAME_INTERVAL_36; // 0.03
    public float _ATTACK2_END_INDEX = 3 * FRAME_INTERVAL_36; // 0.05

    public float _ATTACK3_RUN_INDEX1 = 1 * FRAME_INTERVAL_36; // 0.03
    public float _ATTACK3_RUN_INDEX2 = 2 * FRAME_INTERVAL_36; // 0.04
    public float _ATTACK3_RUN_INDEX3 = 3 * FRAME_INTERVAL_36; // 0.05
    public float _ATTACK3_RUN_INDEX4 = 4 * FRAME_INTERVAL_36; // 0.06
    public float _ATTACK3_RUN_INDEX5 = 5 * FRAME_INTERVAL_36; // 0.07
    public float _ATTACK3_END_INDEX = 6 * FRAME_INTERVAL_36; // 0.08

    /*
    public float _ATTACK1_RUN_INDEX = 1 * FRAME_INTERVAL_36; // 0.03
    public float _ATTACK1_END_INDEX = 2 * FRAME_INTERVAL_36; // 0.07

    public float _ATTACK2_RUN_INDEX1 = 1 * FRAME_INTERVAL_36; // 0.01
    public float _ATTACK2_RUN_INDEX2 = 2 * FRAME_INTERVAL_36; // 0.03
    public float _ATTACK2_END_INDEX = 3 * FRAME_INTERVAL_36; // 0.05

    public float _ATTACK3_RUN_INDEX1 = 1 * FRAME_INTERVAL_36; // 0.03
    public float _ATTACK3_RUN_INDEX2 = 2 * FRAME_INTERVAL_36; // 0.04
    public float _ATTACK3_RUN_INDEX3 = 3 * FRAME_INTERVAL_36; // 0.05
    public float _ATTACK3_RUN_INDEX4 = 4 * FRAME_INTERVAL_36; // 0.06
    public float _ATTACK3_RUN_INDEX5 = 5 * FRAME_INTERVAL_36; // 0.07
    public float _ATTACK3_END_INDEX = 6 * FRAME_INTERVAL_36; // 0.08
    */

    /// <summary>
    /// 공격 코루틴에 대한 포인터입니다.
    /// </summary>
    Coroutine _coroutineAttack = null;
    /// <summary>
    /// 
    /// </summary>
    IEnumerator CoroutineSlideAttack()
    {
        // 
        SoundSaber.Play();

        // 
        yield return new WaitForEndOfFrame();
        _time = 0;
        while (IsAnimationPlaying("SlideAttack") == false)
            yield return false;

        BlockAttacking();
        AttackRequested = false;

        float length = GetCurrentAnimationLength();

        // 
        while (_time < length)
        {
            // 
            if (_time > SLIDE_ATTACK_INTERVAL_2)
            {
                ActivateAttackRange(12);
                DeactivateAttackRange(11);
            }
            else if (_time > SLIDE_ATTACK_INTERVAL_1)
            {
                ActivateAttackRange(11);
            }

            // 
            _time += Time.deltaTime;
            yield return false;
        }

        // 
        DeactivateAttackRange(11);
        DeactivateAttackRange(12);
        StopAttacking();
        UnblockAttacking();
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    IEnumerator CoroutineCrouchAttack()
    {
        // 
        BlockMoving();
        BlockAttacking();

        // 
        SoundSaber.Play();

        // 
        yield return new WaitForEndOfFrame();
        _time = 0;
        while (IsAnimationPlaying("CrouchAttack") == false)
            yield return false;

        AttackRequested = false;
        float length = GetCurrentAnimationLength();

        // 
        while (_time < length)
        {
            // 
            if (_time > CROUCH_ATTACK_TIME_1)
            {
                ActivateAttackRange(10);
            }

            // 
            _time += Time.deltaTime;
            yield return false;
        }

        // 
        DeactivateAttackRange(10);
        StopAttacking();
        UnblockMoving();
        UnblockAttacking();
        yield break;
    }
    /// <summary>
    /// 공중 공격 코루틴입니다.
    /// </summary>
    IEnumerator CoroutineJumpAttack()
    {
        SoundSaber.Play();

        // 
        yield return new WaitForEndOfFrame();
        _time = 0;
        while (IsAnimationPlaying("AirAttack") == false)
            yield return false;

        BlockAttacking();
        AttackRequested = false;
        float length = GetCurrentAnimationLength();

        // 
        while (_time < length)
        {
            // 
            if (_time > JUMP_ATTACK_TIME_2)
            {
                ActivateAttackRange(9);
                DeactivateAttackRange(8);
            }
            else if (_time > JUMP_ATTACK_TIME_1)
            {
                ActivateAttackRange(8);
            }

            // 
            _time += Time.deltaTime;
            yield return false;
        }

        // 
        DeactivateAttackRange(8);
        DeactivateAttackRange(9);
        StopAttacking();
        UnblockAttacking();
        yield break;
    }
    /// <summary>
    /// 공격 코루틴입니다.
    /// </summary>
    IEnumerator CoroutineAttack()
    {
        bool frameEventOccurred;
        try
        {
            // 
            yield return new WaitForEndOfFrame();
            _time = 0;
            frameEventOccurred = false;
            while (IsAnimationPlaying("Attack1") == false)
                yield return false;
            FE_Attack1_1beg();
            yield return new WaitForEndOfFrame();
            while (IsAnimationPlaying("Attack1"))
            {
                if (AttackRequested)
                    break;

                if (frameEventOccurred == false)
                {
                    if (_time > _ATTACK1_END_INDEX)
                    {
                        FE_Attack1_3end();
                        frameEventOccurred = true;
                    }
                    else if (_time > _ATTACK1_RUN_INDEX)
                    {
                        FE_Attack1_2run();
                    }
                }

                _time += Time.deltaTime;
                yield return false;
            }
            if (AttackRequested == false)
            {
                yield break;
            }

            // 
            yield return new WaitForEndOfFrame();
            _time = 0;
            frameEventOccurred = false;
            while (IsAnimationPlaying("Attack2") == false)
                yield return false;
            FE_Attack2_1beg();
            yield return new WaitForEndOfFrame();
            while (IsAnimationPlaying("Attack2"))
            {
                if (AttackRequested)
                    break;

                if (frameEventOccurred == false)
                {
                    if (_time > _ATTACK2_END_INDEX)
                    {
                        FE_Attack2_3end();
                        frameEventOccurred = true;
                    }
                    else if (_time > _ATTACK2_RUN_INDEX2)
                    {
                        FE_Attack2_2run_2();
                    }
                    else if (_time > _ATTACK2_RUN_INDEX1)
                    {
                        FE_Attack2_2run_1();
                    }
                }

                _time += Time.deltaTime;
                yield return false;
            }
            if (AttackRequested == false)
            {
                yield break;
            }

            // 
            yield return new WaitForEndOfFrame();
            _time = 0;
            frameEventOccurred = false;
            while (IsAnimationPlaying("Attack3") == false)
                yield return false;
            FE_Attack3_1beg();
            yield return new WaitForEndOfFrame();
            while (IsAnimationPlaying("Attack3"))
            {
                if (AttackRequested)
                    break;

                if (frameEventOccurred == false)
                {
                    if (_time > _ATTACK3_END_INDEX)
                    {
                        frameEventOccurred = true;
                    }
                    else if (_time > _ATTACK3_RUN_INDEX5)
                    {
                        FE_Attack3_2run_5();
                    }
                    else if (_time > _ATTACK3_RUN_INDEX4)
                    {
                        FE_Attack3_2run_4();
                    }
                    else if (_time > _ATTACK3_RUN_INDEX3)
                    {
                        FE_Attack3_2run_3();
                    }
                    else if (_time > _ATTACK3_RUN_INDEX2)
                    {
                        FE_Attack3_2run_2();
                    }
                    else if (_time > _ATTACK3_RUN_INDEX1)
                    {
                        FE_Attack3_2run_2();
                    }
                }

                _time += Time.deltaTime;
                yield return false;
            }
            if (AttackRequested == false)
            {
                yield break;
            }

            /**
            yield return new WaitForSeconds(0.2f);
            AttackRequested = false;
            while (IsAnimationPlaying("Attack2"))
            {
                if (AttackRequested)
                    break;
                yield return false;
            }
            if (AttackRequested == false)
            {
                Attack_saber2_end();
                yield break;
            }

            yield return new WaitForSeconds(0.2f);
            AttackRequested = false;
            while (IsAnimationPlaying("Attack3"))
            {
                if (AttackRequested)
                    break;
                yield return false;
            }

            AttackEndFromRun_beg();
            */

        }
        finally
        {
            EndAttack();
        }

        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    void EndAttack()
    {
        FE_AttackEnd_1beg();
        FE_AttackEnd_3end();
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
        // 
        StopMoving();
        BlockMoving();

        // 
        Attacking = true;
        AttackRequested = true;

        // 
        AttackCount += 1;
        if (AttackCount == 1)
        {
            _coroutineAttack = StartCoroutine(CoroutineAttack());
        }
    }
    /// <summary>
    /// 플레이어의 공격을 중지합니다.
    /// </summary>
    void StopAttacking()
    {
        // 
        UnblockMoving();

        // 
        Attacking = false;
        AttackRequested = false;

        // 
        AttackCount = 0;
        DeactivateAllAttackRange();
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
        // 
        BlockAirDashing();

        // 
        Attacking = true;
        AttackRequested = true;

        // 
        _coroutineAttack = StartCoroutine(CoroutineJumpAttack());
    }
    /// <summary>
    /// 플레이어가 앉은 채로 공격합니다.
    /// </summary>
    void CrouchAttack()
    {
        Attacking = true;
        AttackRequested = true;

        // 
        _coroutineAttack = StartCoroutine(CoroutineCrouchAttack());
    }
    /// <summary>
    /// 플레이어가 벽을 타면서 공격합니다.
    /// </summary>
    void SlideAttack()
    {
        Attacking = true;
        AttackRequested = true;

        // 
        _coroutineAttack = StartCoroutine(CoroutineSlideAttack());
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

        if (_attacking)
        {
            var curState = _Animator.GetCurrentAnimatorStateInfo(0);
            if (curState.IsName("AirAttack"))
            {
                var nTime = curState.normalizedTime;
                var fTime = nTime - Mathf.Floor(nTime);
                _Animator.Play("LandAttack", 0, fTime);

                // 
                StopMoving();
                BlockMoving();
            }
        }
    }

    ///////////////////////////////////////////////////////////////////
    // 점프 및 낙하
    /// <summary>
    /// 플레이어를 점프하게 합니다.
    /// </summary>
    protected override void Jump()
    {
        // 
        base.Jump();

        // 
        StopAttacking();

        // 
        Voices[0].Play();
        SoundEffects[1].Play();
    }
    /// <summary>
    /// 플레이어의 낙하를 중지합니다.
    /// </summary>
    protected override void StopFalling()
    {
        base.StopFalling();
    }

    ///////////////////////////////////////////////////////////////////
    // 대쉬
    /// <summary>
    /// 플레이어가 대쉬하게 합니다.
    /// </summary>
    protected override void Dash()
    {
        base.Dash();

        /// DeactivateAllAttackRange();
        StopAttacking();

        // 대쉬 효과 애니메이션을 추가합니다.
        GameObject dashFog = CloneObject(effects[0], dashFogPosition);
        if (FacingRight == false)
        {
            var newScale = dashFog.transform.localScale;
            newScale.x = FacingRight ? newScale.x : -newScale.x;
            dashFog.transform.localScale = newScale;
        }
        SoundEffects[3].Play();

        // 대쉬 코루틴을 실행합니다.
        _dashCoroutine = StartCoroutine(CoroutineDash());
    }
    /// <summary>
    /// 플레이어의 대쉬를 중지합니다.
    /// </summary>
    /// <param name="userCanceled">사용자의 입력에 의해 중지되었다면 참입니다.</param>
    protected override void StopDashing(bool userCanceled)
    {
        bool wasDashing = Dashing;
        base.StopDashing(userCanceled);

        if (wasDashing)
        {
            // 대쉬 이펙트를 제거합니다.
            if (_dashBoostEffect != null)
            {
                _dashBoostEffect.GetComponent<EffectScript>().RequestEnd();
                _dashBoostEffect = null;
            }
            // 사용자의 입력에 의해 대쉬가 중지되었다면
            if (userCanceled)
            {
                // 코루틴을 중지합니다.
                StopCoroutine(_dashCoroutine);
                if (DashJumping == false)
                {
                    /// StopDashing();
                    StopAirDashing();
                    StopMoving();
                    SoundEffects[3].Stop();
                    SoundEffects[4].Play();
                }
            }

            /**
            // 코루틴을 중지합니다.
            if (_dashCoroutine != null)
            {
                StopCoroutine(_dashCoroutine);
                if (DashJumping == false)
                {
                    /// StopDashing();
                    StopAirDashing();
                    StopMoving();
                    SoundEffects[3].Stop();
                    SoundEffects[4].Play();
                }
            }
            */
        }
    }

    ///////////////////////////////////////////////////////////////////
    // 벽 타기
    /// <summary>
    /// 플레이어가 벽을 타도록 합니다.
    /// </summary>
    protected override void Slide()
    {
        base.Slide();

        // 코루틴을 시작합니다.
        _slideCoroutine = StartCoroutine(CoroutineSlide());
    }
    /// <summary>
    /// 플레이어의 벽 타기를 중지합니다.
    /// </summary>
    protected override void StopSliding()
    {
        base.StopSliding();

        // 코루틴을 중지합니다.
        if (_slideCoroutine != null)
        {
            StopCoroutine(_slideCoroutine);
        }
    }

    ///////////////////////////////////////////////////////////////////
    // 조합
    /// <summary>
    /// 플레이어가 벽 점프를 합니다.
    /// </summary>
    protected override void WallJump()
    {
        base.WallJump();

        // 코루틴을 시작합니다.
        _wallJumpCoroutine = StartCoroutine(CoroutineWallJump());
    }
    /// <summary>
    /// 플레이어의 벽 점프를 중지합니다.
    /// </summary>
    protected override void StopWallJumping()
    {
        base.StopWallJumping();

        // 코루틴을 중지합니다.
        if (_wallJumpCoroutine != null)
        {
            StopCoroutine(_wallJumpCoroutine);
            _wallJumpCoroutine = null;
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
        if (_dashBoostEffect != null)
        {
            _dashBoostEffect.GetComponent<EffectScript>().RequestEnd();
            _dashBoostEffect = null;
        }
    }
    /// <summary>
    /// 플레이어가 에어 대쉬하게 합니다.
    /// </summary>
    protected override void AirDash()
    {
        base.AirDash();
        SoundEffects[3].Play();

        // 코루틴을 시작합니다.
        _airDashCoroutine = StartCoroutine(CoroutineAirDash());
    }
    /// <summary>
    /// 플레이어의 에어 대쉬를 중지합니다.
    /// </summary>
    protected override void StopAirDashing()
    {
        base.StopAirDashing();
        if (_dashBoostEffect != null)
        {
            _dashBoostEffect.GetComponent<EffectScript>().RequestEnd();
            _dashBoostEffect = null;
        }

        // 코루틴을 중지합니다.
        if (_airDashCoroutine != null)
        {
            StopCoroutine(_airDashCoroutine);
            _airDashCoroutine = null;
        }
    }
    /// <summary>
    /// 플레이어가 벽에서 대쉬 점프하게 합니다.
    /// </summary>
    protected override void WallDashJump()
    {
        base.WallDashJump();

        // 코루틴을 시작합니다.
        _wallJumpCoroutine = StartCoroutine(CoroutineWallJump());
    }

    #endregion





    #region PlayerController 상태 메서드를 재정의 합니다.
    /// <summary>
    /// 플레이어가 사망합니다.
    /// </summary>
    protected override void Dead()
    {
        base.Dead();

        /// _StageManager._deadEffect.RequestRun(_StageManager._player);
        _deadEffect.RequestRun(this);
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
        DeactivateAllAttackRange();

        // 플레이어가 생존해있다면
        if (IsAlive())
        {
            // 대미지 음성 및 효과음을 재생합니다.
            if (BigDamaged)
            {
                VoiceBigDamaged.Play();
                SoundHit.Play();
            }
            else
            {
                VoiceDamaged.Play();
                SoundHit.Play();
            }

            // 발생한 효과를 제거합니다.
            if (_slideFogEffect != null)
            {
                _slideFogEffect.GetComponent<EffectScript>().RequestDestroy();
            }
            if (_dashBoostEffect != null)
            {
                _dashBoostEffect.GetComponent<EffectScript>().RequestDestroy();
            }
        }

        // END_HURT_TIME 시간 후에 대미지를 입은 상태를 종료합니다.
        Invoke("EndHurt", END_HURT_TIME);
    }

    /// <summary>
    /// 다친 상태를 끝냅니다.
    /// </summary>
    protected override void EndHurt()
    {
        base.EndHurt();
        if (Danger && dangerVoicePlayed == false)
        {
            VoiceDanger.Play();
            dangerVoicePlayed = true;
        }
        else
        {
            dangerVoicePlayed = false;
        }
    }

    #endregion





    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 공격 범위를 활성화합니다.
    /// </summary>
    /// <param name="index">활성화할 공격 범위의 인덱스입니다.</param>
    void ActivateAttackRange(int index)
    {
        _attackRange[index].SetActive(true);
    }
    /// <summary>
    /// 공격 범위를 비활성화합니다.
    /// </summary>
    /// <param name="index">비활성화할 공격 범위의 인덱스입니다.</param>
    void DeactivateAttackRange(int index)
    {
        _attackRange[index].SetActive(false);
    }
    /// <summary>
    /// 모든 공격 범위를 비활성화합니다.
    /// </summary>
    void DeactivateAllAttackRange()
    {
        for (int i = 0; i < _attackRange.Length; ++i)
        {
            DeactivateAttackRange(i);
        }
    }

    #endregion





    #region 색상 보조 메서드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    Dictionary<int, Texture2D> _hit_textures = new Dictionary<int, Texture2D>();

    /// <summary>
    /// 텍스쳐가 준비되었는지 확인합니다.
    /// </summary>
    /// <param name="textureID">확인할 텍스쳐의 식별자입니다.</param>
    /// <param name="colorPalette">확인할 팔레트입니다.</param>
    /// <returns>텍스쳐가 준비되었다면 참입니다.</returns>
    protected override bool IsTexturePrepared(int textureID, Color[] colorPalette)
    {
        if (colorPalette == ZColorPalette.InvenciblePalette)
        {
            return _hit_textures.ContainsKey(textureID);
        }

        // 
        return false;
    }
    /// <summary>
    /// 준비된 텍스쳐를 가져옵니다.
    /// </summary>
    /// <param name="textureID">가져올 텍스쳐의 식별자입니다.</param>
    /// <param name="currentPalette">가져올 팔레트입니다.</param>
    /// <returns>준비된 텍스쳐를 반환합니다.</returns>
    protected override Texture2D GetPreparedTexture(int textureID, Color[] currentPalette)
    {
        Texture2D cloneTexture = null;
        if (currentPalette == ZColorPalette.InvenciblePalette)
        {
            cloneTexture = _hit_textures[textureID];
        }
        else
        {
            throw new Exception("예기치 못한 오류");
        }

        return cloneTexture;
    }
    /// <summary>
    /// 컬러 팔레트를 이용하여 생성된 새 텍스쳐를 집합에 넣습니다.
    /// </summary>
    /// <param name="textureID">텍스쳐 식별자입니다.</param>
    /// <param name="cloneTexture">생성한 텍스쳐입니다.</param>
    /// <param name="colorPalette">텍스쳐를 생성하기 위해 사용한 팔레트입니다.</param>
    protected override void AddTextureToSet(int textureID, Texture2D cloneTexture, Color[] colorPalette)
    {
        if (colorPalette == ZColorPalette.InvenciblePalette)
        {
            _hit_textures.Add(textureID, cloneTexture);
        }
        else
        {
            throw new Exception("예기치 못한 추가 오류");
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    protected override void TESTEST1()
    {
        _CurrentPalette = ZColorPalette.InvenciblePalette;
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void TESTEST2()
    {
        _CurrentPalette = null;
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void TESTEST3()
    {
        _CurrentPalette = null;
    }

    #endregion





    #region 구형 정의를 보관합니다.
    ///////////////////////////////////////////////////////////////////
    // 지상 공격
    /// <summary>
    /// 첫 번째 일반 지상 공격 시에 발생합니다.
    /// </summary>
    public void FE_Attack1_1beg()
    {
        // 받은 요청은 삭제합니다.
        AttackRequested = false;

        // 공격 시 불가능한 행동을 막습니다.
        BlockAttacking();
        BlockJumping();
        BlockDashing();
        StopMoving();
        BlockMoving();

        // 효과음을 재생합니다.
        VoiceAttack1.Play();
        SoundSaber.Play();
    }
    /// <summary>
    /// 첫 번째 일반 지상 공격이 공격으로 인정되는 때에 발생합니다.
    /// </summary>
    public void FE_Attack1_2run()
    {
        // 공격 범위를 활성화합니다.
        ActivateAttackRange(0);
    }
    /// <summary>
    /// 첫 번째 일반 지상 공격이 종료할 때 발생합니다.
    /// </summary>
    public void FE_Attack1_3end()
    {
        // 막은 행동을 가능하게 합니다.
        UnblockAttacking();
        UnblockJumping();
        UnblockDashing();
    }
    /// <summary>
    /// 두 번째 일반 지상 공격 시에 발생합니다.
    /// </summary>
    public void FE_Attack2_1beg()
    {
        // 공격 범위를 비활성화합니다.
        DeactivateAttackRange(0);

        // 받은 요청은 삭제합니다.
        AttackRequested = false;

        // 공격 시 불가능한 행동을 막습니다.
        BlockAttacking(); // 공격
        BlockJumping(); // 점프
        BlockDashing(); // 대쉬

        // 효과음을 재생합니다.
        VoiceAttack2.Play();
        SoundSaber.Play();
    }
    /// <summary>
    /// 두 번째 일반 지상 공격이 공격으로 인정되는 때에 발생합니다.
    /// </summary>
    public void FE_Attack2_2run_1()
    {
        // 공격 범위를 활성화합니다.
        ActivateAttackRange(1);
    }
    /// <summary>
    /// 두 번째 일반 지상 공격 2차입니다.
    /// </summary>
    public void FE_Attack2_2run_2()
    {
        // 이전 공격 범위를 비활성화합니다.
        DeactivateAttackRange(1);

        // 공격 범위를 활성화합니다.
        ActivateAttackRange(2);
    }
    /// <summary>
    /// 두 번째 일반 지상 공격이 종료할 때 발생합니다.
    /// </summary>
    public void FE_Attack2_3end()
    {
        // 막은 행동을 가능하게 합니다.
        UnblockAttacking();
        UnblockJumping();
        UnblockDashing();
    }
    /// <summary>
    /// 세 번째 일반 지상 공격 시에 발생합니다.
    /// </summary>
    public void FE_Attack3_1beg()
    {
        // 공격 범위를 비활성화합니다.
        DeactivateAttackRange(1);
        DeactivateAttackRange(1);

        // 받은 요청은 삭제합니다.
        AttackRequested = false;

        // 공격 시 불가능한 행동을 막습니다.
        BlockAttacking(); // 공격
        BlockJumping(); // 점프
        BlockDashing(); // 대쉬

        // 효과음을 재생합니다.
        VoiceAttack3.Play();
        SoundSaber.Play();
    }
    /// <summary>
    /// 세 번째 일반 지상 공격이 공격으로 인정되는 때에 발생합니다.
    /// </summary>
    public void FE_Attack3_2run_1()
    {
        // 공격 범위를 활성화합니다.
        ActivateAttackRange(3);
    }
    /// <summary>
    /// 세 번째 일반 지상 공격 2차입니다.
    /// </summary>
    public void FE_Attack3_2run_2()
    {
        // 이전 공격 범위를 비활성화합니다.
        DeactivateAttackRange(3);

        // 공격 범위를 활성화합니다.
        ActivateAttackRange(4);
    }
    /// <summary>
    /// 세 번째 일반 지상 공격 3차입니다.
    /// </summary>
    public void FE_Attack3_2run_3()
    {
        // 이전 공격 범위를 비활성화합니다.
        DeactivateAttackRange(4);

        // 공격 범위를 활성화합니다.
        ActivateAttackRange(5);
    }
    /// <summary>
    /// 세 번째 일반 지상 공격 4차입니다.
    /// </summary>
    public void FE_Attack3_2run_4()
    {
        // 이전 공격 범위를 비활성화합니다.
        DeactivateAttackRange(5);

        // 공격 범위를 활성화합니다.
        ActivateAttackRange(6);
    }
    /// <summary>
    /// 세 번째 일반 지상 공격 5차입니다.
    /// </summary>
    public void FE_Attack3_2run_5()
    {
        // 이전 공격 범위를 비활성화합니다.
        DeactivateAttackRange(6);

        // 공격 범위를 활성화합니다.
        ActivateAttackRange(7);
    }
    /// <summary>
    /// 지상 공격이 종료할 때 발생합니다.
    /// </summary>
    public void FE_AttackEnd_1beg()
    {
        // 막은 행동을 가능하게 합니다.
        UnblockAttacking();
        UnblockJumping();
        UnblockDashing();

        // 
        StopAttacking();

        // 공격 범위를 비활성화합니다.
        DeactivateAttackRange(2);
    }
    /// <summary>
    /// 지상 공격 모션이 완전히 종료되어 대기 상태로 바뀔 때 발생합니다.
    /// </summary>
    public void FE_AttackEnd_3end()
    {
        StopAttacking();

        // 공격 범위를 모두 비활성화합니다.
        DeactivateAttackRange(0);
        DeactivateAttackRange(1);
        DeactivateAttackRange(2);
        DeactivateAttackRange(3);
        DeactivateAttackRange(4);
        DeactivateAttackRange(5);
        DeactivateAttackRange(6);
        DeactivateAttackRange(7);
    }

    ///////////////////////////////////////////////////////////////////
    // 대쉬

    ///////////////////////////////////////////////////////////////////
    // 벽 타기

    ///////////////////////////////////////////////////////////////////
    // 점프 공격

    #endregion
}