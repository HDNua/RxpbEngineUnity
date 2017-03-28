using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



/// <summary>
/// 1인 스테이지 장면 관리자입니다.
/// </summary>
public class StageManager1P : StageManager
{
    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    /// <summary>
    /// 현재 조작중인 플레이어입니다.
    /// </summary>
    public PlayerController _player;


    #endregion





    #region Unity 개체에 대한 참조를 보관합니다.
    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    new public static StageManager1P Instance
    {
        get
        {
            return GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager1P>();
        }
    }

    /// <summary>
    /// 주 플레이어 개체입니다.
    /// </summary>
    public override PlayerController MainPlayer { get { return _player; } }

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        base.Start();
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
        while (_player.IsHealthFull() == false)
        {
            _player.Heal();
        }
    }
    /// <summary>
    /// 플레이어에게 대미지를 입힙니다.
    /// </summary>
    protected override void HurtPlayer()
    {
        _player.Hurt(TestDamageValue);
    }

    /// <summary>
    /// 플레이어를 변경합니다.
    /// </summary>
    /// <param name="newPlayer">변경 이후의 플레이어입니다.</param>
    public void ChangePlayer(PlayerController newPlayer)
    {
        // 이전 플레이어를 비활성화합니다.
        _player.gameObject.SetActive(false);

        // 새 플레이어를 소환합니다.
        newPlayer.transform.position = _player.transform.position;
        newPlayer.RequestSpawn();
        if (_player.FacingRight == false)
        {
            newPlayer.RequestFlip();
        }

        // 관리자 객체의 필드가 새 플레이어를 가리키도록 합니다.
        _Map.UpdatePlayer(_player = newPlayer);
    }
    
    #endregion
    




    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 스테이지 종료 아이템 획득 코루틴입니다.
    /// </summary>
    protected override IEnumerator CoroutineClearStage()
    {
        // 다음 커밋에서 삭제할 예정입니다.
        /// GameManager.Instance.SpawnPositionIndex = 0;

        // 
        AudioSource audioSource = AudioSources[5];
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return false;
        }

        // 
        _player.RequestCeremony();
        yield return new WaitForSeconds(0.2f);
        AudioSources[7].Play();
        while (_player.IsAnimationPlaying("Ceremony"))
        {
            yield return false;
        }

        // 
        while (_player.IsAnimationPlaying("Ceremony"))
        {
            yield return false;
        }

        AudioSources[13].Play();
        while (_player.IsAnimationPlaying("ReturnBeg"))
        {
            yield return false;
        }

        float time = 0;
        while (_player.IsAnimationPlaying("ReturnRun"))
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
        return _player.transform.position;
    }
    
    #endregion





    #region 메서드를 정의합니다.
    /// <summary>
    /// HUD를 활성화합니다.
    /// </summary>
    public override void EnableHUD()
    {
        _userInterfaceManager.ActivatePlayerHUD();
    }
    
    /// <summary>
    /// 플레이어의 움직임 방지를 요청합니다.
    /// </summary>
    public override void RequestBlockMoving()
    {
        /// print("Block Requested From Stage Manager");
        _player.RequestBlockInput();
    }
    /// <summary>
    /// 플레이어의 움직임 방지 중지를 요청합니다.
    /// </summary>
    public override void RequestUnblockMoving()
    {
        /// print("Unblock Requested From Stage Manager");
        _player.RequestUnblockInput();
    }

    #endregion





    #region 구형 정의를 보관합니다.

    #endregion
}
