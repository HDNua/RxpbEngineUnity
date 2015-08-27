using UnityEngine;
using System.Collections;

/// <summary>
/// 장면 관리자입니다.
/// </summary>
public class SceneManager : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    public Camera MainCamera;
    public ScreenFader fader;
    public AudioClip[] audioClips;

    #endregion



    #region Unity에서 초기화한 속성을 사용 가능한 형태로 보관합니다.
    AudioSource[] audioSources;
    public AudioSource[] AudioSources { get { return audioSources; } }

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    protected virtual void Start()
    {
        // 효과음 리스트를 초기화 합니다.
        audioSources = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = audioClips[i];
        }
    }
    protected virtual void Update()
    {

    }

    #endregion

}
