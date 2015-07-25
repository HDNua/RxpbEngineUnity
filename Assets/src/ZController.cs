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
    public float groundCheckRadius = 0.1f;
    public LayerMask whatIsGround;

    public float walkSpeed = 8;

    public float jumpSpeed = 16;
    public float jumpDecSize = 0.8f;

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

        _animator.SetBool("WalkBlocked", true);
	}
	void Update()
    {
	    if (Input.GetKeyDown(KeyCode.X))
        {
            RequestShot();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            RequestJump();
        }
	}
    void FixedUpdate()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        _animator.SetBool("Grounded", grounded);

        _animator.SetBool("JumpRequestExist", Input.GetKey(KeyCode.C));

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
        _animator.SetBool("WalkBlocked", false);
    }
    void RequestShot()
    {
        if (_animator.GetBool("ShotBlocked") == false)
        {
            _animator.SetBool("ShotRequested", true);
            _animator.SetBool("ShotBlocked", true);
            _animator.SetBool("WalkBlocked", true);
            RequestWalkEnd();
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
        }
    }
    void RequestFall()
    {
        _animator.SetBool("FallRequested", true);
    }
    void RequestFallEnd()
    {
        _animator.SetBool("FallEnd", true);
    }
    void RequestWalk()
    {
        _animator.SetBool("WalkRequestExist", true);
    }
    void RequestWalkEnd()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _animator.SetBool("WalkRequestExist", false);
    }

    #endregion



    #region 프레임 이벤트 핸들러를 정의합니다.
    public void Walk_beg()
    {

    }
    public void Jump_beg()
    {
        jumping = true;
        _animator.SetBool("JumpRequested", false);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
        audioSources[4].Play();
        audioSources[6].Play();
    }
    public void Fall_beg()
    {
        jumping = false;
        falling = true;
        _animator.SetBool("FallRequested", false);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -jumpDecSize);
    }
    public void FallEndFromRun_beg()
    {
        _animator.SetBool("FallEnd", true);
        _animator.SetBool("JumpBlocked", false);
        _animator.SetBool("FallEnd", false);
        falling = false;
        audioSources[5].Play();
    }
    public void FallEndFromRun_end()
    {
    }
    public void Attack_saber1()
    {
        _animator.SetBool("ShotRequested", false);
        audioSources[0].Play();
        audioSources[3].Play();
    }
    public void Attack_saber1_end()
    {
        _animator.SetBool("ShotBlocked", false);
    }
    public void Attack_saber2()
    {
        _animator.SetBool("ShotRequested", false);
        audioSources[1].Play();
        audioSources[3].Play();
    }
    public void Attack_saber2_end()
    {
        _animator.SetBool("ShotBlocked", false);
    }
    public void Attack_saber3()
    {
        _animator.SetBool("ShotRequested", false);
        audioSources[2].Play();
        audioSources[3].Play();
    }
    public void AttackEndFromRun_beg()
    {
        _animator.SetBool("ShotBlocked", false);
    }
    public void AttackEndFromRun_end()
    {
        _animator.SetBool("WalkBlocked", false);
    }

    #endregion 프레임 이벤트 핸들러
}
