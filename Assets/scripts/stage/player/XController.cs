using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 엑스에 대한 컨트롤러입니다.
/// </summary>
public class XController : PlayerController
{
    #region 상수를 정의합니다.
    /// <summary>
    /// 차지 단계가 변하는 시간입니다.
    /// </summary>
    public float[] CHARGE_LEVEL = { 0.2f, 0.3f, 1.7f };
    
    /// <summary>
    /// 샷 발사 시에 반짝이는 시간입니다.
    /// </summary>
    const float LIGHTING_TIME = 0.05f;
    /// <summary>
    /// 샷 상태가 끝나는 시간입니다.
    /// </summary>
    const float END_SHOOTING_TIME = 0.5416667f;
    
    #endregion

    



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 버스터가 발사되는 속도입니다.
    /// </summary>
    public float _shotSpeed = 20;
    /// <summary>
    /// 최대 차지 시간입니다.
    /// </summary>
    public float _maxChargeTime = 3;
    /// <summary>
    /// 버스터 샷 집합입니다.
    /// </summary>
    public GameObject[] _bullets;
    
    /// <summary>
    /// 버스터 샷이 생성되는 위치입니다.
    /// </summary>
    public Transform _shotPosition;
    /// <summary>
    /// 좌우 이동 시 버스터 샷이 생성되는 위치입니다.
    /// </summary>
    public Transform _walkShotPosition;
    /// <summary>
    /// 대쉬 시 버스터 샷이 생성되는 위치입니다.
    /// </summary>
    public Transform _dashShotPosition;
    /// <summary>
    /// 벽 타기 시 버스터 샷이 생성되는 위치입니다.
    /// </summary>
    public Transform _wallShotPosition;
    /// <summary>
    /// 점프 시 버스터 샷이 생성되는 위치입니다.
    /// </summary>
    public Transform _jumpShotPosition;
    /// <summary>
    /// 앉은 상태에서 버스터 샷이 생성되는 위치입니다.
    /// </summary>
    public Transform _crouchShotPosition;
    
    /// <summary>
    /// 차지 효과가 발생하는 위치입니다.
    /// </summary>
    public Transform _chargeEffectPosition;
    
    #endregion

    



    #region 효과 객체를 보관합니다.
    /// <summary>
    /// 대쉬 부스트 효과를 보관합니다.
    /// </summary>
    GameObject _dashBoostEffect = null;
    
    /// <summary>
    /// 차지 효과 1를 보관합니다.
    /// </summary>
    GameObject _chargeEffect1 = null;
    /// <summary>
    /// 차지 효과 2를 보관합니다.
    /// </summary>
    GameObject _chargeEffect2 = null;
    
    #endregion

    



    #region 플레이어의 상태 필드를 정의합니다.
    /// <summary>
    /// 샷 상태라면 참입니다.
    /// </summary>
    bool _shooting = false;
    /// <summary>
    /// 샷 키가 눌린 상태라면 참입니다.
    /// </summary>
    bool _shotPressed = false;
    /// <summary>
    /// 차지한 시간을 나타냅니다.
    /// </summary>
    float _chargeTime = 0;
    /// <summary>
    /// 샷이 막혀있다면 참입니다.
    /// </summary>
    bool _shotBlocked = false;
    
    /// <summary>
    /// 위험 경고 효과음이 재생되었다면 참입니다.
    /// </summary>
    bool _dangerVoicePlayed = false;
    
    /// <summary>
    /// 샷을 발사한 이후로 경과한 시간입니다.
    /// </summary>
    float _shotTime2 = 0;
    /// <summary>
    /// 샷을 발사한 이후로 경과한 시간입니다.
    /// </summary>
    float ShotTime
    {
        get { return _shotTime2; }
        set
        {
            _Animator.SetFloat("ShotTime", _shotTime2 = value);
        }
    }
    
    #endregion
    




    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 샷을 발사하고 있다면 참입니다.
    /// </summary>
    bool Shooting
    {
        get { return _shooting; }
        set
        {
            _Animator.SetBool("Shooting", _shooting = value);
        }
    }
    /// <summary>
    /// 샷이 막혀있다면 참입니다.
    /// </summary>
    public bool ShotBlocked
    {
        get { return _shotBlocked; }
        private set { _shotBlocked = value; }
    }
    
    /// <summary>
    /// 차지 샷 중이라면 참입니다.
    /// </summary>
    bool _chargeShooting;
    /// <summary>
    /// 차지 샷 중이라면 참입니다.
    /// </summary>
    bool ChargeShooting
    {
        get { return _chargeShooting; }
        set { _Animator.SetBool("ChargeShooting", _chargeShooting = value); }
    }
    
    /// <summary>
    /// 현재 무기 상태입니다. 0은 기본입니다.
    /// </summary>
    public int _weaponState = 0;

    #endregion





    #region 추상 프로퍼티를 구현합니다.
    /// <summary>
    /// 
    /// </summary>
    protected override AudioSource VoiceDamaged { get { return Voices[4]; } }
    /// <summary>
    /// 
    /// </summary>
    protected override AudioSource VoiceBigDamaged { get { return Voices[5]; } }
    /// <summary>
    /// 
    /// </summary>
    protected override AudioSource VoiceDanger { get { return Voices[6]; } }

    /// <summary>
    /// 
    /// </summary>
    protected override AudioSource SoundHit { get { return SoundEffects[11]; } }
    
    #endregion



    

    #region MonoBehavior 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        // 
        _DefaultPalette = XColorPalette.DefaultPalette;
    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        ShotTime = 100;
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
                UpdateEffectColor(dashAfterImage, XColorPalette.DefaultPalette, XColorPalette.DashEffectColorPalette);
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
        /**
        // 캐릭터 변경 키가 눌린 경우
        else if (IsKeyDown("ChangeCharacter"))
        {
            /// stageManager.ChangePlayer(stageManager.PlayerZ);
        }
        */
        // 테스트 코드입니다.
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeWeapon(_weaponState == 1 ? 0 : 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeWeapon(_weaponState == 2 ? 0 : 2);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeWeapon(_weaponState == 3 ? 0 : 3);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            ChangeWeapon(_weaponState == 4 ? 0 : 4);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeWeapon(0);
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
                    // 슬라이드가 막힌 경우에는 그냥 떨어집니다.
                    _Velocity = new Vector2
                        (_Velocity.x, _Velocity.y - _jumpDecSize);
                }
                else
                {
                    Slide();
                }
            }
            else if (IsKeyPressed("Jump") == false || _Velocity.y <= 0)
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
                // StopFalling();
                Land();
            }
            else if (Pushing)
            {
                if (SlideBlocked)
                {
                    float vy = _Velocity.y - _jumpDecSize;
                    _Velocity = new Vector2(_Velocity.x, vy > -16 ? vy : -16);
                }
                else
                {
                    Slide();
                }
            }
            else
            {
                float vy = _Velocity.y - _jumpDecSize;
                _Velocity = new Vector2(_Velocity.x, vy > -16 ? vy : -16);
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
                /*
                else if (Shooting)
                {
                    StopAirDashing();
                    Shot();
                }
                */
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
            else if (_Velocity.y == 0f)
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
        if (Dashing)
        {
            if (AirDashing)
            {

            }
            // 대쉬 중에 공중에 뜬 경우
            else if (Landed == false)
            {
                if (SlideBlocked && WallJumping)
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
                        Fall();
                    }
                    else
                    {
                        MoveLeft();
                    }
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
        if (IsKeyPressed("Attack") && ShotBlocked == false)
        {
            if (_shotPressed)
            {
                if (_chargeTime > 0)
                {
                    Charge();
                }
                else
                {
                    BeginCharge();
                }
                _chargeTime = (_chargeTime >= _maxChargeTime)
                    ? _maxChargeTime : (_chargeTime + Time.fixedDeltaTime);
            }
            else
            {
                _shotPressed = true;
            }
        }
        else if (_shotPressed)
        {
            // 차지 시간 초기화 이전에 값을 보관합니다.
            float chargeTime = _chargeTime;

            // 샷을 발사합니다.
            Shot();


            // 이전 애니메이션이 Idle인 경우의 처리입니다.
            if (IsAnimationPlaying("Idle")
                || IsAnimationPlaying("FallEnd")
                || IsAnimationPlaying("Shot")
                || IsAnimationPlaying("ChargeShot"))
            {
                if (chargeTime > CHARGE_LEVEL[2])
                {
                    _Animator.Play("ChargeShot", 0, 0);
                    ShotBlocked = true;
                }
                else
                {
                    _Animator.Play("Shot", 0, 0);
                }
            }
        }
        else if (Shooting)
        {
            UpdateShotRoutine();
        }

        ShotTime += Time.fixedDeltaTime;
    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    protected override void LateUpdate()
    {
        base.LateUpdate();
        UpdateState();

        // 엑스의 색상을 업데이트합니다.
        UpdateColor();
    }

    #endregion
    




    #region 엑스에 대해 새롭게 정의된 행동 메서드의 목록입니다.
    ///////////////////////////////////////////////////////////////////
    // 공격
    /// <summary>
    /// 버스터를 발사합니다.
    /// </summary>
    void Shot()
    {
        // 탄환 객체 인덱스입니다.
        int index = -1;
        if (_chargeTime < CHARGE_LEVEL[1])
        {
            // 탄환 객체 인덱스를 업데이트합니다.
            index = 0;
        }
        else if (_chargeTime < CHARGE_LEVEL[2])
        {
            // 탄환 객체 인덱스를 업데이트합니다.
            index = 1;
        }
        else
        {
            // 탄환 객체 인덱스를 업데이트합니다.
            index = 2;
        }


        // 상태를 업데이트합니다.
        {
            // 차지 효과 객체의 상태를 업데이트 합니다.
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

            // 필드를 초기화합니다.
            _shotPressed = false;
            _chargeTime = 0;
            ShotTime = 0;
            ResetBodyColor();

            if (_chargeCoroutine != null)
            {
                StopCoroutine(_chargeCoroutine);
                _chargeCoroutine = null;
            }

            Shooting = true;
        }

        // 버스터 탄환을 생성하고 초기화합니다.
        StartCoroutine(CreateBulletCoroutine(index));

        // 효과음을 재생합니다.
        SoundEffects[8 + index].Play();
        SoundEffects[7].Stop();

        // 일정 시간 후에 샷 상태를 해제합니다.
        Invoke("EndShot", END_SHOOTING_TIME);
    }
    /// <summary>
    /// 차지를 시작합니다.
    /// </summary>
    void BeginCharge()
    {
        _chargeCoroutine = StartCoroutine(ChargeCoroutine());
    }
    /// <summary>
    /// 차지 상태를 갱신합니다.
    /// </summary>
    void Charge()
    {
        // 차지 효과음 재생에 관한 코드입니다.
        if (_chargeTime < CHARGE_LEVEL[0])
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
        if (_chargeTime < CHARGE_LEVEL[0])
        {

        }
        else if (_chargeEffect1 == null)
        {
            _chargeEffect1 = CloneObject(effects[5], _chargeEffectPosition);
            _chargeEffect1.transform.SetParent(_chargeEffectPosition);
        }
        else if (_chargeTime < CHARGE_LEVEL[2])
        {

        }
        else if (_chargeEffect2 == null)
        {
            _chargeEffect2 = CloneObject(effects[6], _chargeEffectPosition);
            _chargeEffect2.transform.SetParent(_chargeEffectPosition);
        }
        else
        {
        }
    }
    /// <summary>
    /// 버스터 공격을 종료합니다.
    /// </summary>
    void EndShot()
    {
        if (ShotTime >= END_SHOOTING_TIME)
        {
            Shooting = false;
            ShotBlocked = false;
        }
    }


    /// <summary>
    /// 탄환 생성 코루틴입니다.
    /// </summary>
    /// <param name="index">탄환 인덱스입니다.</param>
    /// <returns></returns>
    IEnumerator CreateBulletCoroutine(int index)
    {
        // 사용할 변수를 선언합니다.
        Transform shotPosition = GetShotPosition();
        bool toLeft = (Sliding ? FacingRight : !FacingRight);


        // 탄환 발사 효과에 대해 먼저 생성하고 처리합니다.
        GameObject fireEffect = CloneObject(effects[7 + index], shotPosition);
        if (index == 2)
        {
            if (IsAnimationPlaying("Idle"))
            {
                fireEffect.transform.position = transform.position;
            }
        }
        Vector3 effectScale = fireEffect.transform.localScale;
        effectScale.x *= (toLeft ? -1 : 1);
        fireEffect.transform.localScale = effectScale;
        fireEffect.transform.parent = shotPosition.transform;

        // 버스터 컴포넌트를 발사체에 붙입니다.
        GameObject _bullet = CloneObject(_bullets[index], shotPosition);

        // 2단계 차지샷이라면 잠시 대기합니다.
        if (index == 2)
        {
            _bullet.GetComponent<SpriteRenderer>().color = Color.clear;
            yield return new WaitForSeconds(0.15f);
        }

        // 
        if (_bullet != null)
        {
            _bullet.GetComponent<SpriteRenderer>().color = Color.white;
            _bullet.transform.position = shotPosition.position;

            // 위치를 조정합니다.
            Vector3 bulletScale = _bullet.transform.localScale;
            bulletScale.x *= (toLeft ? -1 : 1);
            _bullet.transform.localScale = bulletScale;
            _bullet.GetComponent<Rigidbody2D>().velocity
                = (toLeft ? Vector3.left : Vector3.right) * _shotSpeed;

            // 탄환 속성을 업데이트 합니다.
            XBusterScript buster = _bullet.GetComponent<XBusterScript>();
            buster.MainCamera = Camera.main; // stageManager.MainCamera;
        }

        yield break;
    }
    /// <summary>
    /// 탄환을 생성합니다.
    /// </summary>
    /// <param name="index">탄환 타입을 표현하는 인덱스입니다.</param>
    void CreateBullet(int index)
    {
        // 사용할 변수를 선언합니다.
        Transform shotPosition = GetShotPosition();
        GameObject _bullet = CloneObject(_bullets[index], shotPosition);
        Vector3 bulletScale = _bullet.transform.localScale;
        bool toLeft = (Sliding ? FacingRight : !FacingRight);


        GameObject fireEffect = CloneObject(effects[7 + index], shotPosition);
        if (index == 2)
        {
            fireEffect.transform.position = transform.position;
        }

        Vector3 effectScale = fireEffect.transform.localScale;


        // 위치를 조정합니다.
        bulletScale.x *= (toLeft ? -1 : 1);
        _bullet.transform.localScale = bulletScale;
        _bullet.GetComponent<Rigidbody2D>().velocity
            = (toLeft ? Vector3.left : Vector3.right) * _shotSpeed;


        // 발사 효과를 생성합니다.
        effectScale.x *= (toLeft ? -1 : 1);
        fireEffect.transform.localScale = effectScale;
        fireEffect.transform.parent = shotPosition.transform;


        // 버스터 컴포넌트를 발사체에 붙입니다.
        XBusterScript buster = _bullet.GetComponent<XBusterScript>();
        buster.MainCamera = Camera.main; // _stageManager.MainCamera;
    }
    /// <summary>
    /// 샷이 시작되는 위치를 결정합니다.
    /// </summary>
    /// <returns>샷이 시작되는 위치를 반환합니다.</returns>
    Transform GetShotPosition()
    {
        Transform ret = _shotPosition;
        if (Jumping || Falling)
        {
            ret = _jumpShotPosition;
        }
        else if (Sliding)
        {
            ret = _wallShotPosition;
        }
        else if (Dashing)
        {
            ret = _dashShotPosition;
        }
        else if (Crouching)
        {
            ret = _crouchShotPosition;
        }
        else if (Moving)
        {
            ret = _walkShotPosition;
        }
        return ret;
    }
    
    #endregion



        

    #region PlayerController 행동 메서드를 위한 코루틴을 정의합니다.
    /// <summary>
    /// 차지 코루틴 필드입니다.
    /// </summary>
    Coroutine _chargeCoroutine = null;
    /// <summary>
    /// 차지 코루틴입니다.
    /// </summary>
    /// <returns>행동 단위가 끝날 때마다 null을 반환합니다.</returns>
    IEnumerator ChargeCoroutine()
    {
        bool chargedColor = false;

        while (_chargeTime >= 0)
        {
            if (_chargeTime < CHARGE_LEVEL[1])
            {
                yield return null;
            }
            else
            {
                if (chargedColor)
                {
                    Color[] palette = (_chargeTime < CHARGE_LEVEL[2]) ?
                        XColorPalette.XCharge1Palette : XColorPalette.XCharge2Palette;
                    _CurrentPalette = palette;
                }
                else
                {
                    ResetBodyColor();
                }
                chargedColor = !chargedColor;
            }
            yield return new WaitForSeconds(TIME_30FPS);
        }
        yield break;
    }
    
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
        yield return new WaitForEndOfFrame();
        if (IsAnimationPlaying("DashBeg"))
        {
            yield return new WaitForSeconds(DASH_BEG_TIME);
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

            yield return new WaitForSeconds(DASH_RUN_TIME);
        }

        // DashEnd (사용자 입력 중지가 아닌 기본 대쉬 중지 행동입니다.)
        if (DashJumping == false)
        {
            StartDashEnd(false);
        }

        // 코루틴을 중지합니다.
        yield break;
    }
    /// <summary>
    /// 대쉬 종료를 시작합니다.
    /// </summary>
    /// <param name="userCanceled">사용자 입력에 의해 대쉬가 중지되었다면 참입니다.</param>
    void StartDashEnd(bool userCanceled)
    {
        StopDashing(userCanceled);
        if (_dashCoroutine != null)
        {
            StopCoroutine(_dashCoroutine);
        }
        _dashCoroutine = StartCoroutine(CoroutineDashEnd());
    }
    /// <summary>
    /// 대쉬 종료 코루틴입니다.
    /// </summary>
    IEnumerator CoroutineDashEnd()
    {
        StopAirDashing();
        SoundEffects[3].Stop();
        SoundEffects[4].Play();
        BlockMoving();
        yield return new WaitForSeconds(DASH_END_TIME);

        UnblockMoving();
        _dashCoroutine = null;
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
    
    #endregion


    


    #region PlayerController 행동 메서드를 재정의합니다.
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
    /// <summary>
    /// 플레이어의 점프를 중지합니다.
    /// </summary>
    protected override void StopJumping()
    {
        base.StopJumping();
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
                StopMoving();
                StartDashEnd(true);
            }
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

        // 대쉬 점프를 합니다.
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
    /// 플레이어 사망을 요청합니다.
    /// </summary>
    public override void RequestDead()
    {
        base.RequestDead();

        // 사망할 때 플레이어가 차지 상태라면 효과를 제거합니다.
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
            _chargeTime = 0;
        }

        // 생존 시간에 생성되었던 효과를 제거합니다.
        if (_slideFogEffect != null)
        {
            _slideFogEffect.GetComponent<EffectScript>().RequestDestroy();
        }
    }
    /// <summary>
    /// 플레이어가 사망합니다.
    /// </summary>
    protected override void Dead()
    {
        base.Dead();

        // 사망 시 입자가 퍼지는 효과를 요청합니다.
        _deadEffect.RequestRun(this);
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

        // 플레이어가 생존해있다면
        float damagedTime = 0;
        if (IsAlive())
        {
            // 대미지 음성 및 효과음을 재생합니다.
            if (BigDamaged)
            {
                VoiceBigDamaged.Play();
                SoundHit.Play();
                damagedTime = BIG_DAMAGED_TIME;
            }
            else
            {
                VoiceDamaged.Play();
                SoundHit.Play();
                damagedTime = DAMAGED_TIME;
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
        Invoke("EndHurt", damagedTime);
    }
    /// <summary>
    /// 대미지 상태를 해제합니다.
    /// </summary>
    protected override void EndHurt()
    {
        base.EndHurt();
        
        // 블록된 행동 상태를 해제합니다.
        ShotBlocked = false;

        // 위험한 상태인데 위험 상태 경고 보이스를 재생하지 않았다면 재생합니다.
        if (Danger && _dangerVoicePlayed == false)
        {
            /// Voices[6].Play();
            VoiceDanger.Play();
            _dangerVoicePlayed = true;
        }
        // 위험 상태에서 벗어나면 위의 스위치를 해제합니다.
        else if (Health > DangerHealth)
        {
            _dangerVoicePlayed = false;
        }
    }

    #endregion


    


    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 재생중인 애니메이션을 특정 시점부터 다시 재생합니다.
    /// </summary>
    /// <param name="fTime">다시 재생할 정규화된 시간입니다.</param>
    void ReplayAnimation(float fTime)
    {
        var stateInfo = _Animator.GetCurrentAnimatorStateInfo(0);
        _Animator.Play(stateInfo.fullPathHash, 0, fTime);
    }
    
    /// <summary>
    /// 샷 루틴을 업데이트 합니다.
    /// </summary>
    void UpdateShotRoutine()
    {

    }
    
    /// <summary>
    /// 무기를 변경합니다.
    /// </summary>
    /// <param name="weaponIndex"></param>
    void ChangeWeapon(int weaponIndex)
    {
        Color[] targetPalette;
        switch (weaponIndex)
        {
            case 1:
                targetPalette = XColorPalette.XWeapon1Palette;
                break;

            case 2:
                targetPalette = XColorPalette.XWeapon2Palette;
                break;

            case 3:
                targetPalette = XColorPalette.XWeapon3Palette;
                break;

            case 4:
                targetPalette = XColorPalette.XWeapon4Palette;
                break;

            default:
                targetPalette = null; // XColorPalette.XDefaultPalette;
                break;
        }

        _weaponState = weaponIndex;
        _CurrentPalette = targetPalette;
    }
    
    /// <summary>
    /// 엑스의 색상을 업데이트합니다.
    /// </summary>
    protected override void UpdateColor()
    {
        base.UpdateColor();

        // 플레이어가 차지 중이라면 색을 업데이트합니다.
        if (_chargeTime > CHARGE_LEVEL[0])
        {
            // 차지 효과 색상을 업데이트 합니다.
            if (_chargeEffect2 != null)
            {
                UpdateChargeEffectColor(_chargeEffect2, XColorPalette.XNormalChargeEffectColorPalette2);
                UpdateChargeEffectColor(_chargeEffect1, XColorPalette.XNormalChargeEffectColorPalette2);
            }
            else if (_chargeEffect1 != null)
            {
                UpdateChargeEffectColor(_chargeEffect1, XColorPalette.XNormalChargeEffectColorPalette1);
            }
        }
        else
        {

        }
    }
    /// <summary>
    /// 차지 효과 개체의 색상표를 업데이트합니다.
    /// </summary>
    /// <param name="chargeEffect">차지 효과 개체입니다.</param>
    /// <param name="colorPalette">차지 효과 개체의 새 색상표입니다.</param>
    void UpdateChargeEffectColor(GameObject chargeEffect, Color[] colorPalette)
    {
        // 차지 효과 개체를 획득합니다.
        EffectScript effect = chargeEffect.GetComponent<EffectScript>();

        // 색상표를 획득합니다.
        Color[] palette = (_chargeTime < CHARGE_LEVEL[2]) ?
            XColorPalette.XNormalChargeEffectColorPalette1 : XColorPalette.XNormalChargeEffectColorPalette2;

        // 텍스쳐를 업데이트합니다.
        effect.RequestUpdateTexture(XColorPalette.XDefaultChargeEffectColorPalette, palette);
    }
    /// <summary>
    /// 엑스의 바디 색상표를 현재 웨폰 상태로 되돌립니다.
    /// </summary>
    void ResetBodyColor()
    {
        _CurrentPalette = GetPalette(_weaponState);
    }

    /// <summary>
    /// 
    /// </summary>
    Dictionary<int, Texture2D> _hit_textures = new Dictionary<int, Texture2D>();
    Dictionary<int, Texture2D> _charge1_textures = new Dictionary<int, Texture2D>();
    Dictionary<int, Texture2D> _charge2_textures = new Dictionary<int, Texture2D>();
    Dictionary<int, Texture2D> _weapon1_textures = new Dictionary<int, Texture2D>();
    Dictionary<int, Texture2D> _weapon2_textures = new Dictionary<int, Texture2D>();
    Dictionary<int, Texture2D> _weapon3_textures = new Dictionary<int, Texture2D>();
    Dictionary<int, Texture2D> _weapon4_textures = new Dictionary<int, Texture2D>();

    /// <summary>
    /// 텍스쳐가 준비되었는지 확인합니다.
    /// </summary>
    /// <param name="textureID">확인할 텍스쳐의 식별자입니다.</param>
    /// <param name="colorPalette">확인할 팔레트입니다.</param>
    /// <returns>텍스쳐가 준비되었다면 참입니다.</returns>
    protected override bool IsTexturePrepared(int textureID, Color[] colorPalette)
    {
        if (colorPalette == XColorPalette.InvenciblePalette)
        {
            return _hit_textures.ContainsKey(textureID);
        }
        else if (colorPalette == XColorPalette.XCharge1Palette)
        {
            return _charge1_textures.ContainsKey(textureID);
        }
        else if (colorPalette == XColorPalette.XCharge2Palette)
        {
            return _charge2_textures.ContainsKey(textureID);
        }
        else if (colorPalette == XColorPalette.XWeapon1Palette)
        {
            return _weapon1_textures.ContainsKey(textureID);
        }
        else if (colorPalette == XColorPalette.XWeapon2Palette)
        {
            return _weapon2_textures.ContainsKey(textureID);
        }
        else if (colorPalette == XColorPalette.XWeapon3Palette)
        {
            return _weapon3_textures.ContainsKey(textureID);
        }
        else if (colorPalette == XColorPalette.XWeapon4Palette)
        {
            return _weapon4_textures.ContainsKey(textureID);
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
        if (currentPalette == XColorPalette.InvenciblePalette)
        {
            cloneTexture = _hit_textures[textureID];
        }
        else if (currentPalette == XColorPalette.XCharge1Palette) // (CHARGE_LEVEL[0] < _chargeTime && _chargeTime < CHARGE_LEVEL[2])
        {
            cloneTexture = _charge1_textures[textureID];
        }
        else if (currentPalette == XColorPalette.XCharge2Palette) // (CHARGE_LEVEL[2] < _chargeTime)
        {
            cloneTexture = _charge2_textures[textureID];
        }
        else if (currentPalette == XColorPalette.XWeapon1Palette) // (_weaponState == 1)
        {
            cloneTexture = _weapon1_textures[textureID];
        }
        else if (currentPalette == XColorPalette.XWeapon2Palette) // (_weaponState == 2)
        {
            cloneTexture = _weapon2_textures[textureID];
        }
        else if (currentPalette == XColorPalette.XWeapon3Palette) // (_weaponState == 3)
        {
            cloneTexture = _weapon3_textures[textureID];
        }
        else if (currentPalette == XColorPalette.XWeapon4Palette) // (_weaponState == 4)
        {
            cloneTexture = _weapon4_textures[textureID];
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
        if (colorPalette == XColorPalette.InvenciblePalette)
        {
            _hit_textures.Add(textureID, cloneTexture);
        }
        else if (colorPalette == XColorPalette.XCharge1Palette)
        {
            _charge1_textures.Add(textureID, cloneTexture);
        }
        else if (colorPalette == XColorPalette.XCharge2Palette)
        {
            _charge2_textures.Add(textureID, cloneTexture);
        }
        else if (colorPalette == XColorPalette.XWeapon1Palette)
        {
            _weapon1_textures.Add(textureID, cloneTexture);
        }
        else if (colorPalette == XColorPalette.XWeapon2Palette)
        {
            _weapon2_textures.Add(textureID, cloneTexture);
        }
        else if (colorPalette == XColorPalette.XWeapon3Palette)
        {
            _weapon3_textures.Add(textureID, cloneTexture);
        }
        else if (colorPalette == XColorPalette.XWeapon4Palette)
        {
            _weapon4_textures.Add(textureID, cloneTexture);
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
        _CurrentPalette = XColorPalette.InvenciblePalette;
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void TESTEST2()
    {
        ResetBodyColor();
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void TESTEST3()
    {
        ResetBodyColor();
    }

    #endregion





    #region 정적 보조 메서드를 정의합니다.
    /// <summary>
    /// 현재 웨폰 상태에 해당하는 팔레트를 가져옵니다.
    /// </summary>
    /// <param name="weaponState">현재 웨폰 상태입니다.</param>
    /// <returns>현재 웨폰 상태에 해당하는 팔레트를 가져옵니다.</returns>
    static Color[] GetPalette(int weaponState)
    {
        Color[] palette;
        switch (weaponState)
        {
            case 1:
                palette = XColorPalette.XWeapon1Palette;
                break;

            case 2:
                palette = XColorPalette.XWeapon2Palette;
                break;

            case 3:
                palette = XColorPalette.XWeapon3Palette;
                break;

            case 4:
                palette = XColorPalette.XWeapon4Palette;
                break;

            default:
                palette = null;
                break;
        }
        return palette;
    }
    
    #endregion


    


    #region 구형 정의를 보관합니다.
    [Obsolete("후에 사용할지도 모르죠?")]
    /// <summary>
    /// 애니메이션 클립 정보를 담은 사전 객체입니다.
    /// </summary>
    Dictionary<string, AnimatorClipInfo> _clips;
    [Obsolete("후에 사용할지도 모르죠?")]
    /// <summary>
    /// 애니메이션 클립 정보를 초기화합니다.
    /// </summary>
    void InitializeAnimatorClips()
    {
        Dictionary<string, AnimatorClipInfo> dict = new Dictionary<string, AnimatorClipInfo>();
        AnimatorClipInfo[] clipInfos = _Animator.GetCurrentAnimatorClipInfo(0);
        foreach (AnimatorClipInfo clipInfo in clipInfos)
        {
            dict[clipInfo.clip.name] = clipInfo;
        }
        _clips = dict;
    }
    [Obsolete("_clips 미사용 경고를 없애기 위해 정의했습니다.")]
    /// <summary>
    /// _clips 변수를 사용하는 척합니다.
    /// </summary>
    void TestAnimatorClips()
    {
        foreach (string clipKey in _clips.Keys)
        {
            AnimatorClipInfo clipInfo = _clips[clipKey];
            Console.WriteLine(clipInfo.clip.length);
        }
    }

    #endregion
}
