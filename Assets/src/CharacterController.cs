using UnityEngine;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour
{
    #region 공용 GameObject 객체에 접근합니다.
    Rigidbody2D _rigidbody;
    Animator _animator;

    #endregion



    #region 상수를 정의합니다.
    public enum GameKey
    {
        // 방향 키를 설정합니다.
        Up,
        Left,
        Down,
        Right,

        // 일반 키를 설정합니다.
        Jump,
        Attack,
        Dash,

        // 확장 키를 설정합니다.

    }

    #endregion 상수



    #region Unity에서 접근하는 필드를 정의합니다.
    // 이동 속성입니다.
    public float walkSpeed = 8;
    public float jumpSpeed = 16;
    public float jumpDecSize = 0.5f;

    public float dashSpeed = 16;
    public float dashForce = 800;

    // 바닥 속성입니다.
    public Transform groundCheck;
    public float groundRadius = 0.01f;
    public LayerMask whatIsGround;

    #endregion Unity 필드



    #region 외부에서 접근 가능한 필드를 정의합니다.
    public Dictionary<GameKey, KeyCode> keySetting;

    #endregion 공용 필드



    #region 필드를 정의합니다.
    bool facingRight;           // 우측을 보고 있다면 true입니다.
    bool grounded;              // 땅에 닿아있다면 true입니다.

    // 걷는 상태를 결정합니다.
    bool walking = false;
    bool walk_beg = false;
    bool walk_run = false;

    // 점프 상태를 결정합니다.
    bool jumping = false;
    bool jump_beg = false;
    bool jump_run = false;

    // 대쉬 상태를 결정합니다.
    bool dashing = false;
    bool dash_beg = false;
    bool dash_run = false;
    bool dash_end = false;

    // 자유 낙하 상태를 결정합니다.
    bool falling = false;
    bool fall_beg = false;
    bool fall_run = false;
    bool fall_end = false;

    // 공격 상태를 결정합니다.
    bool shooting = false;
    bool chargeShooting = false;

    // 키 입력 방지 상태를 결정합니다.
    bool keyBlockEnabled = false;


    #endregion 필드 정의



    #region MyRegion
    void RequestJump(bool state)
    {

    }
    void RequestDash(bool state)
    {

    }
    void RequestWalk(bool state)
    {

    }
    void RequestShot(bool state)
    {

    }
    void RequestKeyBlock(bool state)
    {

    }

    #endregion



    #region 상속된 MonoBehaviour 메서드를 재정의합니다.
    void Start()
    {
        // 공용 메서드를 초기화합니다.
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // 필드를 초기화합니다.
        facingRight = true;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        // 키 설정을 초기화합니다.
        keySetting = new Dictionary<GameKey, KeyCode>();
        keySetting[GameKey.Left] = KeyCode.LeftArrow;
        keySetting[GameKey.Right] = KeyCode.RightArrow;
        keySetting[GameKey.Jump] = KeyCode.C;
        keySetting[GameKey.Attack] = KeyCode.X;
        keySetting[GameKey.Dash] = KeyCode.Z;
    }
    void Update()
    {
        if (IsKeyDown(GameKey.Jump))
        {
            if (grounded == true)
            {
                _animator.SetBool("Jumping", jumping = true);
            }
        }
        if (IsKeyDown(GameKey.Dash))
        {
            _animator.SetBool("Dashing", dashing = true);
        }
        if (IsKeyDown(GameKey.Left) || IsKeyDown(GameKey.Right))
        {
            _animator.SetBool("Walking", walking = true);
        }
        if (IsKeyDown(GameKey.Attack))
        {
            _animator.SetBool("Shooting", shooting = true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _animator.SetBool("ChargeShooting", chargeShooting = true);
        }
    }
    void FixedUpdate()
    {
        // 캐릭터 상태에 대한 값을 나열합니다.
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        _animator.SetBool("Ground", grounded);

        float verSpeed = _rigidbody.velocity.y;
        _animator.SetFloat("VerSpeed", verSpeed);

        // 키 상태에 대한 부울 값을 나열합니다.
        bool isLeftPressed = IsKeyPressed(GameKey.Left);
        bool isRightPressed = IsKeyPressed(GameKey.Right);
        bool isJumpPressed = IsKeyPressed(GameKey.Jump);
        bool isDashPressed = IsKeyPressed(GameKey.Dash);

        // 상태에 대한 반응을 나열합니다.
        if (walking)
        {
            if (isLeftPressed)
            {
                if (facingRight == true)
                    Flip();
                _rigidbody.velocity = new Vector2(-walkSpeed, _rigidbody.velocity.y);
            }
            else if (isRightPressed)
            {
                if (facingRight == false)
                    Flip();
                _rigidbody.velocity = new Vector2(walkSpeed, _rigidbody.velocity.y);
            }
            else if (isJumpPressed)
            {

            }
            else if (isDashPressed)
            {

            }
            else
            {
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
                _animator.SetBool("WalkBeg", walk_beg = false);
                _animator.SetBool("WalkRun", walk_run = false);
                _animator.SetBool("Walking", walking = false);
            }
        }
        if (jumping)
        {
            if (jump_run)
            {
                if (_rigidbody.velocity.y > 0)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y - jumpDecSize);
                }
                else
                {
                    _animator.SetBool("Jumping", jumping = false);
                    _animator.SetBool("Falling", falling = true);
                }
            }
        }
        if (dashing)
        {


            if (isDashPressed == false)
            {
                _animator.SetBool("DashEnd", dash_end = true);
            }
        }
        if (falling)
        {

            if (Mathf.Abs(_rigidbody.velocity.y) > jumpSpeed)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -jumpSpeed);
            }
            else
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y - jumpDecSize);
            }
        }
        if (shooting)
        {



        }
    }

    #endregion MonoBehaviour 메서드 재정의



    #region 애니메이션 이벤트 핸들러를 정의합니다.
    void ReadyToIdle()
    {

    }
    void WalkBegToRun_beg()
    {
        _animator.SetBool("WalkBeg", walk_beg = true);
    }
    void WalkBegToRun_end()
    {
        _animator.SetBool("WalkBeg", walk_beg = false);
        _animator.SetBool("WalkRun", walk_run = true);
    }
    void DashBegToRun_beg()
    {
        _animator.SetBool("DashBeg", dash_beg = true);
        float vx = facingRight ? dashSpeed : -dashSpeed;
        _rigidbody.velocity = new Vector2(vx, _rigidbody.velocity.y);
    }
    void DashBegToRun_end()
    {
        _animator.SetBool("DashBeg", dash_beg = false);
        _animator.SetBool("DashRun", dash_run = true);
    }
    void DashEndFromRun_beg()
    {
        _animator.SetBool("DashRun", dash_run = false);
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }
    void DashEndFromRun_end()
    {
        _animator.SetBool("DashBeg", dash_beg = false);
        _animator.SetBool("DashRun", dash_run = false);
        _animator.SetBool("DashEnd", dash_end = false);
        _animator.SetBool("Dashing", dashing = false);
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }
    void JumpRunning_beg()
    {
        _animator.SetBool("JumpBeg", jump_beg = true);
    }
    void JumpRunning_run()
    {
        _animator.SetBool("JumpBeg", jump_beg = false);
        _animator.SetBool("JumpRun", jump_run = true);

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
    }
    void JumpRunning_end()
    {
        _animator.SetBool("JumpRun", jump_run = false);
        _animator.SetBool("Jumping", jumping = false);
        _animator.SetBool("Falling", falling = true);
    }
    void FallBegToRun_beg()
    {
        _animator.SetBool("FallBeg", fall_beg = true);
    }
    void FallBegToRun_end()
    {
        _animator.SetBool("FallBeg", fall_beg = false);
        _animator.SetBool("FallRun", fall_run = true);
    }
    void FallRunning()
    {

    }
    void FallEndOfRun_beg()
    {
        _animator.SetBool("FallRun", fall_run = false);
        _animator.SetBool("FallEnd", fall_end = true);

        _animator.SetBool("DashBeg", dash_beg = false);
        _animator.SetBool("DashRun", dash_run = false);
        _animator.SetBool("DashEnd", dash_end = false);
        _animator.SetBool("Dashing", dashing = false);
    }
    void FallEndOfRun_end()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _animator.SetBool("FallBeg", fall_beg = false);
        _animator.SetBool("FallRun", fall_run = false);
        _animator.SetBool("Falling", falling = false);
    }
    // 공격 상태
    void ShotRunning_beg()
    {
        _animator.SetBool("Shooting", shooting = true);
    }
    void ShotRunnig_end()
    {
        _animator.SetBool("Shooting", shooting = false);
    }
    void ChargeShotRunning_beg()
    {
        _animator.SetBool("ChargeShooting", chargeShooting = true);
    }
    void ChargeShotRunning_end()
    {
        _animator.SetBool("ChargeShooting", chargeShooting = false);
    }

    #endregion 애니메이션 이벤트 핸들러



    #region 사용자 정의 메서드를 정의합니다.
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    bool IsKeyDown(GameKey gameKey)
    {
        return Input.GetKeyDown(keySetting[gameKey]);
    }
    bool IsKeyPressed(GameKey gameKey)
    {
        return Input.GetKey(keySetting[gameKey]);
    }

    #endregion 사용자 정의 메서드
}
