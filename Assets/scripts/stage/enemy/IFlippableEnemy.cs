using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 방향 전환이 가능한 적을 정의합니다.
/// </summary>
public interface IFlippableEnemy
{
    bool FacingRight { get; set; }
    void Flip();
}