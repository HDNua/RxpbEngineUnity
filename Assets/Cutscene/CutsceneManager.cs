using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour
{
    string NEWLINE { get { return System.Environment.NewLine; } }
    const char SEPERATOR = '|';

    public GameObject textObject;
    public AudioClip[] audioClips;

    AudioSource[] _audioSources;
    GUIText _guiText;

    int subIndex;
    int actionIndex;
    List<string> subList;
    List<string[]> actionList;

    bool inputBlocked = false;
    bool scriptPlaying = false;
    bool EndScriptPlayingRequested = false;

    void Start()
    {
        #region 필드 초기화를 진행합니다.
        subList = new List<string>();
        actionList = new List<string[]>();
        _guiText = textObject.GetComponent<GUIText>();
        subIndex = 0;
        actionIndex = 0;

        _audioSources = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
            _audioSources[i].clip = audioClips[i];
        }

        #endregion

        #region 텍스트 파일로부터 대사를 획득하고 분석합니다.
        string path = System.Environment.CurrentDirectory + "\\Assets\\Cutscene\\CS01_Intro.txt";
        string text = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
        string[] subscriptArray = text.Replace(NEWLINE, "|").Split('|');

        string newSub = null;
        for (int i = 0, len = subscriptArray.Length; i < len; ++i)
        {
            string subscriptElem = subscriptArray[i];
            if (subscriptElem == null)
            {
                throw new System.Exception("INVALID SUBSCRIPT LINE");
            }
            else if (subscriptElem == string.Empty)
            {
                if (newSub != null)
                {
                    newSub += (SEPERATOR);
                }
                continue;
            }

            // 대사라면
            if (subscriptElem == "[")
            {
                newSub = "";
            }
            else if (subscriptElem == "]")
            {
                print(newSub);
                subList.Add(newSub);
                newSub = null;
            }
            // 명령이라면
            else if (subscriptElem[0] == '(')
            {
                actionList.Add(subscriptElem.Substring(1, subscriptElem.Length - 2).Split(' '));
                subList.Add("$");
                continue;
            }
            // 그 외의 경우
            else
            {
                if (newSub == null)
                    throw new System.Exception("INVALID SUBSCRIPT SEPERATOR");
                newSub += (subscriptElem + SEPERATOR);
            }
        }

        #endregion

        // 행동을 시작합니다.
        DoNextScript();
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (inputBlocked)
            {

            }
            else if (scriptPlaying)
            {
                EndScriptPlayingRequested = true;
//                scriptPlaying = false;
            }
            else
            {
                print(string.Format("{0}/{1}", subIndex, subList.Count));
                DoNextScript();
            }
        }
    }

    void DoNextScript()
    {
        if (subIndex >= subList.Count)
        {
            print("SCRIPT ENDED");
            return;
        }

        char check = subList[subIndex][0];
        if (check == '$')
        {
            ++subIndex;
            scriptPlaying = true;
            inputBlocked = true;
            Act();
        }
        else
        {
            scriptPlaying = true;
            inputBlocked = true;
            ShowNextSubscript();
        }
    }

    void ShowNextSubscript()
    {
        _guiText.text = "";
        StartCoroutine(ShowSubscriptChar());
    }
    void Act()
    {
        inputBlocked = true;
        if (actionIndex < actionList.Count)
        {
            _guiText.text = "";
            StartCoroutine(ActWithCoroutine());
        }
    }

    IEnumerator ActWithCoroutine()
    {
        string[] action = actionList[actionIndex++];
        string command = action[0];
        switch (command)
        {
            case "WaitForSeconds":
                float sec = float.Parse(action[1]);
                yield return new WaitForSeconds(sec);
                break;
            case "Warning":
                _audioSources[1].Play();
                yield return new WaitForSeconds(1);
                break;
            case "Load":
                int level = int.Parse(action[1]);
                LoadingSceneManager.LoadLevel(level);
                break;
        }
        DoNextScript();
    }

    IEnumerator ShowSubscriptChar()
    {
        string subscript = subList[subIndex++];
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
                EndScriptPlayingRequested = false;
                scriptPlaying = false;
            }
            // 글자 단위로 소리를 내며 출력합니다.
            else
            {
                _guiText.text += subChar;
                _audioSources[0].Play();
            }
            yield return new WaitForSeconds(0.1f);
        }

        scriptPlaying = false;
        inputBlocked = false;
    }
}