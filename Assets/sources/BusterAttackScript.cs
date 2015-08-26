using UnityEngine;
using System.Collections;

/// <summary>
/// 버스터 공격 스크립트입니다.
/// </summary>
public class BusterAttackScript : AttackScript
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
            EnemyMettoScript metto =
                other.gameObject.GetComponent<EnemyMettoScript>();
            metto.Hurt(damage);
        }
    }
}