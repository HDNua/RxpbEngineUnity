using System;
using UnityEngine;
using System.Collections;
using System.IO;



/// <summary>
/// 이어하기 장면 관리자입니다.
/// </summary>
public class ContinueSceneManager : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 효과음 리스트입니다.
    /// </summary>
    public AudioClip[] _soundEffects;
    /// <summary>
    /// 커서 객체입니다.
    /// </summary>
    public GameObject _cursor;
    /// <summary>
    /// 커서의 시작점입니다.
    /// </summary>
    public float _cursorOriginY = 1.5f;


    /// <summary>
    /// 페이드 인/아웃 효과 관리자입니다.
    /// </summary>
    public ScreenFader _fader;


    /// <summary>
    /// 현재 커서가 가리키는 세이브 파일의 정보입니다.
    /// </summary>
    public GameObject _gameDataPanel;
    /// <summary>
    /// 획득한 '라이프 업' 아이템을 표현합니다.
    /// </summary>
    public GameObject[] _lifeups;


    /// <summary>
    /// 데이터가 없을 때 화면에 출력할 텍스트 객체입니다.
    /// </summary>
    public UnityEngine.UI.Text _noDataText;
    /// <summary>
    /// 현재 Scene의 상태를 표시하는 텍스트 객체입니다.
    /// </summary>
    public UnityEngine.UI.Text _statusText;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// Scene에서 사용할 효과음을 사용 가능한 형태로 보관합니다.
    /// </summary>
    AudioSource[] _seSources;


    /// <summary>
    /// 불러오는 중이라면 참입니다.
    /// </summary>
    bool _loading = false;


    /// <summary>
    /// 
    /// </summary>
    int _saveDataIndex = 0;
    /// <summary>
    /// 
    /// </summary>
    int _saveDataCount = 5;
    /// <summary>
    /// 
    /// </summary>
    GameData _gameData = null;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 효과음 리스트를 초기화 합니다.
        _seSources = new AudioSource[_soundEffects.Length];
        for (int i = 0, len = _seSources.Length; i < len; ++i)
        {
            _seSources[i] = gameObject.AddComponent<AudioSource>();
            _seSources[i].clip = _soundEffects[i];
        }

        // 페이드인 효과를 실행합니다.
        _fader.FadeIn();

        // 필드를 초기화합니다.
        _saveDataCount = PlayerPrefs.GetInt("SaveDataCount", 5);
        if (_saveDataCount > 0)
        {
            ChangeSaveItem(0);
        }
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        // 게임 데이터를 불러오는 중이라면 로딩만 수행합니다.
        if (_loading)
        {
            if (_fader.FadeOutEnded)
            {
                LoadingSceneManager.LoadLevel("StageSelect");
            }
            return;
        }


        // 아무 키나 눌린 경우
        if (Input.anyKeyDown)
        {
            // 테스트 함수를 호출합니다.
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    AddSaveData(_saveDataIndex);
                    return;
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    DeleteSaveData(_saveDataIndex);
                    return;
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    PreserveSaveData();
                    return;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    DeletePlayerPrefsSaveData();
                    return;
                }
            }

            // 
            if (Input.GetButtonDown("Attack"))
            {
                Load();
                return;
            }
            else if (Input.GetButtonDown("Jump"))
            {
                return;
            }
            else if (Input.GetButtonDown("Dash"))
            {
                LoadingSceneManager.LoadLevel("Title");
                return;
            }


            // 
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeSaveItem(_saveDataIndex - 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeSaveItem(_saveDataIndex + 1);
            }
        }
    }


    /// <summary>
    /// 게임 데이터를 추가합니다.
    /// </summary>
    /// <param name="index">추가할 게임 데이터의 인덱스입니다.</param>
    void AddSaveData(int index)
    {
        GameData gameData = new GameData();
        gameData.MaxHealthX = 20;
        gameData.MaxHealthZ = 20;
        gameData.MapStatuses[1].itemLifeUpFound = true;

        // 
        _gameData = gameData;
        GameManager.Instance.RequestSave(index.ToString(), gameData);
        UpdateScene();

        // 상태 메시지를 출력합니다.
        _statusText.text = "New Game Data Saved";
    }
    /// <summary>
    /// 게임 데이터를 디스크에서 제거합니다.
    /// </summary>
    /// <param name="index">삭제할 게임 데이터의 인덱스입니다.</param>
    void DeleteSaveData(int index)
    {
        GameManager.Instance.RequestDeleteData(index.ToString());

        // 
        _gameData = null;
        UpdateScene();

        // 상태 메시지를 출력합니다.
        _statusText.text = "Game Data Deleted";
    }
    /// <summary>
    /// PlayerPrefs 속성을 제거합니다.
    /// </summary>
    void DeletePlayerPrefsSaveData()
    {
        PlayerPrefs.DeleteAll();

        // 상태 메시지를 출력합니다.
        _statusText.text = "PlayerPrefs Initialized";
    }
    /// <summary>
    /// PlayerPrefs 속성을 보존합니다.
    /// </summary>
    void PreserveSaveData()
    {
        PlayerPrefs.Save();

        // 상태 메시지를 출력합니다.
        _statusText.text = "PlayerPrefs Preserved";
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 커서가 다른 세이브 데이터를 가리키게 합니다.
    /// </summary>
    /// <param name="index"></param>
    void ChangeSaveItem(int index)
    {
        // 
        if (_saveDataCount == 0)
            return;
        else if (index < 0 || index >= _saveDataCount)
            return;


        // 
        string filename = index.ToString();
        if (File.Exists(filename))
        {
            _gameData = GameManager.Instance.RequestLoad(index.ToString());
        }
        else
        {
            _gameData = null;
        }


        // 화면을 갱신하고 나머지 필드를 업데이트합니다.
        UpdateScene();
        _seSources[0].Play();
        _cursor.transform.position = new Vector3(_cursor.transform.position.x, _cursorOriginY - index);
        _saveDataIndex = index;
        _statusText.text = "Save Item Changed";
    }
    /// <summary>
    /// 세이브 데이터로부터 게임을 불러옵니다.
    /// </summary>
    void Load()
    {
        if (_gameData == null)
        {

        }
        else
        {
            _loading = true;
            GameManager.Instance.RequestUpdateData(_gameData);

            // 
            _seSources[1].Play();

            // 페이드 아웃을 진행합니다.
            _fader.FadeOut(1);
        }
    }
    /// <summary>
    /// Scene을 새로고침합니다.
    /// </summary>
    void UpdateScene()
    {
        if (_gameData == null)
        {
            _noDataText.enabled = true;
            _gameDataPanel.SetActive(false);
        }
        else
        {
            _noDataText.enabled = false;
            ClearPreviousActiveState();
            GameMapStatus[] mapStatuses = _gameData.MapStatuses;
            for (int i = 0; i < _lifeups.Length; ++i)
            {
                _lifeups[i].SetActive(mapStatuses[i].itemLifeUpFound);
            }
            _gameDataPanel.SetActive(true);
        }
    }


    /// <summary>
    /// 이전의 활성화 상태를 제거합니다.
    /// </summary>
    void ClearPreviousActiveState()
    {
        for (int i = 0; i < _lifeups.Length; ++i)
        {
            _lifeups[i].SetActive(false);
        }
    }


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("GameManager.Instance로 대체되었습니다. 다음 커밋에서 삭제할 예정입니다.")]
    /// <summary>
    /// 게임 시스템 관리자입니다.
    /// </summary>
    GameManager _gameManager = null;


    #endregion
}
