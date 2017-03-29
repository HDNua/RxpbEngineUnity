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
    /// <summary>
    /// 
    /// </summary>
    GameObject _p1 { get { return particles[0]; } }
    /// <summary>
    /// 
    /// </summary>
    GameObject _p2 { get { return particles[1]; } }

    #endregion





    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        /// _stageManager = GetComponentInParent<StageManager1P>();
    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
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
    /// <param name="player">사망한 플레이어입니다.</param>
    void Run(PlayerController player)
    {
        // 플레이어를 투명하게 만듭니다.
        player.GetComponent<SpriteRenderer>().enabled = false;

        // 사망 코루틴을 시작합니다.
        StartCoroutine(DeadEffectCoroutine());
    }

    /// <summary>
    /// 사망 코루틴입니다.
    /// </summary>
    /// <returns>사망 코루틴 IEnumerator를 반환합니다.</returns>
    private IEnumerator DeadEffectCoroutine()
    {
        // 
        float speed = bigParticleSpeed1;
        MakeParticle(_p2, new Vector2(+1.0f, +0.0f), speed);
        MakeParticle(_p2, new Vector2(+1.0f, +1.0f), speed);
        MakeParticle(_p2, new Vector2(+0.0f, +1.0f), speed);
        MakeParticle(_p2, new Vector2(-1.0f, +1.0f), speed);
        MakeParticle(_p2, new Vector2(-1.0f, +0.0f), speed);
        MakeParticle(_p2, new Vector2(-1.0f, -1.0f), speed);
        MakeParticle(_p2, new Vector2(+0.0f, -1.0f), speed);
        MakeParticle(_p2, new Vector2(+1.0f, -1.0f), speed);

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

            MakeParticle(_p1, rv1, speed);
            MakeParticle(_p1, rv2, speed);
            MakeParticle(_p1, rv3, speed);
            MakeParticle(_p1, rv4, speed);

            yield return new WaitForSeconds(particleInterval);
        }

        // 
        speed = bigParticleSpeed2;
        MakeParticle(_p2, new Vector2(+0.3f, +1.0f), speed);
        MakeParticle(_p2, new Vector2(+0.6f, +1.0f), speed);
        MakeParticle(_p2, new Vector2(+1.0f, +0.3f), speed);
        MakeParticle(_p2, new Vector2(+1.0f, +0.6f), speed);
        MakeParticle(_p2, new Vector2(+0.3f, -1.0f), speed);
        MakeParticle(_p2, new Vector2(+0.6f, -1.0f), speed);
        MakeParticle(_p2, new Vector2(+1.0f, -0.3f), speed);
        MakeParticle(_p2, new Vector2(+1.0f, -0.6f), speed);
        MakeParticle(_p2, new Vector2(-0.3f, +1.0f), speed);
        MakeParticle(_p2, new Vector2(-0.6f, +1.0f), speed);
        MakeParticle(_p2, new Vector2(-1.0f, +0.3f), speed);
        MakeParticle(_p2, new Vector2(-1.0f, +0.6f), speed);
        MakeParticle(_p2, new Vector2(-0.3f, -1.0f), speed);
        MakeParticle(_p2, new Vector2(-0.6f, -1.0f), speed);
        MakeParticle(_p2, new Vector2(-1.0f, -0.3f), speed);
        MakeParticle(_p2, new Vector2(-1.0f, -0.6f), speed);

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

            MakeParticle(_p1, rv1, speed);
            MakeParticle(_p1, rv2, speed);
            MakeParticle(_p1, rv3, speed);
            MakeParticle(_p1, rv4, speed);

            yield return new WaitForSeconds(particleInterval);
        }
        
        yield break;
    }

    /// <summary>
    /// 사망 효과 재생을 요청합니다.
    /// </summary>
    public void RequestRun(PlayerController player)
    {
        /* 
         * [v6.0.3] 다음 커밋에서 삭제할 예정입니다.
        if (player == stageManager.PlayerX)
        {
            _p1 = particles[0];
            _p2 = particles[1];
        }
        else if (player == stageManager.PlayerZ)
        {
            _p1 = particles[2];
            _p2 = particles[3];
        }
        else
        {
            throw new Exception();
        }
        */

        transform.position = player.transform.position;
        Run(player);
    }

    #endregion





    #region 구형 정의를 보관합니다.
    [Obsolete("[v6.0.3] 다음 커밋에서 삭제할 예정입니다.")]
    /// <summary>
    /// 
    /// </summary>
    StageManager1P _stageManager;


    #endregion
}