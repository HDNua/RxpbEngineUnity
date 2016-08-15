using UnityEngine;
using System.Collections;



/// <summary>
/// 엑스 캐릭터의 바디 색상을 관리합니다.
/// </summary>
public static class XBodyColors
{
    #region 상수를 정의합니다.
    /// <summary>
    /// 기본 색상 값 배열입니다.
    /// </summary>
    static readonly int[] DEFAULT_COLOR_PALETTE =
        {
            0xE02820, 0x602818, 0xD07858, 0xD8A888,
            0x30C0A0, 0x188868, 0x204860,
            0xB8C0D0, 0x9098A8, 0x506078, 0x283038,
            0x3880D8, 0x3068C8, 0x2040A8, 0x203078,
            0xE86828, 0x803820, 0xD88060, 0xF8E0C8,
            0xA0F8F8, 0x40C8A8, 0x184058,
            0xF0F8F8, 0xB0B8C8, 0x485870, 0x182028,
            0x88D8F8, 0x68A8F8, 0x2048B0, 0x182888,
        };
    /// <summary>
    /// 무기 1 색상 값 배열입니다.
    /// </summary>
    static readonly int[] WEAPON1_COLOR_PALETTE =
        {
            0xE02820, 0x602818, 0xD07858, 0xD8A888,
            0xF0D898, 0xC8A040, 0x804040,
            0xD0C0B8, 0xA89890, 0x685040, 0x283038,
            0xD88830, 0xC86818, 0x984810, 0x703010,
            0xE02820, 0x602818, 0xD07858, 0xD8A888,
            0xF0D898, 0xC8A040, 0x804040,
            0xD0C0B8, 0xA89890, 0x685040, 0x283038,
            0xD88830, 0xC86818, 0x984810, 0x703010,
        };


    #endregion










    #region 필드를 정의합니다.
    /// <summary>
    /// 기본 색상 팔레트입니다.
    /// </summary>
    static readonly Color[] _defaultPalette;
    /// <summary>
    /// 기본 색상 팔레트입니다.
    /// </summary>
    public static Color[] DefaultPalette
    {
        get { return _defaultPalette; }
    }
    /// <summary>
    /// 무기1 색상 팔레트입니다.
    /// </summary>
    static readonly Color[] _weapon1Palette;
    /// <summary>
    /// 무기1 색상 팔레트입니다.
    /// </summary>
    public static Color[] Weapon1Palette
    {
        get { return _weapon1Palette; }
    }


    #endregion










    #region 생성자를 정의합니다.
    /// <summary>
    /// 엑스 캐릭터의 바디 색상을 관리합니다.
    /// </summary>
    static XBodyColors()
    {
        int PALETTE_COUNT = DEFAULT_COLOR_PALETTE.Length;

        Color[] defaultPalette = new Color[PALETTE_COUNT];
        Color[] weapon1Palette = new Color[PALETTE_COUNT];

        for (int i = 0; i < PALETTE_COUNT; ++i)
        {
            defaultPalette[i] = ColorFromInt(DEFAULT_COLOR_PALETTE[i]);
            weapon1Palette[i] = ColorFromInt(WEAPON1_COLOR_PALETTE[i]);
        }

        _defaultPalette = defaultPalette;
        _weapon1Palette = weapon1Palette;
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
}