using UnityEngine;
using System.Collections;
using System;



/// <summary>
/// 적 캐릭터의 바디 색상을 관리합니다.
/// </summary>
public static class EnemyColorPalette
{
    #region 공용 색상표를 정의합니다.
    /// <summary>
    /// 무적 상태 색상표입니다.
    /// </summary>
    static readonly int[] INVENCIBLE_COLOR_PALETTE =
    {
        0x7FA0EE, 0xAEC9E9, 0x7FA0EE, 0x7FA0EE,
        0x7FA0EE, 0x7FA0EE, 0xAEC9E9,
        0x7FA0EE, 0x7FA0EE, 0x6B90D3, 0xAEC9E9,
        0x7FA0EE, 0x7FA0EE, 0x7FA0EE, 0x7FA0EE,
        0x7FA0EE, 0xAEC9E9, 0x7FA0EE, 0x7FA0EE,
        0x7FA0EE, 0x7FA0EE, 0xAEC9E9,
        0x7FA0EE, 0x7FA0EE, 0x6B90D3, 0xAEC9E9,
        0x7FA0EE, 0x7FA0EE, 0x7FA0EE, 0x7FA0EE,
    };


    #endregion










    #region 색상표를 정의합니다.
    /// <summary>
    /// 워크캐논의 기본 색상표입니다.
    /// </summary>
    static readonly int[] WALKCANNON_COLOR_PALETTE =
    {
        0x585868, 0x9494A4, 0x204068, 0xC4C4CC, 0x4878D4, 0x3060AC, 0xD47850, 0xF4F4F4, 0x9CCCF4, 0x78B4F4, 0x785028, 0xF4CC60, 0xFFE4AC, 0x285084, 0xF44038,
        0x585868, 0x9494A4, 0x204068, 0xC4C4CC, 0x4878D4, 0x3060AC, 0xD47850, 0xF4F4F4, 0x9CCCF4, 0x78B4F4, 0x785028, 0xF4CC60, 0xFFE4AC, 0x285084, 0xF44038,
    };
    /// <summary>
    /// 기가 데스의 기본 색상표입니다.
    /// </summary>
    static readonly int[] GIGADEATH_COLOR_PALETTE =
    {
        0xF0F0F0, 0xC0C8D8, 0x808898, 0x505860, 0xD0E0B0, 0x98A878, 0x687848, 0x485828, 0xF0C040, 0xE08830, 0xA86020, 0x683018, 0x483828, 0x8090E0, 0x5868B0,
        0xF0F0F0, 0xC0C8D8, 0x808898, 0x505860, 0xD0E0B0, 0x98A878, 0x687848, 0x485828, 0xF0C040, 0xE08830, 0xA86020, 0x683018, 0x483828, 0x8090E0, 0x5868B0,
    };
    /// <summary>
    /// 트랩 블래스트의 기본 색상표입니다.
    /// </summary>
    static readonly int[] TRAP_BLAST_COLOR_PALETTE =
    {
        0x683010, 0x383840, 0x2848AC, 0x58CCFF, 0xFFFFFF, 0x304830, 0xFF8C28, 0xB45020, 0x287848, 0x686868, 0x9C9C9C, 0xCCCCCC, 0xFFFF48, 0x8CF448, 0xC42020,
        0x683010, 0x383840, 0x2848AC, 0x58CCFF, 0xFFFFFF, 0x304830, 0xFF8C28, 0xB45020, 0x287848, 0x686868, 0x9C9C9C, 0xCCCCCC, 0xFFFF48, 0x8CF448, 0xC42020,
        // 0xF8F8F8, 0xC8C8C8, 0x989898, 0x686868, 0x383840, 0x88F048, 0x287848, 0x304830, 0xF8F848, 0xF88828, 0xB05020, 0x683010, 0x58C8F8, 0x2848A8, 0xC02020,
        // 0xF8F8F8, 0xC8C8C8, 0x989898, 0x686868, 0x383840, 0x88F048, 0x287848, 0x304830, 0xF8F848, 0xF88828, 0xB05020, 0x683010, 0x58C8F8, 0x2848A8, 0xC02020,
    };
    /// <summary>
    /// 스파이키 MK2의 기본 색상표입니다.
    /// </summary>
    static readonly int[] SPIKY_MK2_COLOR_PALETTE =
    {
        0xF0F0F0, 0xC0C0C8, 0x9898A0, 0x707078, 0x484850, 0x282830, 0xF0B020, 0xB08020, 0x605020, 0x688848, 0x406028, 0x304020, 0xB04020, 0x882818, 0x402018,
        0xF0F0F0, 0xC0C0C8, 0x9898A0, 0x707078, 0x484850, 0x282830, 0xF0B020, 0xB08020, 0x605020, 0x688848, 0x406028, 0x304020, 0xB04020, 0x882818, 0x402018,
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
    public static Color[] GigadeathPalette { get; private set; }
    public static Color[] WalkCannonPalette { get; private set; }
    public static Color[] TrapBlastPalette { get; private set; }
    public static Color[] SpikyMK2Plaette { get; private set; }

    #endregion










    #region 생성자를 정의합니다.
    /// <summary>
    /// 캐릭터의 바디 색상표를 초기화합니다.
    /// </summary>
    static EnemyColorPalette()
    {
        // 사용할 변수를 선언합니다.
        int PALETTE_COUNT = GIGADEATH_COLOR_PALETTE.Length;
        Color[] invenciblePalette;
        Color[] gigadeathPalette;
        Color[] walkcannonPalette;
        Color[] trapBlastPalette;
        Color[] spikyMk2Palette;

        // 팔레트를 초기화합니다.
        invenciblePalette = new Color[PALETTE_COUNT];
        gigadeathPalette = new Color[PALETTE_COUNT];
        walkcannonPalette = new Color[PALETTE_COUNT];
        trapBlastPalette = new Color[PALETTE_COUNT];
        spikyMk2Palette = new Color[PALETTE_COUNT];
        for (int i = 0; i < PALETTE_COUNT; ++i)
        {
            // 기본 색상표를 초기화합니다.
            invenciblePalette[i] = ColorFromInt(INVENCIBLE_COLOR_PALETTE[i]);
            gigadeathPalette[i] = ColorFromInt(GIGADEATH_COLOR_PALETTE[i]);
            walkcannonPalette[i] = ColorFromInt(WALKCANNON_COLOR_PALETTE[i]);
            trapBlastPalette[i] = ColorFromInt(TRAP_BLAST_COLOR_PALETTE[i]);
            spikyMk2Palette[i] = ColorFromInt(SPIKY_MK2_COLOR_PALETTE[i]);
        }
        InvenciblePalette = invenciblePalette;
        GigadeathPalette = gigadeathPalette;
        WalkCannonPalette = walkcannonPalette;
        TrapBlastPalette = trapBlastPalette;
        SpikyMK2Plaette = spikyMk2Palette;
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