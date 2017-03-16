using UnityEngine;
using System;
using System.Collections;



/// <summary>
/// 박쥐 적 캐릭터를 정의합니다.
/// </summary>
public class EnemyBattonBoneScript : EnemyScript
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    Rigidbody2D _rigidbody;

    #endregion





    #region Unity에서 접근 가능한 공용 객체를 정의합니다.
    /// <summary>
    /// 자신의 밑에 지면이 존재하는지 검사하기 위해 사용합니다.
    /// </summary>
    public Transform _groundCheck;
    /// <summary>
    /// 자신이 진행하는 방향에 벽이 존재하는지 검사하기 위해 사용합니다.
    /// </summary>
    public Transform _pushCheck;
    /// <summary>
    /// 무엇이 벽인지를 결정합니다. 기본값은 "Wall, MapBlock"입니다.
    /// </summary>
    public LayerMask _whatIsWall;
    /// <summary>
    /// 무엇이 땅인지를 결정합니다. 기본값은 "Ground, TiledGeometry"입니다.
    /// </summary>
    public LayerMask _whatIsGround;

    #endregion
    




    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.
    /// <summary>
    /// 캐릭터가 움직이는 속도를 정의합니다.
    /// </summary>
    public float _movingSpeed = 3;
    /// <summary>
    /// 캐릭터가 도망치는 속도를 정의합니다.
    /// </summary>
    public float _runawaySpeed = 5;

    /// <summary>
    /// 진동 폭의 최솟값입니다.
    /// </summary>
    public float _amp_min = 1;
    /// <summary>
    /// 진동 폭의 최댓값입니다.
    /// </summary>
    public float _amp_max = 2;
    /// <summary>
    /// 각진동수입니다.
    /// </summary>
    public float _ang_freq = 3;
    /// <summary>
    /// 개체가 생존한 시간입니다.
    /// </summary>
    public float _time = 0f;

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

        // 컬러 팔레트를 설정합니다.
        DefaultPalette = EnemyColorPalette.BattonBonePalette;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        // 사용할 변수를 선언합니다.
        Vector2 relativePos = _StageManager.GetCurrentPlayerPosition() - transform.position;
        float angle = Mathf.Atan2(relativePos.y, relativePos.x);
        float distortion = UnityEngine.Random.Range(_amp_min, _amp_max)
            * Mathf.Sin(_ang_freq * _time);

        // 개체의 속도를 변경합니다.
        float vx = _movingSpeed * Mathf.Cos(angle);
        float vy = _movingSpeed * Mathf.Sin(angle) + distortion;
        _rigidbody.velocity = new Vector2(vx, vy);

        // 플레이어를 쫓아갑니다.
        if (relativePos.x < 0 && FacingRight)
            Flip();
        else if (relativePos.x > 0 && !FacingRight)
            Flip();

        // 업데이트의 끝입니다.
        _time += Time.deltaTime;
    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    protected override void LateUpdate()
    {
        base.LateUpdate();

        // 색상을 업데이트합니다.
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
        CreateExplosion(transform.position);

        // 사망 시 아이템 드롭 루틴입니다.
        int dropItem = UnityEngine.Random.Range(0, _items.Length);
        if (_items[dropItem] != null)
        {
            CreateItem(_items[dropItem]);
        }

        // 캐릭터가 사망합니다.
        base.Dead();

        // 
        Invoke("RequestDestroy", 3f);
    }


    #endregion

    



    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 왼쪽으로 이동합니다.
    /// </summary>
    void MoveLeft()
    {
        if (FacingRight)
            Flip();
        _rigidbody.velocity = new Vector2(-_movingSpeed, 0);
    }
    /// <summary>
    /// 오른쪽으로 이동합니다.
    /// </summary>
    void MoveRight()
    {
        if (FacingRight == false)
            Flip();
        _rigidbody.velocity = new Vector2(_movingSpeed, 0);
    }
    /// <summary>
    /// 주변을 방황합니다.
    /// </summary>
    /// <returns>StartCoroutine 호출에 적합한 값을 반환합니다.</returns>
    IEnumerator WalkAround()
    {
        while (Health != 0)
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 1)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
            yield return new WaitForSeconds(3);
        }
    }

    #endregion


    


    #region 구형 정의를 보관합니다.


    #endregion
}