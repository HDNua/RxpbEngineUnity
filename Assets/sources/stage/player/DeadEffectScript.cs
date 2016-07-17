using System;
using UnityEngine;
using System.Collections;



/// <summary>
/// 사망 시 효과를 재생합니다.
/// </summary>
public class DeadEffectScript : MonoBehaviour
{
    #region 상수를 정의합니다.
    const float firstParticleSpeed = 5;
    const float secondParticleSpeed = 2;


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
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (running == false)
        {
            return;
        }

        if (runTime <= 0)
        {
            float speed = firstParticleSpeed;
            MakeParticle(p1, new Vector2(1, 0), speed);
            MakeParticle(p1, new Vector2(1, 1), speed);
            MakeParticle(p1, new Vector2(0, 1), speed);
            MakeParticle(p1, new Vector2(-1, 1), speed);
            MakeParticle(p1, new Vector2(-1, 0), speed);
            MakeParticle(p1, new Vector2(-1, -1), speed);
            MakeParticle(p1, new Vector2(0, -1), speed);
            MakeParticle(p1, new Vector2(1, -1), speed);

            speed = secondParticleSpeed;
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



    #region 메서드를 정의합니다.
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
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
    }
    /// <summary>
    /// 사망 효과 재생을 요청합니다.
    /// </summary>
    public void RequestRun(PlayerController player)
    {
        if (player == stageManager._playerX)
        {
            p1 = particles[0];
            p2 = particles[1];
        }
        else if (player == stageManager._playerZ)
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
}