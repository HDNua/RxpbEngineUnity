using UnityEngine;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    public ScreenFader fader;
    bool fadeRequested = false;

    void Start()
    {
        StartCoroutine(LoadMain());
    }
    IEnumerator LoadMain()
    {
        AsyncOperation async = Application.LoadLevelAsync(2);
        while (async.isDone == false)
        {
            if (async.progress >= 0.8f)
            {
                if (fadeRequested == false)
                {
                    fader.RequestSceneEnd();
                    fadeRequested = true;
                }
            }
            yield return true;
        }
    }
}