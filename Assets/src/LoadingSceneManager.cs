using UnityEngine;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    public ScreenFader fader;
    bool fadeRequested = false;

    void Start()
    {

    }
    void Update()
    {
        if (loadRequested)
        {
//            if (loadingLevel < 0)
//                return;
            StartCoroutine(LoadMain());
            loadRequested = false;
        }
    }

    static bool loadRequested = false;
    static int loadingLevel = -1;

    static string loadingLevelName = null;

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
                        fader.RequestSceneEnd();
                    }
                    fadeRequested = true;
                }
            }
            yield return true;
        }
    }

    /*
    public static void LoadLevel(int level)
    {
        loadingLevel = level;
        loadRequested = true;
        Application.LoadLevel(1);
    }
    */
    public static void LoadLevel(string levelName)
    {
        loadingLevelName = levelName;
        loadRequested = true;
        Application.LoadLevel("Loading");
    }
}