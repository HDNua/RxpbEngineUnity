using UnityEngine;
using System.Collections;

/// <summary>
/// 사망 시 효과를 재생합니다.
/// </summary>
public class DeadEffectScript : MonoBehaviour
{
    public GameObject[] particles;
    StageManager stageManager;
    GameObject p1;
    GameObject p2;

    bool running = false;
    float runTime = 0;

    const float firstParticleSpeed = 5;
    const float secondParticleSpeed = 2;

    void Awake()
    {
        stageManager = GetComponentInParent<StageManager>();
    }
    void Start()
    {

    }
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
        stageManager.player.GetComponent<SpriteRenderer>().enabled = false;
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
}