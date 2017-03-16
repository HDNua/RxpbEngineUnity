using System;
using UnityEngine;



/// <summary>
/// 장면의 페이드인 또는 페이드아웃 효과를 처리합니다.
/// </summary>
public class ScreenFader : MonoBehaviour
{
    #region 필드 및 프로퍼티를 정의합니다.
    /// <summary>
    /// 페이드 인/아웃 관리자입니다.
    /// </summary>
    public static ScreenFader Instance
    {
        get
        {
            return GameObject.FindGameObjectWithTag("Fader")
                .GetComponent<ScreenFader>();
        }
    }

    /// <summary>
    /// GUITexture 개체입니다.
    /// </summary>
    GUITexture _guiTexture;
    /// <summary>
    /// 페이드 인/아웃 속도입니다.
    /// </summary>
    public float fadeSpeed = 1.5f;

    /// <summary>
    /// 
    /// </summary>
    Color _colorDst = Color.black;
    /// <summary>
    /// 
    /// </summary>
    float _thresholdDst = 0.95f;


    /// <summary>
    /// 페이드 인/아웃 텍스쳐 색상 집합입니다.
    /// </summary>
    public Texture[] _textures;
    
    /// <summary>
    /// 페이드 인이 요청되었다면 참입니다.
    /// </summary>
    bool _fadeInRequested = false;
    /// <summary>
    /// 페이드 아웃이 요청되었다면 참입니다.
    /// </summary>
    bool _fadeOutRequested = false;
    
    /// <summary>
    /// 페이드 인이 끝났다면 참입니다.
    /// </summary>
    public bool FadeInEnded { get { return (_guiTexture.color == Color.clear); } }
    /// <summary>
    /// 페이드 아웃이 끝났다면 참입니다.
    /// </summary>
    public bool FadeOutEnded { get { return (_guiTexture.color == _colorDst); } }

    #endregion
    




    #region MonoBehaviour 기본 메서드를 재정의 합니다.
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        _guiTexture = GetComponent<GUITexture>();
        _guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (_fadeInRequested)
        {
            FadeToClear();
            if (_guiTexture.color.a <= 0.05f)
            {
                _guiTexture.color = Color.clear;
                _guiTexture.enabled = false;
                _fadeInRequested = false;
            }
        }
        else if (_fadeOutRequested)
        {
            _guiTexture.enabled = true;
            FadeToDestination();

            if (_guiTexture.color.a >= _thresholdDst)
            {
                _guiTexture.color = _colorDst;
                _fadeOutRequested = false;
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
    void FadeToDestination()
    {
        _guiTexture.color = Color.Lerp
            (_guiTexture.color, _colorDst, fadeSpeed * Time.deltaTime);
    }
    
    /// <summary>
    /// 페이드인 효과를 처리합니다.
    /// </summary>
    public void FadeIn()
    {
        _fadeInRequested = true;
        _fadeOutRequested = false;
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
        _fadeInRequested = false;
        _fadeOutRequested = true;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="guiTextureColor"></param>
    /// <param name="colorDst"></param>
    /// <param name="thresDst"></param>
    public void ChangeFadeTextureColor(Color guiTextureColor, Color colorDst, float thresDst)
    {
        _guiTexture.color = guiTextureColor;
        _colorDst = colorDst;
        _thresholdDst = thresDst;
    }
    /// <summary>
    /// 페이드 인/아웃 텍스쳐 색상을 전환합니다.
    /// </summary>
    public void ChangeFadeTextureColor(int index)
    {
        _guiTexture.texture = _textures[index];
    }

    #endregion
}