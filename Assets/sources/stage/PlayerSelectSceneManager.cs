using UnityEngine;
using System.Collections;

public class PlayerSelectSceneManager : MonoBehaviour
{
    public GameObject objectX;
    public GameObject objectZ;
    public GameObject backX;
    public GameObject backZ;

    public AudioClip[] audioClips;
    public ScreenFader fader;

    AudioSource[] audioSources;

    SpriteRenderer spriteX;
    SpriteRenderer spriteZ;
    Rigidbody2D rbodyX;
    Rigidbody2D rbodyZ;

    SpriteRenderer backSpriteX;
    SpriteRenderer backSpriteZ;

    bool fadeOutRequested = false;
//    bool fadeEnd = false;

    // Use this for initialization
    void Start()
    {
        spriteX = objectX.GetComponent<SpriteRenderer>();
        spriteZ = objectZ.GetComponent<SpriteRenderer>();
        rbodyX = objectX.GetComponent<Rigidbody2D>();
        rbodyZ = objectZ.GetComponent<Rigidbody2D>();
        backSpriteX = backX.GetComponent<SpriteRenderer>();
        backSpriteZ = backZ.GetComponent<SpriteRenderer>();

//        rbodyX.velocity = new Vector2(32, 0);
//        rbodyZ.velocity = new Vector2(-32, 0);

        audioSources = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = audioClips[i];
        }
    }

    bool loadEnd = false;
    string loadingLevelName = null;

    // Update is called once per frame
    void Update()
    {
        if (fadeOutRequested)
        {
            if (fader.FadeOutEnded)
            {
                LoadingSceneManager.LoadLevel(loadingLevelName);
            }
            return;
        }

        // 캐릭터를 불러옵니다.
        if (loadEnd == false)
        {
            if (rbodyX.velocity.x < 0.5f)
            {
                rbodyX.velocity = new Vector2(0, 0);
                rbodyZ.velocity = new Vector2(0, 0);
                loadEnd = true;
            }
            else
            {
//                rbodyX.velocity = new Vector2(rbodyX.velocity.x - 1.5f, 0);
//                rbodyZ.velocity = new Vector2(rbodyZ.velocity.x + 1.5f, 0);
            }
        }

        // 캐릭터를 선택합니다.
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spriteX.sortingOrder = 2;
            spriteZ.sortingOrder = 0;
            backSpriteX.enabled = true;
            backSpriteZ.enabled = false;
            audioSources[0].Play();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            spriteX.sortingOrder = 0;
            spriteZ.sortingOrder = 2;
            backSpriteX.enabled = false;
            backSpriteZ.enabled = true;
            audioSources[0].Play();
        }
        // 결정 키가 눌린 경우입니다.
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // 엑스 선택
            if (backSpriteX.enabled)
            {
                loadingLevelName = "X_introCutscene";
            }
            // 제로 선택
            else
            {
                loadingLevelName = "Z_introCutscene";
            }
            audioSources[1].Play();
            fader.FadeOut(); // fader.RequestSceneEnd();
            fadeOutRequested = true;
        }
    }
}
