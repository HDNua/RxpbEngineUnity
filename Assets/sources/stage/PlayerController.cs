﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PlayerController : MonoBehaviour
{
    #region 컨트롤러가 사용할 공용 형식 또는 값을 정의합니다.
    protected enum GameKey
    {
        Up, Left, Right, Down,
        Jump, Dash, Attack, Weapon, GigaAttack,
        ChangeWeaponLeft, ChangeWeaponRight
    }
    protected static Dictionary<GameKey, KeyCode> GameKeySet;

    #endregion

    #region 컨트롤러가 사용할 Unity 객체에 대한 접근자를 정의합니다.
    protected Rigidbody2D _rigidbody { get { return GetComponent<Rigidbody2D>(); } }
    protected Animator _animator { get { return GetComponent<Animator>(); } }

    #endregion



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public AudioClip[] audioClips;

    public Transform groundCheck;
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public float groundCheckRadius = 0.5f;
    public LayerMask whatIsGround;

    public Transform pushCheck;
    public float pushCheckRadius = 0.1f;
    public LayerMask whatIsWall;

    public float walkSpeed = 5;
    public float jumpSpeed = 16;
    public float jumpDecSize = 0.8f;
    public float slideSpeed = 4f;
    public float spawnSpeed = 16;
    public float dashSpeed = 10;

    public GameObject[] effects;
    public Transform dashFogPosition;
    public Transform dashBoostPosition;
    public Transform slideFogPosition;
    public GameObject slideFogEffect;

    #endregion



    #region Unity를 통해 초기화한 속성을 사용 가능한 형태로 보관합니다.
    /// <summary>
    /// 효과음의 리스트입니다.
    /// </summary>
    AudioSource[] soundEffects;
    /// <summary>
    /// 효과음의 리스트를 반환합니다.
    /// </summary>
    public AudioSource[] SoundEffects { get { return soundEffects; } }

    #endregion



    #region 상태 또는 입력을 확인하는 보조 메서드를 정의합니다.
    /// <summary>
    /// 키가 눌렸는지 확인합니다.
    /// </summary>
    /// <param name="key">상태를 확인할 키입니다.</param>
    /// <returns>키가 눌렸다면 true를 반환합니다.</returns>
    protected bool IsKeyDown(GameKey key)
    {
        return Input.GetKeyDown(GameKeySet[key]);
    }
    /// <summary>
    /// 키가 계속 눌린 상태인지 확인합니다.
    /// </summary>
    /// <param name="key">눌린 상태인지 확인할 키입니다.</param>
    /// <returns>키가 눌린 상태라면 true를 반환합니다.</returns>
    protected bool IsKeyPressed(GameKey key)
    {
        return Input.GetKey(GameKeySet[key]);
    }
    /// <summary>
    /// 애니메이션이 재생 중인지 확인합니다.
    /// </summary>
    /// <param name="stateName">재생 중인지 확인하려는 애니메이션의 이름입니다.</param>
    /// <returns>애니메이션이 재생 중이라면 true를 반환합니다.</returns>
    protected bool IsAnimationPlaying(string stateName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    #endregion



    #region 주인공의 게임 상태 필드를 정의합니다.
    int hitPoint;
    int maxHitPoint;

    #endregion



    #region 주인공의 운동 상태 필드를 정의합니다.
    protected float movingSpeed = 5;
    bool _facingRight = true;
    bool _spawning = true;
    bool _readying = false;
    bool _landed = false;
    bool _moving = false;
    bool _pushing = false;
    bool _jumping = false;
    bool _falling = false;
    bool _dashing = false;
    bool _sliding = false;

    /*
    bool _moveBlocked = false;
    bool _jumpBlocked = false;
    bool _dashBlocked = false;
    bool _airDashBlocked = false;
    */

    int airDashCount = 0;

    #endregion



    #region 플레이어의 상태에 관한 프로퍼티 및 메서드를 정의합니다.
    /// <summary>
    /// 플레이어가 오른쪽을 향하고 있다면 true입니다.
    /// </summary>
    public bool FacingRight { get { return _facingRight; } }
    /// <summary>
    /// 플레이어가 소환중이라면 true입니다.
    /// </summary>
    protected bool Spawning { get { return _spawning; } }
    /// <summary>
    /// 플레이어가 준비중이라면 true입니다.
    /// </summary>
    protected bool Readying { get { return _readying; } }
    /// <summary>
    /// 지상에 있다면 true입니다.
    /// </summary>
    protected bool Landed
    {
        get { return _landed; }
        set { _animator.SetBool("Landed", _landed = value); }
    }
    /// <summary>
    /// 지상에서 이동하고 있다면 true입니다.
    /// </summary>
    protected bool Moving
    {
        get { return _moving; }
        set { _animator.SetBool("Moving", _moving = value); }
    }
    /// <summary>
    /// 벽을 밀고 있다면 true입니다.
    /// </summary>
    protected bool Pushing
    {
        get { return _pushing; }
        set { _animator.SetBool("Pushing", _pushing = value); }
    }
    /// <summary>
    /// 점프 상태라면 true입니다.
    /// </summary>
    protected bool Jumping
    {
        get { return _jumping; }
        set { _animator.SetBool("Jumping", _jumping = value); }
    }
    /// <summary>
    /// 떨어지고 있다면 true입니다.
    /// </summary>
    protected bool Falling
    {
        get { return _falling; }
        set { _animator.SetBool("Falling", _falling = value); }
    }
    /// <summary>
    /// 지상에서 대쉬 중이라면 true입니다.
    /// </summary>
    protected bool Dashing
    {
        get { return _dashing; }
        set { _animator.SetBool("Dashing", _dashing = value); }
    }
    /// <summary>
    /// 벽을 타고 있다면 true입니다.
    /// </summary>
    protected bool Sliding
    {
        get { return _sliding; }
        set { _animator.SetBool("Sliding", _sliding = value); }
    }

    protected bool MoveBlocked { get; set; }
    protected bool JumpBlocked { get; set; }
    protected bool DashBlocked { get; set; }
    protected bool SlideBlocked { get; set; }
    protected bool AirDashBlocked { get; set; }

    protected bool DashJumping { get; set; }
    protected bool WallJumping { get; set; }
    protected bool WallDashJumping { get; set; }
    protected bool AirDashing { get; set; }

    /// <summary>
    /// 플레이어가 죽었는지 확인합니다.
    /// </summary>
    /// <returns>체력이 0 이하라면 true입니다.</returns>
    public bool IsDead()
    {
        return (hitPoint <= 0);
    }
    /// <summary>
    /// 플레이어가 생존해있는지 확인합니다.
    /// </summary>
    /// <returns>체력이 정상 범위에 있다면 true입니다.</returns>
    public bool IsAlive()
    {
        return (0 < hitPoint && hitPoint <= maxHitPoint);
    }
    /// <summary>
    /// 플레이어의 체력이 가득 찼는지 확인합니다.
    /// </summary>
    /// <returns>체력이 가득 찼다면 true입니다.</returns>
    public bool IsHealthFull()
    {
        return (hitPoint == maxHitPoint);
    }

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    /// <summary>
    /// PlayerController 클래스의 필드를 초기화합니다.
    /// </summary>
    protected void Initialize()
    {
        // 효과음 리스트를 초기화 합니다.
        soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].clip = audioClips[i];
        }

        // 키 딕셔너리를 초기화 합니다.
        GameKeySet = new Dictionary<GameKey, KeyCode>();
        GameKeySet[GameKey.Left] = KeyCode.LeftArrow;
        GameKeySet[GameKey.Right] = KeyCode.RightArrow;
        GameKeySet[GameKey.Jump] = KeyCode.C;
        GameKeySet[GameKey.Attack] = KeyCode.X;
        GameKeySet[GameKey.Dash] = KeyCode.Z;
        GameKeySet[GameKey.Weapon] = KeyCode.V;
    }

    #endregion



    #region 플레이어의 상태를 갱신합니다.
    /// <summary>
    /// 플레이어의 물리 상태를 갱신합니다.
    /// </summary>
    protected void UpdateState()
    {
        bool leftLanded = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, groundCheckRadius, whatIsGround);
        bool rightLanded = Physics2D.Raycast(groundCheckRight.position, Vector2.down, groundCheckRadius, whatIsGround);
        Landed = leftLanded || rightLanded;

        bool pushing = Physics2D.Raycast(pushCheck.position, _facingRight ? Vector2.right : Vector2.left, pushCheckRadius, whatIsWall);
        Pushing = pushing && (_facingRight ? IsKeyPressed(GameKey.Right) : IsKeyPressed(GameKey.Left));
    }

    #endregion



    #region 일회성 행동 메서드를 정의합니다.
    /// <summary>
    /// 플레이어를 소환합니다.
    /// </summary>
    protected virtual void Spawn()
    {
        _spawning = true;
        _rigidbody.velocity = new Vector2(0, -spawnSpeed);
    }
    /// <summary>
    /// 플레이어를 소환 상태에서 준비 상태로 전환합니다.
    /// </summary>
    protected void Ready()
    {
        _readying = true;
    }
    /// <summary>
    /// 소환 및 준비를 완료합니다.
    /// </summary>
    protected void EndReady()
    {
        _readying = false;
        _spawning = false;
    }

    #endregion



    #region 플레이어의 행동 메서드를 정의합니다.
    ///////////////////////////////////////////////////////////////////
    // 기본
    /// <summary>
    /// 플레이어가 지상에 착륙할 때의 상태를 설정합니다.
    /// </summary>
    protected virtual void Land()
    {
        StopJumping();
        StopFalling();
    }

    ///////////////////////////////////////////////////////////////////
    // 이동
    /// <summary>
    /// 플레이어를 왼쪽으로 이동합니다.
    /// </summary>
    protected void MoveLeft()
    {
        if (_facingRight)
            Flip();
        _rigidbody.velocity = new Vector2(-movingSpeed, _rigidbody.velocity.y);
        Moving = true;
    }
    /// <summary>
    /// 플레이어를 오른쪽으로 이동합니다.
    /// </summary>
    protected void MoveRight()
    {
        if (_facingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(movingSpeed, _rigidbody.velocity.y);
        Moving = true;
    }
    /// <summary>
    /// 플레이어의 이동을 중지합니다.
    /// </summary>
    protected void StopMoving()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        Moving = false;
    }
    /// <summary>
    /// 플레이어의 이동 요청을 막습니다.
    /// </summary>
    protected void BlockMoving()
    {
        MoveBlocked = true;
    }
    /// <summary>
    /// 플레이어가 이동을 요청할 수 있도록 합니다.
    /// </summary>
    protected void UnblockMoving()
    {
        MoveBlocked = false;
    }
    ///////////////////////////////////////////////////////////////////
    // 점프 및 낙하
    /// <summary>
    /// 플레이어를 점프하게 합니다.
    /// </summary>
    protected virtual void Jump()
    {
        BlockJumping();
        BlockDashing();

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
        Jumping = true;
    }
    /// <summary>
    /// 플레이어의 점프를 중지합니다.
    /// </summary>
    protected void StopJumping()
    {
        UnblockJumping();
        UnblockDashing();
        Jumping = false;
    }
    /// <summary>
    /// 플레이어의 점프 요청을 막습니다.
    /// </summary>
    protected void BlockJumping()
    {
        JumpBlocked = true;
    }
    /// <summary>
    /// 플레이어가 점프할 수 있도록 합니다.
    /// </summary>
    protected void UnblockJumping()
    {
        JumpBlocked = false;
    }
    /// <summary>
    /// 플레이어를 낙하시킵니다.
    /// </summary>
    protected void Fall()
    {
        BlockJumping();
        BlockDashing();

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        Jumping = false;
        Falling = true;
    }
    /// <summary>
    /// 플레이어의 낙하를 중지합니다.
    /// </summary>
    protected void StopFalling()
    {
        UnblockJumping();
        UnblockDashing();

        Falling = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 대쉬
    /// <summary>
    /// 플레이어가 대쉬하게 합니다.
    /// </summary>
    protected virtual void Dash()
    {
        BlockMoving();
        BlockDashing();
//        BlockAirDashing();

        movingSpeed = dashSpeed;
        float vx = _facingRight ? dashSpeed : -dashSpeed;
        _rigidbody.velocity = new Vector2(vx, _rigidbody.velocity.y);
//        SoundEffects[3].Play();
        Dashing = true;
    }
    /// <summary>
    /// 플레이어의 대쉬를 중지합니다. (사용자의 입력에 의함)
    /// </summary>
    protected virtual void StopDashing()
    {
        UnblockMoving();
        UnblockDashing();
//        UnblockAirDashing();

        movingSpeed = walkSpeed;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        Dashing = false;
    }
    /// <summary>
    /// 플레이어의 대쉬 요청을 막습니다.
    /// </summary>
    protected void BlockDashing()
    {
        DashBlocked = true;
    }
    /// <summary>
    /// 플레이어가 대쉬할 수 있도록 합니다.
    /// </summary>
    protected void UnblockDashing()
    {
        DashBlocked = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 벽 타기
    /// <summary>
    /// 플레이어가 벽을 타도록 합니다.
    /// </summary>
    protected void Slide()
    {
        Sliding = true;

        StopMoving();
        StopJumping();
        StopFalling();
        StopDashing();
        ClearAirDashCount();

        GameObject slideFog = Instantiate(effects[2], slideFogPosition.position, slideFogPosition.rotation) as GameObject;
        slideFog.transform.SetParent(groundCheck.transform);
        if (FacingRight == false)
        {
            var newScale = slideFog.transform.localScale;
            newScale.x = FacingRight ? newScale.x : -newScale.x;
            slideFog.transform.localScale = newScale;
        }
        slideFogEffect = slideFog;

        BlockDashing();
        UnblockJumping();
        _rigidbody.velocity = new Vector2(0, -slideSpeed);
    }
    /// <summary>
    /// 플레이어의 벽 타기를 중지합니다.
    /// </summary>
    protected void StopSliding()
    {
        if (slideFogEffect != null)
        {
            slideFogEffect.GetComponent<EffectScript>().RequestEnd();
            slideFogEffect = null;
        }

        UnblockDashing();
        Sliding = false;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
    }
    /// <summary>
    /// 플레이어의 벽 타기 요청을 막습니다.
    /// </summary>
    protected void BlockSliding()
    {
        SlideBlocked = true;
    }
    /// <summary>
    /// 플레이어가 벽 타기할 수 있도록 합니다.
    /// </summary>
    protected void UnblockSliding()
    {
        SlideBlocked = false;
    }

    ///////////////////////////////////////////////////////////////////
    // 사다리 타기
    /// <summary>
    /// 플레이어가 사다리를 타도록 합니다.
    /// </summary>
    protected void RideLadder()
    {
        ClearAirDashCount();

    }

    ///////////////////////////////////////////////////////////////////
    // 조합
    /// <summary>
    /// 플레이어가 벽 점프를 합니다.
    /// </summary>
    protected void WallJump()
    {
        StopJumping();
        StopFalling();
        StopSliding();

        GameObject wallKick = Instantiate(effects[3], slideFogPosition.position, slideFogPosition.rotation) as GameObject;
        _rigidbody.velocity = new Vector2(FacingRight ? -1.5f * movingSpeed : 1.5f * movingSpeed, jumpSpeed /* Mathf.Sqrt(2) */);

        Jumping = true;
        BlockJumping();
        BlockSliding();
        BlockDashing();
        BlockAirDashing();
    }
    /// <summary>
    /// 플레이어가 대쉬 점프하게 합니다.
    /// </summary>
    protected virtual void DashJump()
    {
        StopJumping();
        StopFalling();
        StopSliding();

        //        BlockMoving();
        BlockDashing();
        BlockJumping();

        movingSpeed = dashSpeed;
        _rigidbody.velocity = new Vector2(FacingRight ? movingSpeed : -movingSpeed, jumpSpeed);

        Jumping = true;
        Dashing = true;
        DashJumping = true;
    }
    /// <summary>
    /// 플레이어가 벽에서 대쉬 점프하게 합니다.
    /// </summary>
    protected void WallDashJump()
    {
        StopJumping();
        StopFalling();
        StopSliding();

        GameObject wallKick = Instantiate(effects[3], slideFogPosition.position, slideFogPosition.rotation) as GameObject;

        movingSpeed = dashSpeed;
        _rigidbody.velocity = new Vector2(FacingRight ? -1.5f * movingSpeed : 1.5f * movingSpeed, jumpSpeed / Mathf.Sqrt(2));
        Jumping = true;

//        ++_airDashCount;
        DashJumping = true;
        BlockJumping();
        BlockDashing();
    }
    /// <summary>
    /// 플레이어가 에어 대쉬하게 합니다.
    /// </summary>
    protected void AirDash()
    {
        StopJumping();
        StopFalling();
        StopSliding();

        _rigidbody.velocity = new Vector2(FacingRight ? dashSpeed : -dashSpeed, 0);
        AirDashing = true;

        BlockMoving();
        ++airDashCount;
    }
    /// <summary>
    /// 플레이어의 에어 대쉬를 중지합니다.
    /// </summary>
    protected void StopAirDashing()
    {
        UnblockMoving();
        AirDashing = false;
    }
    /// <summary>
    /// 플레이어의 에어 대쉬 요청을 막습니다.
    /// </summary>
    protected void BlockAirDashing()
    {
        AirDashBlocked = true;
    }
    /// <summary>
    /// 플레이어가 에어 대쉬할 수 있도록 합니다.
    /// </summary>
    protected void UnblockAirDashing()
    {
        AirDashBlocked = false;
    }


    #endregion



    #region 외부에서 요청 가능한 행동 메서드를 정의합니다.
    /// <summary>
    /// 플레이어 소환을 요청합니다.
    /// </summary>
    public void RequestSpawn()
    {
        this.enabled = true;
        Spawn();
    }

    #endregion


    #region 요청을 수행하기 위한 보조 메서드를 정의합니다.
    /// <summary>
    /// 플레이어의 방향을 바꿉니다.
    /// </summary>
    protected void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    /// <summary>
    /// 에어 대쉬 카운트를 초기화 합니다.
    /// </summary>
    protected void ClearAirDashCount()
    {
//        _airDashCount = 0;
    }

    #endregion 보조 메서드 정의
}