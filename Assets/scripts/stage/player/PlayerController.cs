using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 일반 플레이어 컨트롤러입니다.
/// </summary>
public abstract class PlayerController : MonoBehaviour
{
    #region 상수를 정의합니다.
    /// <summary>
    /// 벽 점프가 종료되는 시간입니다.
    /// </summary>
    protected const float WALLJUMP_END_TIME = 0.138888f;


    #endregion



    #region 컨트롤러가 사용할 Unity 객체에 대한 접근자를 정의합니다.
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
    /// Collider2D 요소를 가져옵니다.
    /// </summary>
    protected Collider2D _Collider
    {
        get { return GetComponent<Collider2D>(); }
    }
    /// <summary>
    /// SpriteRenderer 요소를 가져옵니다.
    /// </summary>
    protected SpriteRenderer _Renderer
    {
        get { return GetComponent<SpriteRenderer>(); }
    }


    #endregion










    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 데이터베이스 객체입니다.
    /// </summary>
    public DataBase _database;
    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    public StageManager stageManager;


    /// <summary>
    /// 캐릭터 음성 집합입니다.
    /// </summary>
    public AudioClip[] voiceClips;
    /// <summary>
    /// 캐릭터 효과음 집합입니다.
    /// </summary>
    public AudioClip[] audioClips;


    /// <summary>
    /// 바닥 검사를 위한 위치 객체입니다.
    /// </summary>
    public Transform groundCheck;
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
    /// 무엇이 지형인지를 나타내는 마스크입니다. 기본값은 ""입니다.
    /// </summary>
    public LayerMask whatIsGround;


    /// <summary>
    /// 벽 검사를 위한 충돌체입니다.
    /// </summary>
    public BoxCollider2D pushCheckBox;
    /// <summary>
    /// 벽 검사 범위를 표현하는 실수입니다.
    /// </summary>
    public float pushCheckRadius = 0.1f;
    /// <summary>
    /// 무엇이 벽인지를 나타내는 마스크입니다. 기본값은 "Wall"입니다.
    /// </summary>
    public LayerMask whatIsWall;


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
    /// 벽에서 미끄러지는 속도입니다.
    /// </summary>
    public float _slideSpeed = 4f;
    /// <summary>
    /// 소환 시에 하늘에서 내려올 때의 속도입니다.
    /// </summary>
    public float _spawnSpeed = 16;
    /// <summary>
    /// 대쉬 속도입니다.
    /// </summary>
    public float _dashSpeed = 12;


    /// <summary>
    /// 큰 대미지를 받은 것으로 인정되는 값입니다.
    /// </summary>
    public int BigDamageValue = 10;


    /// <summary>
    /// PlayerController 객체가 사용할 효과 집합입니다.
    /// </summary>
    public GameObject[] effects;
    /// <summary>
    /// 대쉬 연기가 발생하는 위치입니다.
    /// </summary>
    public Transform dashFogPosition;
    /// <summary>
    /// 대쉬 부스터가 발생하는 위치입니다.
    /// </summary>
    public Transform dashBoostPosition;
    /// <summary>
    /// 벽 타기 시 연기가 발생하는 위치입니다.
    /// </summary>
    public Transform slideFogPosition;


    #endregion










    #region Unity를 통해 초기화한 속성을 사용 가능한 형태로 보관합니다.
    /// <summary>
    /// 목소리의 리스트 필드입니다.
    /// </summary>
    AudioSource[] voices;
    /// <summary>
    /// 목소리의 리스트입니다.
    /// </summary>
    public AudioSource[] Voices { get { return voices; } }
    /// <summary>
    /// 효과음의 리스트 필드입니다.
    /// </summary>
    AudioSource[] soundEffects;
    /// <summary>
    /// 효과음의 리스트입니다.
    /// </summary>
    public AudioSource[] SoundEffects { get { return soundEffects; } }


    /// <summary>
    /// 벽 타기 시 연기 효과 객체입니다.
    /// </summary>
    protected GameObject _slideFogEffect;


    #endregion










    #region 상태 또는 입력을 확인하는 보조 메서드를 정의합니다.
    /// <summary>
    /// 키가 눌렸는지 확인합니다.
    /// </summary>
    /// <param name="axisName">상태를 확인할 키의 이름입니다.</param>
    /// <returns>키가 눌렸다면 true를 반환합니다.</returns>
    protected bool IsKeyDown(string axisName)
    {
        return (InputBlocked == false && Input.GetButtonDown(axisName));
    }
    /// <summary>
    /// 키가 계속 눌린 상태인지 확인합니다.
    /// </summary>
    /// <param name="axisName">눌린 상태인지 확인할 키의 이름입니다.</param>
    /// <returns>키가 눌린 상태라면 true를 반환합니다.</returns>
    protected bool IsKeyPressed(string axisName)
    {
        return (InputBlocked == false && Input.GetButton(axisName));
    }
    /// <summary>
    /// 왼쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>왼쪽 키가 눌려있다면 참입니다.</returns>
    protected bool IsLeftKeyPressed()
    {
        /// return (InputBlocked == false && (Input.GetAxisRaw("Horizontal") < 0));
        return (InputBlocked == false && Input.GetKey(KeyCode.LeftArrow));
    }
    /// <summary>
    /// 오른쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>오른쪽 키가 눌려있다면 참입니다.</returns>
    protected bool IsRightKeyPressed()
    {
        /// return (InputBlocked == false && (Input.GetAxisRaw("Horizontal") > 0));
        return (InputBlocked == false && Input.GetKey(KeyCode.RightArrow));
    }
    /// <summary>
    /// 위쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>위쪽 키가 눌려있다면 참입니다.</returns>
    protected bool IsUpKeyPressed()
    {
        /// return (InputBlocked == false && (Input.GetAxisRaw("Vertical") > 0));
        return (InputBlocked == false && Input.GetKey(KeyCode.UpArrow));
    }
    /// <summary>
    /// 아래쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>아래쪽 키가 눌려있다면 참입니다.</returns>
    protected bool IsDownKeyPressed()
    {
        /// return (InputBlocked == false && (Input.GetAxisRaw("Vertical") < 0));
        return (InputBlocked == false && Input.GetKey(KeyCode.DownArrow));
    }


    #endregion










    #region 주인공의 게임 상태 필드를 정의합니다.
    /// <summary>
    /// 플레이어의 현재 체력을 확인합니다.
    /// </summary>
    public int _health = 40;
    /// <summary>
    /// 플레이어의 최대 체력을 확인합니다.
    /// </summary>
    public int _maxHealth = 40;
    /// <summary>
    /// 위험 상태로 바뀌는 체력의 값입니다.
    /// </summary>
    public int _dangerHealth = 10;


    /// <summary>
    /// 플레이어의 현재 체력을 확인합니다.
    /// </summary>
    public int Health
    {
        get { return _health; }
        private set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
        }
    }
    /// <summary>
    /// 플레이어의 최대 체력을 확인합니다.
    /// </summary>
    public int MaxHealth
    {
        get { return _maxHealth; }
        private set { _maxHealth = value; }
    }
    /// <summary>
    /// 위험 상태로 바뀌는 체력의 값입니다.
    /// </summary>
    public int DangerHealth
    {
        get { return _dangerHealth; }
    }


    #endregion










    #region 주인공의 운동 상태 필드를 정의합니다.
    /// <summary>
    /// 플레이어가 걷는 속도입니다.
    /// </summary>
    protected float _movingSpeed = 5;
    /// <summary>
    /// 플레이어가 오른쪽을 보고 있다면 참입니다.
    /// </summary>
    bool _facingRight = true;


    /// <summary>
    /// 이동 요청을 받았다면 참입니다.
    /// </summary>
    bool _moveRequested = false;


    /// <summary>
    /// 플레이어가 소환중이라면 참입니다.
    /// </summary>
    bool _spawning = true;
    /// <summary>
    /// 플레이어가 준비중이라면 true입니다.
    /// </summary>
    bool _readying = false;
    /// <summary>
    /// 지상에 있다면 true입니다.
    /// </summary>
    bool _landed = false;
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
    /// 벽을 타고 있다면 true입니다.
    /// </summary>
    bool _sliding = false;
    /// <summary>
    /// 에어 대쉬중이라면 참입니다.
    /// </summary>
    bool _airDashing = false;


    /// <summary>
    /// 대미지를 입었다면 true입니다.
    /// </summary>
    bool _damaged = false;
    /// <summary>
    /// 무적 상태라면 true입니다.
    /// </summary>
    bool _invencible = false;
    /// <summary>
    /// 진행된 무적 시간을 반환합니다.
    /// </summary>
    float _invencibleTime;
    /// <summary>
    /// 플레이어의 색상을 반환합니다.
    /// </summary>
    Color _playerColor = Color.white;
    /// <summary>
    /// 위험 상태라면 true입니다.
    /// </summary>
    bool _danger = false;
    /// <summary>
    /// 플레이어가 죽었다면 true입니다.
    /// </summary>
    bool _isDead = false;


    /// <summary>
    /// 
    /// </summary>
    float _dashAfterImageTime = 0;
    /// <summary>
    /// 
    /// </summary>
    float _dashAfterImageInterval = 0.05f;
    /// <summary>
    /// 
    /// </summary>
    protected float DashAfterImageTime
    {
        get { return _dashAfterImageTime; }
        set { _dashAfterImageTime = value; }
    }
    /// <summary>
    /// 
    /// </summary>
    protected float DashAfterImageInterval
    {
        get { return _dashAfterImageInterval; }
        // set { _dashAfterImageInterval = value; }
    }


    /// <summary>
    /// 현재 플레이어와 닿아있는 땅 지형의 집합입니다.
    /// </summary>
    HashSet<EdgeCollider2D> groundEdgeSet = new HashSet<EdgeCollider2D>();


    #endregion










    #region 플레이어의 상태에 관한 프로퍼티 및 메서드를 정의합니다.
    /// <summary>
    /// 플레이어가 오른쪽을 향하고 있다면 true입니다.
    /// </summary>
    public bool FacingRight
    {
        get { return _facingRight; }
        private set { _facingRight = value; }
    }
    /// <summary>
    /// 플레이어가 소환중이라면 true입니다.
    /// </summary>
    protected bool Spawning
    {
        get { return _spawning; }
        private set { _Animator.SetBool("Spawning", _spawning = value); }
    }
    /// <summary>
    /// 플레이어가 준비중이라면 true입니다.
    /// </summary>
    protected bool Readying
    {
        get { return _readying; }
        private set { _readying = value; }
    }
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
    /// 벽을 타고 있다면 true입니다.
    /// </summary>
    protected bool Sliding
    {
        get { return _sliding; }
        set { _Animator.SetBool("Sliding", _sliding = value); }
    }


    /// <summary>
    /// 이동이 막혀있다면 true입니다.
    /// </summary>
    protected bool MoveBlocked { get; set; }
    /// <summary>
    /// 점프가 막혀있다면 true입니다.
    /// </summary>
    protected bool JumpBlocked { get; set; }
    /// <summary>
    /// 대쉬가 막혀있다면 true입니다.
    /// </summary>
    protected bool DashBlocked { get; set; }
    /// <summary>
    /// 벽 타기가 막혀있다면 true입니다.
    /// </summary>
    protected bool SlideBlocked { get; set; }
    /// <summary>
    /// 에어 대쉬가 막혀있다면 true입니다.
    /// </summary>
    protected bool AirDashBlocked { get; set; }
    /// <summary>
    /// 플레이어의 입력이 막혀있다면 true입니다.
    /// </summary>
    protected bool InputBlocked { get; set; }


    /// <summary>
    /// 대쉬 점프중이라면 참입니다.
    /// </summary>
    protected bool DashJumping { get; set; }
    /// <summary>
    /// 벽 점프중이라면 참입니다.
    /// </summary>
    protected bool WallJumping { get; set; }
    /// <summary>
    /// 벽 대쉬 점프중이라면 참입니다.
    /// </summary>
    protected bool WallDashJumping { get; set; }
    /// <summary>
    /// 에어 대쉬중이라면 참입니다.
    /// </summary>
    protected bool AirDashing
    {
        get { return _airDashing; }
        set { _Animator.SetBool("AirDashing", _airDashing = value); }
    }


    /// <summary>
    /// 대미지를 입었다면 true입니다.
    /// </summary>
    public bool Damaged
    {
        get { return _damaged; }
        protected set
        {
            _Animator.SetBool("Damaged", _damaged = value);
            BigDamaged = false;
        }
    }
    /// <summary>
    /// 무적 상태라면 true입니다.
    /// </summary>
    public bool Invencible
    {
        get { return _invencible; }
        protected set { _Animator.SetBool("Invencible", _invencible = value); }
    }
    /// <summary>
    /// 진행된 무적 시간을 반환합니다.
    /// </summary>
    public float InvencibleTime
    {
        get { return _invencibleTime; }
        protected set { _invencibleTime = value; }
    }
    /// <summary>
    /// 위험 상태라면 true입니다.
    /// </summary>
    public bool Danger
    {
        get {
            return _danger;
        }
        protected set
        {
            _Animator.SetBool("Danger", _danger = value);
            _Animator.SetFloat("DangerState", _danger ? 1 : 0);
        }
    }
    /// <summary>
    /// 플레이어가 죽었다면 true입니다.
    /// </summary>
    public bool IsDead
    {
        get { return _isDead; }
        private set { _isDead = value; }
    }


    /// <summary>
    /// 플레이어의 색상을 반환합니다.
    /// </summary>
    public Color PlayerColor
    {
        get { return _playerColor; }
        protected set { _playerColor = value; }
    }


    /// <summary>
    /// 플레이어가 생존해있는지 확인합니다.
    /// </summary>
    /// <returns>체력이 정상 범위에 있다면 true입니다.</returns>
    public bool IsAlive()
    {
        return (0 < _health);
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
    /// 이동 요청을 받았다면 참입니다.
    /// </summary>
    public bool MoveRequested
    {
        get { return _moveRequested; }
        private set { _moveRequested = value; }
    }


    /// <summary>
    /// 큰 대미지를 입었다면 참입니다.
    /// </summary>
    public bool BigDamaged {
        get { return _Animator.GetFloat("BigDamaged") == 1 ? true : false; }
        set { _Animator.SetFloat("BigDamaged", value ? 1 : 0); }
    }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected virtual void Awake()
    {
        // 목소리를 포함한 효과음의 리스트를 초기화 합니다.
        voices = new AudioSource[voiceClips.Length];
        for (int i = 0, len = voiceClips.Length; i < len; ++i)
        {
            voices[i] = gameObject.AddComponent<AudioSource>();
            voices[i].clip = voiceClips[i];
            voices[i].playOnAwake = false;
        }
        soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].clip = audioClips[i];
            soundEffects[i].playOnAwake = false;
        }

        // 자식 객체를 초기화 합니다.
        Vector2[] points = new Vector2[]
        {
            new Vector2(_Collider.bounds.max.x, _Collider.bounds.max.y),
            new Vector2(_Collider.bounds.max.x, _Collider.bounds.min.y)
        };
        points[0].x /= transform.localScale.x;
        points[0].y /= transform.localScale.y;
        points[1].x /= transform.localScale.x;
        points[1].y /= transform.localScale.y;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    protected virtual void Update()
    {

    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    protected virtual void LateUpdate()
    {
        // NOTE
        return;

        /**
        if (IsAlive() && Invencible && Damaged == false)
        {
            _Renderer.color = _playerColor;
        }
        else
        {
            _Renderer.color = Color.white;
        }
        */
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
    /// 자식 클래스의 Update()를 실행하기 전에 부모 클래스에서 Update()를 수행합니다.
    /// </summary>
    /// <returns>자식 클래스의 Update 실행을 막으려면 참을 반환합니다.</returns>
    protected virtual bool UpdateController()
    {
        // 소환 중이라면
        if (Spawning)
        {
            return false;
        }
        // 사망했다면
        else if (IsDead)
        {
            if (stageManager._fader.FadeOutEnded)
            {
                // 구형 정의를 새로운 정의로 업데이트 합니다.
                /// stageManager.RestartLevel();
                RequestRestart();
            }
            return false;
        }
        // 사망 직전이라면
        else if (IsAlive() == false)
        {
            _Rigidbody.velocity = Vector2.zero;
            return false;
        }
        // 대미지를 입은 상태라면
        else if (Damaged)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 자식 클래스의 FixedUpdate()를 실행하기 전에 부모 클래스에서 FixedUpdate()를 수행합니다.
    /// </summary>
    /// <returns>자식 클래스의 Update 실행을 막으려면 참을 반환합니다.</returns>
    protected virtual bool FixedUpdateController()
    {
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
            return false;
        }
        // 사망했다면
        else if (IsDead)
        {
            return false;
        }
        // 사망 직전이라면
        else if (IsAlive() == false)
        {
            _Rigidbody.velocity = Vector2.zero;
            return false;
        }
        // 대미지를 입은 상태라면
        else if (Damaged)
        {
            return false;
        }
        return true;
    }


    #endregion










    #region 플레이어의 상태를 갱신합니다.
    /// <summary>
    /// 플레이어의 물리 상태를 갱신합니다.
    /// </summary>
    protected void UpdateState()
    {
        UpdateLanding();
    }
    /// <summary>
    /// 플레이어가 땅과 접촉했는지에 대한 필드를 갱신합니다.
    /// </summary>
    /// <returns>플레이어가 땅에 닿아있다면 참입니다.</returns>
    bool UpdateLanding()
    {
        /**
        RaycastHit2D rayB = Physics2D.Raycast(groundCheckBack.position, Vector2.down, groundCheckRadius, whatIsGround);
        RaycastHit2D rayF = Physics2D.Raycast(groundCheckFront.position, Vector2.down, groundCheckRadius, whatIsGround);

        if (OnGround())
        {
            Landed = true;
        }
        else if (Jumping || Falling)
        {
            Landed = false;
        }
        else if (rayB || rayF)
        {
            Vector3 pos = transform.position;
            pos.y -= Mathf.Min(rayB.distance, rayF.distance);
            transform.position = pos;
            _Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, 0);
            Landed = true;
        }
        else
        {
            Landed = false;
        }
        return Landed;
        */

        RaycastHit2D rayB = Physics2D.Raycast(groundCheckBack.position, Vector2.down, groundCheckRadius, whatIsGround);
        RaycastHit2D rayF = Physics2D.Raycast(groundCheckFront.position, Vector2.down, groundCheckRadius, whatIsGround);

        if (OnGround())
        {
            Landed = true;
        }
        else if (Jumping || Falling)
        {
            Landed = false;
        }
        else if (rayB || rayF)
        {
            Vector3 pos = transform.position;
            if (rayB && !rayF)
            {
                pos.y -= rayB.distance / transform.localScale.y;
            }
            else if (!rayB && rayF)
            {
                pos.y -= rayF.distance / transform.localScale.y;
            }
            else
            {
                pos.y -= Mathf.Min(rayB.distance, rayF.distance) / transform.localScale.y;
            }
            transform.position = pos;
            _Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, 0);
            Landed = true;
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
        if (IsSameLayer(layer, whatIsGround))
        {
            EdgeCollider2D groundCollider = collision.collider as EdgeCollider2D;
            if (IsTouchingGround(groundCollider))
            {
                groundEdgeSet.Add(groundCollider);
            }
            else
            {
                groundEdgeSet.Remove(groundCollider);
            }
        }

        // 벽과 접촉한 경우의 처리입니다.
        if (IsSameLayer(layer, whatIsWall))
        {
            Pushing = IsTouchingWall(collision) && (FacingRight ?
                IsRightKeyPressed() : IsLeftKeyPressed());
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
        if (_Collider.IsTouchingLayers(whatIsGround))
        {
            float playerBottom = _Collider.bounds.min.y;
            foreach (EdgeCollider2D edge in groundEdgeSet)
            {
                float groundTop = edge.bounds.max.y;
                float groundBottom = edge.bounds.min.y;

                // 평면인 경우
                if (groundBottom == groundTop)
                {
                    if (playerBottom >= groundTop)
                    {
                        return true;
                    }
                }
                // 경사면인 경우
                else
                {
                    if (groundBottom <= playerBottom
                        && playerBottom <= groundTop)
                    {
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
        if (_Collider.IsTouchingLayers(whatIsWall))
        {
            if (pushCheckBox.IsTouchingLayers(whatIsWall))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // 벽과 닿아있지 않으면 거짓입니다.
        return false;
    }


    #endregion










    #region 일회성 행동 메서드를 정의합니다.
    /// <summary>
    /// 플레이어를 소환합니다.
    /// </summary>
    protected virtual void Spawn()
    {
        Spawning = true;
        _Rigidbody.velocity = new Vector2(0, -_spawnSpeed);
    }
    /// <summary>
    /// 플레이어를 소환 상태에서 준비 상태로 전환합니다.
    /// </summary>
    protected void Ready()
    {
        Readying = true;
    }
    /// <summary>
    /// 소환 및 준비를 완료합니다.
    /// </summary>
    protected void EndReady()
    {
        Readying = false;
        Spawning = false;
    }
    /// <summary>
    /// 플레이어가 사망합니다.
    /// </summary>
    protected virtual void Dead()
    {
        IsDead = true;
        Invoke("RequestFadeOut", 1);
    }
    /// <summary>
    /// 페이드 아웃을 요청합니다.
    /// </summary>
    void RequestFadeOut()
    {
        stageManager._fader.FadeOut(1);
        Invoke("RequestRestart", 1);
    }
    /// <summary>
    /// 재시작을 요청합니다.
    /// </summary>
    void RequestRestart()
    {
        if (_database.GameManager.GameData.TryCount == 0)
        {
            LoadingSceneManager.LoadLevel("Title");
        }
        else
        {
            _database.GameManager.RequestDecreaseTryCount();
            stageManager.RestartLevel();
        }
    }


    /// <summary>
    /// 플레이어 사망을 요청합니다.
    /// </summary>
    public virtual void RequestDead()
    {
        Health = 0;

        StopMoving();
        BlockMoving();
        _Animator.speed = 0;
        Invoke("Dead", 0.5f);
    }


    #endregion










    #region 플레이어의 행동 메서드를 정의합니다.
    ///////////////////////////////////////////////////////////////////
    // 기본
    /// <summary>
    /// 플레이어가 지상에 착륙할 때의 상태를 설정합니다.
    /// </summary>
    protected virtual void Land()
    {
        StopJumping();
        StopFalling();
        StopDashing();
        StopDashJumping();
        UnblockAirDashing();
    }
    /// <summary>
    /// 플레이어의 입력을 방지합니다.
    /// </summary>
    protected virtual void BlockInput()
    {
        InputBlocked = true;
    }
    /// <summary>
    /// 플레이어의 입력 방지를 해제합니다.
    /// </summary>
    protected virtual void UnblockInput()
    {
        InputBlocked = false;
    }


    ///////////////////////////////////////////////////////////////////
    // 이동
    /// <summary>
    /// 플레이어를 왼쪽으로 이동합니다.
    /// </summary>
    protected virtual void MoveLeft()
    {
        if (_facingRight)
            Flip();

        _Rigidbody.velocity = new Vector2(-_movingSpeed, _Rigidbody.velocity.y);
        Moving = true;
    }
    /// <summary>
    /// 플레이어를 오른쪽으로 이동합니다.
    /// </summary>
    protected virtual void MoveRight()
    {
        if (_facingRight == false)
            Flip();
        _Rigidbody.velocity = new Vector2(_movingSpeed, _Rigidbody.velocity.y);
        Moving = true;
    }
    /// <summary>
    /// 플레이어의 이동을 중지합니다.
    /// </summary>
    protected virtual void StopMoving()
    {
        _Rigidbody.velocity = new Vector2(0, _Rigidbody.velocity.y);
        Moving = false;
    }
    /// <summary>
    /// 플레이어의 이동 요청을 막습니다.
    /// </summary>
    protected virtual void BlockMoving()
    {
        MoveBlocked = true;
    }
    /// <summary>
    /// 플레이어가 이동을 요청할 수 있도록 합니다.
    /// </summary>
    protected virtual void UnblockMoving()
    {
        MoveBlocked = false;
    }


    ///////////////////////////////////////////////////////////////////
    // 점프 및 낙하
    /// <summary>
    /// 플레이어를 점프하게 합니다.
    /// </summary>
    protected virtual void Jump()
    {
        // 개체의 운동 상태를 갱신합니다.
        BlockJumping();
        BlockDashing();
        UnblockAirDashing();
        _Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, _jumpSpeed);
        // _rigidbody.velocity = new Vector2(0, jumpSpeed);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = true;
    }
    /// <summary>
    /// 플레이어의 점프를 중지합니다.
    /// </summary>
    protected virtual void StopJumping()
    {
        // 개체의 운동 상태를 갱신합니다.
        UnblockJumping();
        UnblockDashing();

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = false;
    }
    /// <summary>
    /// 플레이어의 점프 요청을 막습니다.
    /// </summary>
    protected virtual void BlockJumping()
    {
        JumpBlocked = true;
    }
    /// <summary>
    /// 플레이어가 점프할 수 있도록 합니다.
    /// </summary>
    protected virtual void UnblockJumping()
    {
        JumpBlocked = false;
    }
    /// <summary>
    /// 플레이어를 낙하시킵니다.
    /// </summary>
    protected virtual void Fall()
    {
        // 개체의 운동 상태를 갱신합니다.
        BlockJumping();
        BlockDashing();
        _Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, 0);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = false;
        Falling = true;
    }
    /// <summary>
    /// 플레이어의 낙하를 중지합니다.
    /// </summary>
    protected virtual void StopFalling()
    {
        // 개체의 운동 상태를 갱신합니다.
        UnblockJumping();
        UnblockDashing();

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Falling = false;
    }


    ///////////////////////////////////////////////////////////////////
    // 대쉬
    /// <summary>
    /// 플레이어가 대쉬하게 합니다.
    /// </summary>
    protected virtual void Dash()
    {
        // 개체의 운동 상태를 갱신합니다.
        BlockMoving();
        BlockDashing();
        BlockAirDashing();
        _movingSpeed = _dashSpeed;
        _Rigidbody.velocity = new Vector2
            (_facingRight ? _dashSpeed : -_dashSpeed, _Rigidbody.velocity.y);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Dashing = true;
    }
    /// <summary>
    /// 플레이어의 대쉬를 중지합니다. (사용자의 입력에 의함)
    /// </summary>
    protected virtual void StopDashing()
    {
        // 개체의 운동 상태를 갱신합니다.
        UnblockMoving();
        UnblockDashing();
        _movingSpeed = _walkSpeed;
        _Rigidbody.velocity = new Vector2(0, _Rigidbody.velocity.y);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Dashing = false;
    }
    /// <summary>
    /// 플레이어의 대쉬 요청을 막습니다.
    /// </summary>
    protected virtual void BlockDashing()
    {
        DashBlocked = true;
    }
    /// <summary>
    /// 플레이어가 대쉬할 수 있도록 합니다.
    /// </summary>
    protected virtual void UnblockDashing()
    {
        DashBlocked = false;
    }


    ///////////////////////////////////////////////////////////////////
    // 벽 타기
    /// <summary>
    /// 플레이어가 벽을 타도록 합니다.
    /// </summary>
    protected virtual void Slide()
    {
        // 개체의 운동 상태를 갱신합니다.
        StopMoving();
        StopJumping();
        StopFalling();
        StopDashing();
        StopAirDashing();
        BlockDashing();
        UnblockJumping();
        UnblockAirDashing();
        _Rigidbody.velocity = new Vector2(0, -_slideSpeed);

        // 개체의 운동에 따른 효과를 처리합니다.
        GameObject slideFog = CloneObject(effects[2], slideFogPosition);
        slideFog.transform.SetParent(groundCheck.transform);
        if (FacingRight == false)
        {
            var newScale = slideFog.transform.localScale;
            newScale.x = FacingRight ? newScale.x : -newScale.x;
            slideFog.transform.localScale = newScale;
        }
        _slideFogEffect = slideFog;

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Sliding = true;
    }
    /// <summary>
    /// 플레이어의 벽 타기를 중지합니다.
    /// </summary>
    protected virtual void StopSliding()
    {
        // 개체의 운동 상태를 갱신합니다.
        UnblockDashing();
        _Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, 0);

        // 개체의 운동에 따른 효과를 처리합니다.
        if (_slideFogEffect != null)
        {
            _slideFogEffect.transform.SetParent(null);
            _slideFogEffect.GetComponent<EffectScript>().RequestEnd();
            _slideFogEffect = null;
        }

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Sliding = false;
    }
    /// <summary>
    /// 플레이어의 벽 타기 요청을 막습니다.
    /// </summary>
    protected virtual void BlockSliding()
    {
        SlideBlocked = true;
    }
    /// <summary>
    /// 플레이어가 벽 타기할 수 있도록 합니다.
    /// </summary>
    protected virtual void UnblockSliding()
    {
        SlideBlocked = false;
    }


    ///////////////////////////////////////////////////////////////////
    // 사다리 타기
    /// <summary>
    /// 플레이어가 사다리를 타도록 합니다.
    /// </summary>
    protected virtual void RideLadder()
    {
        UnblockAirDashing(); // ClearAirDashCount();
    }


    ///////////////////////////////////////////////////////////////////
    // 조합
    /// <summary>
    /// 플레이어가 벽 점프를 합니다.
    /// </summary>
    protected virtual void WallJump()
    {
        // 개체의 운동 상태를 갱신합니다.
        StopJumping();
        StopFalling();
        StopSliding();
        BlockJumping();
        BlockSliding();
        BlockDashing();
        UnblockAirDashing();
        _Rigidbody.velocity = new Vector2(FacingRight ? -1.5f * _movingSpeed : 1.5f * _movingSpeed, _jumpSpeed);

        // 개체의 운동에 따른 효과를 처리합니다.
        CloneObject(effects[3], slideFogPosition);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = true;

        // 일정 시간 후에 개체의 운동 중단을 요청하는 메서드를 호출합니다.
        Invoke("StopWallJumping", WALLJUMP_END_TIME);
    }
    /// <summary>
    /// 플레이어가 대쉬 점프하게 합니다.
    /// </summary>
    protected virtual void DashJump()
    {
        // 개체의 운동 상태를 갱신합니다.
        StopJumping();
        StopFalling();
        StopSliding();
        BlockDashing();
        BlockJumping();
        BlockAirDashing();
        _movingSpeed = _dashSpeed;
        _Rigidbody.velocity = new Vector2(0, _jumpSpeed);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = true;
        Dashing = true;
        DashJumping = true;
    }
    /// <summary>
    /// 플레이어의 대쉬 점프를 중지합니다.
    /// </summary>
    protected virtual void StopDashJumping()
    {
        Jumping = false;
        Dashing = false;
        DashJumping = false;
    }
    /// <summary>
    /// 플레이어가 벽에서 대쉬 점프하게 합니다.
    /// </summary>
    protected virtual void WallDashJump()
    {
        // 개체의 운동 상태를 갱신합니다.
        StopSliding();
        BlockJumping();
        BlockSliding();
        BlockAirDashing();
        _movingSpeed = _dashSpeed;
        _Rigidbody.velocity = new Vector2
            (FacingRight ? -1.5f * _movingSpeed : 1.5f * _movingSpeed, _jumpSpeed);

        // 개체의 운동에 따른 효과를 처리합니다.
        CloneObject(effects[3], slideFogPosition);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = true;
        Dashing = true;
        DashJumping = true;

        // 일정 시간 후에 개체의 운동 중단을 요청하는 메서드를 호출합니다.
        Invoke("StopWallJumping", WALLJUMP_END_TIME);
    }
    /// <summary>
    /// 플레이어가 에어 대쉬하게 합니다.
    /// </summary>
    protected virtual void AirDash()
    {
        // 개체의 운동 상태를 갱신합니다.
        StopJumping();
        StopFalling();
        StopSliding();
        BlockMoving();
        BlockAirDashing();
        BlockJumping();
        _Rigidbody.velocity = new Vector2(FacingRight ? _dashSpeed : -_dashSpeed, 0);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Dashing = true;
        AirDashing = true;
    }
    /// <summary>
    /// 플레이어의 에어 대쉬를 중지합니다.
    /// </summary>
    protected virtual void StopAirDashing()
    {
        // 개체의 운동 상태를 갱신합니다.
        UnblockMoving();

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Dashing = false;
        AirDashing = false;
    }
    /// <summary>
    /// 플레이어의 에어 대쉬 요청을 막습니다.
    /// </summary>
    protected virtual void BlockAirDashing()
    {
        AirDashBlocked = true; // airDashCount = 1;
    }
    /// <summary>
    /// 플레이어가 에어 대쉬할 수 있도록 합니다.
    /// </summary>
    protected virtual void UnblockAirDashing()
    {
        AirDashBlocked = false; // ClearAirDashCount();
    }
    /// <summary>
    /// 플레이어의 벽 점프를 중지합니다.
    /// </summary>
    protected virtual void StopWallJumping()
    {
        UnblockSliding();
        _Rigidbody.velocity = new Vector2(0, _Rigidbody.velocity.y);
    }


    #endregion










    #region 플레이어의 상태 메서드를 정의합니다.
    /// <summary>
    /// 플레이어가 체력을 회복합니다.
    /// </summary>
    public void Heal()
    {
        Health++;
        if (_health > _dangerHealth)
        {
            Danger = false;
        }
    }
    /// <summary>
    /// 플레이어의 최대 체력을 증가시킵니다.
    /// </summary>
    public void IncreaseMaxHealth()
    {
        MaxHealth++;
    }


    /// <summary>
    /// 플레이어가 대미지를 입습니다.
    /// </summary>
    /// <param name="point">플레이어가 입을 대미지입니다.</param>
    public virtual void Hurt(int point)
    {
        // 체력을 깎습니다.
        Health -= point;
        if (IsAlive() == false)
        {
            RequestDead();
        }
        else if (_health <= _dangerHealth)
        {
            Danger = true;
        }


        // 상태를 업데이트합니다.
        Damaged = true;
        if (point >= BigDamageValue)
        {
            BigDamaged = true;
        }
        Invencible = true;
        InputBlocked = true;
        StopMoving();
        StopDashing();
        StopJumping();
        StopFalling();


        // 플레이어에 대해 넉백 효과를 겁니다.
        if (BigDamaged && IsAlive())
        {
            Vector2 force = (FacingRight ? Vector2.left : Vector2.right) * KnockbackSpeed;
            _Rigidbody.velocity = new Vector2(force.x, KnockbackJumpSize);
            StartCoroutine(CoroutineKnockback());
        }
        else
        {
            float speed = 100;
            Vector2 force = (FacingRight ? Vector2.left : Vector2.right);
            _Rigidbody.velocity = Vector2.zero;
            _Rigidbody.AddForce(force * speed);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineKnockback()
    {
        float timer = 0;
        const float maxTime = 0.361112f;

        while (maxTime > timer)
        {
            timer += Time.deltaTime;
            _Rigidbody.AddForce(Vector2.down * KnockbackJumpDecSize);
            yield return new WaitForFixedUpdate();
        }

        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    public float KnockbackSpeed = 3;
    /// <summary>
    /// 
    /// </summary>
    public float KnockbackJumpSize = 5;
    /// <summary>
    /// 
    /// </summary>
    public float KnockbackJumpDecSize = 40;


    /// <summary>
    /// 대미지 상태를 해제합니다.
    /// </summary>
    protected virtual void EndHurt()
    {
        Damaged = false;
        InputBlocked = false;
        StartCoroutine(CoroutineInvencible());
    }
    /// <summary>
    /// 무적 상태에 대한 코루틴입니다.
    /// </summary>
    /// <returns>코루틴 열거자입니다.</returns>
    IEnumerator CoroutineInvencible()
    {
        InvencibleTime = 0;
        while (InvencibleTime < 1)
        {
            InvencibleTime += Time.deltaTime;
            if ((int)(InvencibleTime * 10) % 2 == 0)
            {
                _playerColor = Color.clear;
            }
            else
            {
                _playerColor = Color.white;
            }
            yield return false;
        }
        Invencible = false;
        _playerColor = Color.white;
        yield return true;
    }


    #endregion









    #region 외부에서 요청 가능한 행동 메서드를 정의합니다.
    /// <summary>
    /// 플레이어 소환을 요청합니다.
    /// </summary>
    public void RequestSpawn()
    {
        gameObject.SetActive(true);
        Spawn();
    }
    /// <summary>
    /// 플레이어의 방향 전환을 요청합니다.
    /// </summary>
    public void RequestFlip()
    {
        Flip();
    }
    /// <summary>
    /// 플레이어의 입력 방지를 요청합니다.
    /// </summary>
    public void RequestBlockInput()
    {
        BlockInput();
    }
    /// <summary>
    /// 플레이어의 입력 방지 중지를 요청합니다.
    /// </summary>
    public void RequestUnblockInput()
    {
        UnblockInput();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="speed"></param>
    public void RequestChangeMovingSpeed(float speed)
    {
        _movingSpeed = speed;
    }
    /// <summary>
    /// 
    /// </summary>
    public void RequestMoveLeft()
    {
        MoveRequested = true;
        MoveLeft();
    }
    /// <summary>
    /// 
    /// </summary>
    public void RequestMoveRight()
    {
        MoveRequested = true;
        MoveRight();
    }
    /// <summary>
    /// 
    /// </summary>
    public void RequestStopMoving()
    {
        MoveRequested = false;
    }







    /// <summary>
    /// 테스트 코드에요.
    /// </summary>
    public bool Returning
    {
        get; protected set;
    }
    /// <summary>
    /// 테스트 코드랍니다.
    /// </summary>
    public void RequestReturn()
    {
        _Animator.Play("X20_Win");
    }


    /// <summary>
    /// 삭제하자
    /// </summary>
    void ________TESTEST()
    {
        Returning = true;
    }


    #endregion









    #region 요청을 수행하기 위한 보조 메서드를 정의합니다.
    /// <summary>
    /// 플레이어의 방향을 바꿉니다.
    /// </summary>
    protected void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    /// <summary>
    /// 현재 재생중인 애니메이션의 길이를 얻습니다.
    /// </summary>
    /// <returns>현재 재생중인 애니메이션의 길이입니다.</returns>
    protected float GetCurrentAnimationLength()
    {
        return _Animator.GetCurrentAnimatorStateInfo(0).length;
    }
    /// <summary>
    /// 현재 재생중인 애니메이션의 재생된 시간을 얻습니다.
    /// </summary>
    /// <returns>현재 재생중인 애니메이션의 재생된 시간입니다.</returns>
    protected float GetCurrentAnimationPlaytime()
    {
        return _Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
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
    /// 객체를 복제합니다.
    /// </summary>
    /// <param name="gObject">복제할 객체의 원본입니다.</param>
    /// <param name="transform">복제된 객체가 위치할 곳입니다.</param>
    /// <returns>Instantiate로 복제한 객체를 GameObject 형식으로 반환합니다.</returns>
    public static GameObject CloneObject(GameObject gObject, Transform transform)
    {
        return Instantiate
            (gObject, transform.position, transform.rotation) as GameObject;
    }


    #endregion










    #region 구형 정의를 보관합니다.



    #endregion
}