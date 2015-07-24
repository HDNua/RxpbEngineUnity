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
    bool walking;               // 걷고 있다면 true입니다.
    bool jumping;               // 점프 상태라면 true입니다.
    bool dashing;               // 대쉬 상태라면 true입니다.

    #endregion 필드 정의



    #region 상속된 MonoBehaviour 메서드를 재정의합니다.
    void Start()
    {
        // 공용 메서드를 초기화합니다.
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // 필드를 초기화합니다.
        facingRight = true;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        walking = false;
        jumping = false;
        dashing = false;

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
                jumping = true;
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
            }
        }
        if (IsKeyDown(GameKey.Dash))
        {
            dashing = true;
            _animator.SetBool("Dashing", true);
            _animator.SetBool("DashKeyPressed", true);

            float vx = facingRight ? dashSpeed : -dashSpeed;
            _rigidbody.velocity = new Vector2(vx, _rigidbody.velocity.y);
        }
        if (IsKeyDown(GameKey.Left) || IsKeyDown(GameKey.Right))
        {
            walking = true;
            _animator.SetBool("Walking", true);
        }


        /*
        if (Input.GetKeyDown(KeyCode.C))
        {
            anim.SetBool("Ground", false);

            Vector2 velocity = Vector2.zero;
            Vector2 force = Vector2.zero;

            //            bool hasToUpdate = false;
            if (grounded)
            {
                //				velocity = new Vector2(this.rigidbody2D.velocity.x, 0);
                force = new Vector2(0, jumpForce);
                //                rigidbody2D.velocity = velocity;
                _rigidbody2D.AddForce(force);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            print("TEST");
        }
        */
    }
    void FixedUpdate()
    {
        // 캐릭터 상태에 대한 부울 값을 나열합니다.
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        _animator.SetBool("Ground", grounded);

        // 키 반응에 대한 부울 값을 나열합니다.
        bool isLeftPressed = IsKeyPressed(GameKey.Left);
        bool isRightPressed = IsKeyPressed(GameKey.Right);
        bool isJumpPressed = IsKeyPressed(GameKey.Jump);
        bool isDashPressed = IsKeyPressed(GameKey.Dash);

        // 상태에 대한 반응을 나열합니다.
        if (dashing) // 대시 중
        {


            

            if (isDashPressed == false)
            {
                _animator.SetBool("DashKeyPressed", false);
                _animator.SetBool("Dashing", false);
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 4, _rigidbody.velocity.y);
                dashing = false;
            }
        }
        if (jumping) // 점프 중
        {


            if (isJumpPressed == false)
            {
                _animator.SetBool("JumpKeyPressed", false);
            }
        }


        // 키에 대한 반응을 나열합니다.
        if (walking)
        {
            if (dashing == false)
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
                else
                {
                    _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
                    walking = false;
                    _animator.SetBool("Walking", false);
                }
            }
        }

        /*
        // get state
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        anim.SetFloat("vSpeed", _rigidbody2D.velocity.y);

        bool isLeftKeyDown = Input.GetKey(KeyCode.LeftArrow);
        bool isRightKeyDown = Input.GetKey(KeyCode.RightArrow);

        bool shotTriggered = Input.GetKeyDown(KeyCode.X);
        anim.SetBool("ShotTriggered", shotTriggered);

        // handle by state
        if (isLeftKeyDown)
        {
            anim.SetFloat("Speed", 0.011f);
            _rigidbody2D.velocity = new Vector2(-maxSpeed, _rigidbody2D.velocity.y);
            if (facingRight == true)
                Flip();
        }
        else if (isRightKeyDown)
        {
            anim.SetFloat("Speed", 0.011f);
            _rigidbody2D.velocity = new Vector2(maxSpeed, _rigidbody2D.velocity.y);
            if (facingRight == false)
                Flip();
        }
        else
        {
            anim.SetFloat("Speed", 0.0f);
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        }
        */
    }

    #endregion MonoBehaviour 메서드 재정의

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
