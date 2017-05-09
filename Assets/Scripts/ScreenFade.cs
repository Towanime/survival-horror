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
        Color newColor = fadeImage.color;
        newColor.a = startingAlpha;
        fadeImage.color = newColor;

        if (fadeOnStartupType == FadeOnStartupType.IN)
        {
            FadeIn();
        } else if (fadeOnStartupType == FadeOnStartupType.OUT)
        {
            FadeOut();
        }
    }

    void FadeIn ()
    {
        fadeImage.CrossFadeAlpha(1, duration, false);
    }

    void FadeOut ()
    {
        fadeImage.CrossFadeAlpha(0, duration, false);
    }
}
