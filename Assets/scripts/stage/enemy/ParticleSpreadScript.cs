using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 사망 시 효과를 재생합니다.
/// </summary>
public class ParticleSpreadScript : MonoBehaviour
{
    #region 상수를 정의합니다.
    /// <summary>
    /// 파편이 튀는 시작 속도입니다.
    /// </summary>
    public Vector2 _velocity = new Vector2(10f, 10f);
    /// <summary>
    /// 파편에 대한 가속도입니다.
    /// </summary>
    public Vector2 _acceleration = new Vector2(0f, 20f);


    #endregion



    #region Unity에서 사용할 공용 필드를 정의합니다.
    /// <summary>
    /// 파편 스프라이트 집합입니다.
    /// </summary>
    public Sprite[] spriteParticles;


    #endregion



    #region 필드를 정의합니다.
    /// <summary>
    /// 파편 개체 집합입니다.
    /// </summary>
    GameObject[] _particles;


    #endregion



    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 객체를 초기화합니다.
    /// </summary>
    protected virtual void Awake()
    {

    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected virtual void Start()
    {
        _particles = new GameObject[spriteParticles.Length];
        for (int i = 0, len = spriteParticles.Length; i < len; ++i)
        {
            Sprite sprite = spriteParticles[i];
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.transform.localScale = new Vector3(4, 4, 1);
            go.transform.position = transform.position;
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerName = "Enemy";
            Rigidbody2D rigidbody = go.AddComponent<Rigidbody2D>();

            // 주어진 방향으로 파편을 초기화합니다.
            Vector2 dir = GetParticleSpreadDirection();

            /// rigidbody.velocity = dir * _particleSpeed;
            rigidbody.velocity = new Vector2(dir.x * _velocity.x, dir.y * _velocity.y);
            _particles[i] = go;
        }

        // 3초 후에 개체를 삭제합니다.
        Invoke("RequestDestroy", 3f);
    }
    /// <summary>
    /// FixedTimestep에 설정된 값에 따라 일정한 간격으로 업데이트 합니다.
    /// 물리 효과가 적용된 오브젝트를 조정할 때 사용됩니다.
    /// (Update는 불규칙한 호출이기 때문에 물리엔진 충돌검사가 제대로 되지 않을 수 있습니다.)
    /// </summary>
    protected virtual void FixedUpdate()
    {
        foreach (GameObject particle in _particles)
        {
            var rig = particle.GetComponent<Rigidbody2D>();

            /// Vector2 v = rig.velocity;
            /// v.y -= _particleAccelaration * Time.fixedDeltaTime;
            /// rig.velocity = v;
            rig.velocity -= _acceleration * Time.fixedDeltaTime;
        }
    }
    /// <summary>
    /// 모든 Update 함수가 호출된 후 마지막으로 호출됩니다.
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LastUpdate를 사용합니다.
    /// </summary>
    protected virtual void LateUpdate()
    {
        foreach (GameObject particle in _particles)
        {
            var renderer = particle.GetComponent<SpriteRenderer>();
            renderer.color = (renderer.color == Color.white) ?
                Color.clear : Color.white;
        }
    }

    #endregion
    




    #region 메서드를 정의합니다.
    /// <summary>
    /// 개체 제거를 요청합니다.
    /// </summary>
    public void RequestDestroy()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// 파편이 튈 방향을 설정합니다. 기본은 사망 시 파편 흩어짐 효과입니다.
    /// </summary>
    /// <returns>파편이 튈 방향을 반환합니다.</returns>
    protected virtual Vector2 GetParticleSpreadDirection()
    {
        Vector2 dir = UnityEngine.Random.insideUnitCircle;
        dir.y = Mathf.Abs(dir.y);
        return dir;
    }


    #endregion





    #region 구형 정의를 보관합니다.
    [Obsolete("_velocity로 대체되었습니다.")]
    /// <summary>
    /// 파편이 튀는 시작 속도입니다.
    /// </summary>
    public float _particleSpeed = 10f;
    [Obsolete("_acceleration으로 대체되었습니다.")]
    /// <summary>
    /// 파편에 대한 가속도입니다.
    /// </summary>
    public float _particleAccelaration = 20f;


    #endregion
}