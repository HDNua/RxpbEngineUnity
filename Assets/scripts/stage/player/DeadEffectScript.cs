using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 사망 시 효과를 재생합니다.
/// </summary>
public class DeadEffectScript : MonoBehaviour
{
    #region 상수를 정의합니다.
    public float smallParticleSpeed = 10;
    public int smallParticleSpreadCount1 = 4;
    public int smallParticleSpreadCount2 = 8;

    public float bigParticleSpeed1 = 3;
    public float bigParticleSpeed2 = 4;

    public float particleInterval = 0.1f;


    #endregion



    #region Unity에서 사용할 공용 필드를 정의합니다.
    public GameObject[] particles;


    #endregion



    #region 필드를 정의합니다.
    StageManager stageManager;
    GameObject p1;
    GameObject p2;


    bool running = false;
    float runTime = 0;


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
    /// 
    /// </summary>
    void Start()
    {

    }

    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 사망 효과 파티클을 생성합니다.
    /// </summary>
    /// <param name="type">사망 효과 타입입니다.</param>
    /// <param name="dir">사망 효과 파티클이 퍼져나가는 방향입니다.</param>
    /// <param name="speed">사망 효과 파티클이 퍼져나가는 속력입니다.</param>
    /// <returns>새 사망 효과 파티클을 반환합니다.</returns>
    GameObject MakeParticle(GameObject type, Vector2 dir, float speed)
    {
        GameObject particle = Instantiate
            (type, transform.position, transform.rotation)
            as GameObject;
        particle.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
        return particle;
    }
    /// <summary>
    /// 사망 효과를 재생합니다.
    /// </summary>
    void Run()
    {
        running = true;
        runTime = 0;

        // 플레이어를 투명하게 만듭니다.
        stageManager._player.GetComponent<SpriteRenderer>().enabled = false;

        // 
        StartCoroutine(DeadEffectCoroutine());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeadEffectCoroutine()
    {
        // 
        float speed = bigParticleSpeed1;
        MakeParticle(p2, new Vector2(+1.0f, +0.0f), speed);
        MakeParticle(p2, new Vector2(+1.0f, +1.0f), speed);
        MakeParticle(p2, new Vector2(+0.0f, +1.0f), speed);
        MakeParticle(p2, new Vector2(-1.0f, +1.0f), speed);
        MakeParticle(p2, new Vector2(-1.0f, +0.0f), speed);
        MakeParticle(p2, new Vector2(-1.0f, -1.0f), speed);
        MakeParticle(p2, new Vector2(+0.0f, -1.0f), speed);
        MakeParticle(p2, new Vector2(+1.0f, -1.0f), speed);

        // 
        speed = smallParticleSpeed;
        for (int i = 0; i < smallParticleSpreadCount1; ++i)
        {
            Vector2 rv1 = UnityEngine.Random.insideUnitCircle;
            Vector2 rv2 = UnityEngine.Random.insideUnitCircle;
            Vector2 rv3 = UnityEngine.Random.insideUnitCircle;
            Vector2 rv4 = UnityEngine.Random.insideUnitCircle;

            rv1.Normalize();
            rv2.Normalize();
            rv3.Normalize();
            rv4.Normalize();

            MakeParticle(p1, rv1, speed);
            MakeParticle(p1, rv2, speed);
            MakeParticle(p1, rv3, speed);
            MakeParticle(p1, rv4, speed);

            yield return new WaitForSeconds(particleInterval);
        }

        // 
        speed = bigParticleSpeed2;
        MakeParticle(p2, new Vector2(+0.3f, +1.0f), speed);
        MakeParticle(p2, new Vector2(+0.6f, +1.0f), speed);
        MakeParticle(p2, new Vector2(+1.0f, +0.3f), speed);
        MakeParticle(p2, new Vector2(+1.0f, +0.6f), speed);
        MakeParticle(p2, new Vector2(+0.3f, -1.0f), speed);
        MakeParticle(p2, new Vector2(+0.6f, -1.0f), speed);
        MakeParticle(p2, new Vector2(+1.0f, -0.3f), speed);
        MakeParticle(p2, new Vector2(+1.0f, -0.6f), speed);
        MakeParticle(p2, new Vector2(-0.3f, +1.0f), speed);
        MakeParticle(p2, new Vector2(-0.6f, +1.0f), speed);
        MakeParticle(p2, new Vector2(-1.0f, +0.3f), speed);
        MakeParticle(p2, new Vector2(-1.0f, +0.6f), speed);
        MakeParticle(p2, new Vector2(-0.3f, -1.0f), speed);
        MakeParticle(p2, new Vector2(-0.6f, -1.0f), speed);
        MakeParticle(p2, new Vector2(-1.0f, -0.3f), speed);
        MakeParticle(p2, new Vector2(-1.0f, -0.6f), speed);

        // 
        speed = smallParticleSpeed;
        for (int i = 0; i < smallParticleSpreadCount2; ++i)
        {
            Vector2 rv1 = UnityEngine.Random.insideUnitCircle;
            Vector2 rv2 = UnityEngine.Random.insideUnitCircle;
            Vector2 rv3 = UnityEngine.Random.insideUnitCircle;
            Vector2 rv4 = UnityEngine.Random.insideUnitCircle;

            rv1.Normalize();
            rv2.Normalize();
            rv3.Normalize();
            rv4.Normalize();

            MakeParticle(p1, rv1, speed);
            MakeParticle(p1, rv2, speed);
            MakeParticle(p1, rv3, speed);
            MakeParticle(p1, rv4, speed);

            yield return new WaitForSeconds(particleInterval);
        }

        yield return false;
    }

    /// <summary>
    /// 사망 효과 재생을 요청합니다.
    /// </summary>
    public void RequestRun(PlayerController player)
    {
        if (player == stageManager.PlayerX)
        {
            p1 = particles[0];
            p2 = particles[1];
        }
        else if (player == stageManager.PlayerZ)
        {
            p1 = particles[2];
            p2 = particles[3];
        }
        else
        {
            throw new System.Exception();
        }

        transform.position = player.transform.position;
        Run();
    }


    #endregion










    #region 구형 정의를 보관합니다.
    [Obsolete("DeadEffectCoroutine()으로 대체되었습니다.")]
    /// <summary>
    /// 
    /// </summary>
    void Update_dep()
    {
        if (running == false)
        {
            return;
        }

        if (runTime <= 0)
        {
            float speed = smallParticleSpeed;
            MakeParticle(p1, new Vector2(1, 0), speed);
            MakeParticle(p1, new Vector2(1, 1), speed);
            MakeParticle(p1, new Vector2(0, 1), speed);
            MakeParticle(p1, new Vector2(-1, 1), speed);
            MakeParticle(p1, new Vector2(-1, 0), speed);
            MakeParticle(p1, new Vector2(-1, -1), speed);
            MakeParticle(p1, new Vector2(0, -1), speed);
            MakeParticle(p1, new Vector2(1, -1), speed);

            speed = bigParticleSpeed2;
            MakeParticle(p2, new Vector2(1, 0.5f), speed);
            MakeParticle(p2, new Vector2(1, 2f), speed);
            MakeParticle(p2, new Vector2(-1, 2f), speed);
            MakeParticle(p2, new Vector2(-1, 0.5f), speed);
            MakeParticle(p2, new Vector2(-1, -0.5f), speed);
            MakeParticle(p2, new Vector2(-1, -2f), speed);
            MakeParticle(p2, new Vector2(1, -2f), speed);
            MakeParticle(p2, new Vector2(1, -0.5f), speed);
        }

        runTime += Time.deltaTime;
    }


    #endregion
}