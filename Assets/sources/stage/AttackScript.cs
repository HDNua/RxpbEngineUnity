using UnityEngine;
using System.Collections;

/// <summary>
/// 공격 스크립트입니다.
/// </summary>
public class AttackScript : MonoBehaviour
{
    public PlayerController player;
    public GameObject[] effects;
    public int damage;

    void Start()
    {

    }
    void Update()
    {

    }

    protected void Attack(EnemyScript enemy)
    {
        enemy.Hurt(damage);
    }
}
