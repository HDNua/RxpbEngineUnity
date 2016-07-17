using System;
using UnityEngine;
using System.Collections;



[RequireComponent(typeof(BoxCollider2D))]
/// <summary>
/// 아이템 스크립트입니다.
/// </summary>
public class ItemScript : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    SpriteRenderer _renderer;


    #endregion










    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// Scene 관리자입니다.
    /// </summary>
    public StageManager _sceneManager;


    /// <summary>
    /// 아이템의 형식을 표현합니다.
    /// </summary>
    public string _itemType;
    /// <summary>
    /// 아이템의 증감량을 정합니다.
    /// </summary>
    public int _itemValue;


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


    #endregion









    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 적 캐릭터가 사망하면서 떨어뜨린 아이템이라면 참입니다.
    /// </summary>
    public bool IsDropped
    {
        get { return _isDropped; }
        set { _isDropped = value; }
    }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        // 필드를 초기화합니다.
        /// _triggerBox = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트 합니다.
    /// </summary>
    void Update()
    {
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
            // 게임 종료 마크를 만났다면
            if (_itemType == "EndGame")
            {
                LoadingSceneManager.LoadLevel("CS03_GaiaFound");
            }
            // 
            else
            {
                // 사용할 변수를 선언합니다.
                GameObject pObject = other.gameObject;
                PlayerController player = pObject.GetComponent<PlayerController>();

                // 아이템 효과를 발동합니다.
                _sceneManager.RequestHeal(player, _itemValue);

                // 먹은 아이템을 삭제합니다.
                Destroy(gameObject);
            }
        }
    }


    #endregion









    #region 메서드를 정의합니다.


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
