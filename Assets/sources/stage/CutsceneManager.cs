using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour
{
    #region 상수를 정의합니다.
    string NEWLINE { get { return System.Environment.NewLine; } }
    const char SEPERATOR = '|';

    #endregion



    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    public GameObject textObject;
    public AudioClip[] audioClips;
    public TextAsset textAsset;

    public SpriteRenderer[] cutsceneImages;
    public ScreenFader fader;

    #endregion



    #region 필드를 정의합니다.
    AudioSource bgmSource;
    AudioSource[] _audioSources;
    GUIText _guiText;

    // 스크립트 리스트입니다.
    int scriptIndex;
    List<string> scriptList;
    int actionScriptIndex;
    List<string[]> actionScriptList;

    // 사용자 인터페이스 상태입니다.
    bool inputBlocked = false;
    bool scriptPlaying = false;
    bool EndScriptPlayingRequested = false;

    // 대사 스크립트 코루틴에 대한 포인터입니다.
    Coroutine showSpeechscriptCoroutine = null;

    #endregion



    #region MonoBehavior 기본 행동을 재정의합니다.
    void Start()
    {
        #region 필드 초기화를 진행합니다.
        scriptList = new List<string>();
        actionScriptList = new List<string[]>();
        _guiText = textObject.GetComponent<GUIText>();
        scriptIndex = 0;
        actionScriptIndex = 0;

        // 배경음 및 효과음을 초기화 합니다.
        bgmSource = GetComponent<AudioSource>();
        _audioSources = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
            _audioSources[i].clip = audioClips[i];
        }

        #endregion

        #region 텍스트 파일로부터 대사를 획득하고 분석합니다.
        /// string path = System.Environment.CurrentDirectory + "\\Assets\\Cutscene\\CS01_Intro.txt";
        /// string text = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
        // 필드를 초기화하기 위해 텍스트 파일의 내용을 가져옵니다.
        string text = textAsset.text;
        string[] scriptArray = text.Replace(NEWLINE, "|").Split('|');
        string newScriptText = null;



        // 스크립트 배열로부터 대사를 획득하고 분석합니다.
        for (int i = 0, len = scriptArray.Length; i < len; ++i)
        {
            // 스크립트를 가져옵니다.
            string scriptArrayText = scriptArray[i];
            if (scriptArrayText == null) // 잘못된 스크립트 개체에 대한 예외 처리입니다.
            {
                throw new System.Exception("INVALID SUBSCRIPT LINE");
            }
            // 빈 문자열 스크립트 개체인 경우
            else if (scriptArrayText == string.Empty) 
            {
                // 이전 스크립트가 다음 스크립트를 사용하려고 할 수 있습니다.
                if (newScriptText != null)
                {
                    // 이전 스크립트와 다음 스크립트 사이에 구분자를 넣어서,
                    // 이후에 구분자를 통해 스크립트를 구분할 수 있게 합니다.
                    newScriptText += (SEPERATOR);
                }
                continue;
            }



            // 대사 시작 기호를 발견했습니다.
            if (scriptArrayText == "[")
            {
                // 스크립트 변수를 초기화합니다.
                newScriptText = "";
            }
            // 대사 닫기 기호를 발견했습니다.
            else if (scriptArrayText == "]")
            {
                scriptList.Add(newScriptText);
                newScriptText = null;
            }
            // 액션 스크립트를 발견했습니다.
            else if (scriptArrayText[0] == '(')
            {
                // 액션 스크립트 리스트에 액션을 추가합니다.
                actionScriptList.Add(scriptArrayText.Substring(1, scriptArrayText.Length - 2).Split(' '));

                // 스크립트 리스트에는 특수 기호를 넣어서 이것이 액션 스크립트를 참조해야 함을 알립니다.
                scriptList.Add("$");
                continue;
            }
            // 그 외의 경우
            else
            {
                // 이전 스크립트가 존재하지 않는다면 예외 처리합니다.
                if (newScriptText == null)
                    throw new System.Exception("INVALID SUBSCRIPT SEPERATOR");

                // 이전 스크립트에 새 스크립트 문자열을 덧붙입니다.
                newScriptText += (scriptArrayText + SEPERATOR);
            }
        }

        #endregion

        // 스크립트를 수행합니다.
        DoNextScript();
    }
    void Update()
    {
        // 종료 키가 눌린 경우
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            // 재생 중인 스크립트를 모두 종료하고 다음 장면으로 넘어갑니다.
            // 보통 마지막 액션을 수행하면 됩니다.
            inputBlocked = true;
            _guiText.text = "";

            actionScriptIndex = actionScriptList.Count - 1;
            StartCoroutine(ActWithCoroutine());
        }
        // 그 외의 키가 눌린 경우
        else if (Input.anyKeyDown)
        {
            // 자막 스크립트면 스킵합니다. 그 외의 경우 무시합니다.

            if (inputBlocked)
            {

            }
            else if (scriptPlaying)
            {
                EndScriptPlayingRequested = true;
            }
            else
            {
                DoNextScript();
            }
        }
    }
    #endregion



    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 다음 스크립트를 실행합니다.
    /// </summary>
    void DoNextScript()
    {
        // 더 이상 수행할 스크립트가 존재하지 않는 경우
        if (scriptIndex >= scriptList.Count)
        {
            // 함수를 종료합니다.
            return;
        }

        // 스크립트가 액션인지, 자막인지 판정합니다.
        char check = scriptList[scriptIndex][0];
        if (check == '$') // 액션 스크립트라면
        {
            ++scriptIndex;
            scriptPlaying = true;
            /// inputBlocked = true;
            DoAction();
        }
        else // 자막 스크립트라면
        {
            scriptPlaying = true;
            /// inputBlocked = true;
            ShowNextSubscript();
        }
    }
    /// <summary>
    /// 다음 자막 스크립트를 출력합니다.
    /// </summary>
    void ShowNextSubscript()
    {
        _guiText.text = "";

        /// showSubscriptCoroutine = ShowSubscriptChar();
        /// StartCoroutine(showSubscriptCoroutine);
        showSpeechscriptCoroutine = StartCoroutine("ShowSubscriptChar");
    }
    /// <summary>
    /// 액션을 수행합니다.
    /// </summary>
    void DoAction()
    {
        inputBlocked = true;
        if (actionScriptIndex < actionScriptList.Count)
        {
            _guiText.text = "";
            /// StartCoroutine(ActWithCoroutine());
            StartCoroutine("ActWithCoroutine");
        }
    }
    /// <summary>
    /// 스크립트에 따라 액션을 수행하는 코루틴입니다.
    /// </summary>
    /// <returns>DoNextScript() 함수 호출의 결과입니다.</returns>
    IEnumerator ActWithCoroutine()
    {
        string[] action = actionScriptList[actionScriptIndex++];
        string command = action[0];

        int index;      // int seIndex, imageIndex;
        float _time;    // float sec, time;
        float fadeSpeed;
        string levelName;
        Vector2 position;

        switch (command)
        {
            case "WaitForSeconds":
                _time = float.Parse(action[1]);
                yield return new WaitForSeconds(_time);
                break;

            case "Load":
                levelName = action[1];
                LoadingSceneManager.LoadLevel(levelName);
                break;

            case "SlideImage":
                index = int.Parse(action[1]);
                _time = float.Parse(action[2]);
                position = new Vector2(float.Parse(action[3]), float.Parse(action[4]));
                {
                    SpriteRenderer image = cutsceneImages[index];
                    Rigidbody2D image_rbody = image.GetComponent<Rigidbody2D>();
                    Vector2 diff = position - new Vector2(image.transform.position.x, image.transform.position.y);
                    image_rbody.velocity = diff / _time;
                    yield return new WaitForSeconds(_time);
                    image_rbody.velocity = Vector2.zero;
                }

                break;

            case "EnableImage":
                index = int.Parse(action[1]);
                cutsceneImages[index].enabled = true;
                break;

            case "DisableImage":
                index = int.Parse(action[1]);
                cutsceneImages[index].enabled = false;
                break;

            case "SceneFadeIn":
                fadeSpeed = float.Parse(action[1]);
                fader.FadeIn(fadeSpeed);
                break;

            case "SceneFadeOut":
                fadeSpeed = float.Parse(action[1]);
                fader.FadeOut(fadeSpeed);
                break;

            case "PlayMusic":
                index = int.Parse(action[1]);
                bgmSource.clip = _audioSources[index].clip;
                bgmSource.Play();
                break;

            case "StopMusic":
                bgmSource.Stop();
                break;

            case "PlaySound":
                index = int.Parse(action[1]);
                _time = float.Parse(action[2]);
                _audioSources[index].Play();
                yield return new WaitForSeconds(_time);
                break;
        }
        DoNextScript();
    }
    /// <summary>
    /// 다음 자막 스크립트 문자를 출력하는 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowSubscriptChar()
    {
        string subscript = scriptList[scriptIndex++];
        string name = null;

        for (int i = 0, len = subscript.Length; i < len; ++i)
        {
            char subChar = subscript[i];

            // 콜론으로 시작하는 문장은 즉시 출력합니다.
            // 대화 시 캐릭터의 이름에 사용합니다.
            if (subChar == ':')
            {
                ++i;
                name = "";
                while (i < len)
                {
                    if (subscript[i] == SEPERATOR)
                    {
                        break;
                    }
                    else
                    {
                        name += subscript[i];
                    }
                    ++i;
                }
                _guiText.text += name + NEWLINE;
            }
            // 분리자인 경우 개행합니다.
            else if (subChar == SEPERATOR)
            {
                _guiText.text += NEWLINE;
            }
            // 스크립트 재생이 사용자에 의해 종료된 경우
            else if (EndScriptPlayingRequested)
            {
                name = "";
                while (i < len)
                {
                    if (subscript[i] == SEPERATOR)
                    {
                        name += NEWLINE;
                    }
                    else
                    {
                        name += subscript[i];
                    }
                    ++i;
                }
                _guiText.text += name;

                /// scriptPlaying = false;
                /// inputBlocked = false;
                EndScriptPlayingRequested = false;
                break;
            }
            // 글자 단위로 소리를 내며 출력합니다.
            else
            {
                _guiText.text += subChar;
                _audioSources[0].Play();
            }
            yield return new WaitForSeconds(0.1f);
        }

        StopCoroutine(showSpeechscriptCoroutine);
        showSpeechscriptCoroutine = null;
        scriptPlaying = false;
        inputBlocked = false;
    }

    #endregion

}