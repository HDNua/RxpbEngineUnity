using UnityEngine;
using System.Collections;

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
    public Transform pushCheck;
    public Transform groundCheck;
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
    bool facingRight = false;
    bool landed = false;
    public int hitPoint = 10;

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
        if (IsDead())
        {
            Dead();
        }

        //        bool wallCheck = Physics2D.Raycast(pushCheck.position, Vector2.left, 0.1f, whatIsWall);
        bool wallCheck = Physics2D.OverlapCircle(pushCheck.position, 0.1f, whatIsWall);
        if (wallCheck == true)
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
            int random = Random.Range(0, 2);
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
    public void Hurt(int damage)
    {
        hitPoint -= damage;
    }

    #endregion


    #region 보조 메서드
    bool IsDead()
    {
        return hitPoint <= 0;
    }
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