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
    };
    /// <summary>
    /// 스파이키 MK2의 기본 색상표입니다.
    /// </summary>
    static readonly int[] SPIKY_MK2_COLOR_PALETTE =
    {
        0xF0F0F0, 0xC0C0C8, 0x9898A0, 0x707078, 0x484850, 0x282830, 0xF0B020, 0xB08020, 0x605020, 0x688848, 0x406028, 0x304020, 0xB04020, 0x882818, 0x402018,
        0xF0F0F0, 0xC0C0C8, 0x9898A0, 0x707078, 0x484850, 0x282830, 0xF0B020, 0xB08020, 0x605020, 0x688848, 0x406028, 0x304020, 0xB04020, 0x882818, 0x402018,
    };
    /// <summary>
    /// 
    /// </summary>
    static readonly int[] INTRO_BOSS_HEAD_COLOR_PALETTE =
    {
//        0x508D43, 0x29312F, 0x454647, 0x5A5638, 0x191818, 0x275637, 0xECEDE1, 0x65756A, 0x56241F, 0xB4AD31, 0x99A7A7, 0xB3462D, 0x546559, 0xB7BFBD, 0x788787,
//        0x508D43, 0x29312F, 0x454647, 0x5A5638, 0x191818, 0x275637, 0xECEDE1, 0x65756A, 0x56241F, 0xB4AD31, 0x99A7A7, 0xB3462D, 0x546559, 0xB7BFBD, 0x788787,
//        0xFFFFFF, 0xE5EEE6, 0xBECBC8, 0x687A6D, 0x3D713C, 0x569143, 0x355C3D, 0xA2B2AB, 0x222B26, 0x402A1D, 0x515A5C, 0x303A34, 0x2F4C33, 0x1B171B, 0x1D1F21, // 0x5C6A60,
//        0xACD39F, 0x120C11, 0x321B15, 0x788B80, 0x7CBD4E, 0x53371A, 0x8A9896, 0x704C1D, 0x292C34, 0x927D55, 0xC3533A, 0x24212C, 0x3D4045, 0x474D4D, 0xFD9789, // 0x843932,
//        0xE5EEE6, 0xBECBC8, 0x687A6D, 0x3D713C, 0x569143, 0x355C3D, 0xA2B2AB, 0x222B26, 0x402A1D, 0x515A5C, 0x303A34, 0x2F4C33, 0x1B171B, 0x1D1F21, 0x5C6A60,
//        0xACD39F, 0x120C11, 0x321B15, 0x788B80, 0x7CBD4E, 0x53371A, 0x8A9896, 0x704C1D, 0x292C34, 0x927D55, 0xC3533A, 0x24212C, 0x3D4045, 0x474D4D, 0xFD9789, // 0x843932,
//        0xC4CAC1, 0x558E44, 0x3B6B3D, 0x7F8D83, 0x23222A, 0x292F32, 0x59635F, 0x39443F, 0x51361B, 0x1E181B, 0x120D12, 0x7CBC4F, 0x3C2419, 0x734920, 0xC75840,
//        0xC4CAC1, 0x558E44, 0x3B6B3D, 0x7F8D83, 0x23222A, 0x292F32, 0x59635F, 0x39443F, 0x51361B, 0x1E181B, 0x120D12, 0x7CBC4F, 0x3C2419, 0x734920, 0xC75840,
        0xE7F1E3, 0x5E9848, 0x386040, 0x293B24, 0x1B171C, 0x2D4E32, 0x586B5A, 0xC0B9AD, 0x3F773B, 0x7EBF4F, 0x282B30, 0x1D2321, 0x563819, 0x3E3420, 0x130D12,
        0x744B1F, 0x281916, 0x26242F, 0x8D9A96, 0x7B867B, 0x2A3138, 0x422926, 0x6C756C, 0x383F41, 0x525759, 0x44484C, 0x35353A, 0x3A1F12, 0x221E29, 0xC54D40,
    };
    /// <summary>
    /// 
    /// </summary>
    static readonly int[] INTRO_BOSS_BODY_COLOR_PALETTE =
    {
        0xD2D4D4, 0x140F14, 0x22212B, 0x282C34, 0xA6A7B0, 0x2D3C38, 0x5A5B5B, 0x4B403E, 0x3C2928, 0x7E6F67, 0x1B1B1E, 0x19221C, 0x366542, 0x8F8F87, 0x284E39,
        0x341914, 0xEDEBEA, 0x529247, 0x1D2B23, 0xFDFCFC, 0x87BB4D, 0xACE560, 0xBCBDC7, 0x745C2D, 0x662A23, 0xCC8144, 0xB93B29, 0xAFB2BE, 0xF3BF50, 0xE2D2CD,
    };
    /// <summary>
    /// 
    /// </summary>
    static readonly int[] INTRO_BOSS_ARM_COLOR_PALETTE =
    {
        0xBDE2C7, 0x82908E, 0x306B3A, 0x434F4F, 0x303943, 0xBDC5C1, 0x4D7243, 0x364449, 0x29342E, 0x292E38, 0xF2F6F5, 0x63736F, 0x202B2A, 0x1B2422, 0xA6B4B0,
        0x625746, 0x4B5B59, 0x736F41, 0x252332, 0x6CA04B, 0x523224, 0x96A39F, 0x707F7E, 0x191B20, 0xBA992B, 0x0F1416, 0x586663, 0xA0701E, 0x785026, 0xC0807B,
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

    public static Color[] IntroBossHeadPalette { get; private set; }
    public static Color[] IntroBossBodyPalette { get; private set; }
    public static Color[] IntroBossArmPalette { get; private set; }

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
        Color[] introBossHeadPalette;
        Color[] introBossBodyPalette;
        Color[] introBossArmPalette;

        // 팔레트를 초기화합니다.
        invenciblePalette = new Color[PALETTE_COUNT];
        gigadeathPalette = new Color[PALETTE_COUNT];
        walkcannonPalette = new Color[PALETTE_COUNT];
        trapBlastPalette = new Color[PALETTE_COUNT];
        spikyMk2Palette = new Color[PALETTE_COUNT];
        introBossHeadPalette = new Color[PALETTE_COUNT];
        introBossBodyPalette = new Color[PALETTE_COUNT];
        introBossArmPalette = new Color[PALETTE_COUNT];

        for (int i = 0; i < PALETTE_COUNT; ++i)
        {
            // 기본 색상표를 초기화합니다.
            invenciblePalette[i] = ColorFromInt(INVENCIBLE_COLOR_PALETTE[i]);
            gigadeathPalette[i] = ColorFromInt(GIGADEATH_COLOR_PALETTE[i]);
            walkcannonPalette[i] = ColorFromInt(WALKCANNON_COLOR_PALETTE[i]);
            trapBlastPalette[i] = ColorFromInt(TRAP_BLAST_COLOR_PALETTE[i]);
            spikyMk2Palette[i] = ColorFromInt(SPIKY_MK2_COLOR_PALETTE[i]);

            introBossHeadPalette[i] = ColorFromInt(INTRO_BOSS_HEAD_COLOR_PALETTE[i]);
            introBossBodyPalette[i] = ColorFromInt(INTRO_BOSS_BODY_COLOR_PALETTE[i]);
            introBossArmPalette[i] = ColorFromInt(INTRO_BOSS_ARM_COLOR_PALETTE[i]);
        }
        InvenciblePalette = invenciblePalette;
        GigadeathPalette = gigadeathPalette;
        WalkCannonPalette = walkcannonPalette;
        TrapBlastPalette = trapBlastPalette;
        SpikyMK2Plaette = spikyMk2Palette;
        IntroBossHeadPalette = introBossHeadPalette;
        IntroBossBodyPalette = introBossBodyPalette;
        IntroBossArmPalette = introBossArmPalette;
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