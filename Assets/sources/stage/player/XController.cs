using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 엑스에 대한 컨트롤러입니다.
/// </summary>
public class XController : PlayerController
{
    #region 상수를 정의합니다.
    /// <summary>
    /// 차지 단계가 변하는 시간입니다.
    /// </summary>
    readonly float[] chargeLevel = { 0.2f, 0.3f, 1.7f };

    /// <summary>
    /// 무적 상태가 유지되는 시간입니다.
    /// </summary>
    const float INVENCIBLE_TIME = 0.361112f;


    /// <summary>
    /// 
    /// </summary>
    const string AniName_Idle = "Idle";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_Jump_1beg = "X03_Jump_1beg";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_Jump_2run = "X03_Jump_2run";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_Fall_1beg = "X03_Fall_1beg";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_Fall_2run = "X03_Fall_2run";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_Fall_3end = "X03_Fall_3end";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_Shot = "X04_Shot";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_ChargeShot = "X05_ChargeShot";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_Walk_1beg = "X06_Walk_1beg";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_Walk_2run = "X06_Walk_2run";


    /// <summary>
    /// 
    /// </summary>
    const string AniName_WalkShot_1beg = "X09_WalkShot_1beg";
    /// <summary>
    /// 
    /// </summary>
    const string AniName_WalkShot_2run = "X09_WalkShot_2run";


    /// <summary>
    /// 
    /// </summary>
    const string AniName_Danger = "X19_Danger";


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
    /// 
    /// </summary>
    public float _endShotTime = 0.5416667f;
    /// <summary>
    /// 버스터 샷 집합입니다.
    /// </summary>
    public GameObject[] _bullets;


    /// <summary>
    /// 버스터 샷이 생성되는 위치입니다.
    /// </summary>
    public Transform _shotPosition;
    /// <summary>
    /// 
    /// </summary>
    public Transform _dashShotPosition;
    /// <summary>
    /// 
    /// </summary>
    public Transform _wallShotPosition;


    /// <summary>
    /// 차지 효과가 발생하는 위치입니다.
    /// </summary>
    public Transform _chargeEffectPosition;


    #endregion










    #region 효과 객체를 보관합니다.
    /// <summary>
    /// 
    /// </summary>
    GameObject _dashBoostEffect = null;


    #endregion










    #region 플레이어의 상태 필드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    bool _shooting = false;
    /// <summary>
    /// 
    /// </summary>
    bool _shotPressed = false;
    /// <summary>
    /// 
    /// </summary>
    float _chargeTime = 0;
    /// <summary>
    /// 
    /// </summary>
    float _shotTime = 0;
    /// <summary>
    /// 
    /// </summary>
    bool _shotBlocked = false;
    /// <summary>
    /// 
    /// </summary>
    GameObject _chargeEffect1 = null;
    /// <summary>
    /// 
    /// </summary>
    GameObject _chargeEffect2 = null;


    /// <summary>
    /// 
    /// </summary>
    bool _dangerVoicePlayed = false;


    /// <summary>
    /// 완전히 버스터가 차지된 상태라면 참입니다.
    /// </summary>
    bool _fullyCharged = false;


    #endregion









    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 샷을 발사하고 있다면 참입니다.
    /// </summary>
    bool Shooting
    {
        get { return _shooting; }
        set { _animator.SetBool("Shooting", _shooting = value); }
    }
    /// <summary>
    /// 샷이 막혀있다면 참입니다.
    /// </summary>
    public bool ShotBlocked
    {
        get { return _shotBlocked; }
        private set { _shotBlocked = value; }
    }


    #endregion










    #region MonoBehavior 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {

    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
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


        // 애니메이터를 업데이트합니다.
        UpdateAnimator();


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
        // 캐릭터 변경 키가 눌린 경우
        else if (IsKeyDown("ChangeCharacter"))
        {
            stageManager.ChangePlayer(stageManager._playerZ);
        }


        // 애니메이터를 업데이트합니다.
//        UpdateAnimator();
    }
    /// <summary>
    /// FixedTimestep에 설정된 값에 따라 일정한 간격으로 업데이트 합니다.
    /// 물리 효과가 적용된 오브젝트를 조정할 때 사용됩니다.
    /// (Update는 불규칙한 호출이기 때문에 물리엔진 충돌검사가 제대로 되지 않을 수 있습니다.)
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
                    (_rigidbody.velocity.x, _rigidbody.velocity.y - _jumpDecSize);
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
                float vy = _rigidbody.velocity.y - _jumpDecSize;
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
        if (IsKeyPressed("Attack"))
        {
            if (_shotPressed)
            {
                if (_chargeTime > 0)
                {
                    // _chargeEffect2 = CloneObject(effects[6], transform);
                    Charge();
                }
                else
                {
                    // _chargeEffect1 = CloneObject(effects[5], transform);
                    BeginCharge();
                }
                _chargeTime = (_chargeTime >= _maxChargeTime)
                    ? _maxChargeTime : (_chargeTime + Time.fixedDeltaTime);
            }
            else
            {
                // Fire();
                _shotPressed = true;
            }
        }
        else if (_shotPressed)
        {
            Shot();
        }
        _shotTime += Time.fixedDeltaTime;
    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    protected override void LateUpdate()
    {
        base.LateUpdate();


        // 플레이어가 차지 중이라면 색을 업데이트합니다.
        if (_chargeTime > 0)
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
    void Shot()
    {
        // 탄환 객체 인덱스입니다.
        int index = -1;
        if (_chargeTime < chargeLevel[1])
        {
            // 탄환 객체 인덱스를 업데이트합니다.
            index = 0; // _animator.Play("Shot", 0, 0);
            ShotBlocked = true;
        }
        else if (_chargeTime < chargeLevel[2])
        {
            // 탄환 객체 인덱스를 업데이트합니다.
            index = 1; // _animator.Play("Shot", 0, 0);
            ShotBlocked = true;
        }
        else
        {
            index = 2; // _animator.Play("ChargeShot", 0, 0);
            ShotBlocked = true;
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
            _shotTime = 0;

            if (_chargeCoroutine != null)
            {
                StopCoroutine(_chargeCoroutine);
                _chargeCoroutine = null;
            }
            Shooting = true;
        }


        // 플레이어의 상태에 따라 애니메이션을 결정합니다.
        _shotCoroutine = StartCoroutine(UpdateShotAnimator());


        // 버스터 탄환을 생성하고 초기화합니다.
        CreateBullet(index);

        // 효과음을 재생합니다.
        SoundEffects[8 + index].Play();
        SoundEffects[7].Stop();

        // 일정 시간 후에 샷 상태를 해제합니다.
        Invoke("EndShot", _endShotTime);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    private void CreateBullet(int index)
    {
        Transform shotPosition = GetShotPosition();
        GameObject _bullet = CloneObject(_bullets[index], shotPosition);
        Vector3 bulletScale = _bullet.transform.localScale;
        bool toLeft = (Sliding ? FacingRight : !FacingRight);


        bulletScale.x *= (toLeft ? -1 : 1); // (FacingRight ? -1 : 1) : (FacingRight ? 1 : -1));
        _bullet.transform.localScale = bulletScale;
        _bullet.GetComponent<Rigidbody2D>().velocity
            = (toLeft ? Vector3.left : Vector3.right) * _shotSpeed;


        XBusterScript buster = _bullet.GetComponent<XBusterScript>();
        buster.MainCamera = stageManager._mainCamera;
    }


    /// <summary>
    /// 차지를 시작합니다.
    /// </summary>
    void BeginCharge()
    {
        /// StartCoroutine(CoroutineCharge());
        _chargeCoroutine = StartCoroutine(ChargeCoroutine());
    }
    /// <summary>
    /// 차지 상태를 갱신합니다.
    /// </summary>
    void Charge()
    {
        // 차지 효과음 재생에 관한 코드입니다.
        if (_chargeTime < chargeLevel[0]) // chargeLevel[1] - 0.1f
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
        if (_chargeTime < chargeLevel[0])
        {

        }
        else if (_chargeEffect1 == null)
        {
            _chargeEffect1 = CloneObject(effects[5], _chargeEffectPosition);
            _chargeEffect1.transform.SetParent(_chargeEffectPosition);
        }
        else if (_chargeTime < chargeLevel[2])
        {

        }
        else if (_chargeEffect2 == null)
        {
            _chargeEffect2 = CloneObject(effects[6], _chargeEffectPosition);
            _chargeEffect2.transform.SetParent(_chargeEffectPosition);


            // 필드를 업데이트합니다.
            _fullyCharged = true;
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
        if (_shotTime >= _endShotTime)
        {
            float nTime = GetCurrentAnimationPlaytime();
            float fTime = nTime - Mathf.Floor(nTime);


            /// ShotTriggered = false;
            Shooting = false;


            string nextStateName = null;
            if (Landed)
            {
                if (Moving)
                {
                    nextStateName = AniName_Walk_2run;
                }
                else
                {
                    nextStateName = AniName_Idle;
                }
            }
            else
            {

            }
            _animator.Play(nextStateName, 0, fTime);
        }
        ShotBlocked = false;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Transform GetShotPosition()
    {
        Transform ret = _shotPosition;
        if (Sliding)
        {
            ret = _wallShotPosition;
        }
        else if (Dashing)
        {
            ret = _dashShotPosition;
        }
        return ret;
    }


    #endregion










    #region PlayerController 행동 메서드를 위한 코루틴을 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    Coroutine _shotCoroutine;
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateShotAnimator()
    {
        if (_shotTime > _endShotTime)
        {
            _animator.Play(AniName_Idle);
            yield break;
        }

        const float lightingTime = 0.3f;
        bool endLight = false;
        

        // 발사 직후 빛나는 시간이라면
        if (_shotTime < lightingTime)
        {
            float nTime = GetCurrentAnimationPlaytime();
            float fTime = nTime - Mathf.Floor(nTime);


            // 
            if (Jumping)
            {

            }
            else if (Falling)
            {

            }
            else if (Sliding)
            {

            }
            else if (Dashing)
            {
                if (IsAnimationPlaying("X07_Dash_3end"))
                {
                    _animator.Play("X10_DashShotLight_3end", 0, fTime);
                }
                else if (IsAnimationPlaying("X07_Dash_2run"))
                {
                    _animator.Play("X10_DashShotLight_2run", 0, fTime);
                }
                else if (IsAnimationPlaying("X07_Dash_1beg"))
                {
                    _animator.Play("X10_DashShotLight_1beg", 0, fTime);
                }
                else
                {
                    Debug.Log("Don't know!");
                }
            }
            else if (Moving)
            {
                if (IsAnimationPlaying("X06_Walk_2run"))
                {
                    _animator.Play("X09_WalkShotLight_2run", 0, fTime);
                }
                else if (IsAnimationPlaying("X06_Walk_1beg"))
                {
                    _animator.Play("X09_WalkShotLight_1beg", 0, fTime);
                }
                else 
                {
                    Debug.Log("Don't know!");
                }
            }
            else
            {
                // 완전히 차지된 경우
                if (_fullyCharged)
                {
                    // ChargeShot 애니메이션을 재생합니다.
                    _animator.Play(AniName_ChargeShot, 0, 0);
                    _fullyCharged = false;
                }
                else
                {
                    // Shot 애니메이션을 재생합니다.
                    _animator.Play(AniName_Shot, 0, 0);
                }
            }
            _renderer.color = PlayerColor = Color.white;
        }
        // 빛나는 시간이 끝난 직후
        else if (endLight == false)
        {
            float nTime = GetCurrentAnimationPlaytime();
            float fTime = nTime - Mathf.Floor(nTime);


            //
            if (Jumping)
            {

            }
            else if (Falling)
            {

            }
            else if (Sliding)
            {

            }
            else if (Dashing)
            {
                if (IsAnimationPlaying("X10_DashShotLight_3end"))
                {
                    _animator.Play("X10_DashShot_3end", 0, fTime);
                }
                else if (IsAnimationPlaying("X10_DashShotLight_2run"))
                {
                    _animator.Play("X10_DashShot_2run", 0, fTime);
                }
                else if (IsAnimationPlaying("X10_DashShotLight_1beg"))
                {
                    _animator.Play("X10_DashShot_1beg", 0, fTime);
                }
                else
                {
                    Debug.Log("Don't know!");
                }
            }
            else if (Moving)
            {
                if (IsAnimationPlaying("X09_WalkShotLight_2run"))
                {
                    _animator.Play("X09_WalkShot_2run", 0, fTime);
                }
                else if (IsAnimationPlaying("X09_WalkShotLight_1beg"))
                {
                    _animator.Play("X09_WalkShot_1beg", 0, fTime);
                }
                else
                {
                    Debug.Log("Don't know!");
                }
            }
            else
            {
                // 완전히 차지된 경우
                if (_fullyCharged)
                {
                    // ChargeShot 애니메이션을 재생합니다.
                    _animator.Play(AniName_ChargeShot, 0, 0);
                    _fullyCharged = false;
                }
                else
                {
                    // Shot 애니메이션을 재생합니다.
                    _animator.Play(AniName_Shot, 0, 0);
                }
            }
            _renderer.color = PlayerColor = Color.white;


            // 스위치를 올립니다.
            endLight = true;
        }

        // 
        yield break;
    }


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
        float startTime = 0;
        while (_chargeTime >= 0)
        {
            if (_chargeTime < chargeLevel[1])
            {

            }
            else
            {
                int cTime = (int)(startTime * 10) % 3;
                if (cTime != 0)
                {
                    PlayerColor = (_chargeTime < chargeLevel[2]) ?
                        Color.cyan : Color.green;
                }
                else
                {
                    PlayerColor = Color.white;
                }

                startTime += Time.fixedDeltaTime;
            }
            yield return null;
        }

        yield return null;
    }


    /// <summary>
    /// 대쉬 코루틴 필드입니다.
    /// </summary>
    Coroutine _dashCoroutine = null;
    /// <summary>
    /// 대쉬 코루틴입니다.
    /// </summary>
    /// <returns>행동 단위가 끝날 때마다 null을 반환합니다.</returns>
    IEnumerator DashCoroutine()
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

        // DashEnd
        if (DashJumping == false)
        {
            StopDashing();
            StopAirDashing();
            StopMoving();
            SoundEffects[3].Stop();
            SoundEffects[4].Play();
        }

        // 코루틴을 중지합니다.
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
    IEnumerator AirDashCoroutine()
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
    IEnumerator SlideCoroutine()
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
    IEnumerator WallJumpCoroutine()
    {
        // WallJumpBeg
        {
            SoundEffects[5].Play();
        }

        // WallJumpEnd
        {
            // UnblockSliding();
            // _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
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
    /// <summary>
    /// 플레이어를 왼쪽으로 이동합니다.
    /// </summary>
    protected override void MoveLeft()
    {
        base.MoveLeft();

        /**
        // 코루틴을 시작합니다.
        if (_moveCoroutine == null)
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }
        */
    }
    /// <summary>
    /// 플레이어를 오른쪽으로 이동합니다.
    /// </summary>
    protected override void MoveRight()
    {
        base.MoveRight();

        /**
        // 코루틴을 시작합니다.
        if (_moveCoroutine == null)
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }
        */
    }
    /// <summary>
    /// 플레이어의 이동을 중지합니다.
    /// </summary>
    protected override void StopMoving()
    {
        base.StopMoving();

        /**
        // 코루틴을 중지합니다.
        StopCoroutine(_moveCoroutine);
        */
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

        /**
        // 코루틴을 시작합니다.
        _jumpCoroutine = StartCoroutine(JumpCoroutine());
        */
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void StopJumping()
    {
        base.StopJumping();

        /**
        // 코루틴을 중지합니다.
        StopCoroutine(_jumpCoroutine);
        */
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
        _dashCoroutine = StartCoroutine(DashCoroutine());
    }
    /// <summary>
    /// 플레이어의 대쉬를 중지합니다. (사용자의 입력에 의함)
    /// </summary>
    protected override void StopDashing()
    {
        base.StopDashing();


        // 대쉬 이펙트를 제거합니다.
        if (_dashBoostEffect != null)
        {
            _dashBoostEffect.GetComponent<EffectScript>().RequestEnd();
            _dashBoostEffect = null;
        }


        // 코루틴을 중지합니다.
        if (_dashCoroutine != null)
        {
            StopCoroutine(_dashCoroutine);
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
        _slideCoroutine = StartCoroutine(SlideCoroutine());
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
        _wallJumpCoroutine = StartCoroutine(WallJumpCoroutine());
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
        _airDashCoroutine = StartCoroutine(AirDashCoroutine());
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
        _wallJumpCoroutine = StartCoroutine(WallJumpCoroutine());
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
    }
    /// <summary>
    /// 플레이어가 사망합니다.
    /// </summary>
    protected override void Dead()
    {
        base.Dead();


        // 사망 시 입자가 퍼지는 효과를 요청합니다.
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


        // 플레이어가 생존해있다면
        if (IsAlive())
        {
            Voices[4].Play();
            SoundEffects[11].Play();
        }

        // END_HURT_LENGTH 시간 후에 대미지를 입은 상태를 종료합니다.
        Invoke("EndHurt", INVENCIBLE_TIME);
    }
    /// <summary>
    /// 대미지 상태를 해제합니다.
    /// </summary>
    protected override void EndHurt()
    {
        base.EndHurt();


        // 위험한 상태인데 위험 상태 경고 보이스를 재생하지 않았다면 재생합니다.
        if (Danger && _dangerVoicePlayed == false)
        {
            Voices[6].Play();
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


    /// <summary>
    /// 
    /// </summary>
    void UpdateAnimator()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    void UpdateAnimator2()
    {
        /*
        if (Shooting)
        {

        }
        else
        {
            if (AirDashing)
            {
                if (IsAnimationPlaying("X07_Dash_1beg")
                    || IsAnimationPlaying("X07_Dash_2run")
                    || IsAnimationPlaying("X07_Dash_3end"))
                {

                }
                else
                {
                    _animator.Play("X07_Dash_2run");
                }
            }
            else if (Sliding)
            {

            }
            else if (Dashing)
            {
                _animator.Play("");
            }
            else if (Falling)
            {
                if (Landed)
                {
                    _animator.Play(AniName_Fall_3end);
                }
                else if (IsAnimationPlaying(AniName_Fall_2run))
                {

                }
                else if (IsAnimationPlaying(AniName_Fall_1beg)
                    && GetCurrentAnimationPlaytime() >= 1)
                {
                    _animator.Play(AniName_Fall_2run);
                }
                else if (IsAnimationPlaying(AniName_Fall_1beg) == false)
                {
                    _animator.Play(AniName_Fall_1beg);
                }
            }
            else if (Jumping)
            {
                if (IsAnimationPlaying(AniName_Jump_2run))
                {

                }
                else if (IsAnimationPlaying(AniName_Jump_1beg)
                    && GetCurrentAnimationPlaytime() >= 1)
                {
                    _animator.Play(AniName_Jump_2run);
                }
                else if (IsAnimationPlaying(AniName_Jump_1beg) == false)
                {
                    _animator.Play(AniName_Jump_1beg);
                }
            }
            else if (Moving)
            {
                if (IsAnimationPlaying(AniName_Walk_2run))
                {

                }
                else if (IsAnimationPlaying(AniName_Walk_1beg)
                    && GetCurrentAnimationPlaytime() >= 1)
                {
                    _animator.Play(AniName_Walk_2run);
                }
                else if (IsAnimationPlaying(AniName_Walk_1beg) == false)
                {
                    _animator.Play(AniName_Walk_1beg);
                }
            }
            else
            {
                if (Danger)
                {
                    if (IsAnimationPlaying(AniName_Danger) == false
                        && GetCurrentAnimationPlaytime() >= 1)
                    {
                        _animator.Play(AniName_Danger);
                    }
                }
                else
                {
                    if (IsAnimationPlaying(AniName_Idle) == false
                        && GetCurrentAnimationPlaytime() >= 1)
                    {
                        _animator.Play(AniName_Idle);
                    }
                }
            }
        }
        */
    }


    #endregion










    #region 구형 정의를 보관합니다.
    /// <summary>
    /// 
    /// </summary>
//    Coroutine _jumpCoroutine = null;
    [Obsolete("")]
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpCoroutine()
    {


        yield break;
    }


    /// <summary>
    /// 
    /// </summary>
    Coroutine _moveCoroutine = null;
    [Obsolete("")]
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveCoroutine()
    {
        /*
        // 이미 걷기 애니메이션이 재생중이라면 무시합니다.
        if (IsAnimationPlaying(AniName_Walk_1beg) || IsAnimationPlaying(AniName_Walk_2run))
            yield break;

        
        // 애니메이션 재생을 시작합니다.
        _animator.Play(AniName_Walk_1beg);

        // 첫 번째 애니메이션 재생이 끝날 때까지 코루틴은 대기합니다.
        yield return new WaitForSeconds(GetCurrentAnimationLength());


        // 첫 번째 애니메이션 재생이 끝나면 두 번째 애니메이션으로 교체합니다.
        _animator.Play(AniName_Walk_2run);
        /**
        if (Moving && !Dashing && !Jumping && !Sliding)
        {
            _animator.Play(AniName_Walk_1beg, 0, 0);
            Debug.Log(AniName_Walk_1beg + " playing");
        }
        */


        // 코루틴을 종료합니다.

        _moveCoroutine = null;
        yield break;
    }
    [Obsolete("")]
    /// <summary>
    /// 
    /// </summary>
    void StopMoveCoroutine()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
    }


    #endregion
}
