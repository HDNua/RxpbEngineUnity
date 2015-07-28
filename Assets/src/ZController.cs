using UnityEngine;
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

    public float walkSpeed = 8;

    public float jumpSpeed = 16;
    public float jumpDecSize = 0.8f;

    public BoxCollider2D groundCheck2;

    #endregion Unity 공용 필드



    #region 주인공의 상태 필드를 정의합니다.
    bool facingRight = true;

    bool jumping = false;
    bool falling = false;

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

        RequestWalkBlock();
//        _animator.SetBool("WalkBlocked", true);
	}
	void Update()
    {
	    if (Input.GetKeyDown(KeyCode.X))
        {
            if (_animator.GetBool("ShotBlocked") == false)
            {
                RequestShot();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_animator.GetBool("JumpBlocked") == false)
            {
                RequestJump();
            }
        }
	}
    void FixedUpdate()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        // Physics2D.OverlapArea(groundCheck2.bounds.min, groundCheck2.bounds.max, whatIsGround);
        _animator.SetBool("Grounded", grounded);

        _animator.SetBool("JumpRequestExist", Input.GetKey(KeyCode.C));
        _animator.SetBool("Jumping", jumping);
        _animator.SetBool("Falling", falling);

        if (grounded == false)
        {
            if (jumping)
            {
                if (_animator.GetBool("JumpRequestExist") == false)
                {
                    RequestFall();
                }
                else
                {
                    float verSpeed = _rigidbody.velocity.y; // _animator.GetFloat("VerSpeed");
                    if (verSpeed > 0)
                    {
                        float vy = _rigidbody.velocity.y - jumpDecSize;
                        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, vy > 0 ? vy : 0);
                    }
                    else
                    {
                        RequestFall();
                    }
                }
            }
            else if (falling)
            {
                float verSpeed = _rigidbody.velocity.y; // _animator.GetFloat("VerSpeed");
                if (verSpeed > -jumpSpeed)
                {
                    float vy = _rigidbody.velocity.y - jumpDecSize;
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, vy > -jumpSpeed ? vy : -jumpSpeed);
                }
            }
            else
            {
                RequestFall();
            }
            _animator.SetFloat("VerSpeed", _rigidbody.velocity.y);
        }
        else
        {
            if (falling)
            {
                RequestFallEnd();
            }
        }

        if (_animator.GetBool("WalkBlocked") == false)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (facingRight == true)
                    Flip();
                _rigidbody.velocity = new Vector2(-walkSpeed, _rigidbody.velocity.y);
                RequestWalk();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (facingRight == false)
                    Flip();
                _rigidbody.velocity = new Vector2(walkSpeed, _rigidbody.velocity.y);
                RequestWalk();
            }
            else if (_animator.GetBool("WalkRequestExist"))
            {
                RequestWalkEnd();
            }
        }
    }

    #endregion MonoBehaviour 기본 메서드 재정의



    #region 요청을 수행하기 위한 보조 메서드를 정의합니다.
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    #endregion 보조 메서드 정의



    #region 요청 메서드를 정의합니다.
    void RequestIdle()
    {
        _animator.SetBool("FallRequested", false);
        _animator.SetBool("JumpBlocked", false);
        _animator.SetBool("FallEnd", false);
        _animator.SetBool("ShotBlocked", false);
        _animator.SetBool("WalkBlocked", false);
    }
    void RequestShot()
    {
        if (_animator.GetBool("ShotBlocked") == false)
        {
            _animator.SetBool("ShotRequested", true);
            _animator.SetBool("ShotBlocked", true);
            _animator.SetBool("JumpBlocked", true);

            // 지상에 있을 경우
            if (jumping == false && falling == false)
            {
                RequestWalkBlock();
                RequestWalkEnd();
            }
        }
    }
    void RequestJump()
    {
        if (_animator.GetBool("JumpBlocked") == false)
        {
            _animator.SetBool("JumpRequestExist", true);
            _animator.SetBool("JumpRequested", true);
            _animator.SetBool("JumpBlocked", true);
            _animator.SetBool("WalkBlocked", false);
            jumping = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
        }
    }
    void RequestFall()
    {
        jumping = false;
        falling = true;

        _animator.SetBool("JumpBlocked", true);
        _animator.SetBool("FallRequested", true);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -jumpDecSize);
    }
    void RequestFallEnd()
    {
        _animator.SetBool("FallEndRequested", true);
        falling = false;

        // 공중 공격 중이었다면
        if (_animator.GetBool("ShotBlocked"))
        {
            RequestWalkBlock();
            RequestWalkEnd();
        }

        var curState = _animator.GetCurrentAnimatorStateInfo(0);
        if (curState.IsName("Z_jumpShot_1inAir"))
        {
            var nTime = curState.normalizedTime;
            var fTime = nTime - Mathf.Floor(nTime);
            _animator.Play("Z_jumpShot_2ground", 0, fTime);
            RequestWalkEnd();
            RequestWalkBlock();
            _animator.SetBool("FallEndRequested", false);
        }
    }
    void RequestWalk()
    {
        _animator.SetBool("FallEnd", false);
        _animator.SetBool("WalkRequestExist", true);
    }
    void RequestWalkEnd()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _animator.SetBool("WalkRequestExist", false);
    }
    void RequestWalkBlock()
    {
        _animator.SetBool("WalkBlocked", true);
    }

    #endregion



    #region 프레임 이벤트 핸들러를 정의합니다.
    public void RemainIdle()
    {
        _animator.SetBool("ShotBlocked", false);
        _animator.SetBool("JumpBlocked", false);
        _animator.SetBool("WalkBlocked", false);
    }
    public void WalkBeg_beg()
    {
    }
    public void WalkBeg_end()
    {
    }
    public void Jump_beg()
    {
        _animator.SetBool("JumpRequested", false);
        audioSources[4].Play();
        audioSources[6].Play();
    }
    public void Fall_beg()
    {
        _animator.SetBool("FallRequested", false);
    }
    public void FallEndFromRun_beg()
    {
        RequestWalkBlock();
        _animator.SetBool("FallRequested", false);
        _animator.SetBool("FallEndRequested", false);
        _animator.SetBool("FallEnd", true);
        _animator.SetBool("JumpBlocked", false);
        audioSources[5].Play();
    }
    public void FallEndFromRun_end()
    {
        RequestIdle();
    }
    public void Attack_saber1()
    {
        _animator.SetBool("ShotRequested", false);
        _animator.SetBool("JumpBlocked", true);
        audioSources[0].Play();
        audioSources[3].Play();
    }
    public void Attack_saber1_end()
    {
        _animator.SetBool("ShotBlocked", false);
        _animator.SetBool("JumpBlocked", false);
    }
    public void Attack_saber2()
    {
        _animator.SetBool("ShotRequested", false);
        _animator.SetBool("JumpBlocked", true);
        audioSources[1].Play();
        audioSources[3].Play();
    }
    public void Attack_saber2_end()
    {
        _animator.SetBool("ShotBlocked", false);
        _animator.SetBool("JumpBlocked", false);
    }
    public void Attack_saber3()
    {
        _animator.SetBool("ShotRequested", false);
        _animator.SetBool("JumpBlocked", true);
        audioSources[2].Play();
        audioSources[3].Play();
    }
    public void AttackEndFromRun_beg()
    {
        _animator.SetBool("ShotBlocked", false);
    }
    public void AttackEndFromRun_end()
    {
        _animator.SetBool("JumpBlocked", false);
        _animator.SetBool("WalkBlocked", false);
    }
    public void JumpShot_beg()
    {
        _animator.SetBool("ShotRequested", false);
        _animator.SetBool("ShotBlocked", true);
        audioSources[3].Play();
    }
    public void JumpShot_end()
    {
        _animator.SetBool("ShotBlocked", false);
    }

    #endregion 프레임 이벤트 핸들러
}
