using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



[Serializable]
/// <summary>
/// 게임 관리자입니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region 게임 시스템이 공유하는 속성을 정의합니다.
    /// <summary>
    /// 게임 관리자입니다.
    /// </summary>
    public static GameManager Instance { get; set; }


    /// <summary>
    /// 엑스의 최대 체력입니다.
    /// </summary>
    public int MaxHealthX { get { return _maxHealthX; } }
    /// <summary>
    /// 제로의 최대 체력입니다.
    /// </summary>
    public int MaxHealthZ { get { return _maxHealthZ; } }


    /// <summary>
    /// 맵 상태 집합입니다.
    /// </summary>
    public GameMapStatus[] MapStatuses { get { return _mapStatus; } }


    #endregion



    #region 필드를 정의합니다.
    GameData GameData;


    /// <summary>
    /// 엑스의 최대 체력입니다.
    /// </summary>
    int _maxHealthX = 20;
    /// <summary>
    /// 제로의 최대 체력입니다.
    /// </summary>
    int _maxHealthZ = 20;


    /// <summary>
    /// 맵 상태 집합입니다.
    /// </summary>
    GameMapStatus[] _mapStatus;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 맵 상태를 업데이트합니다.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="mapStatus"></param>
    public void UpdateMapStatus(int index, GameMapStatus mapStatus)
    {
        _mapStatus[index] = mapStatus;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    void Save(string filename)
    {
        Stream ws = new FileStream(filename, FileMode.Create);
        BinaryFormatter serializer = new BinaryFormatter();

        serializer.Serialize(ws, this);
        ws.Close();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    GameManager Load(string filename)
    {
        Stream rs = new FileStream(filename, FileMode.Open);
        BinaryFormatter deserializer = new BinaryFormatter();

        GameManager gameData = (GameManager)deserializer.Deserialize(rs);
        rs.Close();

        return gameData;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameData"></param>
    void UpdateGameData(GameManager gameData)
    {
        // 필드를 업데이트 합니다.
        _maxHealthX = gameData._maxHealthX;
        _maxHealthZ = gameData._maxHealthZ;
        _mapStatus = gameData._mapStatus;
    }


    #endregion









    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    public void RequestSave(string filename)
    {
        Save(filename);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public GameManager RequestLoad(string filename)
    {
        return Load(filename);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_saveData"></param>
    public void RequestUpdateData(GameManager _saveData)
    {
        UpdateGameData(_saveData);
    }


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
