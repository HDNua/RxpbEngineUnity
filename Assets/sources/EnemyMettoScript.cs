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

    #endregion



    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.
    public float movingSpeed;

    bool facingRight = false;

    #endregion



    #region MyRegion
    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(WalkAround());
    }
    protected override void Update()
    {
        base.Update();
    }

    #endregion



    #region EnemyScript의 메서드를 오버라이드 합니다.
    public override void Dead()
    {
        SoundEffects[0].Play();
        Instantiate(effects[0], transform.position, transform.rotation);
        base.Dead();
    }

    #endregion



    #region 보조 메서드를 정의합니다.
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
}