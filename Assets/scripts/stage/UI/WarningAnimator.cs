using System;
using UnityEngine;
using System.Collections.Generic;



/// <summary>
/// 경고 애니메이션을 관리합니다.
/// </summary>
public class WarningAnimator : MonoBehaviour
{
    #region 필드를 정의합니다.
    /// <summary>
    /// 스테이지 관리자입니다.
    /// </summary>
    public StageManager _stageManager;
    
    #endregion


    


    #region MonoBehaviour 기본 메서드를 재정의합니다.
    /// <summary>
    /// MonoBehaviour 개체를 초기화합니다.
    /// </summary>
    void Start()
    {
        /*
         * [v6.2.1] 다음 커밋에서 삭제할 예정입니다.
         * 
        // 예외 메시지 리스트를 생성합니다.
        List<string> exceptionList = new List<string>();

        // 빈 필드가 존재하는 경우 예외 메시지를 추가합니다.
        if (_stageManager == null)
            exceptionList.Add("ReadyAnimator.StageManager == null");

        // 예외 메시지가 하나 이상 존재하는 경우 예외를 발생하고 중지합니다.
        if (exceptionList.Count > 0)
        {
            foreach (string msg in exceptionList)
            {
                Handy.Log("ReadyAnimator Error: {0}", msg);
            }
            throw new Exception("데이터베이스 필드 정의 부족");
        }
        */

        _stageManager = StageManager.Instance;
    }

    #endregion
}
