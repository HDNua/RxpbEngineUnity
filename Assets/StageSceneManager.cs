using UnityEngine;
using System.Collections;

/// <summary>
/// 스테이지 장면 관리자입니다.
/// </summary>
public class StageSceneManager : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.

    #endregion



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public Camera mainCam;
    public ScreenFader fader;
    public AudioClip[] audioClips;

    public Map map;
    public ReadyAnimator ready;
    public PlayerController player;
    public Transform playerSpawnPos;
    public PlayerController PlayerX;
    public PlayerController PlayerZ;

    #endregion



    #region Unity에서 초기화한 속성을 사용 가능한 형태로 보관합니다.
    /// <summary>
    /// 효과음의 리스트입니다.
    /// </summary>
    AudioSource[] soundEffects;

    /// <summary>
    /// 효과음의 리스트를 반환합니다.
    /// </summary>
    public AudioSource[] SoundEffects { get { return soundEffects; } }

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    void Start()
    {
        // 불러온 캐릭터를 잠깐 사용 불가능하게 합니다.
        PlayerX.gameObject.SetActive(false);
        PlayerZ.gameObject.SetActive(false);

        // 효과음 리스트를 초기화 합니다.
        soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].clip = audioClips[i];
        }

        // 맵 데이터를 초기화합니다.
        player.transform.position = playerSpawnPos.transform.position;
        map.Player = player;

        // 페이드인 효과를 처리합니다.
        fader.FadeIn();
    }
    void Update()
    {
        if (fader.FadeInEnded)
        {
            ready.gameObject.SetActive(true); // = true;
        }
    }

    #endregion



    #region MyRegion
    /// <summary>
    /// 플레이어를 변경합니다.
    /// </summary>
    /// <param name="newPlayer">변경 이후의 플레이어입니다.</param>
    public void ChangePlayer(PlayerController newPlayer)
    {
        // 이전 플레이어를 비활성화합니다.
        player.gameObject.SetActive(false); // gameObject.SetActive(false);

        // 새 플레이어를 소환합니다.
        newPlayer.transform.position = player.transform.position;
        newPlayer.RequestSpawn();
        if (player.FacingRight == false)
        {
            newPlayer.RequestFlip();
        }

        // 관리자 객체의 필드가 새 플레이어를 가리키도록 합니다.
        map.Player = player = newPlayer;
    }

    #endregion



    #region 보조 메서드를 정의합니다.


    #endregion
}