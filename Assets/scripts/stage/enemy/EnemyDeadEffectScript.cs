using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 사망 시 효과를 재생합니다.
/// </summary>
public class EnemyDeadEffectScript : MonoBehaviour
{
    #region 상수를 정의합니다.
    /// <summary>
    /// 파편이 튀는 시작 속도입니다.
    /// </summary>
    public float _particleSpeed = 10f;
    /// <summary>
    /// 파편에 대한 가속도입니다.
    /// </summary>
    public float _particleAccelaration = 20f;


    #endregion



    #region Unity에서 사용할 공용 필드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    public Sprite[] spriteParticles;


    #endregion



    #region 필드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    StageManager stageManager;
    /// <summary>
    /// 
    /// </summary>
    GameObject[] particles;


    #endregion



    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        stageManager = GetComponentInParent<StageManager>();
    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        particles = new GameObject[spriteParticles.Length];
        for (int i = 0, len = spriteParticles.Length; i < len; ++i)
        {
            Sprite sprite = spriteParticles[i];
            Vector2 dir = UnityEngine.Random.insideUnitCircle;
            dir.y = Mathf.Abs(dir.y);

            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.transform.localScale = new Vector3(4, 4, 1);
            go.transform.position = transform.position;

            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerName = "Enemy";

            Rigidbody2D rigidbody = go.AddComponent<Rigidbody2D>();
            rigidbody.velocity = dir * _particleSpeed;

            particles[i] = go;
        }

        // 
        Invoke("RequestDestroy", 3f);
    }
    /// <summary>
    /// FixedTimestep에 설정된 값에 따라 일정한 간격으로 업데이트 합니다.
    /// 물리 효과가 적용된 오브젝트를 조정할 때 사용됩니다.
    /// (Update는 불규칙한 호출이기 때문에 물리엔진 충돌검사가 제대로 되지 않을 수 있습니다.)
    /// </summary>
    void FixedUpdate()
    {
        foreach (GameObject particle in particles)
        {
            var rig = particle.GetComponent<Rigidbody2D>();
            Vector2 v = rig.velocity;
            v.y -= _particleAccelaration * Time.fixedDeltaTime;
            rig.velocity = v;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void LateUpdate()
    {
        foreach (GameObject particle in particles)
        {
            var renderer = particle.GetComponent<SpriteRenderer>();
            renderer.color = (renderer.color == Color.white) ?
                Color.clear : Color.white;
        }
    }

    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    void RequestDestroy()
    {
        Destroy(gameObject);
    }

    #endregion


    
    #region 구형 정의를 보관합니다.

    #endregion
}