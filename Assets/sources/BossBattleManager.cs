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
    /// 스테이지 관리자입니다.
    /// </summary>
    StageManager _stageManager;
    /// <summary>
    /// 사용자 인터페이스 관리자입니다.
    /// </summary>
    UIManager _userInterfaceManager;


    // 절차:
    // 1. 경고
    // 2. 등장
    // 3. 대사 (생략 가능)
    // 4. 준비
    // 5. 시작
    bool _warning = false;
    bool _appearing = false;
    bool _scripting = false;
    bool _readying = false;
    bool _fighting = false;


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        _stageManager = _database.StageManager;
        _userInterfaceManager = _database.UIManager;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        if (_fighting)
        {

        }
        else if (_boss.IsDead)
        {

        }
        else if (_readying)
        {

        }
        else if (_scripting)
        {

        }
        else if (_appearing)
        {

        }
        else if (_warning)
        {

        }
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    void ActivateBossHUD()
    {
        _userInterfaceManager.ActivateBossHUD();
    }



    #endregion










    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 보스 전투 시나리오 실행을 요청합니다.
    /// </summary>
    public void RequestBossBattleScenario()
    {
        Warning();
    }


    #endregion









    #region 시작 메서드를 정의합니다.
    /// <summary>
    /// 경고 화면을 표시합니다.
    /// </summary>
    void Warning()
    {
        _warning = true;
        StartCoroutine(CoroutineWarning());
    }
    /// <summary>
    /// 
    /// </summary>
    void Appear()
    {
        _warning = false;
        _appearing = true;
        StartCoroutine(CoroutineAppearing());
    }
    /// <summary>
    /// 
    /// </summary>
    void Script()
    {
        _appearing = false;
        _scripting = true;
        StartCoroutine(CoroutineScripting());
    }
    /// <summary>
    /// 
    /// </summary>
    void Ready()
    {
        _scripting = false;
        _readying = true;
        StartCoroutine(CoroutineReadying());
    }
    /// <summary>
    /// 
    /// </summary>
    void Fight()
    {
        _readying = false;
        _fighting = true;

        _stageManager.RequestUnblockMoving();
        GetComponent<AudioSource>().Play();
        StartCoroutine(CoroutineFighting());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineWarning()
    {
        // 
        _stageManager.RequestBlockMoving();
        _stageManager.StopBackgroundMusic();


        // 
        AudioClip warningSound = _stageManager._audioClips[8];
        AudioSource warningSource = _stageManager.AudioSources[8];
        float length = warningSound.length * 0.9f;

        warningSource.Play();
        yield return new WaitForSeconds(length);
        warningSource.Play();
        yield return new WaitForSeconds(length);
        warningSource.Play();
        yield return new WaitForSeconds(length);
        warningSource.Play();
        yield return new WaitForSeconds(length);
        warningSource.Play();
        yield return new WaitForSeconds(length);


        // 
        Appear();
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineAppearing()
    {
        _boss._Rigidbody2D.velocity = new Vector2(0, -_boss._movingSpeed);
        while (_boss.transform.position.y > _angle[0].position.y)
        {
            yield return null;
        }
        _boss._Rigidbody2D.velocity = Vector2.zero;

        Script();
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineScripting()
    {


        Ready();
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineReadying()
    {
        

        Fight();
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineFighting()
    {
        float movingSpeed = _boss._movingSpeed;
        while (_boss.IsAlive())
        {
            // RightDown -> LeftDown
            _boss._Rigidbody2D.velocity = new Vector2(-movingSpeed, 0);
            while (_boss.transform.position.x > _angle[1].position.x)
            {
                yield return null;
            }
            _boss.transform.position = new Vector3(_angle[1].position.x, _boss.transform.position.y, _boss.transform.position.z);


            // LeftDown -> LeftUp
            _boss._Rigidbody2D.velocity = new Vector2(0, movingSpeed);
            while (_boss.transform.position.y < _angle[2].position.y)
            {
                yield return null;
            }
            _boss.transform.position = new Vector3(_boss.transform.position.x, _angle[2].position.y, _boss.transform.position.z);


            // LeftUp -> RightUp
            _boss._Rigidbody2D.velocity = new Vector2(movingSpeed, 0);
            while (_boss.transform.position.x < _angle[3].position.x)
            {
                yield return null;
            }
            _boss.transform.position = new Vector3(_angle[3].position.x, _boss.transform.position.y, _boss.transform.position.z);


            // RightUp -> RightDown
            _boss._Rigidbody2D.velocity = new Vector2(0, -_boss._movingSpeed);
            while (_boss.transform.position.y > _angle[0].position.y)
            {
                yield return null;
            }
            _boss.transform.position = new Vector3(_boss.transform.position.x, _angle[0].position.y, _boss.transform.position.z);
        }

        yield break;
    }


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("_warning으로 대체되었습니다.")]
    /// <summary>
    /// 경고 출력이 끝났다면 참입니다.
    /// </summary>
    bool _isWarningEnded = false;
    [Obsolete("_warning으로 대체되었습니다.")]
    /// <summary>
    /// 경고 출력이 끝났다면 참입니다.
    /// </summary>
    public bool IsWarningEnded
    {
        set { _isWarningEnded = value; }
    }


    [Obsolete("_script으로 대체되었습니다.")]
    /// <summary>
    /// 준비가 끝났다면 참입니다.
    /// </summary>
    bool _isReady = false;


    [Obsolete("")]
    void Update_dep()
    {
        if (_isWarningEnded == false)
        {
            return;
        }

        if (_isReady)
        {
            if (_boss.transform.position.y < _angle[0].position.y)
            {
                _boss.transform.position = new Vector2(_boss.transform.position.x, _angle[0].position.y);
                _isReady = false;
                GetComponent<AudioSource>().Play();
                return;
            }
            _boss._Rigidbody2D.velocity = Vector2.down * _boss._movingSpeed;
            return;
        }



        if (_boss.IsDead)
        {
            EndBattle();
        }



        Vector2 direction = _boss._Rigidbody2D.velocity;
        if (direction.x != 0) direction.x = direction.x > 0 ? 1 : -1;
        if (direction.y != 0) direction.y = direction.y > 0 ? 1 : -1;



        if (direction == Vector2.down && _boss.transform.position.y < _angle[0].position.y)
        {
            _boss._Rigidbody2D.velocity = Vector2.left * _boss._movingSpeed;
            _boss.transform.position = new Vector2(_boss.transform.position.x, _angle[0].position.y);
        }
        else if (direction == Vector2.left && _boss.transform.position.x < _angle[1].position.x)
        {
            _boss._Rigidbody2D.velocity = Vector2.up * _boss._movingSpeed;
            _boss.transform.position = new Vector2(_angle[1].position.x, _boss.transform.position.y);
        }
        else if (direction == Vector2.up && _boss.transform.position.y > _angle[2].position.y)
        {
            _boss._Rigidbody2D.velocity = Vector2.right * _boss._movingSpeed;
            _boss.transform.position = new Vector2(_boss.transform.position.x, _angle[2].position.y);
        }
        else if (direction == Vector2.right && _boss.transform.position.x > _angle[3].position.x)
        {
            _boss._Rigidbody2D.velocity = Vector2.down * _boss._movingSpeed;
            _boss.transform.position = new Vector2(_angle[3].position.x, _boss.transform.position.y);
        }
        else if (direction == Vector2.zero)
        {
            _boss._Rigidbody2D.velocity = Vector2.left * _boss._movingSpeed;
        }
        else
        {
            /// Debug.Log("unknown direction " + direction);
        }
    }
    [Obsolete("")]
    /// <summary>
    /// 전투 전에 캐릭터 간 스크립트를 진행합니다.
    /// </summary>
    void BeginScript()
    {
        _isReady = true;
        StartCoroutine(CoroutineBeginScript());
    }
    [Obsolete("")]
    /// <summary>
    /// 전투를 시작합니다.
    /// </summary>
    void BeginBattle()
    {
        _stageManager.RequestUnblockMoving();
    }
    [Obsolete("")]
    /// <summary>
    /// 전투를 끝냅니다.
    /// </summary>
    void EndBattle()
    {
        StartCoroutine(CoroutineEndBattle());
        Debug.Log("end of game");
    }


    [Obsolete("")]
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineBeginScript()
    {
        ActivateBossHUD();



        // 
        BeginBattle();
        yield break;
    }
    [Obsolete("")]
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineEndBattle()
    {
        _stageManager.AudioSources[9].Play();
        yield return new WaitForSeconds(_stageManager._audioClips[9].length);


        // 
        yield break;
    }

    #endregion
}
