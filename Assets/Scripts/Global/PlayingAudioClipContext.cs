using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingAudioClipContext {

    public enum PlaybackType
    {
        REGULAR,
        FADE_IN,
        FADE_OUT
    }

    public GameObject audioSourceContainer;
    public AudioClipInfo audioClipInfo;
    public AudioSource audioSource;
    public PlaybackType playbackType = PlaybackType.REGULAR;
    public float fadeSpeed;
}
