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
    AudioSource[] seSources;

    int menuIndex = 1;
    bool changeSceneRequested = false;
    string nextLevelName = null;

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    void Start()
    {
        // 효과음 리스트를 초기화 합니다.
        seSources = new AudioSource[soundEffects.Length];
        for (int i = 0, len = seSources.Length; i < len; ++i)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
            seSources[i].clip = soundEffects[i];
        }

        // 페이드인 효과를 실행합니다.
        fader.FadeIn();
    }
    void Update()
    {
        // 장면 전환 요청을 확인한 경우의 처리입니다.
        if (changeSceneRequested)
        {
            if (fader.FadeOutEnded)
            {
                LoadingSceneManager.LoadLevel(nextLevelName);
            }
            return;
        }

        // 키 입력에 대한 처리입니다.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (0 < menuIndex)
            {
                ChangeMenuItem(menuIndex - 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (menuIndex < menuItems.Length - 1)
            {
                ChangeMenuItem(menuIndex + 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (menuIndex)
            {
                case 0:
                    nextLevelName = "CS00_PreviousStory";
                    changeSceneRequested = true;
                    fader.FadeOut(1);
                    break;

                case 1:
                    nextLevelName = "01_Intro"; //"CS01_Prologue";
                    changeSceneRequested = true;
                    fader.FadeOut(1);
                    break;

                case 2:
                    break;

                default:
                    nextLevelName = null;
                    break;
            }
            seSources[1].Play();
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
        GameObject prevItem = menuItems[menuIndex];
        GameObject nextItem = menuItems[index];
        prevItem.GetComponent<SpriteRenderer>().sprite = sprites[2 * menuIndex + 1];
        nextItem.GetComponent<SpriteRenderer>().sprite = sprites[2 * index];
        menuIndex = index;
        seSources[0].Play();
    }

    #endregion
}