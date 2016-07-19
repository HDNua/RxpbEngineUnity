using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



[Serializable]
/// <summary>
/// 맵 상태입니다.
/// </summary>
public struct GameMapStatus
{
    /// <summary>
    /// 맵을 클리어했다면 참입니다.
    /// </summary>
    public bool cleared;


    /// <summary>
    /// 
    /// </summary>
    public bool[] itemFound;


    /// <summary>
    /// '라이프 업' 아이템을 획득했습니다.
    /// </summary>
    public bool itemLifeUpFound;
    /// <summary>
    /// '라이프 서브탱크' 아이템을 획득했습니다.
    /// </summary>
    public bool itemECanFound;
    /// <summary>
    /// '웨폰 서브탱크' 아이템을 획득했습니다.
    /// </summary>
    public bool itemWCanFound;
    /// <summary>
    /// '웨폰 서브탱크' 아이템을 획득했습니다.
    /// </summary>
    public bool itemXCanFound;
}
