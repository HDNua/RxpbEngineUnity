using System;
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
    /// 방패 개체입니다.
    /// </summary>
    public EnemyScript _guard;
    /// <summary>
    /// 탄환 개체입니다.
    /// </summary>
    public EnemyBulletScript[] _bullets;
    /// <summary>
    /// 탄환 발사 지점입니다.
    /// </summary>
    public Transform[] _shotPosition;

    /// <summary>
    /// 
    /// </summary>
    public Transform _ultimateStartPosition;

    /// <summary>
    /// X축 속력입니다.
    /// </summary>
    public float _movingSpeedX = 1;
    /// <summary>
    /// Y축 속력입니다.
    /// </summary>
    public float _movingSpeedY = 2;
    /// <summary>
    /// 궁극기 시전 시 X축 속력입니다.
    /// </summary>
    public float _ultimateSpeedX1 = 5;
    /// <summary>
    /// 궁극기 시전 시 Y축 속력입니다.
    /// </summary>
    public float _ultimateSpeedY1 = 5;

    /// <summary>
    /// 방패를 들어 막는 시간입니다.
    /// </summary>
    public float _guardTime = 3f;
    /// <summary>
    /// 추적 시간입니다.
    /// </summary>
    public float _followTime = 5f;
    /// <summary>
    /// 샷 간격입니다.
    /// </summary>
    public float _shotInterval = 2f;

    /// <summary>
    /// 궁극기 1의 샷 발사 회수 1입니다.
    /// </summary>
    public int _shotCount_1_1 = 4;
    /// <summary>
    /// 궁극기 1의 샷 발사 회수 2입니다.
    /// </summary>
    public int _shotCount_1_2 = 8;
    /// <summary>
    /// 궁극기 2의 샷 발사 회수 1입니다.
    /// </summary>
    public int _shotCount_2_1 = 4;
    /// <summary>
    /// 궁극기 1의 샷 발사 간격입니다.
    /// </summary>
    public float _ultimateInterval1 = 0.3f;
    /// <summary>
    /// 궁극기 2의 샷 발사 간격입니다.
    /// </summary>
    public float _ultimateInterval2 = 0.3f;

    #endregion





    #region 캐릭터의 운동 상태 필드를 정의합니다.
    /// <summary>
    /// 캐릭터가 공격 중이라면 참입니다.
    /// </summary>
    bool _attacking = false;
    /// <summary>
    /// 방패를 들어 막는 중이라면 참입니다.
    /// </summary>
    bool _guarding = false;

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
    /// <summary>
    /// 방패를 들어 막는 중이라면 참입니다.
    /// </summary>
    bool Guarding
    {
        get { return _guarding; }
        set { _Animator.SetBool("Guarding", _guarding = value); }
    }

    /// <summary>
    /// 궁극기가 활성화되었다면 참입니다.
    /// </summary>
    bool UltimateEnabled { get; set; }

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 컬러 팔레트를 설정합니다.
        DefaultPalette = EnemyColorPalette.IntroBossHoverMechPalette;

        // 비행 상태로 변경합니다.
        Flying = true;
        Landed = false;
        StopFalling();

        // 아래로 내려오는 것으로 시작합니다.
        MoveDown();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        // 등장이 끝나지 않았다면
        if (AppearEnded == false)
        {
            // 다음 문장을 수행하지 않습니다.
            return;
        }
        // 전투 중인 상태가 아니라면 
        else if (Fighting == false)
        {
            // 다음 문장을 수행하지 않습니다.
            return;
        }

        //
        if (UltimateEnabled == false && Health <= _dangerHealth)
        {
            // 
            StopMoving();
            StopAttack();
            StopGuarding();

            // 
            if (_coroutineAttack != null)
            {
                StopCoroutine(_coroutineAttack);
                _coroutineAttack = null;
            }
            if (_coroutineGuard != null)
            {
                StopCoroutine(_coroutineGuard);
                _coroutineGuard = null;
            }
            if (_coroutineFollow != null)
            {
                StopCoroutine(_coroutineFollow);
                _coroutineFollow = null;
            }

            // 
            UltimateEnabled = true;
            ReadyUltimate();
        } 
    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    protected override void LateUpdate()
    {
        base.LateUpdate();

        // 색상을 업데이트합니다.
        UpdateColor();
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





    #region EnemyScript의 메서드를 오버라이드합니다.
    /// <summary>
    /// 캐릭터가 사망합니다.
    /// </summary>
    public override void Dead()
    {
        // 모든 탄환을 제거합니다.
        _StageManager.RequestDisableAllEnemy();

        // 개체 대신 놓일 그림을 활성화합니다.
        Vector3 position = transform.position;
        BossDeadEffectScript effect = BossBattleManager.Instance._bossDeadEffect;
        effect.transform.position = position;
        if (FacingRight)
            effect.transform.localScale = new Vector3
                (-effect.transform.localScale.x, effect.transform.localScale.x);
        effect.gameObject.SetActive(true);

        // 플레이어의 움직임을 막습니다.
        _StageManager.RequestBlockMoving();

        // 개체 자신을 제거합니다.
        RequestDestroy();
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
    /// 이동을 중지합니다.
    /// </summary>
    protected override void StopMoving()
    {
        _Velocity = new Vector2(0, 0);

        // 
        Moving = false;
    }

    /// <summary>
    /// 공격합니다.
    /// </summary>
    void Attack()
    {
        Attacking = true;

        // 공격 코루틴을 시작합니다.
        _coroutineAttack = StartCoroutine(CoroutineAttack());
    }
    /// <summary>
    /// 공격을 중지합니다.
    /// </summary>
    void StopAttack()
    {
        Attacking = false;
    }
    /// <summary>
    /// 방패를 들어 공격을 막습니다.
    /// </summary>
    void Guard()
    {
        Guarding = true;
        _guard.gameObject.SetActive(true);

        // 막기 코루틴을 시작합니다.
        _coroutineGuard = StartCoroutine(CoroutineGuard());
    }
    /// <summary>
    /// 막기를 중지합니다.
    /// </summary>
    void StopGuarding()
    {
        Guarding = false;
        _guard.gameObject.SetActive(false);
    }
    /// <summary>
    /// 플레이어를 추적합니다.
    /// </summary>
    void Follow()
    {
        Guarding = false;
        Attacking = false;

        // 추적 코루틴을 시작합니다.
        _coroutineFollow = StartCoroutine(CoroutineFollow());
    }
    /// <summary>
    /// 추적을 중지합니다.
    /// </summary>
    void StopFollowing()
    {
        StopMoving();
    }
    /// <summary>
    /// 방어 상태로 플레이어를 추적합니다.
    /// </summary>
    void GuardFollow()
    {
        Guarding = true;
        Attacking = false;

        // 가드를 활성화합니다.
        _guard.gameObject.SetActive(true);

        // 추적 코루틴을 시작합니다.
        _coroutineFollow = StartCoroutine(CoroutineFollow());
    }
    /// <summary>
    /// 추적을 중지합니다.
    /// </summary>
    void StopGuardFollowing()
    {
        StopGuarding();
        StopMoving();
    }

    /// <summary>
    /// 궁극기를 준비합니다.
    /// </summary>
    void ReadyUltimate()
    {
        StartCoroutine(CoroutineReadyUltimate());
    }
    /// <summary>
    /// 궁극기 1을 시전합니다.
    /// </summary>
    void Ultimate1()
    {
        Guarding = false;

        StartCoroutine(CoroutineUltimate1());
    }
    /// <summary>
    /// 궁극기 2를 시전합니다.
    /// </summary>
    void Ultimate2()
    {
        StartCoroutine(CoroutineUltimate2());
    }
    
    /// <summary>
    /// 전투 시작 액션입니다.
    /// </summary>
    public override void Fight()
    {
        base.Fight();

        // 
        Guard();
    }

    #endregion





    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 특정 지점으로 이동합니다.
    /// </summary>
    /// <param name="t">이동할 지점입니다.</param>
    void MoveTo(Transform t)
    {
        // 사용할 변수를 선언합니다.
        Vector2 relativePos = t.position - transform.position;

        // 플레이어를 향해 수평 방향 전환합니다.
        if (relativePos.x < 0)
        {
            MoveLeft();
        }
        else if (relativePos.x > 0)
        {
            MoveRight();
        }
        // 플레이어를 향해 수직 방향 전환합니다.
        if (relativePos.y > 0)
        {
            MoveUp();
        }
        else if (relativePos.y < 0)
        {
            MoveDown();
        }
    }
    /// <summary>
    /// 플레이어를 향해 이동합니다.
    /// </summary>
    private void MoveToPlayer()
    {
        // 사용할 변수를 선언합니다.
        Vector3 playerPos = _StageManager.GetCurrentPlayerPosition();
        Vector2 relativePos = playerPos - transform.position;

        // 플레이어를 향해 수평 방향 전환합니다.
        if (relativePos.x < 0)
        {
            MoveLeft();
        }
        else if (relativePos.x > 0)
        {
            MoveRight();
        }
        // 플레이어를 향해 수직 방향 전환합니다.
        if (relativePos.y > 0)
        {
            MoveUp();
        }
        else if (relativePos.y < 0)
        {
            MoveDown();
        }
    }
    /// <summary>
    /// 탄환을 발사합니다.
    /// </summary>
    /// <param name="shotPosition">탄환을 발사할 위치입니다.</param>
    public void Shot(Transform shotPosition)
    {
        SoundEffects[1].Play();
        GameObject effect = Instantiate(effects[1], shotPosition.position, shotPosition.rotation);
        if (FacingRight)
        {
            Vector3 scale = effect.transform.localScale;
            effect.transform.localScale = new Vector3(-scale.x, scale.y);
        }

        // 
        EnemyBulletScript bullet = Instantiate
            (_bullets[0], shotPosition.position, shotPosition.rotation)
            as EnemyBulletScript;
        bullet.transform.parent = _StageManager._enemyParent.transform;

        // 
        bullet.FacingRight = FacingRight;
        bullet.MoveTo(_StageManager.GetCurrentPlayerPosition());
    }
    /// <summary>
    /// 탄환을 발사합니다.
    /// </summary>
    /// <param name="shotPosition">탄환을 발사할 위치입니다.</param>
    /// <param name="destination">탄환의 목적지입니다.</param>
    public void Shot(Transform shotPosition, Vector3 destination)
    {
        SoundEffects[1].Play();
        GameObject effect = Instantiate(effects[1], shotPosition.position, shotPosition.rotation);
        if (FacingRight)
        {
            Vector3 scale = effect.transform.localScale;
            effect.transform.localScale = new Vector3(-scale.x, scale.y);
        }

        // 
        EnemyBulletScript bullet = Instantiate
            (_bullets[0], shotPosition.position, shotPosition.rotation)
            as EnemyBulletScript;
        bullet.transform.parent = _StageManager._enemyParent.transform;

        // 
        bullet.FacingRight = FacingRight;
        bullet.MoveTo(destination);
    }

    /// <summary>
    /// 막기 다음 액션을 수행합니다.
    /// </summary>
    void PerformActionAfterGuard()
    {
        Attack();
    }
    /// <summary>
    /// 공격 다음 액션을 수행합니다.
    /// </summary>
    void PerformActionAfterAttack()
    {
        if (UltimateEnabled)
        {
            GuardFollow();
        }
        else
        {
            Follow();
        }
    }
    /// <summary>
    /// 추적 다음 액션을 수행합니다.
    /// </summary>
    void PerformActionAfterFollow()
    {
        if (UltimateEnabled)
        {
            ReadyUltimate();
        }
        else
        {
            Guard();
        }
    }
    /// <summary>
    /// 궁극기를 시전한 다음 액션을 수행합니다.
    /// </summary>
    void PerformActionAfterUltimate()
    {
        Guard();
    }

    #endregion





    #region 코루틴 메서드를 정의합니다.
    /// <summary>
    /// 공격 코루틴입니다.
    /// </summary>
    Coroutine _coroutineAttack;
    /// <summary>
    /// 방어 코루틴입니다.
    /// </summary>
    Coroutine _coroutineGuard;
    /// <summary>
    /// 추적 코루틴입니다.
    /// </summary>
    Coroutine _coroutineFollow;
    
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

        // 탄환을 세 번 발사합니다.
        Shot(_shotPosition[0]);
        yield return new WaitForSeconds(_shotInterval);
        Shot(_shotPosition[0]);
        yield return new WaitForSeconds(_shotInterval);
        Shot(_shotPosition[0]);
        yield return new WaitForSeconds(_shotInterval);

        // 공격을 끝냅니다.
        Attacking = false;
        PerformActionAfterAttack();
        _coroutineAttack = null;
        yield break;
    }
    /// <summary>
    /// 막기 코루틴입니다.
    /// </summary>
    IEnumerator CoroutineGuard()
    {
        float time = 0;
        while (time < _guardTime)
        {
            yield return false;
            time += Time.deltaTime;
        }

        // 막기 상태를 끝냅니다.
        StopGuarding();
        PerformActionAfterGuard();
        _coroutineGuard = null;
        yield break;
    }
    /// <summary>
    /// 추적 코루틴입니다.
    /// </summary>
    IEnumerator CoroutineFollow()
    {
        float time = 0;
        while (time < _followTime)
        {
            MoveToPlayer();
            time += Time.deltaTime;
            yield return false;
        }

        // 막기 상태를 끝냅니다.
        StopFollowing();
        PerformActionAfterFollow();
        _coroutineFollow = null;
        yield break;
    }

    /// <summary>
    /// 궁극기를 준비합니다.
    /// </summary>
    IEnumerator CoroutineReadyUltimate()
    {
        float originalSpeedX = _movingSpeedX;
        float originalSpeedY = _movingSpeedY;

        _movingSpeedX = _ultimateSpeedX1;
        _movingSpeedY = _ultimateSpeedY1;

        MoveTo(_ultimateStartPosition);
        while (true)
        {
            // 
            Vector3 newPos = transform.position;
            Vector3 ultPos = _ultimateStartPosition.position;

            // 
            if (newPos.x > ultPos.x)
                newPos.x = ultPos.x;
            if (newPos.y < ultPos.y)
                newPos.y = ultPos.y;
            /// Handy.Log("NewPos={0}, UltPos={1}", newPos, ultPos);
            transform.position = newPos;

            // 
            if (Vector3.Distance(newPos, ultPos) < 0.1f)
            {
                break;
            }

            // 
            yield return false;
        }
        StopMoving();
        StopGuarding();

        // 
        _movingSpeedX = originalSpeedX;
        _movingSpeedY = originalSpeedY;
        Ultimate1();
        yield break;
    }
    /// <summary>
    /// 궁극기 1을 시전합니다.
    /// </summary>
    private IEnumerator CoroutineUltimate1()
    {
        float originalSpeedX = _movingSpeedX;
        float originalSpeedY = _movingSpeedY;

        // 
        _movingSpeedX = _ultimateSpeedX1;
        _movingSpeedY = _ultimateSpeedY1;
        if (FacingRight)
            Flip();

        // 
        MoveUp();
        for (int i = 0; i < _shotCount_1_1; ++i)
        {
            Shot(_shotPosition[1], transform.position - new Vector3(100, 100));
            yield return new WaitForSeconds(_ultimateInterval1);
        }
        StopMoving();

        // 
        MoveLeft();
        for (int i = 0; i < _shotCount_1_2; ++i)
        {
            Shot(_shotPosition[1], transform.position - new Vector3(100, 100));
            yield return new WaitForSeconds(_ultimateInterval1);
        }
        StopMoving();

        // 궁극기를 끝냅니다.
        _movingSpeedX = originalSpeedX;
        _movingSpeedY = originalSpeedY;
        Ultimate2();
        yield break;
    }
    /// <summary>
    /// 궁극기 2를 시전합니다.
    /// </summary>
    private IEnumerator CoroutineUltimate2()
    {
        float originalSpeedX = _movingSpeedX;
        float originalSpeedY = _movingSpeedY;

        // 
        _movingSpeedX = _ultimateSpeedX1;
        _movingSpeedY = _ultimateSpeedY1;

        Flip();
        MoveDown();
        for (int i = 0; i < _shotCount_2_1; ++i)
        {
            Shot(_shotPosition[1], transform.position + new Vector3(100, 0));
            yield return new WaitForSeconds(_ultimateInterval1);
        }
        StopMoving();

        // 궁극기를 끝냅니다.
        _movingSpeedX = originalSpeedX;
        _movingSpeedY = originalSpeedY;
        PerformActionAfterUltimate();
        yield break;
    }

    #endregion





    #region 구형 정의를 보관합니다.


    #endregion
}