using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 엑스에 대한 컨트롤러입니다.
/// </summary>
public class XController : PlayerController
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.


    #endregion










    #region 효과 객체를 보관합니다.
    GameObject dashBoostEffect = null;


    #endregion










    #region 플레이어의 상태 필드를 정의합니다.
    bool _shooting = false;
    bool Shooting
    {
        get { return _shooting; }
        set { _animator.SetBool("Shooting", _shooting = value); }
    }
    bool shotPressed = false;
    float chargeTime = 0;
    public float maxChargeTime = 3;
    bool ShotTriggered
    {
        get { return _shooting; }
        set { _animator.SetBool("Shooting", _shooting = value); }
    }
    public GameObject[] bullets;
    public Transform shotPosition;
    public float shotSpeed = 10;
    float shotTime = 0;
    public float endShotTime = 0.5416667f; // 0.4f;
    readonly float[] chargeLevel = { 0.2f, 0.3f, 1.7f };

    bool _shotBlocked = false;
    public bool ShotBlocked
    {
        get { return _shotBlocked; }
        private set { _shotBlocked = value; }
    }

    GameObject _chargeEffect1 = null;
    GameObject _chargeEffect2 = null;
    public Transform chargeEffectPosition;

    bool dangerVoicePlayed = false;
    const float endHurtLength = 0.361112f;



    bool fullyCharged = false;


    #endregion










    #region MonoBehavior 기본 메서드를 재정의합니다.
    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); // Initialize();
        // _renderer = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        // Initialize();
    }
    /// <summary>
    /// 
    /// </summary>
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
        // 캐릭터 변경 키가 눌린 경우
        else if (IsKeyDown("ChangeCharacter"))
        {
            stageManager.ChangePlayer(stageManager._playerZ);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        UpdateState();

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
            else if (_rigidbody.velocity.y == 0f)
            {
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
            if (IsLeftKeyPressed()) // if (IsKeyPressed(GameKey.Left))
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
            else if (IsRightKeyPressed()) // else if (IsKeyPressed(GameKey.Right))
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
        if (IsKeyPressed("Attack")) // if (IsKeyPressed(GameKey.Attack))
        {
            if (shotPressed)
            {
                if (chargeTime > 0)
                {
                    // _chargeEffect2 = CloneObject(effects[6], transform);
                    Charge();
                }
                else
                {
                    // _chargeEffect1 = CloneObject(effects[5], transform);
                    BeginCharge();
                }
                chargeTime = (chargeTime >= maxChargeTime)
                    ? maxChargeTime : (chargeTime + Time.fixedDeltaTime);
            }
            else
            {
                // Fire();
                shotPressed = true;
            }


            /// print(chargeTime);
        }
        else if (shotPressed)
        {
            Fire();
        }
        shotTime += Time.fixedDeltaTime;


        /**
        if (IsAnimationPlaying("X_WallJump"))
        {
            print(GetCurrentAnimationLength());
        }
        */
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (chargeTime > 0)
        {
            _renderer.color = PlayerColor;
        }
    }


    #endregion










    #region 엑스에 대해 새롭게 정의된 행동 메서드의 목록입니다.
    ///////////////////////////////////////////////////////////////////
    // 공격
    /// <summary>
    /// 버스터를 발사합니다.
    /// </summary>
    void Fire()
    {
        int index = -1;
        if (chargeTime < chargeLevel[1])
        {
            index = 0; // _animator.Play("Shot", 0, 0);
            ShotBlocked = true;
        }
        else if (chargeTime < chargeLevel[2])
        {
            index = 1; // _animator.Play("Shot", 0, 0);
            ShotBlocked = true;
        }
        else
        {
            index = 2; // _animator.Play("ChargeShot", 0, 0);
            ShotBlocked = true;
        }

        // 상태를 업데이트 합니다.
        if (_chargeEffect1 != null)
        {
            if (_chargeEffect2 != null)
            {
                _chargeEffect2.GetComponent<EffectScript>().RequestDestroy();
                _chargeEffect2 = null;
            }
            _chargeEffect1.GetComponent<EffectScript>().RequestDestroy();
            _chargeEffect1 = null;
        }

        ShotTriggered = true;
        shotPressed = false;
        chargeTime = 0;
        shotTime = 0;
        StopCoroutine("CoroutineCharge");
        Shooting = true;
        if (Moving)
        {
            float nTime = GetCurrentAnimationPlaytime();
            float fTime = nTime - Mathf.Floor(nTime);
            _animator.Play("MoveShotRun", 0, fTime);
        }
        else
        {
            // _animator.Play(0, 0, 0);

            // [2016-02-06. 05:37] 노멀 샷과 차지 샷의 애니메이션 재생 개선.
            if (fullyCharged) // 완전히 차지된 경우
            {
                // ChargeShot 애니메이션을 재생합니다.
                _animator.Play("ChargeShot", 0, 0);
                fullyCharged = false;
            }
            else
            {
                // Shot 애니메이션을 재생합니다.
                _animator.Play("Shot", 0, 0); 
            }
        }
        _renderer.color = PlayerColor = Color.white;

        // 버스터 탄환을 생성하고 초기화합니다.
        // GameObject _bullet = I_nstantiate(bullets[index], shotPosition.position, shotPosition.rotation) as GameObject;
        GameObject _bullet = CloneObject(bullets[index], shotPosition);
        Vector3 bulletScale = _bullet.transform.localScale;
        bulletScale.x *= FacingRight ? 1 : -1;
        _bullet.transform.localScale = bulletScale;
        _bullet.GetComponent<Rigidbody2D>().velocity
            = (FacingRight ? Vector3.right : Vector3.left) * shotSpeed;
        XBusterScript buster = _bullet.GetComponent<XBusterScript>();
        buster.MainCamera = stageManager._mainCamera;

        // 효과음을 재생합니다.
        SoundEffects[8 + index].Play();
        SoundEffects[7].Stop();

        // 일정 시간 후에 샷 상태를 해제합니다.
        Invoke("EndShot", endShotTime);
    }
    /// <summary>
    /// 차지를 시작합니다.
    /// </summary>
    void BeginCharge()
    {
        // I_nstantiate(effects[5], transform.position, transform.rotation);
        // _chargeEffect1 = CloneObject(effects[5], transform);
        StartCoroutine(CoroutineCharge());
    }
    /// <summary>
    /// 차지 상태를 갱신합니다.
    /// </summary>
    void Charge()
    {
        // 차지 효과음 재생에 관한 코드입니다.
        if (chargeTime < chargeLevel[0]) // chargeLevel[1] - 0.1f
        {

        }
        else if (SoundEffects[7].isPlaying == false)
        {
            SoundEffects[7].time = 0;
            SoundEffects[7].Play();
        }
        else if (SoundEffects[7].time >= 2.9f)
        {
            SoundEffects[7].time = 2.1f;
        }

        // 차지 애니메이션 재생에 관한 코드입니다.
        if (chargeTime < chargeLevel[0])
        {

        }
        else if (_chargeEffect1 == null)
        {
            _chargeEffect1 = CloneObject(effects[5], chargeEffectPosition);
            _chargeEffect1.transform.SetParent(chargeEffectPosition);
        }
        else if (chargeTime < chargeLevel[2])
        {

        }
        else if (_chargeEffect2 == null)
        {
            _chargeEffect2 = CloneObject(effects[6], chargeEffectPosition);
            _chargeEffect2.transform.SetParent(chargeEffectPosition);



            // [2016-02-06. 05:37] fullyCharged 필드 처리 추가.
            fullyCharged = true;
        }
        else
        {
        }
    }
    /// <summary>
    /// 차지 코루틴입니다.
    /// </summary>
    /// <returns>코루틴 열거자입니다.</returns>
    IEnumerator CoroutineCharge()
    {
        float startTime = 0;
        while (chargeTime >= 0)
        {
            if (chargeTime < chargeLevel[1])
            {

            }
            else
            {
                int cTime = (int)(startTime * 10) % 3;
                if (cTime != 0)
                {
                    PlayerColor = (chargeTime < chargeLevel[2]) ?
                        Color.cyan : Color.green;
                }
                else
                {
                    PlayerColor = Color.white;
                }

                startTime += Time.fixedDeltaTime;
            }
            yield return false;
        }

        yield return true;
    }
    /// <summary>
    /// 버스터 공격을 종료합니다.
    /// </summary>
    void EndShot()
    {
        if (shotTime >= endShotTime)
        {
            float nTime = GetCurrentAnimationPlaytime();
            float fTime = nTime - Mathf.Floor(nTime);
            ShotTriggered = false;

            string nextStateName = null;
            if (Landed)
            {
                if (Moving)
                {
                    nextStateName = "MoveRun";
                }
                else
                {
                    nextStateName = "Idle";
                }
            }
            else
            {

            }
            _animator.Play(nextStateName, 0, fTime);
        }
        ShotBlocked = false;
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
        SoundEffects[1].Play();
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
    /// 플레이어 사망을 요청합니다.
    /// </summary>
    public override void RequestDead()
    {
        base.RequestDead();

        if (_chargeEffect1 != null)
        {
            if (_chargeEffect2 != null)
            {
                _chargeEffect2.GetComponent<EffectScript>().RequestDestroy();
                _chargeEffect2 = null;
            }
            _chargeEffect1.GetComponent<EffectScript>().RequestDestroy();
            _chargeEffect1 = null;
            SoundEffects[7].Stop();
            chargeTime = 0;
        }
    }
    /// <summary>
    /// 플레이어가 사망합니다.
    /// </summary>
    protected override void Dead()
    {
        base.Dead();
        stageManager._deadEffect.RequestRun(stageManager._player);
        Voices[9].Play();
        SoundEffects[12].Play();
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
            Voices[4].Play();
            SoundEffects[11].Play();
        }
        Invoke("EndHurt", endHurtLength);
    }
    /// <summary>
    /// 대미지 상태를 해제합니다.
    /// </summary>
    protected override void EndHurt()
    {
        base.EndHurt();
        if (Danger && dangerVoicePlayed == false)
        {
            Voices[6].Play();
            dangerVoicePlayed = true;
        }
        else if (Health > DangerHealth)
        {
            dangerVoicePlayed = false;
        }
    }

    #endregion










    #region 프레임 이벤트 핸들러를 정의합니다.
    ///////////////////////////////////////////////////////////////////
    // 점프 및 낙하
    /// <summary>
    /// 점프 시작 시에 발생합니다.
    /// </summary>
    void FE_JumpBeg()
    {
        // 슬라이드를 금지합니다.
//        BlockSliding();
//        Invoke("UnblockSliding", 0.1f);

//        SlideBlocked = true;
    }
    /// <summary>
    /// 점프 시작이 끝난 후에 발생합니다.
    /// </summary>
    void FE_JumpRun()
    {
        // 금지한 슬라이딩을 해제합니다.
//        UnblockSliding();

//        SlideBlocked = false;
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
        // UnblockSliding();
        // _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }

    ///////////////////////////////////////////////////////////////////
    // 기타
    void FE_Flash()
    {
        
    }

    #endregion










    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 두 색상이 서로 같은 색인지 확인합니다.
    /// </summary>
    /// <param name="color1">비교할 색입니다.</param>
    /// <param name="color2">비교할 색입니다.</param>
    /// <returns>두 색의 rgba 값이 서로 같으면 참입니다.</returns>
    bool IsSameColor(Color color1, Color color2)
    {
        return (color1.r == color2.r
            && color1.g == color2.g
            && color1.b == color2.b
            && color1.a == color2.a);
    }

    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("Fire로 대체되었습니다.", true)]
    /// <summary>
    /// 버스터 공격합니다.
    /// </summary>
    void Shot()
    {
        if (Shooting)
        {
            _animator.Play("Shot", 0, 0);
        }

        Shooting = true;
        Invoke("StopShot", 1);
    }
    [Obsolete("Fire로 대체되었습니다.", true)]
    /// <summary>
    /// 버스터 공격을 중지합니다.
    /// </summary>
    void StopShot()
    {
        Shooting = false;
    }

    #endregion
}
