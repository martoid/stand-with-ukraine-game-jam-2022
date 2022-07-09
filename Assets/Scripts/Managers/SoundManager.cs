using DG.Tweening;
using System;
using UnityEngine;

public enum SoundType
{
    carrotChop = 0,
    talking = 1,
    boilingWater = 2,
    choppingSound = 3,
    grabSound = 4,
    splash = 5,
    discard = 6,
    grate = 7,
    pourLiquid = 8,
    releaseBottle = 9,
}

[Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSO soundsSo;

    public static SoundManager instance;

    [SerializeField] private GameObject _musicSource, _effectSource;

    private AudioSource music;

    void Awake()
    {
        instance = this;
    }

    public void PlayEffect(SoundType clip, float deviation = 0, bool loop = false)
    {
        var sound = Instantiate(_effectSource, transform);
        var audioClip = FindClip(clip, soundsSo.sounds);
        var source = sound.GetComponent<AudioSource>();

        if (loop)
        {
            source.clip = audioClip;
            source.loop = true;
            source.Play();
        }
        else
        {
            source.PlayOneShot(audioClip);
            source.pitch += UnityEngine.Random.Range(-deviation, deviation);
            DOVirtual.DelayedCall(audioClip.length, () => sound.SetActive(false));
        }
    }

    public void PlayMusic(SoundType clip)
    {
        var sound = Instantiate(_musicSource, transform);
        var audioClip = FindClip(clip, soundsSo.sounds);

        if (music) music.Stop();

        music = sound.GetComponent<AudioSource>();
        music.clip = audioClip;
        music.loop = true;
        music.Play();
    }

    public AudioSource GetSoundObject(SoundType clip)
    {
        var sound = Instantiate(_effectSource, transform);
        var audioClip = FindClip(clip, soundsSo.sounds);
        var source = sound.GetComponent<AudioSource>();
        source.clip = audioClip;
        source.loop = true;

        return source;
    }

    private AudioClip FindClip(SoundType clip, Sound[] sounds)
    {
        foreach (var sound in sounds)
        {
            if (sound.type == clip)
                return sound.clip;
        }

        return null;
    }
}
