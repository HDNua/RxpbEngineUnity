using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



/// <summary>
/// Loading Scene 관리자입니다.
/// </summary>
public class LoadingSceneManager : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public ScreenFader fader;


    #endregion



    #region 필드를 정의합니다.
    bool fadeRequested = false;

    static bool loadRequested = false;
    static string loadingLevelName = null;


    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        // 페이드인 효과를 추가합니다.
        fader.FadeIn();
    }
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMain()
    {
        // 구형 정의를 새로운 정의로 업데이트 합니다.
        AsyncOperation async = SceneManager.LoadSceneAsync(loadingLevelName); // Application.LoadLevelAsync(loadingLevelName);

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
        async.allowSceneActivation = true;
    }

    /// <summary>
    /// 장면을 불러옵니다.
    /// </summary>
    /// <param name="levelName">불러올 장면의 이름입니다.</param>
    public static void LoadLevel(string levelName)
    {
        loadingLevelName = levelName;
        loadRequested = true;

        // 구형 정의를 새로운 정의로 업데이트 합니다.
        SceneManager.LoadScene("Loading"); // Application.LoadLevel("Loading");
    }


    #endregion
}