using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 적 캐릭터가 소환되는 영역을 지정합니다.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class EnemySpawnZone : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 적 캐릭터 템플릿입니다.
    /// </summary>
    public EnemyScript _enemyTemplate;
    /// <summary>
    /// 적 캐릭터가 재소환되는 영역이라면 참입니다.
    /// </summary>
    public bool _respawnable = true;
    /// <summary>
    /// 왼쪽을 바라보고 있다면 참입니다.
    /// </summary>
    public bool _facingRight = false;
    
    #endregion

    

    #region 필드를 정의합니다.
    /// <summary>
    /// 적 캐릭터가 소환되는 영역입니다.
    /// </summary>
    BoxCollider2D _collider;
    
    /// <summary>
    /// 스폰 영역에 존재하는 적에 대한 포인터입니다.
    /// </summary>
    EnemyScript _enemyScript;
    
    /// <summary>
    /// 1회 소환된 적이 있다면 참입니다.
    /// </summary>
    bool _onceSpawned = false;
    
    #endregion
    




    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 적 소환 영역의 왼쪽 경계 X 좌표입니다.
    /// </summary>
    public float Left
    {
        get { return _collider.bounds.min.x; }
    }
    /// <summary>
    /// 적 소환 영역의 오른쪽 경계 X 좌표입니다.
    /// </summary>
    public float Right
    {
        get { return _collider.bounds.max.x; }
    }


    /// <summary>
    /// 적 캐릭터가 재소환되는 영역이라면 참입니다.
    /// </summary>
    public bool Respawnable
    {
        get { return _respawnable; }
    }


    #endregion


    


    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {

    }
    
    #endregion

    



    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 적 캐릭터 소환을 요청합니다.
    /// </summary>
    public void RequestSpawnEnemy()
    {
        if (_onceSpawned == false || Respawnable)
        {
            _enemyScript = Instantiate
                (_enemyTemplate, transform.position, transform.rotation)
                as EnemyScript;
            _enemyScript.FacingRight = _facingRight;
            _enemyScript.SpawnZone = this;

            _onceSpawned = true;
        }
    }
    /// <summary>
    /// 적 캐릭터 제거를 요청합니다.
    /// </summary>
    public void RequestDestroyEnemy()
    {
        if (_enemyScript != null)
        {
            Destroy(_enemyScript.gameObject);
        }
    }
    
    #endregion
}
