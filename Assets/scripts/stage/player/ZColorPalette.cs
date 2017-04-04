using UnityEngine;
using System.Collections;
using System;



/// <summary>
/// 제로 캐릭터의 바디 색상을 관리합니다.
/// </summary>
public static class ZColorPalette
{
    #region 공용 색상표를 정의합니다.
    /// <summary>
    /// 무적 상태 색상표입니다.
    /// </summary>
    static readonly int[] INVENCIBLE_COLOR_PALETTE =
    {
        0xE4BE9F, 0xE9B888, 0xE5AF7F, 0xE5A773,
        0xEFEDE0, 0xF3E9DF, 0xEDDFD1,
        0xF8F6F6, 0xF0D8C4, 0xE8BFA1, 0xF1C7B1,
        0xE8BF9F, 0xE99558, 0xE79752, 0xEBA06C,
        0xE4BE9F, 0xE9B888, 0xE5AF7F, 0xE5A773,
        0xEFEDE0, 0xF3E9DF, 0xEDDFD1,
        0xF8F6F6, 0xF0D8C4, 0xE8BFA1, 0xF1C7B1,
        0xE8BF9F, 0xE99558, 0xE79752, 0xEBA06C,
    };
    
    #endregion





    #region 제로 색상표를 정의합니다.
    /// <summary>
    /// 제로의 기본 색상표입니다.
    /// </summary>
    static readonly int[] Z_DEFAULT_COLOR_PALETTE =
    {
        0x900000, 0x600000, 0xF01000, 0xE86868,
        0xF0F0F0, 0xA8B0C8, 0x0080F8,
        0x40C070, 0x985840, 0x606880, 0x202848,
        0xE8C838, 0xF0C090, 0xC88060, 0x187828,
        0x900000, 0x600000, 0xF01000, 0xE86868,
        0xF0F0F0, 0xA8B0C8, 0x0080F8,
        0x40C070, 0x985840, 0x606880, 0x202848,
        0xE8C838, 0xF0C090, 0xC88060, 0x187828,
    };

    /// <summary>
    /// 엑스가 대쉬할 때 잔상 효과의 색상표입니다.
    /// </summary>
    static readonly int[] Z_DASHAFTERIMAGE_COLOR_PALETTE =
    {
        0x900800, 0x900800, 0x900800, 0x900800,
        0x900800, 0x900800, 0x900800,
        0x900800, 0x900800, 0x900800, 0x900800,
        0x900800, 0x900800, 0x900800, 0x900800,
        0x900800, 0x900800, 0x900800, 0x900800,
        0x900800, 0x900800, 0x900800,
        0x900800, 0x900800, 0x900800, 0x900800,
        0x900800, 0x900800, 0x900800, 0x900800,
    };

    #endregion





    #region 필드 및 프로퍼티를 정의합니다.
    /// <summary>
    /// 무적 색상 팔레트입니다.
    /// </summary>
    public static Color[] InvenciblePalette { get; private set; }
    
    /// <summary>
    /// 기본 색상 팔레트입니다.
    /// </summary>
    public static Color[] DefaultPalette { get; private set; }

    /// <summary>
    /// 제로 대쉬 효과 색상표입니다.
    /// </summary>
    public static Color[] DashEffectColorPalette { get; private set; }


    #endregion





    #region 생성자를 정의합니다.
    /// <summary>
    /// 제로 캐릭터의 바디 색상표를 초기화합니다.
    /// </summary>
    static ZColorPalette()
    {
        // 사용할 변수를 선언합니다.
        int PALETTE_COUNT = Z_DEFAULT_COLOR_PALETTE.Length;
        Color[] invenciblePalette;
        Color[] defaultPalette;
        Color[] dashEffectColorPalette;
        
        // 팔레트를 초기화합니다.
        invenciblePalette = new Color[PALETTE_COUNT];
        defaultPalette = new Color[PALETTE_COUNT];
        dashEffectColorPalette = new Color[PALETTE_COUNT];
        for (int i = 0; i < PALETTE_COUNT; ++i)
        {
            // 기본 색상표를 초기화합니다.
            invenciblePalette[i] = ColorFromInt(INVENCIBLE_COLOR_PALETTE[i]);
            defaultPalette[i] = ColorFromInt(Z_DEFAULT_COLOR_PALETTE[i]);
            dashEffectColorPalette[i] = ColorFromInt(Z_DASHAFTERIMAGE_COLOR_PALETTE[i], 0.6f);
        }
        InvenciblePalette = invenciblePalette;
        DefaultPalette = defaultPalette;
        DashEffectColorPalette = dashEffectColorPalette;
    }

    #endregion





    #region 메서드를 정의합니다.
    /// <summary>
    /// 색상 값을 받아서 해당하는 색상을 반환합니다.
    /// </summary>
    /// <param name="c">색상 값입니다.</param>
    /// <param name="alpha">알파 값입니다.</param>
    /// <returns>색상 값을 받아서 해당하는 색상을 반환합니다.</returns>
    public static Color ColorFromInt(int c, float alpha = 1.0f)
    {
        int r = (c >> 16) & 0x000000FF;
        int g = (c >> 8) & 0x000000FF;
        int b = c & 0x000000FF;

        Color ret = ColorFromIntRGB(r, g, b);
        ret.a = alpha;

        return ret;
    }
    /// <summary>
    /// 정수 RGB 값을 받아서 해당하는 색상을 반환합니다.
    /// </summary>
    /// <param name="r">[0, 255] 사이의 R 값입니다.</param>
    /// <param name="g">[0, 255] 사이의 G 값입니다.</param>
    /// <param name="b">[0, 255] 사이의 B 값입니다.</param>
    /// <returns>정수 RGB 값을 받아서 해당하는 색상을 반환합니다.</returns>
    public static Color ColorFromIntRGB(int r, int g, int b)
    {
        return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f, 1.0f);
    }


    #endregion





    #region 구형 정의를 보관합니다.


    #endregion
}