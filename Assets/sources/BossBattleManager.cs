using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 보스 전투 관리자입니다.
/// </summary>
public class BossBattleManager : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 데이터베이스입니다.
    /// </summary>
    public DataBase _database;


    /// <summary>
    /// 보스 캐릭터입니다.
    /// </summary>
    public CommanderYammarkScript _boss;


    /// <summary>
    /// 각입니다.
    /// </summary>
    public Transform[] _angle;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    bool _isWarningEnded = false;
    /// <summary>
    /// 
    /// </summary>
    public bool IsWarningEnded
    {
        set { _isWarningEnded = value; }
    }


    /// <summary>
    /// 
    /// </summary>
    bool _isReady = false;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {

    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        if (_isWarningEnded == false)
            return;

        if (_isReady)
        {
            if (_boss.transform.position.y < _angle[0].position.y)
            {
                _boss.transform.position = new Vector2(_boss.transform.position.x, _angle[0].position.y);
                _isReady = false;
                GetComponent<AudioSource>().Play();
                return;
            }
            _boss.Rigidbody2D.velocity = Vector2.down * _boss._movingSpeed;
            return;
        }



        if (_boss.IsDead)
        {
            EndBattle();
        }



        Vector2 direction = _boss.Rigidbody2D.velocity;
        if (direction.x != 0) direction.x = direction.x > 0 ? 1 : -1;
        if (direction.y != 0) direction.y = direction.y > 0 ? 1 : -1;



        if (direction == Vector2.down && _boss.transform.position.y < _angle[0].position.y)
        {
            _boss.Rigidbody2D.velocity = Vector2.left * _boss._movingSpeed;
            _boss.transform.position = new Vector2(_boss.transform.position.x, _angle[0].position.y);
        }
        else if (direction == Vector2.left && _boss.transform.position.x < _angle[1].position.x)
        {
            _boss.Rigidbody2D.velocity = Vector2.up * _boss._movingSpeed;
            _boss.transform.position = new Vector2(_angle[1].position.x, _boss.transform.position.y);
        }
        else if (direction == Vector2.up && _boss.transform.position.y > _angle[2].position.y)
        {
            _boss.Rigidbody2D.velocity = Vector2.right * _boss._movingSpeed;
            _boss.transform.position = new Vector2(_boss.transform.position.x, _angle[2].position.y);
        }
        else if (direction == Vector2.right && _boss.transform.position.x > _angle[3].position.x)
        {
            _boss.Rigidbody2D.velocity = Vector2.down * _boss._movingSpeed;
            _boss.transform.position = new Vector2(_angle[3].position.x, _boss.transform.position.y);
        }
        else if (direction == Vector2.zero)
        {
            _boss.Rigidbody2D.velocity = Vector2.left * _boss._movingSpeed;
        }
        else
        {
            /// Debug.Log("unknown direction " + direction);
        }
    }




    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 경고 화면을 표시합니다.
    /// </summary>
    void Warning()
    {
        _database.StageManager.StopBackgroundMusic();
        Debug.Log("warning");
    }
    /// <summary>
    /// 전투 전에 캐릭터 간 스크립트를 진행합니다.
    /// </summary>
    void BeginScript()
    {
        _isWarningEnded = true;
        _isReady = true;

        Debug.Log("begin script: blah blah blah");

    }
    /// <summary>
    /// 전투를 시작합니다.
    /// </summary>
    void BeginBattle()
    {
        Debug.Log("begin battle");
    }


    /// <summary>
    /// 전투를 끝냅니다.
    /// </summary>
    void EndBattle()
    {
        Debug.Log("end of game");
    }


    #endregion










    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 보스 전투 시나리오 실행을 요청합니다.
    /// </summary>
    public void RequestBossBattleScenario()
    {
        Warning();
        Invoke("BeginScript", 4);
        Invoke("BeginBossBattle", 5);
    }





    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
