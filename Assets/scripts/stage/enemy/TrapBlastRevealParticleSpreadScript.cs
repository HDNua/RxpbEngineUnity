using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



/// <summary>
/// 트랩 블래스트 등장 파편 효과를 정의합니다.
/// </summary>
public class TrapBlastRevealParticleSpreadScript: ParticleSpreadScript
{
    /// <summary>
    /// 파편이 튈 방향을 설정합니다.
    /// </summary>
    /// <returns>파편이 튈 방향을 반환합니다.</returns>
    protected override Vector2 GetParticleSpreadDirection()
    {
        EnemyTrapBlastScript trapBlast = GetComponentInParent<EnemyTrapBlastScript>();

        Vector2 dir = UnityEngine.Random.insideUnitCircle;
        dir.x = (trapBlast.FacingRight ? 1 : -1) * Mathf.Abs(dir.x);
        return dir;
    }
}
