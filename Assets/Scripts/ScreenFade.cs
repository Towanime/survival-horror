using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

    public Image fadeImage;
    public float startingAlpha = 1;
    public float duration = 2;
    public FadeOnStartupType fadeOnStartupType = FadeOnStartupType.NONE;

    public enum FadeOnStartupType
    {
        NONE,
        IN,
        OUT
    }

    void Start()
    {
        SetAlpha(startingAlpha);
        if (fadeOnStartupType == FadeOnStartupType.IN)
        {
            FadeIn();
        } else if (fadeOnStartupType == FadeOnStartupType.OUT)
        {
            FadeOut();
        }
    }

    public void FadeIn ()
    {
        fadeImage.CrossFadeAlpha(1, duration, false);
    }

    public void FadeOut ()
    {
        fadeImage.CrossFadeAlpha(0, duration, false);
    }

    public void SetAlpha(float alpha)
    {
        Color newColor = fadeImage.color;
        newColor.a = alpha;
        fadeImage.color = newColor;
    }
}
