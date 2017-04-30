using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get { return instance; } }

    [Range(0f, 1f)]
    public float globalSfxVolume = 1f;
    [Range(0f, 1f)]
    public float globalMusicVolume = 1f;
    public AudioClipInfo[] sounds;

    private Dictionary<SoundId, AudioClipInfo> soundsDictionary = new Dictionary<SoundId, AudioClipInfo>();
    private Dictionary<SoundId, List<PlayingAudioClipContext>> soundsBeingPlayed = new Dictionary<SoundId, List<PlayingAudioClipContext>>();
    private List<PlayingAudioClipContext> soundsToStop = new List<PlayingAudioClipContext>();
    private static SoundManager instance;

    protected SoundManager()
    {
        
    }

    void Awake()
    {
        instance = this;
        for (int i = 0; i < sounds.Length; i++)
        {
            AudioClipInfo sound = sounds[i];
            soundsDictionary.Add(sound.id, sound);
            soundsBeingPlayed.Add(sound.id, new List<PlayingAudioClipContext>());
        }
    }

    void Update()
    {
        // Check which sources have stopped playing and store the reference
        foreach (KeyValuePair<SoundId, List<PlayingAudioClipContext>> pair in soundsBeingPlayed)
        {
            foreach (PlayingAudioClipContext context in pair.Value)
            {
                if (context.playbackType == PlayingAudioClipContext.PlaybackType.FADE_IN)
                {
                    context.audioSource.volume += context.fadeSpeed * Time.deltaTime;
                }
                if (context.playbackType == PlayingAudioClipContext.PlaybackType.FADE_OUT)
                {
                    context.audioSource.volume -= context.fadeSpeed * Time.deltaTime;
                }
                if (!context.audioSource.isPlaying)
                {
                    soundsToStop.Add(context);
                }
            }
        }
        // Return those objects to the pool
        foreach (PlayingAudioClipContext context in soundsToStop)
        {
            AudioSourceContainerPool.instance.ReleaseObject(context.audioSourceContainer);
            soundsBeingPlayed[context.audioClipInfo.id].Remove(context);
        }
        soundsToStop.Clear();
    }

    /// <summary>
    /// Randomly choses one of the given audio clips and plays it.
    /// </summary>
    /// <param name="soundsIds"></param>
    public PlayingAudioClipContext PlayNewRandom(SoundId[] soundsIds)
    {
        int index = Random.Range(0, soundsIds.Length);
        return PlayNew(soundsIds[index]);
    }

    /// <summary>
    /// Play an aduio clip.
    /// </summary>
    /// <param name="audioClip"></param>
    public PlayingAudioClipContext PlayNew(SoundId soundId)
    {
        AudioClipInfo audioClipInfo = soundsDictionary[soundId];
        // Get the audio source container from the pool
        GameObject poolObject = AudioSourceContainerPool.instance.GetObject();
        poolObject.SetActive(true);
        // Get the audio source component, set the clip to play and play it
        AudioSource audioSource = poolObject.GetComponent<AudioSource>();
        SetInitialValues(audioSource, audioClipInfo);
        audioSource.Play();

        PlayingAudioClipContext context = new PlayingAudioClipContext();
        context.audioSource = audioSource;
        context.audioClipInfo = audioClipInfo;
        context.audioSourceContainer = poolObject;
        context.playbackType = PlayingAudioClipContext.PlaybackType.REGULAR;
        // Keep reference to the objects to return them later to the pool
        soundsBeingPlayed[soundId].Add(context);
        return context;
    }

    /// <summary>
    /// Play an aduio clip.
    /// </summary>
    /// <param name="audioClip"></param>
    public PlayingAudioClipContext Play(SoundId soundId)
    {
        PlayingAudioClipContext context = GetCurrentPlayingSound(soundId);
        if (context != null)
        {
            return context;
        }
        return PlayNew(soundId);
    }

    public PlayingAudioClipContext FadeIn(SoundId soundId, float speed)
    {
        PlayingAudioClipContext context = GetCurrentPlayingSound(soundId);
        if (context == null)
        {
            context = PlayNew(soundId);
        }
        context.playbackType = PlayingAudioClipContext.PlaybackType.FADE_IN;
        context.fadeSpeed = speed;
        return context;
    }

    public void FadeOut(SoundId soundId, float speed)
    {
        PlayingAudioClipContext context = GetCurrentPlayingSound(soundId);
        if (context != null)
        {
            context.playbackType = PlayingAudioClipContext.PlaybackType.FADE_OUT;
            context.fadeSpeed = speed;
        }
    }

    /// <summary>
    /// Stops every audio source currently playing the given clip.
    /// </summary>
    /// <param name="audioClip"></param>
    public void StopFirst(SoundId soundId)
    {
        PlayingAudioClipContext context = GetCurrentPlayingSound(soundId);
        if (context != null)
        {
            context.audioSource.Stop();
        }
    }

    public void StopAll(SoundId soundId)
    {
        foreach(PlayingAudioClipContext context in soundsBeingPlayed[soundId])
        {
            context.audioSource.Stop();
        }
    }

    /// <summary>
    /// Stops every audio source currently playing the given clip and then plays it from the begining.
    /// </summary>
    /// <param name="audioClip"></param>
    public PlayingAudioClipContext StopAndPlay(SoundId soundId)
    {
        PlayingAudioClipContext context = GetCurrentPlayingSound(soundId);
        if (context != null)
        {
            SetInitialValues(context.audioSource, context.audioClipInfo);
            context.playbackType = PlayingAudioClipContext.PlaybackType.REGULAR;
            context.audioSource.time = 0;
            return context;
        } else
        {
            return PlayNew(soundId);
        }
    }

    private PlayingAudioClipContext GetCurrentPlayingSound(SoundId soundId)
    {
        List<PlayingAudioClipContext> list = soundsBeingPlayed[soundId];
        if (list.Count > 0)
        {
            return list[0];
        }
        return null;
    }

    private void SetInitialValues(AudioSource audioSource, AudioClipInfo audioClipInfo)
    {
        audioSource.clip = audioClipInfo.audioClip;
        audioSource.volume = globalSfxVolume * audioClipInfo.volume;
        audioSource.loop = false;
    }
}
