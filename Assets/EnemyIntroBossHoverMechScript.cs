using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 인트로 스테이지 보스 적 캐릭터를 정의합니다.
/// </summary>
public class EnemyIntroBossHoverMechScript : EnemyBossScript
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.

    #endregion





    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    public float _movingSpeedX = 1;
    /// <summary>
    /// 
    /// </summary>
    public float _movingSpeedY = 2;

    #endregion





    #region 캐릭터의 운동 상태 필드를 정의합니다.
    /// <summary>
    /// 캐릭터가 공격 중이라면 참입니다.
    /// </summary>
    bool _attacking = false;

    #endregion





    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.
    /// <summary>
    /// 캐릭터가 공격 중이라면 참입니다.
    /// </summary>
    bool Attacking
    {
        get { return _attacking; }
        set { _Animator.SetBool("Attacking", _attacking = value); }
    }

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 
        Flying = true;
        Landed = false;
        StopFalling();
        MoveDown();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        // 
        if (AppearEnded == false)
        {
            return;
        }

        // 
        MoveToPlayer();
    }

    #endregion





    #region Collider2D의 기본 메서드를 재정의합니다.
    /// <summary>
    /// 충돌체가 여전히 트리거 내부에 있습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        // 트리거가 발동한 상대 충돌체가 플레이어라면 대미지를 입힙니다.
        if (other.CompareTag("Player"))
        {
            GameObject pObject = other.gameObject;
            PlayerController player = pObject.GetComponent<PlayerController>();


            // 플레이어가 무적 상태이거나 죽었다면
            if (player.Invencible || player.IsDead)
            {
                // 아무 것도 하지 않습니다.

            }
            // 그 외의 경우
            else
            {
                // 플레이어에게 대미지를 입힙니다.
                player.Hurt(Damage);
            }
        }
    }

    #endregion





    #region EnemyBossScript의 메서드를 오버라이드합니다.
    /// <summary>
    /// 지상에 착륙합니다.
    /// </summary>
    protected override void Land()
    {
        base.Land();
        SoundEffects[1].Play();
    }
    /// <summary>
    /// 등장 액션입니다.
    /// </summary>
    public override void Appear()
    {
        Fall();

        // 
        StartCoroutine(CoroutineAppear());
    }
    /// <summary>
    /// 점프하게 합니다.
    /// </summary>
    protected override void Jump()
    {
        base.Jump();
        SoundEffects[2].Play();
    }
    /// <summary>
    /// 낙하합니다.
    /// </summary>
    protected override void Fall()
    {
        base.Fall();
    }

    #endregion




    #region 행동 메서드를 정의합니다.
    /// <summary>
    /// 공격합니다.
    /// </summary>
    private void Attack()
    {
        Attacking = true;

        // 
        StartCoroutine(CoroutineAttack());
    }
    /// <summary>
    /// 공격을 중지합니다.
    /// </summary>
    private void StopAttack()
    {
        Attacking = false;
    }

    /// <summary>
    /// 왼쪽으로 이동합니다.
    /// </summary>
    protected override void MoveLeft()
    {
        if (FacingRight)
            Flip();

        Moving = true;
        _Rigidbody.velocity = new Vector2(-_movingSpeedX, _Rigidbody.velocity.y);
    }
    /// <summary>
    /// 오른쪽으로 이동합니다.
    /// </summary>
    protected override void MoveRight()
    {
        if (FacingRight == false)
            Flip();

        Moving = true;
        _Rigidbody.velocity = new Vector2(_movingSpeedX, _Rigidbody.velocity.y);
    }
    /// <summary>
    /// 위쪽으로 이동합니다.
    /// </summary>
    protected void MoveUp()
    {
        Moving = true;
        _Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, _movingSpeedY);
    }
    /// <summary>
    /// 아래쪽으로 이동합니다.
    /// </summary>
    protected void MoveDown()
    {
        Moving = true;
        _Rigidbody.velocity = new Vector2(_Rigidbody.velocity.x, -_movingSpeedY);
    }

    /// <summary>
    /// 전투 시작 액션입니다.
    /// </summary>
    public override void Fight()
    {
        MoveLeft();
    }

    #endregion





    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 등장 코루틴입니다.
    /// </summary>
    IEnumerator CoroutineAppear()
    {
        // 지상에 떨어질 때까지 대기합니다.
        while (Landed == false)
        {
            yield return false;
        }
        StopMoving();

        // 떨어지고 나서 몇 초간 대기합니다.
        while (IsAnimationPlaying("FallRun"))
            yield return false;
        while (IsAnimationPlaying("FallEnd"))
            yield return false;

        // 공격을 두 번 합니다.
        Attack();
        while (Attacking)
            yield return false;

        // 등장을 마칩니다.
        AppearEnded = true;
        yield break;
    }
    /// <summary>
    /// 공격 코루틴입니다.
    /// </summary>
    IEnumerator CoroutineAttack()
    {
        // 움직임을 멈춥니다.
        StopMoving();

        // 
        while (IsAnimationPlaying("Idle"))
            yield return false;

        /**
        SoundEffects[4].Play();
        while (IsAnimationPlaying("Attack1"))
            yield return false;

        SoundEffects[5].Play();
        while (IsAnimationPlaying("Attack2"))
            yield return false;
        */

        // 공격을 종료합니다.
        Attacking = false;
        while (IsAnimationPlaying("Idle"))
            yield return false;

        yield break;
    }

    /// <summary>
    /// 플레이어를 향해 이동합니다.
    /// </summary>
    private void MoveToPlayer()
    {
        // 사용할 변수를 선언합니다.
        Vector3 playerPos = _StageManager.GetCurrentPlayerPosition();
        Vector2 relativePos = playerPos - transform.position;

        // 플레이어를 향해 방향을 바꿉니다.
        if (relativePos.x < 0 && FacingRight)
        {
            MoveLeft();
        }
        else if (relativePos.x > 0 && !FacingRight)
        {
            MoveRight();
        }
        /*
        Handy.Log("PlayerPos={2}, SelfPos={3}, RelPos={0}, FacingRight={1}",
            relativePos, FacingRight, playerPos, transform.position);
        */

        if (relativePos.y > 0)
        {
            MoveUp();
        }
        else if (relativePos.y < 0)
        {
            MoveDown();
        }
    }

    #endregion





    #region 구형 정의를 보관합니다.


    #endregion
}