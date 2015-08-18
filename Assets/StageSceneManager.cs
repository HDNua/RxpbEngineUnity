using UnityEngine;
using System.Collections;

public class StageSceneManager : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    Animator _animator;

    #endregion



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public PlayerController player;
    public AudioClip[] soundClips;

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
        // 효과음 리스트를 초기화 합니다.
        soundEffects = new AudioSource[soundClips.Length];
        for (int i = 0, len = soundClips.Length; i < len; ++i)
        {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].clip = soundClips[i];
        }
    }
    void Update()
    {

    }

    #endregion



    #region 보조 메서드를 정의합니다.



    #endregion
}