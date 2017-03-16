using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// Commander Yammark 보스 적 스크립트입니다.
/// </summary>
public class CommanderYammarkScript : EnemyScript, IBossEnemy
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 캐릭터의 이동 속도입니다.
    /// </summary>
    public float _movingSpeed = 5;


    /// <summary>
    /// 캐릭터가 등장할 지점입니다.
    /// </summary>
    public Transform _appearPoint;


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    Rigidbody2D _rigidbody;


    #endregion










    #region 프로퍼티를 정의합니다.
    /// <summary>
    /// 충돌체 컴포넌트입니다.
    /// </summary>
    public Rigidbody2D _Rigidbody2D
    {
        get { return _rigidbody; }
    }


    #endregion










    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        /// base.Start();

        _rigidbody = GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBevahiour 개체 정보를 업데이트합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }


    #endregion










    #region Trigger 관련 메서드를 재정의합니다.
    /// <summary>
    /// 충돌체가 여전히 트리거 내부에 있습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        // 트리거가 발동한 상대 충돌체가 플레이어라면 대미지를 입힙니다.
        if (other.CompareTag("Player"))
        {
            GameObject pObject = other.gameObject;
            PlayerController player = pObject.GetComponent<PlayerController>();


            // 플레이어가 무적 상태이거나 죽었다면
            if (player.Invencible || player.IsDead)
            {
                // 아무 것도 하지 않습니다.

            }
            // 그 외의 경우
            else
            {
                // 플레이어에게 대미지를 입힙니다.
                player.Hurt(Damage);
            }
        }
    }


    #endregion










    #region EnemyScript의 메서드를 오버라이드 합니다.
    /// <summary>
    /// 캐릭터가 사망합니다.
    /// </summary>
    public override void Dead()
    {
        /**
        // 사망 코루틴을 시작합니다.
        StartCoroutine(DeadCoroutine());

        // 폭발 효과를 생성하고 효과음을 재생합니다.
        SoundE_ffects[0].Play();
        Instantiate(e_ffects[0], transform.position, transform.rotation);

        // 사망 시 아이템 드롭 루틴입니다.
        int dropItem = UnityEngine.Random.Range(0, _items.Length);
        if (_items[dropItem] != null)
        {
            CreateItem(_items[dropItem]);
        }

        // 캐릭터가 사망합니다.
        base.Dead();
        */

        throw new NotImplementedException();
    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator DeadCoroutine()
    {





        // 캐릭터가 사망합니다.
        base.Dead();
        yield break;
    }


    #endregion









    #region IBossEnemy 인터페이스를 구현합니다.
    /// <summary>
    /// 보스 캐릭터가 등장합니다.
    /// </summary>
    void IBossEnemy.Appear()
    {
        
    }


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
