using UnityEngine;
using System.Collections;

public class ZController : MonoBehaviour
{
    #region 공용 Unity 객체를 정의합니다.
    Rigidbody2D _rigidbody;
    Animator _animator;

    #endregion 공용 Unity 객체



    #region 필드를 정의합니다.
    AudioSource[] audioSources;


    #endregion



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    public AudioClip[] audioClips;

    #endregion



    #region MyRegion
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        audioSources = new AudioSource[audioClips.Length];
        for (int i = 0, len = audioClips.Length; i < len; ++i)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = audioClips[i];
        }
	}
	void Update()
    {
	    if (Input.GetKeyDown(KeyCode.X))
        {
            RequestShot();
        }
	}
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {

        }
    }

    #endregion



    #region 요청 메서드를 정의합니다.
    void RequestShot()
    {
        if (_animator.GetBool("ShotBlocked") == false)
        {
            _animator.SetBool("ShotRequested", true);
            _animator.SetBool("ShotBlocked", true);
        }
    }



    #endregion



    #region 프레임 이벤트 핸들러를 정의합니다.
    public void Attack_saber1()
    {
        _animator.SetBool("ShotRequested", false);
        audioSources[0].Play();
        audioSources[3].Play();
    }
    public void Attack_saber1_end()
    {
        _animator.SetBool("ShotBlocked", false);
    }
    public void Attack_saber2()
    {
        _animator.SetBool("ShotRequested", false);
        audioSources[1].Play();
        audioSources[3].Play();
    }
    public void Attack_saber2_end()
    {
        _animator.SetBool("ShotBlocked", false);
    }
    public void Attack_saber3()
    {
        _animator.SetBool("ShotRequested", false);
        audioSources[2].Play();
        audioSources[3].Play();
    }
    public void Attack_saber3_end()
    {

    }
    public void Attack_end()
    {
        _animator.SetBool("ShotBlocked", false);
    }

    #endregion 프레임 이벤트 핸들러
}
