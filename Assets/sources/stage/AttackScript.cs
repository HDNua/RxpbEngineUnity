using UnityEngine;
using System.Collections;



/// <summary>
/// 공격 스크립트입니다.
/// </summary>
public class AttackScript : MonoBehaviour
{
    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    // public PlayerController player;
    public GameObject[] effects;
    public int damage;
    public AudioClip[] audioClips;


    #endregion



    
    
    
    
    
    
    
    #region 필드를 정의합니다.
    AudioSource[] soundEffects;
    public AudioSource[] SoundEffects { get { return soundEffects; } }


    #endregion











    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected virtual void Awake()
    {
        // 효과음을 초기화 합니다.
        soundEffects = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            soundEffects[i] = gameObject.AddComponent<AudioSource>();
            soundEffects[i].clip = audioClips[i];
        }
    }
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    protected virtual void Start()
    {

    }
    /// <summary>
    /// 프레임이 갱신될 때 MonoBehaviour 개체 정보를 업데이트합니다.
    /// </summary>
    protected virtual void Update()
    {

    }
    /// <summary>
    /// FixedTimestep에 설정된 값에 따라 일정한 간격으로 업데이트합니다.
    /// 물리 효과가 적용된 오브젝트를 조정할 때 사용됩니다.
    /// (Update는 불규칙한 호출이기 때문에 물리엔진 충돌검사가 제대로 되지 않을 수 있습니다.)
    /// </summary>
    protected virtual void FixedUpdate()
    {

    }


    #endregion










    #region 메서드를 정의합니다.
    /// <summary>
    /// 적 캐릭터에게 대미지를 입힙니다.
    /// </summary>
    /// <param name="enemy">입힐 대미지입니다.</param>
    protected void Attack(EnemyScript enemy)
    {
        enemy.Hurt(damage);
    }


    #endregion
}
