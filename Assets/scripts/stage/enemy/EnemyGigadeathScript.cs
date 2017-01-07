using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 기가데스를 정의합니다.
/// </summary>
public class EnemyGigadeathScript : EnemyScript, IShootableEnemy
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    /// <summary>
    /// Rigidbody2D 요소를 가져옵니다.
    /// </summary>
    Rigidbody2D _rigidbody;
    /// <summary>
    /// BoxCollider2D 요소를 가져옵니다.
    /// </summary>
    Collider2D _collider2D;


    #endregion
    


    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    /// <summary>
    /// 자신의 밑에 지면이 존재하는지 검사하기 위해 사용합니다.
    /// </summary>
    public Transform _groundCheck;
    /// <summary>
    /// 무엇이 땅인지를 결정합니다. 기본값은 "Ground, TiledGeometry"입니다.
    /// </summary>
    public LayerMask _whatIsGround;
    
    /// <summary>
    /// 탄환 시작 위치입니다.
    /// </summary>
    public Transform[] _shotPoints;
    /// <summary>
    /// 탄환 개체입니다.
    /// </summary>
    public EnemyBulletScript _bullet;
    
    #endregion



    

    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.

    #endregion



    

    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 필드를 초기화합니다.
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();

        // 자신과 가장 가까운 바닥으로 y 좌표를 옮깁니다.
        RaycastHit2D groundRay = Physics2D.Raycast
            (_groundCheck.position, Vector2.down, 10f, _whatIsGround);
        Vector2 newPos = transform.position;
        newPos.y -= Mathf.Abs(_collider2D.bounds.min.y - groundRay.point.y);
        transform.position = newPos;

        // 컬러 팔레트를 설정합니다.
        DefaultPalette = EnemyColorPalette.GigadeathPalette;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    protected override void LateUpdate()
    {
        base.LateUpdate();

        UpdateColor();
    }


    #endregion



    

    #region Collider2D의 기본 메서드를 재정의합니다.
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
    




    #region EnemyScript의 메서드를 오버라이드합니다.
    /// <summary>
    /// 캐릭터가 사망합니다.
    /// </summary>
    public override void Dead()
    {
        // 폭발 효과를 생성하고 효과음을 재생합니다.
        SoundEffects[0].Play();
        Instantiate(effects[0], transform.position, transform.rotation);

        // 사망 시 아이템 드롭 루틴입니다.
        int dropItem = Random.Range(0, _items.Length);
        if (_items[dropItem] != null)
        {
            CreateItem(_items[dropItem]);
        }

        // 캐릭터가 사망합니다.
        base.Dead();
    }
    /// <summary>
    /// 캐릭터에게 대미지를 입힙니다.
    /// </summary>
    /// <param name="damage">대미지 값입니다.</param>
    public override void Hurt(int damage)
    {
        base.Hurt(damage);

        // 무적 상태 코루틴을 시작합니다.
        StartCoroutine(CoroutineInvencible());
    }

    #endregion





    #region IShootableEnemy를 구현합니다.
    /// <summary>
    /// 탄환 발사를 요청합니다.
    /// </summary>
    public void RequestShot()
    {
        Shot(_shotPoints[Random.Range(0, _shotPoints.Length)]);
    }
    /// <summary>
    /// 탄환을 발사합니다.
    /// </summary>
    /// <param name="shotPosition">탄환을 발사할 위치입니다.</param>
    public void Shot(Transform shotPosition)
    {
        SoundEffects[1].Play();
        Instantiate(effects[1], shotPosition.position, shotPosition.rotation);

        // 
        EnemyBulletScript bullet = Instantiate
            (_bullet, shotPosition.position, shotPosition.rotation)
            as EnemyBulletScript;
        bullet.transform.parent = _StageManager._enemyParent.transform;
        bullet.MoveTo(FacingRight ? Vector3.right : Vector3.left);
    }

    #endregion
    


    

    #region 보조 메서드를 정의합니다.


    #endregion





    #region 요청 메서드를 정의합니다.


    #endregion





    #region 구형 정의를 보관합니다.


    #endregion
}
