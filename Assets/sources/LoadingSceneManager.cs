using UnityEngine;
using System;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    #region 필드를 정의합니다.
    public ScreenFader fader;
    bool fadeRequested = false;

    static bool loadRequested = false;
    static string loadingLevelName = null;

    [Obsolete("장면의 이름을 이용하여 불러오십시오.", true)]
    static int loadingLevel = -1;

    #endregion

    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    void Start()
    {
        // 페이드인 효과를 추가합니다.
        fader.FadeIn();
    }
    void Update()
    {
        if (loadRequested)
        {
            StartCoroutine(LoadMain());
            loadRequested = false;
        }
    }

    #endregion



    #region 보조 메서드를 정의합니다.
    IEnumerator LoadMain()
    {
        AsyncOperation async = Application.LoadLevelAsync(loadingLevelName);
        while (async.isDone == false)
        {
            if (async.progress >= 0.8f)
            {
                if (fadeRequested == false)
                {
                    if (fader != null)
                    {
                        fader.FadeOut(); // fader.RequestSceneEnd();
                    }
                    fadeRequested = true;
                }
            }
            yield return true;
        }
    }

    /// <summary>
    /// 장면을 불러옵니다.
    /// </summary>
    /// <param name="levelName">불러올 장면의 이름입니다.</param>
    public static void LoadLevel(string levelName)
    {
        loadingLevelName = levelName;
        loadRequested = true;
        Application.LoadLevel("Loading");
    }

    [Obsolete("장면의 이름을 이용하여 불러오십시오.", true)]
    /// <summary>
    /// 장면을 불러옵니다.
    /// </summary>
    /// <param name="level">불러올 장면의 인덱스입니다.</param>
    public static void LoadLevel(int level)
    {
        loadingLevel = level;
        loadRequested = true;
        Application.LoadLevel(1);
    }

    #endregion
}