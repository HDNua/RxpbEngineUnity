using UnityEngine;
using System.Collections;

/// <summary>
/// 멧토 적 캐릭터를 정의합니다.
/// </summary>
public class EnemyMettoScript : EnemyScript
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    Rigidbody2D _rigidbody;

    #endregion



    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    public Transform groundCheck;
    public Transform pushCheck;
    public LayerMask whatIsWall;

    #endregion



    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.
    public float movingSpeed;

    bool facingRight = false;

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(WalkAround());
    }
    protected override void Update()
    {
        base.Update();

        Vector3 direction = facingRight ? Vector3.right : Vector3.left;
        RaycastHit2D pushRay = Physics2D.Raycast
            (pushCheck.position, direction, 0.1f, whatIsWall);
        if (pushRay)
        {
            if (facingRight)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject pObject = other.gameObject;
            PlayerController player = pObject.GetComponent<PlayerController>();

            if (player.Invencible || player.IsDead)
            {

            }
            else
            {
                player.Hurt(Damage);
            }
        }
    }

    #endregion



    #region EnemyScript의 메서드를 오버라이드 합니다.
    /// <summary>
    /// 캐릭터가 사망합니다.
    /// </summary>
    public override void Dead()
    {
        SoundEffects[0].Play();
        Instantiate(effects[0], transform.position, transform.rotation);
        base.Dead();
    }

    #endregion



    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 왼쪽으로 이동합니다.
    /// </summary>
    void MoveLeft()
    {
        if (facingRight)
            Flip();
        _rigidbody.velocity = new Vector2(-movingSpeed, 0);
    }
    /// <summary>
    /// 오른쪽으로 이동합니다.
    /// </summary>
    void MoveRight()
    {
        if (facingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(movingSpeed, 0);
    }
    /// <summary>
    /// 방향을 바꿉니다.
    /// </summary>
    void Flip()
    {
        if (facingRight)
        {
            _rigidbody.transform.localScale = new Vector3
                (-_rigidbody.transform.localScale.x, _rigidbody.transform.localScale.y);
        }
        else
        {
            _rigidbody.transform.localScale = new Vector3
                (-_rigidbody.transform.localScale.x, _rigidbody.transform.localScale.y);
        }
        facingRight = !facingRight;
    }
    /// <summary>
    /// 주변을 방황합니다.
    /// </summary>
    /// <returns>StartCoroutine 호출에 적합한 값을 반환합니다.</returns>
    IEnumerator WalkAround()
    {
        while (_health != 0)
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
}