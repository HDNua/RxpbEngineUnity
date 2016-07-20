using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 타이틀 화면을 처리합니다.
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    public GameObject[] menuItems;
    public Sprite[] sprites;
    public AudioClip[] soundEffects;
    public ScreenFader fader;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    AudioSource[] _seSources;


    /// <summary>
    /// 
    /// </summary>
    int _menuIndex = 1;
    /// <summary>
    /// 
    /// </summary>
    bool _changeSceneRequested = false;
    /// <summary>
    /// 
    /// </summary>
    string _nextLevelName = null;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        Time.timeScale = 1;

        // 효과음 리스트를 초기화 합니다.
        _seSources = new AudioSource[soundEffects.Length];
        for (int i = 0, len = _seSources.Length; i < len; ++i)
        {
            _seSources[i] = gameObject.AddComponent<AudioSource>();
            _seSources[i].clip = soundEffects[i];
        }

        // 페이드인 효과를 실행합니다.
        fader.FadeIn();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        // 장면 전환 요청을 확인한 경우의 처리입니다.
        if (_changeSceneRequested)
        {
            if (fader.FadeOutEnded)
            {
                LoadingSceneManager.LoadLevel(_nextLevelName);
            }
            return;
        }

        // 키 입력에 대한 처리입니다.
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (0 < _menuIndex)
                {
                    ChangeMenuItem(_menuIndex - 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_menuIndex < menuItems.Length - 1)
                {
                    ChangeMenuItem(_menuIndex + 1);
                }
            }
            else if (IsSelectKeyPressed())
            {
                switch (_menuIndex)
                {
                    case 0:
                        _nextLevelName = "CS00_PreviousStory";
                        _changeSceneRequested = true;
                        fader.FadeOut(1);
                        break;

                    case 1:
                        _nextLevelName = "CS01_Prologue";
                        _changeSceneRequested = true;
                        fader.FadeOut(1);
                        break;

                    case 2:
                        _nextLevelName = "Continue";
                        _changeSceneRequested = true;
                        fader.FadeOut(1);
                        break;

                    case 3:
                        _nextLevelName = "01_Intro";
                        _changeSceneRequested = true;
                        fader.FadeOut(1);
                        break;

                    case 4:
                        Application.Quit();
                        break;

                    default:
                        _nextLevelName = null;
                        break;
                }
                _seSources[1].Play();
            }
        }
    }


    #endregion










    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 메뉴 아이템 선택을 변경합니다.
    /// </summary>
    /// <param name="index">선택할 메뉴 아이템의 인덱스입니다.</param>
    void ChangeMenuItem(int index)
    {
        GameObject prevItem = menuItems[_menuIndex];
        GameObject nextItem = menuItems[index];
        prevItem.GetComponent<SpriteRenderer>().sprite = sprites[2 * _menuIndex + 1];
        nextItem.GetComponent<SpriteRenderer>().sprite = sprites[2 * index];
        _menuIndex = index;
        _seSources[0].Play();
    }


    /// <summary>
    /// 선택 키가 눌렸는지 확인합니다.
    /// </summary>
    /// <returns>선택 키가 눌렸다면 참입니다.</returns>
    bool IsSelectKeyPressed()
    {
        return (Input.GetKeyDown(KeyCode.Return) || Input.GetButton("Attack") || Input.GetKey(KeyCode.Space));
    }


    #endregion
}