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
    /// 
    /// </summary>
    public GameObject _cursor;


    /// <summary>
    /// 
    /// </summary>
    public GameManager _gameManager;


    /// <summary>
    /// 
    /// </summary>
    public GameObject[] _lifeups;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 
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
    GameManager _saveData;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        _saveDataCount = PlayerPrefs.GetInt("SaveDataCount", 0);
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


            return;
        }


        // 
        if (Input.anyKeyDown)
        {
            // 
            if (Input.GetButtonDown("Attack"))
            {
                Load();
                return;
            }
            else if (Input.GetButtonDown("Jump"))
            {
                AddSaveData();
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


    void AddSaveData()
    {
        GameManager gameData = new GameManager();
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    void ChangeSaveItem(int index)
    {
        if (_saveDataCount == 0)
            return;


        index = Mathf.Clamp(index, 0, _saveDataCount - 1);
        if (index < 0)
            index = 0;
        else if (index >= _saveDataCount)
            index = _saveDataCount - 1;


        // 
        string filename = index.ToString();
        if (File.Exists(filename))
        {
            _saveData = _gameManager.RequestLoad(index.ToString());
        }
        else
        {
            _saveData = null;
        }


        // 
        _cursor.transform.position = new Vector3(_cursor.transform.position.x, index);
        UpdateScreen();
        _saveDataIndex = index;
    }
    /// <summary>
    /// 
    /// </summary>
    void Load()
    {
        if (_saveData == null)
        {

        }
        else
        {
            _loading = true;
            _gameManager.RequestUpdateData(_saveData);
            LoadingSceneManager.LoadLevel("StageSelect");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void UpdateScreen()
    {
        if (_saveData == null)
        {
            for (int i = 0; i < _lifeups.Length; ++i)
            {
                _lifeups[i].SetActive(true);
            }
        }
        else
        {
            GameMapStatus[] mapStatuses = _saveData.MapStatuses;
            for (int i = 0; i < _lifeups.Length; ++i)
            {
                _lifeups[i].SetActive(mapStatuses[i].itemLifeUpFound);
            }
        }
    }


    #endregion
}
