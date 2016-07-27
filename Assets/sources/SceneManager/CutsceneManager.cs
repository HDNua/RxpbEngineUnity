using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 컷신을 관리합니다.
/// </summary>
public class CutsceneManager : MonoBehaviour
{
    #region 상수를 정의합니다.
    string NEWLINE { get { return System.Environment.NewLine; } }
    const char SEPERATOR = '|';


    #endregion










    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    public GameObject _textObject;
    public AudioClip[] _audioClips;
    public TextAsset _textAsset;

    public SpriteRenderer[] _cutsceneImages;
    public ScreenFader _fader;


    #endregion










    #region 필드를 정의합니다.
    AudioSource _bgmSource;
    AudioSource[] _audioSources;
    GUIText _guiText;

    // 스크립트 리스트입니다.
    int _scriptIndex;
    List<string> _scriptList;
    int _actionScriptIndex;
    List<string[]> _actionScriptList;

    // 사용자 인터페이스 상태입니다.
    bool _inputBlocked = false;
    bool _actionScriptPlaying = false; // 액션 스크립트가 재생중이라면 참입니다.
    bool _speechScriptPlaying = false; // 대사 스크립트가 재생중이라면 참입니다.
    bool _scriptEndRequested = false; // 스크립트 skip이 요청되었습니다.

    // 대사 스크립트 코루틴에 대한 포인터입니다.
    Coroutine _speechScriptCoroutine = null;
//    Coroutine _actionScriptCoroutine = null;


    #endregion










    #region MonoBehavior 기본 행동을 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        #region 필드 초기화를 진행합니다.
        _scriptList = new List<string>();
        _actionScriptList = new List<string[]>();
        _guiText = _textObject.GetComponent<GUIText>();
        _scriptIndex = 0;
        _actionScriptIndex = 0;

        // 배경음 및 효과음을 초기화 합니다.
        _bgmSource = GetComponent<AudioSource>();
        _audioSources = new AudioSource[_audioClips.Length];
        for (int i = 0, len = _audioClips.Length; i < len; ++i)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
            _audioSources[i].clip = _audioClips[i];
        }

        #endregion

        #region 텍스트 파일로부터 대사를 획득하고 분석합니다.
        /// string path = System.Environment.CurrentDirectory + "\\Assets\\Cutscene\\CS01_Intro.txt";
        /// string text = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);

        // 필드를 초기화하기 위해 텍스트 파일의 내용을 가져옵니다.
        string text = _textAsset.text;
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
                _scriptList.Add(newScriptText);
                newScriptText = null;
            }
            // 액션 스크립트를 발견했습니다.
            else if (scriptArrayText[0] == '(')
            {
                // 액션 스크립트 리스트에 액션을 추가합니다.
                _actionScriptList.Add(scriptArrayText.Substring(1, scriptArrayText.Length - 2).Split(' '));

                // 스크립트 리스트에는 특수 기호를 넣어서 이것이 액션 스크립트를 참조해야 함을 알립니다.
                _scriptList.Add("$");
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
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        // 종료 키가 눌린 경우
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            // 재생 중인 스크립트를 모두 종료하고 다음 장면으로 넘어갑니다.
            // 보통 마지막 액션을 수행하면 됩니다.
            _inputBlocked = true;
            _guiText.text = "";

            _actionScriptIndex = _actionScriptList.Count - 1;
            StartCoroutine(ActWithCoroutine());
        }
        // 그 외의 키가 눌린 경우
        else if (Input.anyKeyDown)
        {
            // 자막 스크립트면 스킵합니다. 그 외의 경우 무시합니다.

            if (_inputBlocked)
            {

            }
            else if (_actionScriptPlaying)
            {

            }
            else if (_speechScriptPlaying)
            {
                _scriptEndRequested = true;
            }
            else
            {
                DoNextScript();
            }
        }
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 다음 스크립트를 실행합니다.
    /// </summary>
    void DoNextScript()
    {
        // 더 이상 수행할 스크립트가 존재하지 않는 경우
        if (_scriptIndex >= _scriptList.Count)
        {
            // 함수를 종료합니다.
            return;
        }

        // 스크립트가 액션인지, 자막인지 판정합니다.
        char check = _scriptList[_scriptIndex][0];
        if (check == '$') // 액션 스크립트라면
        {
            ++_scriptIndex;
            _actionScriptPlaying = true;
            DoAction();
        }
        else // 자막 스크립트라면
        {
            _speechScriptPlaying = true;
            ShowNextSubscript();
        }
    }
    /// <summary>
    /// 다음 자막 스크립트를 출력합니다.
    /// </summary>
    void ShowNextSubscript()
    {
        _guiText.text = "";
        _speechScriptCoroutine = StartCoroutine("ShowSubscriptChar");
    }
    /// <summary>
    /// 액션을 수행합니다.
    /// </summary>
    void DoAction()
    {
        _inputBlocked = true;
        if (_actionScriptIndex < _actionScriptList.Count)
        {
            _guiText.text = "";
            // _actionScriptCoroutine = StartCoroutine("ActWithCoroutine");
            StartCoroutine("ActWithCoroutine");
        }
    }
    /// <summary>
    /// 스크립트에 따라 액션을 수행하는 코루틴입니다.
    /// </summary>
    /// <returns>DoNextScript() 함수 호출의 결과입니다.</returns>
    IEnumerator ActWithCoroutine()
    {
        string[] action = _actionScriptList[_actionScriptIndex++];
        string command = action[0];

        int index;
        float _time;
        float fadeSpeed;
        string levelName;
        Vector2 position;

        // 명령에 따라 액션을 수행합니다.
        switch (command)
        {
            // 일정 시간동안 대기합니다.
            case "WaitForSeconds":
                _time = float.Parse(action[1]);
                yield return new WaitForSeconds(_time);
                break;

            // 레벨을 불러옵니다.
            case "Load":
                levelName = action[1];
                LoadingSceneManager.LoadLevel(levelName);
                break;

            // 이미지를 슬라이드합니다.
            case "SlideImage":
                index = int.Parse(action[1]);
                _time = float.Parse(action[2]);
                position = new Vector2(float.Parse(action[3]), float.Parse(action[4]));
                {
                    SpriteRenderer image = _cutsceneImages[index];
                    Rigidbody2D image_rbody = image.GetComponent<Rigidbody2D>();
                    Vector2 diff = position - new Vector2(image.transform.position.x, image.transform.position.y);
                    image_rbody.velocity = diff / _time;
                    yield return new WaitForSeconds(_time);
                    image_rbody.velocity = Vector2.zero;
                }
                break;
            
            // 이미지를 화면에 표시합니다.
            case "EnableImage":
                index = int.Parse(action[1]);
                _cutsceneImages[index].enabled = true;
                break;

            // 화면에 표시한 이미지를 disable합니다.
            case "DisableImage":
                index = int.Parse(action[1]);
                _cutsceneImages[index].enabled = false;
                break;
                
            // 장면에 페이드인 효과를 부여합니다.
            case "SceneFadeIn":
                fadeSpeed = float.Parse(action[1]);
                _fader.FadeIn(fadeSpeed);
                break;

            // 장면에 페이드아웃 효과를 부여합니다.
            case "SceneFadeOut":
                fadeSpeed = float.Parse(action[1]);
                _fader.FadeOut(fadeSpeed);
                break;

            // 배경 음악을 재생합니다.
            case "PlayMusic":
                index = int.Parse(action[1]);
                if (action.Length > 2)
                {
                    _bgmSource.volume = float.Parse(action[2]);
                }
                else
                {
                    _bgmSource.volume = 1;
                }

                _bgmSource.clip = _audioSources[index].clip;
                _bgmSource.Play();
                break;

            // 음악 재생을 중지합니다.
            case "StopMusic":
                _bgmSource.Stop();
                break;

            // 효과음을 재생합니다.
            case "PlaySound":
                index = int.Parse(action[1]);
                _time = float.Parse(action[2]);
                _audioSources[index].Play();
                yield return new WaitForSeconds(_time);
                break;

            // 
            case "FadeOutMusic":
                while (_bgmSource.volume > 0)
                {
                    _bgmSource.volume -= Time.deltaTime;
                    yield return new WaitForSeconds(0.5f);
                }
                break;
        }

        // 액션 수행이 모두 종료되었으므로 스위치를 닫습니다.
        _actionScriptPlaying = false;

        /**
        // CHECK: 문제가 발생할 때 이 부분을 먼저 확인하세요.
        // 원래는 이런 구문이 없었습니다.
        {
            StopCoroutine(_actionScriptCoroutine);
            _actionScriptCoroutine = null;
        }
        */

        // 다음 스크립트를 수행합니다.
        DoNextScript();
    }
    /// <summary>
    /// 다음 자막 스크립트 문자를 출력하는 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowSubscriptChar()
    {
        string speechText = _scriptList[_scriptIndex++];
        string name = null;



        // 대사 텍스트를 출력합니다.
        for (int i = 0, len = speechText.Length; i < len; ++i)
        {
            char subChar = speechText[i];

            // 콜론으로 시작하는 문장은 즉시 출력합니다.
            // 대화 시 캐릭터의 이름에 사용합니다.
            if (subChar == ':')
            {
                ++i;
                name = "";
                while (i < len)
                {
                    if (speechText[i] == SEPERATOR)
                    {
                        break;
                    }
                    else
                    {
                        name += speechText[i];
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
            else if (_scriptEndRequested)
            {
                name = "";
                while (i < len)
                {
                    if (speechText[i] == SEPERATOR)
                    {
                        name += NEWLINE;
                    }
                    else
                    {
                        name += speechText[i];
                    }
                    ++i;
                }
                _guiText.text += name;

                // 스크립트 종료 요청을 모두 수행했으므로 스위치를 닫습니다.
                _scriptEndRequested = false;
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


        // StopCoroutine(_speechScriptCoroutine);
        _speechScriptCoroutine = null;
        _inputBlocked = false;

        _speechScriptPlaying = false;
    }


    #endregion

}