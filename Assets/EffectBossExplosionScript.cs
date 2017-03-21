using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 보스 연속 폭발 효과입니다.
/// </summary>
public class EffectBossExplosionScript : EffectScript
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    /// <summary>
    /// 폭발 회수입니다.
    /// </summary>
    public int _explosionCount = 100;

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
    /// <summary>
    /// 폭발 간격 왜곡의 최솟값입니다.
    /// </summary>
    public float _minIntervalDistortion = 0.2f;
    /// <summary>
    /// 폭발 간격 왜곡의 최댓값입니다.
    /// </summary>
    public float _maxIntervalDistortion = 0.2f;

    /// <summary>
    /// 
    /// </summary>
    public SpriteRenderer _yellowRay;
    /// <summary>
    /// 
    /// </summary>
    public SpriteRenderer _blueRay;

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



    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 사망 코루틴입니다.
    /// </summary>
    private IEnumerator CoroutineExplosion()
    {
        // 
        for (int i = 0; i < _explosionCount; ++i)
        {
            float distortionX = UnityEngine.Random.Range(_minDistortionX, _maxDistortionX);
            float distortionY = UnityEngine.Random.Range(_minDistortionY, _maxDistortionY);

            // 
            Vector3 position = transform.position + new Vector3(distortionX, distortionY);
            CreateExplosion(position);

            CreateYellowRay();
            CreateYellowRay();
            CreateYellowRay();

            // 
            float distortionTime = UnityEngine.Random.Range
                (_minIntervalDistortion, _maxIntervalDistortion);
            yield return new WaitForSeconds(_deadEffectInterval + distortionTime);
        }

        // 개체를 제거합니다.
        RequestDestroy();
        yield break;
    }

    /// <summary>
    /// 폭발 효과 개체를 생성합니다.
    /// </summary>
    /// <param name="position">폭발 효과 개체를 생성할 위치입니다.</param>
    void CreateExplosion(Vector3 position)
    {
        EffectScript effect = DataBase.Instance.Explosion1Effect;
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                effect = DataBase.Instance.Explosion1Effect;
                break;
            case 1:
                effect = DataBase.Instance.Explosion2Effect;
                break;
            case 2:
                effect = DataBase.Instance.Explosion3Effect;
                break;

            default:
                effect = DataBase.Instance.Explosion4Effect;
                break;
        }
        Instantiate(effect, position, transform.rotation);
    }

    /// <summary>
    /// 
    /// </summary>
    void CreateYellowRay()
    {
        SpriteRenderer sr = Instantiate(_yellowRay, transform.position, transform.rotation);
        sr.transform.RotateAround
            (transform.position, Vector3.right, UnityEngine.Random.Range(0, 360f));

        StartCoroutine(CoroutineYellowRay(sr));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sr"></param>
    /// <returns></returns>
    private IEnumerator CoroutineYellowRay(SpriteRenderer sr)
    {
        yield return new WaitForSeconds(0.3f);

        sr.gameObject.SetActive(false);
        yield break;
    }

    /// <summary>
    /// 
    /// </summary>
    void CreateBlueRay()
    {

    }

    #endregion
}