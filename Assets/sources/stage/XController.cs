using UnityEngine;
using System.Collections;
using System;

public class XController : PlayerController
{
    #region 효과 객체를 보관합니다.
    GameObject dashBoostEffect = null;

    #endregion


    #region MonoBehavior 기본 메서드를 재정의합니다.
    void Start()
    {
        Initialize();
    }
    void Update()
    {
        UpdateState();

        // 소환 중이라면
        if (Spawning)
        {
            // 준비 중이라면
            if (Readying)
            {
                // 준비가 끝나서 대기 상태로 전환되었다면
                if (IsAnimationPlaying("Idle"))
                    // 준비를 완전히 종료합니다.
                    EndReady();
            }
            // 준비 중이 아닌데 지상에 착륙했다면
            else if (Landed)
            {
                // 준비 상태로 전환합니다.
                Ready();
            }
            return;
        }

        // 새로운 사용자 입력을 확인합니다.
        if (IsKeyDown(GameKey.Jump))
        {
            if (JumpBlocked)
            {

            }
            else if (Pushing)
            {
                if (IsKeyPressed(GameKey.Dash))
                {
                    WallDashJump();
                }
                else
                {
                    WallJump();
                }
            }
            else if (Dashing)
            {
                DashJump();
            }
            else if (Landed && IsKeyPressed(GameKey.Dash))
            {
                DashJump();
            }
            else
            {
                Jump();
            }
        }
        else if (IsKeyDown(GameKey.Dash))
        {
            if (DashBlocked)
            {

            }
            else if (Landed == false)
            {
                AirDash();
            }
            else
            {
                Dash();

            }
        }

        // 기존 사용자 입력을 확인합니다.
        // 점프 중이라면
        if (Jumping)
        {
            if (Landed)
            {
                if (_rigidbody.velocity.y != 16f)
                {
                    Land();
                }
            }
            else if (Pushing)
            {
                if (SlideBlocked)
                {

                }
                else
                {
                    Slide();
                }
            }
            else if (IsKeyPressed(GameKey.Jump) == false
                || _rigidbody.velocity.y <= 0)
            {
                Fall();
            }
            else
            {
                _rigidbody.velocity = new Vector2
                    (_rigidbody.velocity.x, _rigidbody.velocity.y - jumpDecSize);
            }
        }
        // 떨어지고 있다면
        else if (Falling)
        {
            if (Landed)
            {
//                StopFalling();
                Land();
            }
            else if (Pushing)
            {
                Slide();
            }
            else
            {
                float vy = _rigidbody.velocity.y - jumpDecSize;
                _rigidbody.velocity = new Vector2
                    (_rigidbody.velocity.x, vy > -16 ? vy : -16);
            }
        }
        // 대쉬 중이라면
        else if (Dashing)
        {
            if (Landed == false)
            {
                StopDashing();
                Fall();
            }
            else if (IsKeyPressed(GameKey.Dash) == false)
            {
                StopDashing();
            }
        }
        // 벽을 타고 있다면
        else if (Sliding)
        {
            if (Pushing == false)
            {
                StopSliding();
                Fall();
            }
            else if (Landed)
            {
                StopSliding();
                Fall();
//                Land();
            }
        }
        // 벽을 밀고 있다면
        else if (Pushing)
        {
            if (SlideBlocked)
            {

            }
            else
            {

            }
        }
        // 그 외의 경우
        else
        {
            if (Landed == false)
            {
                Fall();
            }
        }

        // 방향 키 입력에 대해 처리합니다.
        // 대쉬 중이라면
        if (Dashing)
        {
            if (Landed == false)
            {
                if (IsKeyPressed(GameKey.Left))
                {
                    MoveLeft();
                }
                else if (IsKeyPressed(GameKey.Right))
                {
                    MoveRight();
                }
                else
                {
                    StopMoving();
                }
            }
        }
        // 움직임이 막힌 상태라면
        else if (MoveBlocked)
        {

        }
        // 벽 점프 중이라면
        else if (SlideBlocked)
        {

        }
        // 그 외의 경우
        else
        {
            if (IsKeyPressed(GameKey.Left))
            {
                if (FacingRight == false && Pushing)
                {
                    StopMoving();
                }
                else
                {
                    MoveLeft();
                }
            }
            else if (IsKeyPressed(GameKey.Right))
            {
                if (FacingRight && Pushing)
                {
                    StopMoving();
                }
                else
                {
                    MoveRight();
                }
            }
            else
            {
                StopMoving();
            }
        }
    }

    #endregion



    #region 플레이어의 행동 메서드를 정의합니다.
    ///////////////////////////////////////////////////////////////////
    // 기본
    /// <summary>
    /// 플레이어를 소환합니다.
    /// </summary>
    protected override void Spawn()
    {
        base.Spawn();
        SoundEffects[0].Play();
    }
    /// <summary>
    /// 플레이어가 지상에 착륙할 때의 상태를 설정합니다.
    /// </summary>
    protected override void Land()
    {
        base.Land();

        if (Dashing)
        {
            StopDashing();
        }
        SoundEffects[2].Play();
    }

    ///////////////////////////////////////////////////////////////////
    // 점프 및 낙하
    /// <summary>
    /// 플레이어를 점프하게 합니다.
    /// </summary>
    protected override void Jump()
    {
        base.Jump();
        SoundEffects[1].Play();
    }

    ///////////////////////////////////////////////////////////////////
    // 대쉬
    /// <summary>
    /// 플레이어가 대쉬하게 합니다.
    /// </summary>
    protected override void Dash()
    {
        base.Dash();

        // 대쉬 효과 애니메이션을 추가합니다.
        GameObject dashFog = Instantiate(effects[0], dashFogPosition.position, dashFogPosition.rotation) as GameObject;
        if (FacingRight == false)
        {
            var newScale = dashFog.transform.localScale;
            newScale.x = FacingRight ? newScale.x : -newScale.x;
            dashFog.transform.localScale = newScale;
        }
        SoundEffects[3].Play();
    }
    /// <summary>
    /// 플레이어의 대쉬를 중지합니다. (사용자의 입력에 의함)
    /// </summary>
    protected override void StopDashing()
    {
        base.StopDashing();
        if (dashBoostEffect != null)
        {
            dashBoostEffect.GetComponent<EffectScript>().RequestEnd();
            dashBoostEffect = null;
        }
    }

    protected override void DashJump()
    {
        base.DashJump();

        SoundEffects[3].Stop();
        SoundEffects[1].Play();
        if (dashBoostEffect != null)
        {
            dashBoostEffect.GetComponent<EffectScript>().RequestEnd();
            dashBoostEffect = null;
        }
    }

    #endregion



    #region 프레임 이벤트 핸들러를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    protected void FE_WallJumpBeg()
    {
//        BlockSliding();
    }
    /// <summary>
    /// 
    /// </summary>
    protected void FE_WallJumpEnd()
    {
        UnblockSliding();
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }
    /// <summary>
    /// 
    /// </summary>
    protected void FE_DashRunBeg()
    {
        GameObject dashBoost = Instantiate(effects[1], dashBoostPosition.position, dashBoostPosition.rotation) as GameObject;
        dashBoost.transform.SetParent(groundCheck.transform);
        if (FacingRight == false)
        {
            var newScale = dashBoost.transform.localScale;
            newScale.x = FacingRight ? newScale.x : -newScale.x;
            dashBoost.transform.localScale = newScale;
        }

        dashBoostEffect = dashBoost;
    }
    /// <summary>
    /// 플레이어의 대쉬 상태를 종료하도록 요청합니다.
    /// </summary>
    protected void FE_DashRunEnd()
    {
        StopDashing();
    }
    /// <summary>
    /// 대쉬가 사용자에 의해 중지될 때 발생합니다.
    /// </summary>
    public void FE_DashEndBeg()
    {
        StopMoving();
        BlockMoving();
        BlockJumping();
        BlockDashing();

        SoundEffects[3].Stop();
        SoundEffects[4].Play();
    }
    /// <summary>
    /// 대쉬 점프 모션이 사용자에 의해 완전히 중지되어 대기 상태로 바뀔 때 발생합니다.
    /// </summary>
    public void FE_DashEndEnd()
    {
        UnblockMoving();
        UnblockJumping();
        UnblockDashing();
        //        StopDashing();
    }

    #endregion



    #region 보조 메서드를 정의합니다.

    #endregion
}
