using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 연속 폭발 효과입니다.
/// </summary>
public class EffectMultipleExplosionScript : EffectScript
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 폭발 회수입니다.
    /// </summary>
    public int _explosionCount = 3;

    /// <summary>
    /// X축 왜곡의 최솟값입니다.
    /// </summary>
    public float _minDistortionX = -1;
    /// <summary>
    /// X축 왜곡의 최댓값입니다.
    /// </summary>
    public float _maxDistortionX = 1;
    /// <summary>
    /// Y축 왜곡의 최솟값입니다.
    /// </summary>
    public float _minDistortionY = -1;
    /// <summary>
    /// Y축 왜곡의 최댓값입니다.
    /// </summary>
    public float _maxDistortionY = 1;

    /// <summary>
    /// 폭발 간격입니다.
    /// </summary>
    public float _deadEffectInterval = 0.2f;

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 객체를 초기화합니다.
    /// </summary>
    void Start()
    {
        StartCoroutine(CoroutineExplosion());
    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    void Update()
    {

    }

    #endregion

    /// <summary>
    /// 사망 코루틴입니다.
    /// </summary>
    private IEnumerator CoroutineExplosion()
    {
        // 
        for (int i = 0; i < _explosionCount; ++i)
        {
            float distortionX = Random.Range(_minDistortionX, _maxDistortionX);
            float distortionY = Random.Range(_minDistortionY, _maxDistortionY);

            // 
            Vector3 position = transform.position + new Vector3(distortionX, distortionY);
            Instantiate(DataBase.Instance.ExplosionEffect, position, transform.rotation);

            // 
            yield return new WaitForSeconds(_deadEffectInterval);
        }

        // 개체를 제거합니다.
        RequestDestroy();
        yield break;
    }
}
