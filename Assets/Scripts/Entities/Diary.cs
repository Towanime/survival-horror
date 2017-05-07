using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diary : MonoBehaviour {

    public Image image;
    public float fadeTime = 0.5f;

    private GameObject gameStateMachine;
    private bool open;
    private bool fading;

    void Start()
    {
        gameStateMachine = GameObject.FindGameObjectWithTag("GameStateMachine");
    }

    public IEnumerator Close()
    {
        if (open && !fading)
        {
            open = false;
            fading = true;
            image.CrossFadeAlpha(0, fadeTime, false);
            yield return new WaitForSeconds(fadeTime);
            image.enabled = false;
            gameStateMachine.SendMessage("OnBookClosed");
            fading = false;
        }
    }

    public IEnumerator Open()
    {
        if (!open && !fading)
        {
            open = true;
            fading = true;
            image.enabled = true;
            gameStateMachine.SendMessage("OnBookOpened");
            image.CrossFadeAlpha(0, 0, false);
            image.CrossFadeAlpha(1, fadeTime, false);
            yield return new WaitForSeconds(fadeTime);
            fading = false;
        }
    }
}
