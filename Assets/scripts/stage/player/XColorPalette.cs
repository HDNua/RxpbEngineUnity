using UnityEngine;
using System.Collections;
using System;



/// <summary>
/// 엑스 캐릭터의 바디 색상을 관리합니다.
/// </summary>
public static class XColorPalette
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



    

    #region 엑스 색상표를 정의합니다.
    /// <summary>
    /// 엑스의 기본 색상표입니다.
    /// </summary>
    static readonly int[] X_DEFAULT_COLOR_PALETTE =
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
    /// 엑스가 1단계 차지할 때의 색상표입니다.
    /// </summary>
    static readonly int[] X_CHARGE1_COLOR_PALETTE =
    {
        0x2D42EA, 0x2A4C75, 0x6185D4, 0x9EBFF1,
        0x4ADAF4, 0x1A81AC, 0x2E5C91,
        0xBBF2FD, 0x81ADBB, 0x40738A, 0x314D9C,
        0x5CBAFF, 0x4D94F5, 0x356DD9, 0x2B51AB,
        0x2D42EA, 0x2A4C75, 0x6185D4, 0x9EBFF1,
        0x4ADAF4, 0x1A81AC, 0x2E5C91,
        0xBBF2FD, 0x81ADBB, 0x40738A, 0x314D9C,
        0x5CBAFF, 0x4D94F5, 0x356DD9, 0x2B51AB,
    };
    /// <summary>
    /// 엑스가 2단계 차지할 때의 색상표입니다.
    /// </summary>
    static readonly int[] X_CHARGE2_COLOR_PALETTE =
    {
        0x28E830, 0x187040, 0x58D078, 0x98E8B8,
        0x40F0D0, 0x109878, 0x188050,
        0xB8F8E8, 0x80B8A8, 0x287058, 0x209040,
        0x48F8A8, 0x38E888, 0x20C860, 0x18A040,
        0x28E830, 0x187040, 0x58D078, 0x98E8B8,
        0x40F0D0, 0x109878, 0x188050,
        0xB8F8E8, 0x80B8A8, 0x287058, 0x209040,
        0x48F8A8, 0x38E888, 0x20C860, 0x18A040,
    };
    
    /// <summary>
    /// 엑스가 대쉬할 때 잔상 효과의 색상표입니다.
    /// </summary>
    static readonly int[] X_DASHAFTERIMAGE_COLOR_PALETTE =
    {
        0x1840A0, 0x1840A0, 0x1840A0, 0x1840A0,
        0x1840A0, 0x1840A0, 0x1840A0, 
        0x1840A0, 0x1840A0, 0x1840A0, 0x1840A0,
        0x1840A0, 0x1840A0, 0x1840A0, 0x1840A0,
        0x1840A0, 0x1840A0, 0x1840A0, 0x1840A0,
        0x1840A0, 0x1840A0, 0x1840A0, 
        0x1840A0, 0x1840A0, 0x1840A0, 0x1840A0,
        0x1840A0, 0x1840A0, 0x1840A0, 0x1840A0,
    };

    /// <summary>
    /// 무기 1 색상 값 배열입니다.
    /// </summary>
    static readonly int[] X_WEAPON1_COLOR_PALETTE =
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
    /// <summary>
    /// 무기 2 색상 값 배열입니다.
    /// </summary>
    static readonly int[] X_WEAPON2_COLOR_PALETTE =
    {
        0xE02121, 0x602111, 0xD07051, 0xD0A081,
        0xE0D180, 0xE08140, 0x703121,
        0xB0C1D0, 0x9090A0, 0x506071, 0x203131,
        0x60B151, 0x208140, 0x107031, 0x005020,
        0xE02121, 0x602111, 0xD07051, 0xD0A081,
        0xE0D180, 0xE08140, 0x703121,
        0xB0C1D0, 0x9090A0, 0x506071, 0x203131,
        0x60B151, 0x208140, 0x107031, 0x005020,
    };
    /// <summary>
    /// 무기 3 색상 값 배열입니다.
    /// </summary>
    static readonly int[] X_WEAPON3_COLOR_PALETTE =
    {
        0xE02121, 0x602111, 0xD07051, 0xD0A081,
        0x20D1F0, 0x2081B0, 0x205160,
        0xC0C0D0, 0x9090A0, 0x506071, 0x203131,
        0xE061B1, 0xD04070, 0xA03151, 0x601120,
        0xE02121, 0x602111, 0xD07051, 0xD0A081,
        0x20D1F0, 0x2081B0, 0x205160,
        0xC0C0D0, 0x9090A0, 0x506071, 0x203131,
        0xE061B1, 0xD04070, 0xA03151, 0x601120,
    };
    /// <summary>
    /// 무기 4 색상 값 배열입니다.
    /// </summary>
    static readonly int[] X_WEAPON4_COLOR_PALETTE =
    {
        0xE02820, 0x602818, 0xD07858, 0xD8A888,
        0xF0F0F0, 0xA0A0A0, 0x505058,
        0xD0C0B8, 0xA89890, 0x685040, 0x283038,
        0xE05858, 0xE03030, 0xA00008, 0x580008,
        0xE02820, 0x602818, 0xD07858, 0xD8A888,
        0xF0F0F0, 0xA0A0A0, 0x505058,
        0xD0C0B8, 0xA89890, 0x685040, 0x283038,
        0xE05858, 0xE03030, 0xA00008, 0x580008,
    };
    
    /// <summary>
    /// 엑스의 기본 차지 색상 팔레트입니다.
    /// </summary>
    static readonly int[] X_DEFAULT_CHARGE_COLOR_PALETTE =
    {
        0x3068C8, 0x3880D8, 0x9098A8, 0xB8C0D0
    };
    /// <summary>
    /// 엑스의 기본 차지 1단계 색상 팔레트입니다.
    /// </summary>
    static readonly int[] X_NORMAL1_CHARGE_COLOR_PALETTE =
    {
        0x4898F8, 0x58B8F8, 0xC0F0F8, 0xB8C0D0
    };
    /// <summary>
    /// 엑스의 기본 차지 2단계 색상 팔레트입니다.
    /// </summary>
    static readonly int[] X_NORMAL2_CHARGE_COLOR_PALETTE =
    {
        0x38E888, 0x48F8A8, 0x80B8A8, 0xB8F8E8
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
    /// 차지 1단계 바디 색상표입니다.
    /// </summary>
    public static Color[] XCharge1Palette { get; private set; }
    /// <summary>
    /// 차지 2단계 바디 색상표입니다.
    /// </summary>
    public static Color[] XCharge2Palette { get; private set; }
    
    /// <summary>
    /// 무기1 색상 팔레트입니다.
    /// </summary>
    public static Color[] XWeapon1Palette { get; private set; }
    /// <summary>
    /// 무기2 색상 팔레트입니다.
    /// </summary>
    public static Color[] XWeapon2Palette { get; private set; }
    /// <summary>
    /// 무기3 색상 팔레트입니다.
    /// </summary>
    public static Color[] XWeapon3Palette { get; private set; }
    /// <summary>
    /// 무기4 색상 팔레트입니다.
    /// </summary>
    public static Color[] XWeapon4Palette { get; private set; }
    
    /// <summary>
    /// 엑스 기본 차지 효과 색상입니다.
    /// </summary>
    public static Color[] XDefaultChargeEffectColorPalette { get; private set; }
    /// <summary>
    /// 엑스 기본 차지 효과 1 색상입니다.
    /// </summary>
    public static Color[] XNormalChargeEffectColorPalette1 { get; private set; }
    /// <summary>
    /// 엑스 기본 차지 효과 2 색상입니다.
    /// </summary>
    public static Color[] XNormalChargeEffectColorPalette2 { get; private set; }
    
    /// <summary>
    /// 엑스 대쉬 효과 색상표입니다.
    /// </summary>
    public static Color[] DashEffectColorPalette { get; private set; }
    
    #endregion

    



    #region 생성자를 정의합니다.
    /// <summary>
    /// 엑스 캐릭터의 바디 색상표를 초기화합니다.
    /// </summary>
    static XColorPalette()
    {
        // 사용할 변수를 선언합니다.
        int PALETTE_COUNT = X_DEFAULT_COLOR_PALETTE.Length;
        Color[] invenciblePalette;
        Color[] xDefaultPalette;
        Color[] xCharge1Palette;
        Color[] xCharge2Palette;
        Color[] xDashEffectColorPalette;
        Color[] xWeapon1Palette;
        Color[] xWeapon2Palette;
        Color[] xWeapon3Palette;
        Color[] xWeapon4Palette;


        // 팔레트를 초기화합니다.
        invenciblePalette = new Color[PALETTE_COUNT];
        xDefaultPalette = new Color[PALETTE_COUNT];
        xDashEffectColorPalette = new Color[PALETTE_COUNT];


        xCharge1Palette = new Color[PALETTE_COUNT];
        xCharge2Palette = new Color[PALETTE_COUNT];
        xWeapon1Palette = new Color[PALETTE_COUNT];
        xWeapon2Palette = new Color[PALETTE_COUNT];
        xWeapon3Palette = new Color[PALETTE_COUNT];
        xWeapon4Palette = new Color[PALETTE_COUNT];
        for (int i = 0; i < PALETTE_COUNT; ++i)
        {
            // 기본 색상표를 초기화합니다.
            invenciblePalette[i] = ColorFromInt(INVENCIBLE_COLOR_PALETTE[i]);
            xDefaultPalette[i] = ColorFromInt(X_DEFAULT_COLOR_PALETTE[i]);
            xDashEffectColorPalette[i] = ColorFromInt(X_DASHAFTERIMAGE_COLOR_PALETTE[i], 0.6f);


            // 차지 색상표를 초기화합니다.
            xCharge1Palette[i] = ColorFromInt(X_CHARGE1_COLOR_PALETTE[i]);
            xCharge2Palette[i] = ColorFromInt(X_CHARGE2_COLOR_PALETTE[i]);


            // 웨폰 색상표를 초기화합니다.
            xWeapon1Palette[i] = ColorFromInt(X_WEAPON1_COLOR_PALETTE[i]);
            xWeapon2Palette[i] = ColorFromInt(X_WEAPON2_COLOR_PALETTE[i]);
            xWeapon3Palette[i] = ColorFromInt(X_WEAPON3_COLOR_PALETTE[i]);
            xWeapon4Palette[i] = ColorFromInt(X_WEAPON4_COLOR_PALETTE[i]);
        }
        InvenciblePalette = invenciblePalette;
        DefaultPalette = xDefaultPalette;
        DashEffectColorPalette = xDashEffectColorPalette;


        XCharge1Palette = xCharge1Palette;
        XCharge2Palette = xCharge2Palette;
        XWeapon1Palette = xWeapon1Palette;
        XWeapon2Palette = xWeapon2Palette;
        XWeapon3Palette = xWeapon3Palette;
        XWeapon4Palette = xWeapon4Palette;



        // 차지 효과 색상표를 초기화합니다.
        Color[] p1 = new Color[4];
        Color[] p2 = new Color[4];
        Color[] p3 = new Color[4];
        for (int i = 0; i < 4; ++i)
        {
            p1[i] = ColorFromInt(X_DEFAULT_CHARGE_COLOR_PALETTE[i]);
            p2[i] = ColorFromInt(X_NORMAL1_CHARGE_COLOR_PALETTE[i]);
            p3[i] = ColorFromInt(X_NORMAL2_CHARGE_COLOR_PALETTE[i]);
        }
        XDefaultChargeEffectColorPalette = p1;
        XNormalChargeEffectColorPalette1 = p2;
        XNormalChargeEffectColorPalette2 = p3;
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