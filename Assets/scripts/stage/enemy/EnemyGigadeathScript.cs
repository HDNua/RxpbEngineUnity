using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 
/// </summary>
public class EnemyGigadeathScript : EnemyScript, IFlippableEnemy
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
    /// 
    /// </summary>
    public Transform[] _shotPoints;
    /// <summary>
    /// 
    /// </summary>
    public EnemyGigadeathBulletScript _bullet;


    #endregion










    #region 캐릭터의 상태 필드 및 프로퍼티를 정의합니다.
    /// <summary>
    /// 캐릭터가 오른쪽을 보고 있다면 참입니다.
    /// </summary>
    bool _facingRight = false;
    /// <summary>
    /// 캐릭터가 오른쪽을 보고 있다면 참입니다.
    /// </summary>
    public bool FacingRight
    {
        get { return _facingRight; }
        set { if (_facingRight != value) Flip(); }
    }


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
        RaycastHit2D groundRay = Physics2D.Raycast(_groundCheck.position, Vector2.down, 10f, _whatIsGround);
        Vector2 newPos = transform.position;
        newPos.y -= Mathf.Abs(_collider2D.bounds.min.y - groundRay.point.y);
        transform.position = newPos;
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }
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
        int dropItem = UnityEngine.Random.Range(0, _items.Length);
        if (_items[dropItem] != null)
        {
            CreateItem(_items[dropItem]);
        }

        // 캐릭터가 사망합니다.
        base.Dead();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    public override void Hurt(int damage)
    {
        base.Hurt(damage);

        // 
        StartCoroutine(CoroutineInvencible());
    }


    /// <summary>
    /// 1/30 프레임 간의 시간입니다.
    /// </summary>
    public const float TIME_30FPS = 0.0333333f;
    /// <summary>
    /// 1/60 프레임 간의 시간입니다.
    /// </summary>
    public const float TIME_60FPS = 0.0166667f;
    /// <summary>
    /// 
    /// </summary>
    const float INVENCIBLE_TIME_LENGTH = 1f;
    Color[] _currentPalette = null;


    /// <summary>
    /// 엑스의 색상을 업데이트합니다.
    /// </summary>
    void UpdateColor()
    {
        if (_currentPalette != null)
        {
            // 바디 색상을 맞춥니다.
            UpdateBodyColor(_currentPalette);
        }
    }
    /// <summary>
    /// 엑스의 색상을 주어진 팔레트로 업데이트합니다.
    /// </summary>
    /// <param name="_currentPalette">현재 팔레트입니다.</param>
    void UpdateBodyColor(Color[] currentPalette)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Texture2D texture = renderer.sprite.texture;

        // !!!!! IMPORTANT !!!!!
        // 1. 텍스쳐 파일은 Read/Write 속성이 Enabled여야 합니다.
        // 2. 반드시 Generate Mip Maps 속성을 켜십시오.
        Color[] colors = texture.GetPixels();
        Color[] pixels = new Color[colors.Length];
        Color[] DefaultPalette = EnemyColorPalette.GigadeathPalette;


        // 모든 픽셀을 돌면서 색상을 업데이트합니다.
        for (int pixelIndex = 0, pixelCount = colors.Length; pixelIndex < pixelCount; ++pixelIndex)
        {
            Color color = colors[pixelIndex];
            if (color.a == 1)
            {
                for (int targetIndex = 0, targetPixelCount = DefaultPalette.Length; targetIndex < targetPixelCount; ++targetIndex)
                {
                    Color colorDst = DefaultPalette[targetIndex];
                    if (Mathf.Approximately(color.r, colorDst.r) &&
                        Mathf.Approximately(color.g, colorDst.g) &&
                        Mathf.Approximately(color.b, colorDst.b) &&
                        Mathf.Approximately(color.a, colorDst.a))
                    {
                        pixels[pixelIndex] = currentPalette[targetIndex];
                        break;
                    }
                }
            }
            else
            {
                pixels[pixelIndex] = color;
            }
        }


        // 텍스쳐를 복제하고 새 픽셀 팔레트로 덮어씌웁니다.
        Texture2D cloneTexture = new Texture2D(texture.width, texture.height);
        cloneTexture.filterMode = FilterMode.Point;
        cloneTexture.SetPixels(pixels);
        cloneTexture.Apply();

        // 새 텍스쳐를 렌더러에 반영합니다.
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", cloneTexture);
        renderer.SetPropertyBlock(block);
    }
    /// <summary>
    /// 무적 상태에 대한 코루틴입니다.
    /// </summary>
    /// <returns>코루틴 열거자입니다.</returns>
    IEnumerator CoroutineInvencible()
    {
        float invencibleTime = 0;
        bool invencibleColorState = false;
        while (invencibleTime < INVENCIBLE_TIME_LENGTH)
        {
            invencibleTime += TIME_30FPS + Time.deltaTime;

            if (invencibleColorState)
            {
                TESTEST1();
            }
            else
            {
                TESTEST2();
            }
            invencibleColorState = !invencibleColorState;
            yield return new WaitForSeconds(TIME_30FPS);
        }
        Invencible = false;
        TESTEST3();
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    protected void TESTEST1()
    {
        _currentPalette = EnemyColorPalette.InvenciblePalette;
        // UpdateBodyColor();
    }
    /// <summary>
    /// 
    /// </summary>
    protected void TESTEST2()
    {
        ResetBodyColor();
    }
    /// <summary>
    /// 
    /// </summary>
    protected void TESTEST3()
    {
        ResetBodyColor();
    }
    /// <summary>
    /// 엑스의 바디 색상표를 현재 웨폰 상태로 되돌립니다.
    /// </summary>
    void ResetBodyColor()
    {
        _currentPalette = EnemyColorPalette.GigadeathPalette;
    }


    #endregion










    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 방향을 바꿉니다.
    /// </summary>
    public void Flip()
    {
        if (_facingRight)
        {
            _rigidbody.transform.localScale = new Vector3
                (-_rigidbody.transform.localScale.x, _rigidbody.transform.localScale.y);
        }
        else
        {
            _rigidbody.transform.localScale = new Vector3
                (-_rigidbody.transform.localScale.x, _rigidbody.transform.localScale.y);
        }
        _facingRight = !_facingRight;
    }

    /// <summary>
    /// 탄환을 발사합니다.
    /// </summary>
    /// <param name="shotPosition">탄환을 발사할 위치입니다.</param>
    public void Shot(Transform shotPosition)
    {
        SoundEffects[1].Play();
        Instantiate(effects[1], shotPosition.position, shotPosition.rotation);
        Instantiate(_bullet, shotPosition.position, shotPosition.rotation);
    }


    #endregion



    #region 요청 메서드를 정의합니다.
    /// <summary>
    /// 탄환 발사를 요청합니다.
    /// </summary>
    public void RequestShot()
    {
        Shot(_shotPoints[Random.Range(0, _shotPoints.Length)]);
    }


    #endregion










    #region 구형 정의를 보관합니다.


    #endregion
}
