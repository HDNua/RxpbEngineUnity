using UnityEngine;
using System;

/// <summary>
/// 장면의 페이드인 또는 페이드아웃 효과를 처리합니다.
/// </summary>
public class ScreenFader : MonoBehaviour
{
    #region 필드 및 프로퍼티를 정의합니다.
    GUITexture _guiTexture;
    public float fadeSpeed = 1.5f;

    bool fadeInRequested = false;
    bool fadeOutRequested = false;

    public bool FadeInEnded { get { return (_guiTexture.color == Color.clear); } }
    public bool FadeOutEnded { get { return (_guiTexture.color == Color.black); } }

    #endregion



    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    void Awake()
    {
        _guiTexture = GetComponent<GUITexture>();
        _guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
    }
    void Update()
    {
        if (fadeInRequested)
        {
            FadeToClear();
            if (_guiTexture.color.a <= 0.05f)
            {
                _guiTexture.color = Color.clear;
                _guiTexture.enabled = false;
                fadeInRequested = false;
            }
        }
        else if (fadeOutRequested)
        {
            _guiTexture.enabled = true;
            FadeToBlack();

            if (_guiTexture.color.a >= 0.95f)
            {
                _guiTexture.color = Color.black;
                fadeOutRequested = false;
            }
        }
    }

    #endregion



    #region 보조 메서드를 정의합니다.
    /// <summary>
    /// 페이드인 효과를 한 단계 진행합니다.
    /// </summary>
    void FadeToClear()
    {
        _guiTexture.color = Color.Lerp
            (_guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 페이드아웃 효과를 한 단계 진행합니다.
    /// </summary>
    void FadeToBlack()
    {
        _guiTexture.color = Color.Lerp
            (_guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 페이드인 효과를 처리합니다.
    /// </summary>
    public void FadeIn()
    {
        fadeInRequested = true;
        fadeOutRequested = false;
    }
    /// <summary>
    /// 페이드인 효과를 처리합니다.
    /// </summary>
    /// <param name="fadeSpeed">페이드인 속도입니다.</param>
    public void FadeIn(float fadeSpeed)
    {
        this.fadeSpeed = fadeSpeed;
        FadeIn();
    }
    /// <summary>
    /// 페이드아웃 효과를 처리합니다.
    /// </summary>
    public void FadeOut()
    {
        fadeInRequested = false;
        fadeOutRequested = true;
    }
    /// <summary>
    /// 페이드아웃 효과를 처리합니다.
    /// </summary>
    /// <param name="fadeSpeed">페이드아웃 속도입니다.</param>
    public void FadeOut(float fadeSpeed)
    {
        this.fadeSpeed = fadeSpeed;
        FadeOut();
    }
    #endregion
}