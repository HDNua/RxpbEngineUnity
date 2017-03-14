using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 
/// </summary>
public class BossEnemyHeadIntroScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// 
    /// </summary>
    private void LateUpdate()
    {
        // UpdateColor(GetComponent<SpriteRenderer>(), EnemyColorPalette.InvenciblePalette);
        if (flag == 1)
        {
            flag += 1;
        }
    }

    public int flag = 0;



    /// <summary>
    /// 
    /// </summary>
    public void UpdateColor(SpriteRenderer renderer, Color[] currentPalette)
    {
        Texture2D texture = renderer.sprite.texture;

        // !!!!! IMPORTANT !!!!!
        // 1. 텍스쳐 파일은 Read/Write 속성이 Enabled여야 합니다.
        // 2. 반드시 Generate Mip Maps 속성을 켜십시오.
        Color[] colors = texture.GetPixels();
        Color[] pixels = new Color[colors.Length];
        Color[] DefaultPalette = EnemyColorPalette.IntroBossHeadPalette;

        // 모든 픽셀을 돌면서 색상을 업데이트합니다.
        for (int pixelIndex = 0, pixelCount = colors.Length; pixelIndex < pixelCount; ++pixelIndex)
        {
            Color color = colors[pixelIndex];
            if (color.a == 1)
            {
                for (int targetIndex = 0, targetPixelCount = DefaultPalette.Length; targetIndex < targetPixelCount; ++targetIndex)
                {
                    Color colorDst = DefaultPalette[targetIndex];
                    if (Mathf.Approximately(color.r, colorDst.r) &&
                        Mathf.Approximately(color.g, colorDst.g) &&
                        Mathf.Approximately(color.b, colorDst.b) &&
                        Mathf.Approximately(color.a, colorDst.a))
                    {
                        pixels[pixelIndex] = currentPalette[targetIndex];
                        break;
                    }
                }
            }
            else
            {
                pixels[pixelIndex] = color;
            }
        }

        // 텍스쳐를 복제하고 새 픽셀 팔레트로 덮어씌웁니다.
        Texture2D cloneTexture = new Texture2D(texture.width, texture.height);
        cloneTexture.filterMode = FilterMode.Point;
        cloneTexture.SetPixels(pixels);
        cloneTexture.Apply();

        // 새 텍스쳐를 렌더러에 반영합니다.
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", cloneTexture);
        renderer.SetPropertyBlock(block);
    }
}
