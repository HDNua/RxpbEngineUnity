using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class PlayerController : MonoBehaviour
{
    #region 컨트롤러가 사용할 공용 형식 또는 값을 정의합니다.

    #endregion



    #region 컨트롤러가 사용할 Unity 객체에 대한 접근자를 정의합니다.
    protected Rigidbody2D _rigidbody
    {
        get { return GetComponent<Rigidbody2D>(); }
    }
    protected Animator _animator
    {
        get { return GetComponent<Animator>(); }
    }
    protected Collider2D _collider
    {
        get { return GetComponent<Collider2D>(); }
    }
    protected SpriteRenderer _renderer
    {
        get { return GetComponent<SpriteRenderer>(); }
    }

    #endregion



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public StageManager stageManager;

    public AudioClip[] voiceClips;
    public AudioClip[] audioClips;

    public Transform groundCheck;
    public Transform groundCheckBack;
    public Transform groundCheckFront;
    public float groundCheckRadius = 0.1f;
    public LayerMask whatIsGround;
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

    [Obsolete("", true)]
    public Transform pushCheck;
    [Obsolete("", true)]
    public float pushCheckRadius = 0.1f;
    [Obsolete("", true)]
    public EdgeCollider2D groundCheckEdge;
    [Obsolete("", true)]
    public EdgeCollider2D pushCheckEdge;

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
        return (InputBlocked == false && (Input.GetAxisRaw("Horizontal") < 0));
    }
    /// <summary>
    /// 오른쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>오른쪽 키가 눌려있다면 참입니다.</returns>
    protected bool IsRightKeyPressed()
    {
        return (InputBlocked == false && (Input.GetAxisRaw("Horizontal") > 0));
    }
    /// <summary>
    /// 위쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>위쪽 키가 눌려있다면 참입니다.</returns>
    protected bool IsUpKeyPressed()
    {
        return (InputBlocked == false && (Input.GetAxisRaw("Vertical") > 0));
    }
    /// <summary>
    /// 아래쪽 키가 눌려있는지 확인합니다.
    /// </summary>
    /// <returns>아래쪽 키가 눌려있다면 참입니다.</returns>
    protected bool IsDownKeyPressed()
    {
        return (InputBlocked == false && (Input.GetAxisRaw("Vertical") < 0));
    }

    #endregion



    #region 주인공의 게임 상태 필드를 정의합니다.
    int health = 20;
    int maxHealth = 20;
    int dangerHealth = 6;

    /// <summary>
    /// 플레이어의 현재 체력을 확인합니다.
    /// </summary>
    public int Health
    {
        get { return health; }
        private set { health = value; }
    }
    /// <summary>
    /// 플레이어의 최대 체력을 확인합니다.
    /// </summary>
    public int MaxHealth
    {
        get { return maxHealth; }
        private set { maxHealth = value; }
    }
    /// <summary>
    /// 위험 상태로 바뀌는 체력의 값입니다.
    /// </summary>
    public int DangerHealth { get { return dangerHealth; } }

    #endregion



    #region 주인공의 운동 상태 필드를 정의합니다.
    protected float movingSpeed = 5;
    bool _facingRight = true;
    bool _spawning = true;
    bool _readying = false;
    bool _landed = false;
    bool _moving = false;
    bool _pushing = false;
    bool _jumping = false;
    bool _falling = false;
    bool _dashing = false;
    bool _sliding = false;
    bool _airDashing = false;

    bool _damaged = false;
    bool _invencible = false;
    float _invencibleTime;

    bool _danger = false;
    bool _isDead = false;

    float _dashAfterImageTime = 0;
    float _dashAfterImageInterval = 0.05f;
    protected float DashAfterImageTime
    {
        get { return _dashAfterImageTime; }
        set { _dashAfterImageTime = value; }
    }
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
        private set { _animator.SetBool("Spawning", _spawning = value); }
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
    protected bool Landed
    {
        get { return _landed; }
        set { _animator.SetBool("Landed", _landed = value); }
    }
    /// <summary>
    /// 지상에서 이동하고 있다면 true입니다.
    /// </summary>
    protected bool Moving
    {
        get { return _moving; }
        set { _animator.SetBool("Moving", _moving = value); }
    }
    /// <summary>
    /// 벽을 밀고 있다면 true입니다.
    /// </summary>
    protected bool Pushing
    {
        get { return _pushing; }
        set { _animator.SetBool("Pushing", _pushing = value); }
    }
    /// <summary>
    /// 점프 상태라면 true입니다.
    /// </summary>
    protected bool Jumping
    {
        get { return _jumping; }
        set { _animator.SetBool("Jumping", _jumping = value); }
    }
    /// <summary>
    /// 떨어지고 있다면 true입니다.
    /// </summary>
    protected bool Falling
    {
        get { return _falling; }
        set { _animator.SetBool("Falling", _falling = value); }
    }
    /// <summary>
    /// 지상에서 대쉬 중이라면 true입니다.
    /// </summary>
    protected bool Dashing
    {
        get { return _dashing; }
        set { _animator.SetBool("Dashing", _dashing = value); }
    }
    /// <summary>
    /// 벽을 타고 있다면 true입니다.
    /// </summary>
    protected bool Sliding
    {
        get { return _sliding; }
        set { _animator.SetBool("Sliding", _sliding = value); }
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

    protected bool DashJumping { get; set; }
    protected bool WallJumping { get; set; }
    protected bool WallDashJumping { get; set; }
    protected bool AirDashing
    {
        get { return _airDashing; }
        set { _animator.SetBool("AirDashing", _airDashing = value); }
    }

    /// <summary>
    /// 대미지를 입었다면 true입니다.
    /// </summary>
    public bool Damaged
    {
        get { return _damaged; }
        protected set { _animator.SetBool("Damaged", _damaged = value); }
    }
    /// <summary>
    /// 무적 상태라면 true입니다.
    /// </summary>
    public bool Invencible
    {
        get { return _invencible; }
        protected set { _animator.SetBool("Invencible", _invencible = value); }
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
        get { return _danger; }
        protected set { _animator.SetBool("Danger", _danger = value); }
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
    /// 플레이어가 생존해있는지 확인합니다.
    /// </summary>
    /// <returns>체력이 정상 범위에 있다면 true입니다.</returns>
    public bool IsAlive()
    {
        return (0 < health);
    }
    /// <summary>
    /// 플레이어의 체력이 가득 찼는지 확인합니다.
    /// </summary>
    /// <returns>체력이 가득 찼다면 true입니다.</returns>
    public bool IsHealthFull()
    {
        return (health == maxHealth);
    }

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
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
    }
    protected virtual void Update()
    {

    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        UpdatePhysicsState(collision);
    }
    protected void OnCollisionStay2D(Collision2D collision)
    {
        UpdatePhysicsState(collision);
    }
    protected void OnCollisionExit2D(Collision2D collision)
    {
        UpdatePhysicsState(collision);
    }
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
            if (stageManager.fader.FadeOutEnded)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            return false;
        }
        // 사망 직전이라면
        else if (IsAlive() == false)
        {
            _rigidbody.velocity = Vector2.zero;
            return false;
        }
        return true;
    }
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
            _rigidbody.velocity = Vector2.zero;
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
        RaycastHit2D rayB = Physics2D.Raycast(groundCheckBack.position, Vector2.down, groundCheckRadius, whatIsGround);
        RaycastHit2D rayM = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, whatIsGround);
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
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
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
            // Pushing = IsTouchingWall(collision) && (FacingRight ?
            //  IsKeyPressed(GameKey.Right) : IsKeyPressed(GameKey.Left));
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
        if (_collider.IsTouchingLayers(whatIsGround))
        {
            float playerBottom = _collider.bounds.min.y;
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
        if (_collider.IsTouching(groundCollider))
        {
            Bounds groundBounds = groundCollider.bounds;
            if (groundBounds.min.y == groundBounds.max.y)
            {
                float playerBot = _collider.bounds.min.y;
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
                float playerBot = _collider.bounds.min.y;
                float groundTop = groundBounds.max.y;
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
        if (_collider.IsTouchingLayers(whatIsWall))
        {
            float playerBottom = _collider.bounds.min.y;
            float wallTop = collision.collider.bounds.max.y;

            // 땅을 밟고 있지 않다면 참입니다.
            if (Landed == false)
            {
                return true;
            }
            // 
            else if (playerBottom >= wallTop)
            {
                return false;
            }
            // 
            else
            {
                return true;
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
        _rigidbody.velocity = new Vector2(0, -spawnSpeed);
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
    /// 
    /// </summary>
    void RequestFadeOut()
    {
        stageManager.fader.FadeOut(1);
        Invoke("RequestRestart", 1);
    }
    /// <summary>
    /// 
    /// </summary>
    void RequestRestart()
    {

    }

    /// <summary>
    /// 플레이어 사망을 요청합니다.
    /// </summary>
    public void RequestDead()
    {
        Health = 0;

        StopMoving();
        BlockMoving();
        _animator.speed = 0;
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
    protected void BlockInput()
    {
        InputBlocked = true;
    }
    /// <summary>
    /// 플레이어의 입력 방지를 해제합니다.
    /// </summary>
    protected void UnblockInput()
    {
        InputBlocked = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 이동
    /// <summary>
    /// 플레이어를 왼쪽으로 이동합니다.
    /// </summary>
    protected void MoveLeft()
    {
        if (_facingRight)
            Flip();

        _rigidbody.velocity = new Vector2(-movingSpeed, _rigidbody.velocity.y);
        Moving = true;
    }
    /// <summary>
    /// 플레이어를 오른쪽으로 이동합니다.
    /// </summary>
    protected void MoveRight()
    {
        if (_facingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(movingSpeed, _rigidbody.velocity.y);
        Moving = true;
    }
    /// <summary>
    /// 플레이어의 이동을 중지합니다.
    /// </summary>
    protected void StopMoving()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        Moving = false;
    }
    /// <summary>
    /// 플레이어의 이동 요청을 막습니다.
    /// </summary>
    protected void BlockMoving()
    {
        MoveBlocked = true;
    }
    /// <summary>
    /// 플레이어가 이동을 요청할 수 있도록 합니다.
    /// </summary>
    protected void UnblockMoving()
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
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
        // _rigidbody.velocity = new Vector2(0, jumpSpeed);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = true;
    }
    /// <summary>
    /// 플레이어의 점프를 중지합니다.
    /// </summary>
    protected void StopJumping()
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
    protected void BlockJumping()
    {
        JumpBlocked = true;
    }
    /// <summary>
    /// 플레이어가 점프할 수 있도록 합니다.
    /// </summary>
    protected void UnblockJumping()
    {
        JumpBlocked = false;
    }
    /// <summary>
    /// 플레이어를 낙하시킵니다.
    /// </summary>
    protected void Fall()
    {
        // 개체의 운동 상태를 갱신합니다.
        BlockJumping();
        BlockDashing();
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

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
        movingSpeed = dashSpeed;
        _rigidbody.velocity = new Vector2
            (_facingRight ? dashSpeed : -dashSpeed, _rigidbody.velocity.y);

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
        movingSpeed = walkSpeed;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Dashing = false;
    }
    /// <summary>
    /// 플레이어의 대쉬 요청을 막습니다.
    /// </summary>
    protected void BlockDashing()
    {
        DashBlocked = true;
    }
    /// <summary>
    /// 플레이어가 대쉬할 수 있도록 합니다.
    /// </summary>
    protected void UnblockDashing()
    {
        DashBlocked = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 벽 타기
    /// <summary>
    /// 플레이어가 벽을 타도록 합니다.
    /// </summary>
    protected void Slide()
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
        _rigidbody.velocity = new Vector2(0, -slideSpeed);

        // 개체의 운동에 따른 효과를 처리합니다.
        GameObject slideFog = Instantiate(effects[2], slideFogPosition.position, slideFogPosition.rotation) as GameObject;
        slideFog.transform.SetParent(groundCheck.transform);
        if (FacingRight == false)
        {
            var newScale = slideFog.transform.localScale;
            newScale.x = FacingRight ? newScale.x : -newScale.x;
            slideFog.transform.localScale = newScale;
        }
        slideFogEffect = slideFog;

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Sliding = true;
    }
    /// <summary>
    /// 플레이어의 벽 타기를 중지합니다.
    /// </summary>
    protected void StopSliding()
    {
        // 개체의 운동 상태를 갱신합니다.
        UnblockDashing();
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

        // 개체의 운동에 따른 효과를 처리합니다.
        if (slideFogEffect != null)
        {
            slideFogEffect.transform.SetParent(null);
            slideFogEffect.GetComponent<EffectScript>().RequestEnd();
            slideFogEffect = null;
        }

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Sliding = false;
    }
    /// <summary>
    /// 플레이어의 벽 타기 요청을 막습니다.
    /// </summary>
    protected void BlockSliding()
    {
        SlideBlocked = true;
    }
    /// <summary>
    /// 플레이어가 벽 타기할 수 있도록 합니다.
    /// </summary>
    protected void UnblockSliding()
    {
        SlideBlocked = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 사다리 타기
    /// <summary>
    /// 플레이어가 사다리를 타도록 합니다.
    /// </summary>
    protected void RideLadder()
    {
        UnblockAirDashing(); // ClearAirDashCount();
    }

    ///////////////////////////////////////////////////////////////////
    // 조합
    /// <summary>
    /// 플레이어가 벽 점프를 합니다.
    /// </summary>
    protected void WallJump()
    {
        // 개체의 운동 상태를 갱신합니다.
        StopJumping();
        StopFalling();
        StopSliding();
        BlockJumping();
        BlockSliding();
        BlockDashing();
        UnblockAirDashing();
        _rigidbody.velocity = new Vector2
            (FacingRight ? -1.5f * movingSpeed : 1.5f * movingSpeed,
            jumpSpeed /* Mathf.Sqrt(2) */);

        // 개체의 운동에 따른 효과를 처리합니다.
        GameObject wallKick = Instantiate
            (effects[3], slideFogPosition.position, slideFogPosition.rotation)
            as GameObject;

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = true;
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
        movingSpeed = dashSpeed;
        _rigidbody.velocity = new Vector2(0, jumpSpeed);

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = true;
        Dashing = true;
        DashJumping = true;
    }
    /// <summary>
    /// 플레이어의 대쉬 점프를 중지합니다.
    /// </summary>
    protected void StopDashJumping()
    {
        Jumping = false;
        Dashing = false;
        DashJumping = false;
    }
    /// <summary>
    /// 플레이어가 벽에서 대쉬 점프하게 합니다.
    /// </summary>
    protected void WallDashJump()
    {
        // 개체의 운동 상태를 갱신합니다.
        // StopJumping();
        // StopFalling();
        // StopDashing();
        // BlockDashing();
        StopSliding();
        BlockJumping();
        BlockSliding();
        BlockAirDashing();
        movingSpeed = dashSpeed;
        _rigidbody.velocity = new Vector2
            (FacingRight ? -1.5f * movingSpeed : 1.5f * movingSpeed, jumpSpeed);

        // 개체의 운동에 따른 효과를 처리합니다.
        GameObject wallKick = Instantiate
            (effects[3], slideFogPosition.position, slideFogPosition.rotation)
            as GameObject;

        // 개체의 운동 상태가 갱신되었음을 알립니다.
        Jumping = true;
        Dashing = true;
        DashJumping = true;
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
        _rigidbody.velocity = new Vector2
            (FacingRight ? dashSpeed : -dashSpeed, 0);

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
    protected void BlockAirDashing()
    {
        AirDashBlocked = true; // airDashCount = 1;
    }
    /// <summary>
    /// 플레이어가 에어 대쉬할 수 있도록 합니다.
    /// </summary>
    protected void UnblockAirDashing()
    {
        AirDashBlocked = false; // ClearAirDashCount();
    }

    #endregion



    #region 플레이어의 상태 메서드를 정의합니다.
    /// <summary>
    /// 플레이어가 대미지를 입습니다.
    /// </summary>
    /// <param name="damage">플레이어가 입을 대미지입니다.</param>
    public virtual void Hurt(int damage)
    {
        Health -= damage;
        if (IsAlive() == false)
        {
            RequestDead();
        }
        else if (health <= dangerHealth)
        {
            Danger = true;
        }

        Damaged = true;
        Invencible = true;
        InputBlocked = true;
    }
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
    System.Collections.IEnumerator CoroutineInvencible()
    {
        InvencibleTime = 0;
        while (InvencibleTime < 1)
        {
            InvencibleTime += Time.deltaTime;
            yield return false;
        }
        Invencible = false;
        yield return true;
    }

    #endregion


    #region 외부에서 요청 가능한 행동 메서드를 정의합니다.
    /// <summary>
    /// 플레이어 소환을 요청합니다.
    /// </summary>
    public void RequestSpawn()
    {
        gameObject.SetActive(true); // this.enabled = true;
        Spawn();
    }
    /// <summary>
    /// 플레이어의 방향 전환을 요청합니다.
    /// </summary>
    public void RequestFlip()
    {
        Flip();
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
        return _animator.GetCurrentAnimatorStateInfo(0).length;
    }
    /// <summary>
    /// 현재 재생중인 애니메이션의 재생된 시간을 얻습니다.
    /// </summary>
    /// <returns>현재 재생중인 애니메이션의 재생된 시간입니다.</returns>
    protected float GetCurrentAnimationPlaytime()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    /// <summary>
    /// 애니메이션이 재생 중인지 확인합니다.
    /// </summary>
    /// <param name="stateName">재생 중인지 확인하려는 애니메이션의 이름입니다.</param>
    /// <returns>애니메이션이 재생 중이라면 true를 반환합니다.</returns>
    protected bool IsAnimationPlaying(string stateName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    #endregion 보조 메서드 정의



    #region 구형 정의를 보관합니다.


    #endregion
}