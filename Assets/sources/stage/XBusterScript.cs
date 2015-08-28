using UnityEngine;
using System.Collections;

/// <summary>
/// 버스터 공격 스크립트입니다.
/// </summary>
public class XBusterScript : AttackScript
{
    Camera mainCamera;
    public Camera MainCamera { set { mainCamera = value; } }

    void Start()
    {

    }
    void Update()
    {
        if (mainCamera != null)
        {
            Vector3 camPos = mainCamera.transform.position;
            Vector3 bulPos = transform.position;
            if (Mathf.Abs(camPos.x - bulPos.x) > 10)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();

            if (enemy.Invencible)
            {

            }
            else
            {
                enemy.Hurt(damage);
                AudioSource seHit = enemy.gameObject.AddComponent<AudioSource>();
                seHit.clip = SoundEffects[0].clip;
                seHit.Play();
            }

            Destroy(gameObject);
            /*
            EnemyMettoScript metto =
                other.gameObject.GetComponent<EnemyMettoScript>();
            metto.Hurt(damage);
            */
        }
    }
}