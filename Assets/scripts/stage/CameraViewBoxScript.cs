using UnityEngine;
using System.Collections;



/// <summary>
/// 카메라 뷰 박스입니다.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class CameraViewBoxScript : MonoBehaviour
{
    /// <summary>
    /// 충돌체가 트리거 내부로 진입했습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemySpawnZone"))
        {
            EnemySpawnZone zone = other.gameObject.GetComponent<EnemySpawnZone>();
            zone.RequestSpawnEnemy();
        }
    }
    /// <summary>
    /// 충돌체가 여전히 트리거 내부에 있습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerStay2D(Collider2D other)
    {

    }
    /// <summary>
    /// 충돌체가 트리거 내부로 진입했습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("EnemySpawnZone"))
        {
            EnemySpawnZone zone = other.gameObject.GetComponent<EnemySpawnZone>();
            zone.RequestDestroyEnemy();
        }
    }
}
