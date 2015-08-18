using UnityEngine;
using System.Collections;

public class UIAnimator : MonoBehaviour
{
    #region 컨트롤러가 사용할 Unity 객체를 정의합니다.
    Animator _animator;

    #endregion 컨트롤러용 Unity 객체



    #region Unity에서 접근 가능한 공용 필드를 정의합니다.
    // public UIController _UIController;

    public StageSceneManager stageManager;
    public PlayerController player;

    #endregion Unity 공용 필드



    #region 필드를 정의합니다.


    #endregion 필드



    #region MonoBehaviour가 정의하는 기본 메서드를 재정의합니다.
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {

    }

    #endregion MonoBehaviour 기본 메서드 재정의



    #region 행동 메서드를 정의합니다.


    #endregion 행동 메서드



    #region 요청 메서드를 정의합니다.


    #endregion 요청 메서드



    #region 프레임 이벤트 핸들러를 정의합니다.
    public void PlayReadyVoice()
    {
        // _UIController.se[0].Play();
        stageManager.SoundEffects[0].Play();
    }
    public void RequestSpawn()
    {
        player.RequestSpawn();
        //        _ZController.RequestSpawn();
    }
    public void StopReadyAnimation()
    {
        //_animator.ac
    }

    #endregion 프레임 이벤트 핸들러



    #region 요청을 수행하기 위한 보조 메서드를 정의합니다.


    #endregion 보조 메서드 정의
}
