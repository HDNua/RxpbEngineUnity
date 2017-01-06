using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 탄환을 발사할 수 있는 적을 정의합니다.
/// </summary>
public interface IShootableEnemy
{
    /// <summary>
    /// 탄환 발사를 요청합니다.
    /// </summary>
    void RequestShot();

    /// <summary>
    /// 탄환을 발사합니다.
    /// </summary>
    /// <param name="shotPosition">탄환을 발사할 위치입니다.</param>
    void Shot(Transform shotPosition);
}