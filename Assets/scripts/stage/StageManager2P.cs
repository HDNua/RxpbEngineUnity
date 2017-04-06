using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



/// <summary>
/// 협동 모드 스테이지 장면 관리자입니다.
/// </summary>
public class StageManager2P : StageManager
{
    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    /// <summary>
    /// 플레이어 집합입니다.
    /// </summary>
    public PlayerController[] _players;

    #endregion





    #region Unity 개체에 대한 참조를 보관합니다.
    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    new public static StageManager2P Instance
    {
        get
        {
            return GameObject.FindGameObjectWithTag("StageManager")
                .GetComponent<StageManager2P>();
        }
    }

    /// <summary>
    /// 주 플레이어 개체입니다.
    /// </summary>
    public override PlayerController MainPlayer { get { return _players[0]; } }
    /// <summary>
    /// 부 플레이어 개체입니다.
    /// </summary>
    public PlayerController SubPlayer { get { return _players[1]; } }

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        MainPlayer._playerIndex = 0;
        SubPlayer._playerIndex = 1;

        Collider2D mainCollider = MainPlayer.GetComponent<Collider2D>();
        Collider2D subCollider = SubPlayer.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(mainCollider, subCollider, true);
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

    #endregion





    #region 기능 메서드를 정의합니다.
    /// <summary>
    /// 플레이어의 체력을 회복합니다.
    /// </summary>
    protected override void HealPlayer()
    {
        foreach (var player in _players)
        {
            while (player.IsHealthFull() == false)
            {
                player.Heal();
            }
        }
    }
    /// <summary>
    /// 플레이어에게 대미지를 입힙니다.
    /// </summary>
    protected override void HurtPlayer()
    {
        foreach (var player in _players)
        {
            player.Hurt(TestDamageValue);
        }
    }

    #endregion





    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 스테이지 종료 아이템 획득 코루틴입니다.
    /// </summary>
    protected override IEnumerator CoroutineClearStage()
    {
        // 다음 커밋에서 삭제할 예정입니다.
        AudioSource audioSource = AudioSources[5];
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return false;
        }

        // 
        foreach (var player in _players)
            player.RequestCeremony();
        yield return new WaitForSeconds(0.2f);
        AudioSources[7].Play();
        while (MainPlayer.IsAnimationPlaying("Ceremony"))
        {
            yield return false;
        }

        // 
        while (MainPlayer.IsAnimationPlaying("Ceremony"))
        {
            yield return false;
        }

        AudioSources[13].Play();
        while (MainPlayer.IsAnimationPlaying("ReturnBeg"))
        {
            yield return false;
        }

        float time = 0;
        while (MainPlayer.IsAnimationPlaying("ReturnRun"))
        {
            time += Time.deltaTime;

            if (time > _returningTime)
                break;

            yield return false;
        }

        //
        GameEnded = true;
        
        _fader.FadeOut();
        yield break;
    }

    /// <summary>
    /// 현재 조작중인 플레이어의 위치를 반환합니다.
    /// </summary>
    /// <returns>현재 조작중인 플레이어의 위치입니다.</returns>
    public override Vector3 GetCurrentPlayerPosition()
    {
        PlayerController target;
        if (MainPlayer.IsAlive())
            target = MainPlayer;
        else
            target = _players[1];

        // 
        return target.transform.position;
    }

    #endregion





    #region 메서드를 정의합니다.    
    /// <summary>
    /// HUD를 활성화합니다.
    /// </summary>
    public override void EnableHUD()
    {
        _userInterfaceManager.ActivateMainPlayerHUD();
        _userInterfaceManager.ActivateSubPlayerHUD();
    }

    /// <summary>
    /// 플레이어의 움직임 방지를 요청합니다.
    /// </summary>
    public override void RequestBlockMoving()
    {
        foreach (var player in _players)
        {
            player.RequestBlockInput();
        }
    }
    /// <summary>
    /// 플레이어의 움직임 방지 중지를 요청합니다.
    /// </summary>
    public override void RequestUnblockMoving()
    {
        foreach (var player in _players)
        {
            player.RequestUnblockInput();
        }
    }

    #endregion





    #region 구형 정의를 보관합니다.


    #endregion
}
