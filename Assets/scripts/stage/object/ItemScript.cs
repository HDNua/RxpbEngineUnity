using System;
using UnityEngine;
using System.Collections;



[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
/// <summary>
/// 아이템 스크립트입니다.
/// </summary>
public class ItemScript : MonoBehaviour
{
    #region 공용 형식을 정의합니다.
    public enum ItemType : int
    {
        LifeEnergy,
        WeaponEnergy,
        LifeUp,
        ECan,
        WCan,
        XCan,
        TryUp,
    }

    #endregion



    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    /// <summary>
    /// 렌더러 개체입니다.
    /// </summary>
    SpriteRenderer _renderer;
    /// <summary>
    /// Rigidbody2D 개체입니다.
    /// </summary>
    Rigidbody2D _rigidbody;

    #endregion


    


    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 아이템을 사용했을 때 재생할 효과음의 인덱스 리스트입니다.
    /// </summary>
    public int[] _soundEffectIndexes;
    
    /// <summary>
    /// 아이템의 형식을 표현합니다.
    /// </summary>
    public string _itemType;
    /// <summary>
    /// 아이템의 증감량을 정합니다.
    /// </summary>
    public int _itemValue;
    
    /// <summary>
    /// 아이템이 드롭될 확률입니다. 0에서 100 사이의 값을 가집니다.
    /// </summary>
    public int _probability = 50;
    
    /// <summary>
    /// 특수 아이템이라면 참입니다.
    /// </summary>
    public bool _markedSpecial;

    /// <summary>
    /// 무엇이 땅인지를 결정합니다. 기본값은 "Ground, TiledGeometry"입니다.
    /// </summary>
    public LayerMask _whatIsGround;

    /// <summary>
    /// 
    /// </summary>
    public Vector2 _initialVelocity = new Vector2(0, 10);
    /// <summary>
    /// 
    /// </summary>
    public Vector2 _acceleration = new Vector2(0, 20);
    /// <summary>
    /// 
    /// </summary>
    public float _landOffset = 2f;

    #endregion

    



    #region 필드를 정의합니다.
    /// <summary>
    /// 적 캐릭터가 사망하면서 떨어뜨린 아이템이라면 참입니다.
    /// </summary>
    bool _isDropped;
    /// <summary>
    /// 떨어진 아이템의 생존 시간입니다.
    /// </summary>
    float _lifetime = 4f;
    /// <summary>
    /// 떨어진 아이템이 반짝이기 시작하는 시간입니다.
    /// </summary>
    float _blinkTime = 2f;

    /// <summary>
    /// 
    /// </summary>
    RaycastHit2D _groundRay;
    /// <summary>
    /// 
    /// </summary>
    bool _landed = false;
    
    #endregion

    



    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 아이템을 사용했을 때 재생할 효과음의 인덱스 리스트입니다.
    /// </summary>
    public int[] SoundEffectIndexes
    {
        get { return _soundEffectIndexes; }
    }
    /// <summary>
    /// 아이템의 형식을 표현합니다.
    /// </summary>
    public string Type
    {
        get { return _itemType; }
    }
    /// <summary>
    /// 아이템의 증감량을 정합니다.
    /// </summary>
    public int Value
    {
        get { return _itemValue; }
    }
    
    /// <summary>
    /// 적 캐릭터가 사망하면서 떨어뜨린 아이템이라면 참입니다.
    /// </summary>
    public bool IsDropped
    {
        get { return _isDropped; }
        set { _isDropped = value; }
    }
    
    /// <summary>
    /// 아이템이 드롭될 확률입니다. 0에서 100 사이의 값을 가집니다.
    /// </summary>
    public int Probability
    {
        get { return _probability; }
        set { _probability = Mathf.Clamp(value, 0, 100); }
    }

    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    StageManager1P _StageManager { get { return StageManager1P.Instance; } }

    #endregion


    


    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();

        // 
        _rigidbody.velocity = _initialVelocity;

        // 
        /// Handy.Log("{0}: {1}", name, transform.position);
        /// _groundRay = Physics2D.Raycast(transform.position, Vector2.down, 10f, _whatIsGround);
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
        /// Debug.DrawRay(transform.position, Vector3.down * (10f), Color.red);

        // 땅을 감지하기 전까지 루틴을 수행하지 않습니다.
        if (_groundRay == false)
        {
            _groundRay = Physics2D.Raycast(transform.position, Vector2.down, 10f, _whatIsGround);
            _rigidbody.velocity -= _acceleration * Time.deltaTime;
        }
        // 땅을 감지한 이후엔 착륙하지 않은 경우에만 수행합니다.
        else if (_landed == false)
        {
            if (transform.position.y - _groundRay.point.y < _landOffset)
            {
                transform.position = _groundRay.point + new Vector2(0, _landOffset);
                _rigidbody.velocity = Vector2.zero;

                _landed = true;
            }
            else
            {
                _rigidbody.velocity -= _acceleration * Time.deltaTime;
            }
        }

        // 특별한 아이템으로 마크되어있다면
        if (_markedSpecial)
        {
            // 사라지지 않게 합니다.
            return;
        }
        
        // 드롭된 아이템이라면
        if (_isDropped)
        {
            _lifetime -= Time.deltaTime;
            if (_lifetime < 0f)
            {
                Destroy(gameObject);
                return;
            }
            else if (_lifetime < _blinkTime)
            {
                _renderer.color = (_renderer.color == Color.white) ? Color.clear : Color.white;
            }
        }
    }

    #endregion





    #region Trigger 관련 메서드를 정의합니다.
    /// <summary>
    /// 충돌체가 트리거 내부로 진입했습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 닿았다면 플레이어가 획득합니다.
        if (other.CompareTag("Player"))
        {
            // 사용할 변수를 선언합니다.
            GameObject pObject = other.gameObject;
            PlayerController player = pObject.GetComponent<PlayerController>();

            // 아이템 효과를 발동합니다.
            _StageManager.ActivateItem(player, this);

            // 먹은 아이템을 삭제합니다.
            Destroy(gameObject);
        }
    }


    #endregion









    #region 메서드를 정의합니다.


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
