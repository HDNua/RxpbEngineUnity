using UnityEngine;
using System;
using System.Collections;

[Obsolete("EnemyMettoScript로 대체되었습니다.", true)]
public class MettoController : MonoBehaviour
{
    #region Component
    Rigidbody2D _rigidbody;
    BoxCollider2D _collider;

    #endregion



    #region Readonly Unity GameObject
    public GameObject explosion;

    #endregion



    #region Children GameObject
    public float groundCheckRadius = 0.1f;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    public float pushCheckRadius = 0.1f;
    public Transform pushCheck;
    public LayerMask whatIsWall;

    #endregion



    #region Unity Accessable Property
    public float movingSpeed;

    public AudioClip[] audioClips;

    #endregion



    #region 기타 초기화할 GameObject의 리스트입니다.
    AudioSource[] audioSources;


    #endregion


    #region 캐릭터 상태 필드를 정의합니다.
    public int hitPoint = 10;

    bool _landed = false;
    bool facingRight = false;

    #endregion


    #region Unity Default Methods
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        audioSources = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = audioClips[i];
        }

        StartCoroutine(WalkAround());
    }
    void Update()
    {
        // 체력이 0 이하인 경우 사망합니다.
        if (IsDead())
        {
            Dead();
        }

        // 캐릭터의 상태 정보를 획득합니다.
        _landed = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        bool wallCheck = Physics2D.OverlapCircle(pushCheck.position, pushCheckRadius, whatIsWall);

        // 획득한 상태 정보를 바탕으로 캐릭터의 상태를 변경합니다.
        if (wallCheck == true) // 벽에 닿은 경우 벽을 통과하지 않도록 합니다.
        {
            if (facingRight)
                MoveLeft();
            else
                MoveRight();
        }
    }
    IEnumerator WalkAround()
    {
        while (hitPoint != 0)
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 1)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
            yield return new WaitForSeconds(1);
        }
    }

    #endregion



    #region 충돌 이벤트 메서드를 정의합니다.
    void OnTriggerStay2D(Collider2D other)
    {
    }

    #endregion



    #region 외부에서 적 캐릭터의 상태를 변화시키기 위한 메서드입니다.
    /// <summary>
    /// 대미지를 입습니다.
    /// </summary>
    /// <param name="damage">캐릭터에 가해진 대미지의 크기입니다.</param>
    public void Hurt(int damage)
    {
        hitPoint -= damage;
    }

    #endregion


    #region 보조 메서드
    /// <summary>
    /// 체력이 0 이하인지 확인합니다.
    /// </summary>
    /// <returns>체력이 0 이하라면 true입니다.</returns>
    bool IsDead()
    {
        return hitPoint <= 0;
    }
    /// <summary>
    /// 캐릭터가 사망합니다.
    /// </summary>
    void Dead()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void MoveLeft()
    {
        if (facingRight)
            Flip();
        _rigidbody.velocity = new Vector2(-movingSpeed, 0);
    }
    void MoveRight()
    {
        if (facingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(movingSpeed, 0);
    }
    void Flip()
    {
        if (facingRight)
        {
            _rigidbody.transform.localScale = new Vector3(-_rigidbody.transform.localScale.x, _rigidbody.transform.localScale.y);
        }
        else
        {
            _rigidbody.transform.localScale = new Vector3(-_rigidbody.transform.localScale.x, _rigidbody.transform.localScale.y);
        }
        facingRight = !facingRight;
    }

    #endregion
}