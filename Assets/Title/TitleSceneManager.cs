using UnityEngine;
using System.Collections;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject[] menuItems;
    public Sprite[] sprites;
    public AudioClip[] soundEffects;

    public ScreenFader fader;

    int menuIndex = 0;
    AudioSource[] seSources;

    bool gameStartRequested = false;

    void Start()
    {
        seSources = new AudioSource[soundEffects.Length];
        for (int i = 0, len = seSources.Length; i < len; ++i)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
            seSources[i].clip = soundEffects[i];
        }
    }
    void Update()
    {
        if (gameStartRequested)
        {
            if (fader.FadeOutEnded)
                Application.LoadLevel(1);
            return;
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (0 < menuIndex)
            {
                ChangeMenuItem(menuIndex - 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (menuIndex <= menuItems.Length)
            {
                ChangeMenuItem(menuIndex + 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (menuIndex)
            {
                case 0:
                    fader.RequestSceneEnd();
                    gameStartRequested = true;
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
            seSources[1].Play();
        }
    }

    void ChangeMenuItem(int index)
    {
        GameObject prevItem = menuItems[menuIndex];
        GameObject nextItem = menuItems[index];
        prevItem.GetComponent<SpriteRenderer>().sprite = sprites[2 * menuIndex + 1];
        nextItem.GetComponent<SpriteRenderer>().sprite = sprites[2 * index];
        menuIndex = index;
        seSources[0].Play();
    }
}